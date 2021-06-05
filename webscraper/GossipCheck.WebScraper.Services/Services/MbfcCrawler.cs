using AngleSharp.Html.Parser;
using GossipCheck.WebScraper.Services.ConfigurationOptionModels;
using GossipCheck.WebScraper.Services.Exceptions;
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

        public MbfcCrawler(IOptions<MbfcServiceConfig> config)
        {
            this.config = config.Value;
            client = new HttpClient
            {
                BaseAddress = new Uri(config.Value.ServiceUrl)
            };
        }

        public async Task<Dictionary<string, string>> GetMbfcReport(string url)
        {
            var hostName = GetHostName(url);

            var retry = this.config.RetryCount;
            while (retry > 0)
            {
                try
                {
                    var pageUrl = await GetMbfcPageUrl(hostName);
                    var report = await GetMbfcReportDictionary(pageUrl);
                    report["PageUrl"] = pageUrl;
                    report["Source"] = hostName;

                    return report;
                }
                catch (HttpRequestException)
                {
                    retry--;
                    await Task.Delay(this.config.RetryInterval);
                }
                catch
                {
                    throw new MbfcParserException();
                }
            }

            throw new MbfcRequestException();
        }

        private async Task<Dictionary<string, string>> GetMbfcReportDictionary(string pageUrl)
        {
            var response = await client.GetAsync(pageUrl);
            response.EnsureSuccessStatusCode();
            var html = await response.Content.ReadAsStringAsync();

            var parser = new HtmlParser();
            var document = parser.ParseDocument(html);
            var detailedReport = document.QuerySelector("h3 + p")?.TextContent;
            var matches = Regex.Matches(
                detailedReport,
                @".+?(?=Bias Rating|Factual Reporting|Country|Media Type|Traffic.Popularity|MBFC Credibility Rating|World Press Freedom Rank|$)",
                RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

            return matches
                .Cast<Match>()
                .Select(x => x.Value.Split(':'))
                .ToDictionary(x => x[0].ToPascalCase(), x => x[1].Trim());
        }

        private async Task<string> GetMbfcPageUrl(string host)
        {
            var response = await client.GetAsync($"?s={HttpUtility.UrlEncode(host)}");
            response.EnsureSuccessStatusCode();
            var html = await response.Content.ReadAsStringAsync();

            var parser = new HtmlParser();
            var document = parser.ParseDocument(html);
            var pageUrl = document
                .QuerySelectorAll(@"a[href][title][rel=""bookmark""]")
                .Select(x => x.GetAttribute("href"))
                .FirstOrDefault(x => Regex.IsMatch(host, Regex.Replace(x, @"\W|", ".?")));

            return new Uri(new Uri(config.ServiceUrl), pageUrl).ToString();
        }

        private string GetHostName(string url)
        {
            var sourceUrl = new Uri(url, UriKind.RelativeOrAbsolute);
            return sourceUrl.IsAbsoluteUri
                ? sourceUrl.DnsSafeHost
                : sourceUrl.ToString();
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}