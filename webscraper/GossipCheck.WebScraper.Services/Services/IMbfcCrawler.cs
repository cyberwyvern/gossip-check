using GossipCheck.WebScraper.Services.Models;
using System.Threading.Tasks;

namespace GossipCheck.WebScraper.Services.Services
{
    public interface IMbfcCrawler
    {
        Task<MbfcReport> GetMbfcReport(string url);
    }
}