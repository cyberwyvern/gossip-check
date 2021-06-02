using GossipCheck.WebScraper.Services.ConfigurationOptionModels;
using GossipCheck.WebScraper.Services.Models;
using Microsoft.Extensions.Options;
using System;
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

        public async Task<MbfcReport> GetMbfcReport(string url)
        {
            var source = new Uri(url).GetLeftPart(UriPartial.Authority);
            var pageUrl = await GetMbfcPageUrl(url);
            if (pageUrl == null)
            {
                return null;
            }

            var response = await client.GetAsync(new Uri(pageUrl));
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();

            var name = GetFirstMatch(body, @"<h1.*title.*>(.*?)<\/h1>");
            var biasRating = GetFirstMatch(body, @"Bias\sRating:.*?>([^<]+?)<");
            var factualReporting = GetFirstMatch(body, @"Factual\sReporting:.*?>([^<]+?)<");
            var country = GetFirstMatch(body, @"Country:.*?>([^<]+?)<");
            var mediaType = GetFirstMatch(body, @"Media\sType:.*?>([^<]+?)<");
            var trafficPopularity = GetFirstMatch(body, @"Traffic\/Popularity:.*?>([^<]+?)<");
            var mbfcCredibilityRating = GetFirstMatch(body, @"MBFC\sCredibility\sRating:.*?>([^<]+?)<");

            return new MbfcReport
            {
                Source = source,
                Name = name,
                BiasRating = biasRating,
                FactualReporting = factualReporting,
                Country = country,
                MediaType = mediaType,
                TrafficPopularity = trafficPopularity,
                MBFCCredibilityRating = mbfcCredibilityRating,
                PageUrl = pageUrl
            };
        }

        private async Task<string> GetMbfcPageUrl(string source)
        {
            var hostName = new Uri(source).Host;
            var response = await client.GetAsync($"?s={HttpUtility.UrlEncode(hostName)}");
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            var namePattern = hostName
                .Split('.')
                .Select(x => $"({string.Join(".?", x.ToCharArray())})")
                .Aggregate((x, y) => x + '|' + y);

            var relativeUri = GetFirstMatch(body, $@"href=""/({namePattern})/""");
            if (relativeUri == null)
            {
                return null;
            }

            return new Uri(new Uri(config.ServiceUrl), relativeUri).ToString();
        }

        private string GetFirstMatch(string input, string regexPattern)
        {
            var match = Regex.Match(
                input,
                regexPattern,
                RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);

            return match.Success ? match.Groups[1].Value : null;
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}