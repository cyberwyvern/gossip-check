using GossipCheck.DAO.Entities;

namespace GossipCheck.BLL.Models
{
    public class RelatedArticleReport
    {
        public string ArticleUrl { get; set; }

        public Stance Stance { get; set; }

        public MediaType MediaType { get; set; }

        public FactualReporting Factuality { get; set; }

        public BiasRating Bias { get; set; }

        public TrafficPopularity Popularity { get; set; }

        public MbfcCredibilityRating MbfcCredibilityRating { get; set; }

        public string Reasoning { get; set; }

        public string Country { get; set; }

        public int? WorldPressFreedomRank { get; set; }

        public string MbfcPageUrl { get; set; }
    }
}