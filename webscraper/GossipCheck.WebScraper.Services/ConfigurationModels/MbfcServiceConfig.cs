namespace GossipCheck.WebScraper.Services.ConfigurationOptionModels
{
    public class MbfcServiceConfig
    {
        public string ServiceUrl { get; set; }

        public int Attempts { get; set; }

        public int RetryInterval { get; set; }

        public int SearchVisits { get; set; }
    }
}