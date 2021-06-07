using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using GossipCheck.WebScraper.Services.ConfigurationOptionModels;
using GossipCheck.WebScraper.Services.Exceptions;
using GossipCheck.WebScraper.Services.Mapping;
using GossipCheck.WebScraper.Services.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace GossipCheck.WebScraper.Services.Services
{
    public class MbfcCrawler : IMbfcCrawler, IDisposable
    {
        private readonly HttpClient client;
        private readonly MbfcServiceConfig config;
        private readonly string[] reportFieldsList = new[] {
            "Bias Rating",
            "Factual Reporting",
            "Country",
            "County",
            "Media Type",
            "Traffic.Popularity",
            "MBFC Credibility Rating",
            "World Press Freedom Rank"
        };

        private IHtmlDocument searchPage;
        private IHtmlDocument reportPage;

        public MbfcCrawler(IOptions<MbfcServiceConfig> config)
        {
            this.config = config.Value;
            this.client = new HttpClient
            {
                BaseAddress = new Uri(config.Value.ServiceUrl)
            };
        }

        public async Task<MbfcReport> GetMbfcReport(string url)
        {
            for (var i = 0; i < this.config.Attempts; i++)
            {
                try
                {
                    return (await this.AttemptGetReport(url)).ToMbfcReport();
                }
                catch (HttpRequestException)
                {
                    await Task.Delay(this.config.RetryInterval);
                }
                catch
                {
                    throw new MbfcParserException();
                }
            }

            throw new MbfcRequestException();
        }

        private async Task<Dictionary<string, string>> AttemptGetReport(string url)
        {
            var hostName = this.GetHostName(url);
            await this.RetrieveSearchPage(hostName);
            var reportUrls = this.SearchReportPages(hostName)
                .Take(this.config.SearchVisits)
                .ToList();

            var urlIndex = 0;
            string reportPageUrl = null;
            while (reportPageUrl == null && urlIndex < reportUrls.Count())
            {
                this.reportPage?.Dispose();
                this.reportPage = null;

                await this.RetrieveReportPage(reportUrls[urlIndex]);
                if (this.ValidateSourceOnReportPage(hostName))
                {
                    reportPageUrl = reportUrls[urlIndex];
                }

                urlIndex++;
            }

            if (reportPageUrl == null)
            {
                throw new MbfcParserException("Invalid report page");
            }

            var report = this.GetReportDictionary();
            foreach (var kv in this.GetReportDictionaryFromImages())
            {
                if (!report.ContainsKey(kv.Key))
                {
                    report[kv.Key] = kv.Value;
                }
            }

            report["PageUrl"] = reportPageUrl;
            report["Source"] = hostName;

            return report;
        }

        private async Task RetrieveSearchPage(string host)
        {
            var response = await this.client.GetAsync($"?s={HttpUtility.UrlEncode(host)}");
            response.EnsureSuccessStatusCode();
            var html = await response.Content.ReadAsStringAsync();

            var parser = new HtmlParser();
            this.searchPage = parser.ParseDocument(html);
        }

        private async Task RetrieveReportPage(string reportPageUrl)
        {
            var response = await this.client.GetAsync(reportPageUrl);
            response.EnsureSuccessStatusCode();
            var html = await response.Content.ReadAsStringAsync();

            var parser = new HtmlParser();
            this.reportPage = parser.ParseDocument(html);
        }

        private Dictionary<string, string> GetReportDictionary()
        {
            var detailedReport = this.reportPage.QuerySelectorAll("p")
                .Select(x => x.TextContent)
                .FirstOrDefault(x => this.reportFieldsList.Any(f => x.Contains(f)));

            var matches = Regex.Matches(
                detailedReport ?? string.Empty,
                @$".+?(?={string.Join(":|", this.reportFieldsList)}|$)",
                RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

            return matches
                .Cast<Match>()
                .Select(x => x.Value.Split(':'))
                .ToDictionary(x => x[0].ToPascalCase(), x => x[1].Trim());
        }

        private Dictionary<string, string> GetReportDictionaryFromImages()
        {
            var factualReportingImage = this.reportPage.QuerySelector(@"img[alt*=""Factual Reporting""]");

            var dictionary = new Dictionary<string, string>();
            if (factualReportingImage != null)
            {
                var src = factualReportingImage.GetAttribute("src");
                var value = Regex.Match(src, @"(?<=MBFC).+(?=\.png)", RegexOptions.IgnoreCase).Value;
                dictionary["FactualReporting"] = value;
            }

            return dictionary;
        }

        private IEnumerable<string> SearchReportPages(string host)
        {
            var reportPages = this.searchPage
                .QuerySelectorAll(@"a[href][title][rel=""bookmark""]")
                .Select(x => new Uri(new Uri(this.config.ServiceUrl), x.GetAttribute("href")).ToString())
                .ToList();

            var bestMatch = reportPages.FirstOrDefault(x => Regex.IsMatch(host, Regex.Replace(x, @"\W|", ".?"), RegexOptions.IgnoreCase));
            if (bestMatch != null)
            {
                reportPages.Remove(bestMatch);
                reportPages.Prepend(bestMatch);
            }

            return reportPages;
        }

        private string GetHostName(string url)
        {
            var sourceUrl = new Uri(url, UriKind.RelativeOrAbsolute);
            return sourceUrl.IsAbsoluteUri
                ? sourceUrl.DnsSafeHost
                : sourceUrl.ToString();
        }

        private bool ValidateSourceOnReportPage(string host)
        {
            return this.reportPage?.QuerySelector($@"a[href*=""{host}""]") != null;
        }

        public void Dispose()
        {
            this.client.Dispose();
            this.searchPage?.Dispose();
            this.reportPage?.Dispose();
        }
    }
}