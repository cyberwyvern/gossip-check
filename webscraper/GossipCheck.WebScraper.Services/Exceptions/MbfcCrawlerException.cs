using System;

namespace GossipCheck.WebScraper.Services.Exceptions
{
    public class MbfcCrawlerException : Exception
    {
        public MbfcCrawlerException()
        {
        }

        public MbfcCrawlerException(string message) : base(message)
        {
        }

        public MbfcCrawlerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
