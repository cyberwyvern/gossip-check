using GossipCheck.BLL.ConfigurationModels;
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

namespace GossipCheck.BLL.Services
{
    public class StanceDetectorFascade : IStanceDetectorFascade
    {
        private readonly StanceDetectorServiceConfig config;
        private readonly HttpClient client;

        public StanceDetectorFascade(IOptions<StanceDetectorServiceConfig> config)
        {
            this.config = config.Value;
            client = new HttpClient();
        }

        public async Task<IEnumerable<KeyValuePair<string, Stance>>> GetSourceStances(string textOrigin)
        {
            KeywordsExtractionResponse keywordsResponse = await GetKeywords(textOrigin);
            ArticleSearchResponse articlesResponse = await SearchArticles(new ArticleSearchRequest
            {
                Language = keywordsResponse.Language,
                Keywords = keywordsResponse.Keywords
            });

            StanceDetectionRequest requestObject = new StanceDetectionRequest
            {
                Headline = string.Join(' ', keywordsResponse.Keywords),
                Bodies = articlesResponse.Articles
                    .GroupBy(x => x.Link)
                    .Select(x => x.First())
                    .ToDictionary(x => x.Link, x => x.Summary)
            };

            IEnumerable<KeyValuePair<string, Stance>> result = await GetStances(requestObject);

            return result;
        }

        private async Task<IEnumerable<KeyValuePair<string, Stance>>> GetStances(StanceDetectionRequest requestObj)
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(new Uri(config.StanceDetectionAiUrl), "predict"),
                Content = new StringContent(JsonConvert.SerializeObject(requestObj), Encoding.UTF8, "application/json"),
            };

            using HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string body = await response.Content.ReadAsStringAsync();
            Dictionary<string, Stance> responseObj = JsonConvert.DeserializeObject<Dictionary<string, Stance>>(body);

            return responseObj;
        }

        private async Task<KeywordsExtractionResponse> GetKeywords(string textOrigin)
        {
            KeywordsExtractionRequest requestObj = new KeywordsExtractionRequest { TextOrigin = textOrigin };
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(new Uri(config.WebScraperUrl), "webscraper/extract-keywords"),
                Content = new StringContent(JsonConvert.SerializeObject(requestObj), Encoding.UTF8, "application/json"),
            };

            using HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string body = await response.Content.ReadAsStringAsync();
            KeywordsExtractionResponse responseObj = JsonConvert.DeserializeObject<KeywordsExtractionResponse>(body);

            return responseObj;
        }

        private async Task<ArticleSearchResponse> SearchArticles(ArticleSearchRequest requestObj)
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(new Uri(config.WebScraperUrl), "webscraper/search-articles"),
                Content = new StringContent(JsonConvert.SerializeObject(requestObj), Encoding.UTF8, "application/json"),
            };

            using HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string body = await response.Content.ReadAsStringAsync();
            ArticleSearchResponse responseObj = JsonConvert.DeserializeObject<ArticleSearchResponse>(body);

            return responseObj;
        }
    }
}
