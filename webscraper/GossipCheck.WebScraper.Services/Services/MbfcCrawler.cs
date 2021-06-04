using GossipCheck.WebScraper.Services.ConfigurationOptionModels;
using GossipCheck.WebScraper.Services.Exceptions;
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
            try
            {
                var pageUrl = await GetMbfcPageUrl(url);
                var reportHtml = await GetMbfcReportHtml(pageUrl);

                var report = RegexMapperHelper.Map<MbfcReport>(reportHtml);
                report.Source = new Uri(url).GetLeftPart(UriPartial.Authority);
                report.PageUrl = pageUrl;

                return report;
            }
            catch (HttpRequestException)
            {
                throw new MbfcRequestException();
            }
            catch
            {
                throw new MbfcParserException();
            }
        }

        private async Task<string> GetMbfcReportHtml(string pageUrl)
        {
            var response = await client.GetAsync(new Uri(pageUrl));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
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

            var match = Regex.Match(
                body,
                $@"href=""/({namePattern})/""",
                RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);

            if (match.Success)
            {
                var relativeUrl = match.Groups[1].Value;
                return new Uri(new Uri(config.ServiceUrl), relativeUrl).ToString();
            }

            return null;
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}