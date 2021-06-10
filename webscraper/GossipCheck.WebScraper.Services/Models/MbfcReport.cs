namespace GossipCheck.WebScraper.Services.Models
{
    public class MbfcReport
    {
        public string Source { get; set; }

        public string PageUrl { get; set; }

        public FactualReporting? FactualReporting { get; set; }

        public BiasRating? BiasRating { get; set; }

        public MediaType? MediaType { get; set; }

        public TrafficPopularity? TrafficPopularity { get; set; }

        public MbfcCredibilityRating? MbfcCredibilityRating { get; set; }

        public string Reasoning { get; set; }

        public string Country { get; set; }

        public int? WorldPressFreedomRank { get; set; }
    }
}