using System.Collections.Generic;

namespace GossipCheck.WebScraper.Services.Services
{
    public interface INLUService
    {
        (Language, IEnumerable<string>) ExtractKeywords(string origin);
    }
}