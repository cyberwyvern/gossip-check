using GossipCheck.WebScraper.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GossipCheck.WebScraper.Services.Services
{
    public interface IArticleSearchEngine
    {
        Task<IEnumerable<Article>> SearchArticles(IEnumerable<string> keywords, Language? language);
    }
}