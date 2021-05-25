using System.Collections.Generic;

namespace GossipCheck.WebScraper.Services
{
    public interface INLUService
    {
        (Language, IEnumerable<string>) ExtractKeywords(string origin);
    }
}