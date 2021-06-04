namespace GossipCheck.WebScraper.Services.Models
{
    [RegexMapper(@"Detailed\sReport.*?<p>.+?<\/p>")]
    public class MbfcReport
    {
        public string Source { get; set; }

        [RegexMapper(@"Bias\sRating:.*?strong>(.+?)<", Group = 1)]
        public string BiasRating { get; set; }

        [RegexMapper(@"Factual\sReporting:.*?strong>(.+?)<", Group = 1)]
        public string FactualReporting { get; set; }

        [RegexMapper(@"Country:.*?strong>(.+?)\s", Group = 1)]
        public string Country { get; set; }

        [RegexMapper(@"(\d+\/\d+)\sPress\sFreedom", Group = 1)]
        [RegexMapper(@"World\sPress\sFreedom\sRank:.*?(\d+\/\d+)", Group = 1)]
        public string WorldPressFreedomRank { get; set; }

        [RegexMapper(@"Media\sType:.*?strong>(.+?)<", Group = 1)]
        public string MediaType { get; set; }

        [RegexMapper(@"Traffic\/Popularity:.*?strong>(.+?)<", Group = 1)]
        public string TrafficPopularity { get; set; }

        [RegexMapper(@"MBFC\sCredibility\sRating:.*?strong>(.+?)<", Group = 1)]
        public string MBFCCredibilityRating { get; set; }

        public string PageUrl { get; set; }
    }
}