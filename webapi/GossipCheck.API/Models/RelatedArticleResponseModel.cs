namespace GossipCheck.API.Models
{
    public class RelatedArticleResponseModel
    {
        public string ArticleUrl { get; set; }

        public string Stance { get; set; }

        public string Factuality { get; set; }

        public string Bias { get; set; }

        public string MediaType { get; set; }

        public string Popularity { get; set; }

        public string MbfcCredibilityRating { get; set; }

        public string Reasoning { get; set; }

        public string Country { get; set; }

        public int? WorldPressFreedomRank { get; set; }

        public string MbfcPageUrl { get; set; }
    }
}
