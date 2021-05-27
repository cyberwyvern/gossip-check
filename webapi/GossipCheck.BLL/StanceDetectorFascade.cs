using GossipCheck.BLL.Interface;
using GossipCheck.BLL.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GossipCheck.BLL
{
    public class StanceDetectorFascade : IStanceDetectorFascade
    {
        private readonly StanceDetectorServiceConfig config;
        private readonly HttpClient client;

        public StanceDetectorFascade(IOptions<StanceDetectorServiceConfig> config)
        {
            this.config = config.Value;
            this.client = new HttpClient();
        }

        public async Task<IEnumerable<KeyValuePair<string, Stance>>> GetSourceStances(string textOrigin)
        {
            var keywordsResponse = await GetKeywords(textOrigin);
            var articlesResponse = await SearchArticles(new ArticleSearchRequest
            {
                Language = keywordsResponse.Language,
                Keywords = keywordsResponse.Keywords
            });

            var requestObject = new StanceDetectionRequest
            {
                Headline = string.Join(' ', keywordsResponse.Keywords),
                Bodies = articlesResponse.Articles
                    .GroupBy(x => x.Link)
                    .Select(x => x.First())
                    .ToDictionary(x => x.Link, x => x.Summary)
            };

            var result = await GetStances(requestObject);

            return result;
        }

        private async Task<IEnumerable<KeyValuePair<string, Stance>>> GetStances(StanceDetectionRequest requestObj)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(new Uri(config.StanceDetectionAiUrl), "predict"),
                Content = new StringContent(JsonConvert.SerializeObject(requestObj), Encoding.UTF8, "application/json"),
            };

            using var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            var responseObj = JsonConvert.DeserializeObject<Dictionary<string, Stance>>(body);

            return responseObj;
        }

        private async Task<KeywordsExtractionResponse> GetKeywords(string textOrigin)
        {
            var requestObj = new KeywordsExtractionRequest { TextOrigin = textOrigin };
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(new Uri(config.WebScraperUrl), "webscraper/extract-keywords"),
                Content = new StringContent(JsonConvert.SerializeObject(requestObj), Encoding.UTF8, "application/json"),
            };

            using var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            var responseObj = JsonConvert.DeserializeObject<KeywordsExtractionResponse>(body);

            return responseObj;
        }

        private async Task<ArticleSearchResponse> SearchArticles(ArticleSearchRequest requestObj)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(new Uri(config.WebScraperUrl), "webscraper/search-articles"),
                Content = new StringContent(JsonConvert.SerializeObject(requestObj), Encoding.UTF8, "application/json"),
            };

            using var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            var responseObj = JsonConvert.DeserializeObject<ArticleSearchResponse>(body);

            return responseObj;
        }
    }
}
