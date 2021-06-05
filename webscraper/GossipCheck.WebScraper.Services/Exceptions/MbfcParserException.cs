using System;

namespace GossipCheck.WebScraper.Services.Exceptions
{
    public class MbfcParserException : Exception
    {
        public MbfcParserException()
        {
        }

        public MbfcParserException(string message) : base(message)
        {
        }
    }
}
