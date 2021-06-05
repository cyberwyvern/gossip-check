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
            var hostName = GetHostName(url);

            var retry = this.config.RetryCount;
            while (retry > 0)
            {
                try
                {
                    var pageUrl = await GetMbfcPageUrl(hostName);
                    var reportHtml = await GetMbfcReportHtml(pageUrl);

                    var report = RegexMapperHelper.Map<MbfcReport>(reportHtml);
                    report.Source = hostName;
                    report.PageUrl = pageUrl;

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

        private async Task<string> GetMbfcReportHtml(string pageUrl)
        {
            var response = await client.GetAsync(new Uri(pageUrl));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        private async Task<string> GetMbfcPageUrl(string hostName)
        {
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