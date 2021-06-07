using GossipCheck.API.Models;
using GossipCheck.BLL.Models;
using GossipCheck.DAO.Entities;

namespace GossipCheck.API.Extensions
{
    public static class RelatedArticleReportMapping
    {
        public static RelatedArticleReport ToRelatedArticle(this MbfcReport mbfcReport, string articleUrl, Stance stance)
        {
            return new RelatedArticleReport
            {
                ArticleUrl = articleUrl,
                Stance = stance.ToString(),
                Factuality = mbfcReport.FactualReporting.ToString(),
                Bias = mbfcReport.BiasRating.ToString(),
                MediaType = mbfcReport.MediaType.ToString(),
                Popularity = mbfcReport.TrafficPopularity.ToString(),
                MbfcCredibilityRating = mbfcReport.MbfcCredibilityRating.ToString(),
                Reasoning = mbfcReport.Reasoning,
                Country = mbfcReport.Country,
                WorldPressFreedomRank = mbfcReport.WorldPressFreedomRank,
                MbfcPageUrl = mbfcReport.PageUrl
            };
        }
    }
}
