using GossipCheck.WebScraper.Services.ConfigurationOptionModels;
using GossipCheck.WebScraper.Services.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace GossipCheck.WebScraper.Services.Services
{
    public class WebScraperService : IWebScraperService
    {
        private readonly ScraperServiceConfig config;

        public WebScraperService(IOptions<ScraperServiceConfig> config)
        {
            this.config = config.Value;
        }

        public async Task<IEnumerable<Article>> SearchArticles(Language? language, IEnumerable<string> keywords)
        {
            Dictionary<string, string> queryStringDict = new Dictionary<string, string>
            {
                { "q", HttpUtility.UrlEncode(string.Join(" ", keywords)) },
                { "media", "True" }
            };

            if (language.HasValue)
            {
                queryStringDict.Add("lang", LanguageCodes.Codes[language.Value]);
            }

            string query = string.Join("&", queryStringDict.Select(x => $"{x.Key}={x.Value}"));

            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{config.ServiceUrl}?{query}"),
                Headers =
                {
                    { "x-rapidapi-key", config.ApiKey },
                    { "x-rapidapi-host", config.ServiceHost },
                },
            };

            using HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string body = await response.Content.ReadAsStringAsync();

            IEnumerable<Article> result = JToken.Parse(body)["articles"]
                .ToObject<ScraperArticleModel[]>()
                .Select(x => new Article
                {
                    Link = x.link,
                    PublishedDate = x.published_date,
                    Summary = x.summary,
                    Title = x.title
                });

            return result;
        }
    }
}