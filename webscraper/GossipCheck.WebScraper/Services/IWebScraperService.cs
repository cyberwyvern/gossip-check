using GossipCheck.WebScraper.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GossipCheck.WebScraper.Services
{
    public interface IWebScraperService
    {
        Task<IEnumerable<Article>> SearchArticles(Language? language, IEnumerable<string> keywords);
    }
}