using GossipCheck.WebScraper.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GossipCheck.WebScraper.Services.Services
{
    public interface IWebScraperService
    {
        Task<IEnumerable<Article>> SearchArticles(Language? language, IEnumerable<string> keywords);
    }
}