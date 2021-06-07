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
        private const string detailedReportSelector = "p";
        private const string factualReportingImageSelector = @"img[alt*=""Factual Reporting""]";
        private const string reportPageUrlSelector = @"a[href][title][rel=""bookmark""]";
        private const string sourceUrlSelector = @"a[href*=""{0}""]";

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
            url.EnsureWebUrl();

            for (var i = 0; i < this.config.Attempts; i++)
            {
                try
                {
                    return (await this.AttemptGetReport(new Uri(url).Host)).ToMbfcReport();
                }
                catch (HttpRequestException)
                {
                    await Task.Delay(this.config.RetryInterval);
                }
            }

            throw new MbfcCrawlerException($"Could not get response from {this.config.ServiceUrl}");
        }

        private async Task<Dictionary<string, string>> AttemptGetReport(string host)
        {
            await this.RetrieveSearchPage(host);
            var reportUrls = this.SearchReportPages(host).Take(this.config.SearchVisits).ToList();

            string reportPageUrl = null;
            foreach (var url in reportUrls)
            {
                await this.RetrieveReportPage(url);
                if (this.ValidateSourceOnReportPage(host))
                {
                    reportPageUrl = url;
                    break;
                }
            }

            try
            {
                return new Dictionary<string, string>
                {
                    { "PageUrl", reportPageUrl ?? throw new MbfcCrawlerException($"Report page not found for {host}") },
                    { "Source", host }
                }
                .Union(this.GetReportDictionary())
                .Union(this.GetReportDictionaryFromImages())
                .GroupBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.First().Value);
            }
            catch (Exception ex)
            {
                throw new MbfcCrawlerException($"Parsing error for {host}", ex);
            }
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
            if (this.reportPage != null)
            {
                this.reportPage.Dispose();
            }

            var response = await this.client.GetAsync(reportPageUrl);
            response.EnsureSuccessStatusCode();
            var html = await response.Content.ReadAsStringAsync();

            var parser = new HtmlParser();
            this.reportPage = parser.ParseDocument(html);
        }

        private Dictionary<string, string> GetReportDictionary()
        {
            var detailedReport = this.reportPage.QuerySelectorAll(detailedReportSelector)
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
            var factualReportingImage = this.reportPage.QuerySelector(factualReportingImageSelector);

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
                .QuerySelectorAll(reportPageUrlSelector)
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

        private bool ValidateSourceOnReportPage(string host)
        {
            return this.reportPage?.QuerySelector(string.Format(sourceUrlSelector, host)) != null;
        }

        public void Dispose()
        {
            this.client.Dispose();
            this.searchPage?.Dispose();
            this.reportPage?.Dispose();
        }
    }
}