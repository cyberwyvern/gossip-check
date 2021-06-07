using System.Collections.Generic;

namespace GossipCheck.WebScraper.Services.Services
{
    public interface INLUService
    {
        IEnumerable<string> ExtractKeywords(string textOrUrl, out Language language);
    }
}