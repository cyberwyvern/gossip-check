using GossipCheck.WebScraper.ConfigurationOptionModels;
using IBM.Cloud.SDK.Core.Authentication.Iam;
using IBM.Watson.NaturalLanguageUnderstanding.v1;
using IBM.Watson.NaturalLanguageUnderstanding.v1.Model;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GossipCheck.WebScraper.Services
{
    public class NLUService : INLUService
    {
        private const string ApiVersion = "2019-07-12";
        private const int KeyWordsLimit = 8;

        private readonly NLUServiceConfig config;
        private readonly NaturalLanguageUnderstandingService watson;

        public NLUService(IOptions<NLUServiceConfig> config)
        {
            this.config = config.Value;

            var authenticator = new IamAuthenticator(this.config.ApiKey);
            watson = new NaturalLanguageUnderstandingService(ApiVersion, authenticator);
            watson.SetServiceUrl(this.config.ServiceUrl);
        }

        public (Language, IEnumerable<string>) ExtractKeywords(string origin)
        {
            var features = new Features
            {
                Keywords = new KeywordsOptions
                {
                    Emotion = false,
                    Sentiment = false,
                    Limit = KeyWordsLimit
                }
            };

            var result = Util.IsUrl(origin)
                ? watson.Analyze(features, url: origin)
                : watson.Analyze(features, text: origin);

            if (LanguageCodes.Codes.ContainsValue(result.Result.Language))
            {
                var language = LanguageCodes.Codes.First(x => x.Value == result.Result.Language).Key;
                var keywords = result.Result.Keywords.Select(x => x.Text).ToArray();

                return (language, keywords);
            }
            else
            {
                throw new Exception("Unsupported language");
            }
        }
    }
}