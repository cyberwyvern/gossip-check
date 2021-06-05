using System.Collections.Generic;
using System.Threading.Tasks;

namespace GossipCheck.WebScraper.Services.Services
{
    public interface IMbfcCrawler
    {
        Task<Dictionary<string, string>> GetMbfcReport(string url);
    }
}