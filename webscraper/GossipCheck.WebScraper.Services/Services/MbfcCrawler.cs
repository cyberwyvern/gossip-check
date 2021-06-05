using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using GossipCheck.WebScraper.Services.ConfigurationOptionModels;
using GossipCheck.WebScraper.Services.Exceptions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Drawing;
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
            string pageUrl = null;
            while (retry > 0)
            {
                try
                {
                    pageUrl = await GetMbfcPageUrl(hostName);
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
                    Console.WriteLine($"!!!!!!!!!!!!!!!!!!!!!!!! {url} - {pageUrl}");
                    throw;
                    //throw new MbfcParserException();
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
            var detailedReport = document.QuerySelectorAll("p")
                .Select(x => x.TextContent)
                .FirstOrDefault(x => x.Contains("Factual Reporting", StringComparison.OrdinalIgnoreCase));

            if (detailedReport == null)
            {
                return await GetMbfcReportDictionaryFromImages(document);
            }

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
            var pageUrls = document
                .QuerySelectorAll(@"a[href][title][rel=""bookmark""]")
                .Select(x => x.GetAttribute("href"))
                .ToArray();

            var pageUrl = pageUrls.FirstOrDefault(x => Regex.IsMatch(host, Regex.Replace(x, @"\W|", ".?"), RegexOptions.IgnoreCase))
                ?? pageUrls.FirstOrDefault();

            return new Uri(new Uri(config.ServiceUrl), pageUrl).ToString();
        }

        private string GetHostName(string url)
        {
            var sourceUrl = new Uri(url, UriKind.RelativeOrAbsolute);
            return sourceUrl.IsAbsoluteUri
                ? sourceUrl.DnsSafeHost
                : sourceUrl.ToString();
        }

        private async Task<Dictionary<string, string>> GetMbfcReportDictionaryFromImages(IHtmlDocument document)
        {
            var factualReportingImageUrl = document
                .QuerySelector(@"img[alt*=""Factual Reporting""]")
                .GetAttribute("src")
                .Split('?')[0];

            return new Dictionary<string, string>
            {
                {"Factual Reporting:", await GetFactualReportingFromImage(factualReportingImageUrl)}
            };
        }

        private async Task<string> GetFactualReportingFromImage(string url)
        {
            const int rows = 8;
            const int margin = 10;
            const string white = "ffffffff";

            var response = await client.GetAsync(url);
            using var imgBytes = await response.Content.ReadAsStreamAsync();
            using var bmp = new Bitmap(imgBytes);

            var step = bmp.Height / rows;
            int startPixel = step + step / 2;
            var factualReportingResultDictionary = new Dictionary<string, bool>
            {
                {"Very High",      bmp.GetPixel(bmp.Width / margin, startPixel + step * 0).Name != white},
                {"High",           bmp.GetPixel(bmp.Width / margin, startPixel + step * 1).Name != white},
                {"Mostly Factual", bmp.GetPixel(bmp.Width / margin, startPixel + step * 2).Name != white},
                {"Mixed",          bmp.GetPixel(bmp.Width / margin, startPixel + step * 3).Name != white},
                {"Low",            bmp.GetPixel(bmp.Width / margin, startPixel + step * 4).Name != white},
                {"Very Low",       bmp.GetPixel(bmp.Width / margin, startPixel + step * 5).Name != white}
            };

            return factualReportingResultDictionary.FirstOrDefault(x => x.Value).Key;
        }

        private bool CheckSourceOnPage(string host, IHtmlDocument document)
        {
            return document.QuerySelector($"a[href*={host}]") != null;
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}