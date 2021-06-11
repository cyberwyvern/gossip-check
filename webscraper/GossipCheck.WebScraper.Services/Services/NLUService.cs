using GossipCheck.WebScraper.Services.ConfigurationOptionModels;
using IBM.Cloud.SDK.Core.Authentication.Iam;
using IBM.Watson.NaturalLanguageUnderstanding.v1;
using IBM.Watson.NaturalLanguageUnderstanding.v1.Model;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace GossipCheck.WebScraper.Services.Services
{
    public class NLUService : INLUService
    {
        private const string ApiVersion = "2019-07-12";

        private readonly NLUServiceConfig config;
        private readonly NaturalLanguageUnderstandingService watson;

        public NLUService(IOptions<NLUServiceConfig> config)
        {
            this.config = config.Value;

            var authenticator = new IamAuthenticator(this.config.ApiKey);
            this.watson = new NaturalLanguageUnderstandingService(ApiVersion, authenticator);
            this.watson.SetServiceUrl(this.config.ServiceUrl);
        }

        public IEnumerable<string> ExtractKeywords(string textOrUrl, int limit, out Language language)
        {
            var features = new Features
            {
                Keywords = new KeywordsOptions
                {
                    Emotion = false,
                    Sentiment = false,
                    Limit = limit
                }
            };

            var result = textOrUrl.IsWebUrl()
                ? this.watson.Analyze(features, url: textOrUrl)
                : this.watson.Analyze(features, text: textOrUrl);

            if (LanguageCodes.Codes.ContainsValue(result.Result.Language))
            {
                language = LanguageCodes.Codes.First(x => x.Value == result.Result.Language).Key;

                var totalWords = Regex.Split(textOrUrl, @"\s+");
                var keywords = textOrUrl.IsWebUrl() || totalWords.Length >= limit
                    ? result.Result.Keywords.Select(x => x.Text).ToArray()
                    : totalWords;

                return keywords;
            }
            else
            {
                throw new Exception("Unsupported language");
            }
        }
    }
}