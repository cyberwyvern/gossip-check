namespace GossipCheck.WebScraper.Services.ConfigurationOptionModels
{
    public class MbfcServiceConfig
    {
        public string ServiceUrl { get; set; }

        public int RetryCount { get; set; }

        public int RetryInterval { get; set; }
    }
}