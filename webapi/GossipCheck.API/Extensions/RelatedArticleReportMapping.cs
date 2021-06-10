using GossipCheck.API.Models;
using GossipCheck.BLL.Extensions;
using GossipCheck.BLL.Models;
using GossipCheck.DAO.Entities;

namespace GossipCheck.API.Extensions
{
    public static class RelatedArticleReportMapping
    {
        public static RelatedArticleResponseModel ToResponseModel(this RelatedArticleReport report)
        {
            return new RelatedArticleResponseModel
            {
                ArticleUrl = report.ArticleUrl,
                Stance = report.Stance.ToSentenceCase(),
                Factuality = report.Factuality != FactualReporting.NA ? report.Factuality.ToSentenceCase() : default,
                Bias = report.Bias != BiasRating.NA ? report.Bias.ToSentenceCase() : default,
                MediaType = report.MediaType != MediaType.NA ? report.MediaType.ToSentenceCase() : default,
                Popularity = report.Popularity != TrafficPopularity.NA ? report.Popularity.ToSentenceCase() : default,
                MbfcCredibilityRating = report.MbfcCredibilityRating != MbfcCredibilityRating.NA ? report.MbfcCredibilityRating.ToSentenceCase() : default,
                Reasoning = report.Reasoning,
                Country = report.Country,
                WorldPressFreedomRank = report.WorldPressFreedomRank,
                MbfcPageUrl = report.MbfcPageUrl
            };
        }
    }
}
