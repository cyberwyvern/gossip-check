using GossipCheck.BLL.Models;
using GossipCheck.DAO.Entities;

namespace GossipCheck.BLL.Extensions
{
    public static class RelatedArticleReportMapping
    {
        public static RelatedArticleReport ToRelatedArticleReport(this MbfcReport report, string atricleUrl, Stance stance)
        {
            return new RelatedArticleReport
            {
                ArticleUrl = atricleUrl,
                Stance = stance,
                MediaType = report.MediaType,
                Factuality = report.FactualReporting,
                Bias = report.BiasRating,
                Popularity = report.TrafficPopularity,
                MbfcCredibilityRating = report.MbfcCredibilityRating,
                Reasoning = report.Reasoning,
                Country = report.Country,
                WorldPressFreedomRank = report.WorldPressFreedomRank,
                MbfcPageUrl = report.PageUrl
            };
        }
    }
}
