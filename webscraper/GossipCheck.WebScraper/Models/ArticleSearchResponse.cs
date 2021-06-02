using GossipCheck.WebScraper.Services.Models;
using System.Collections.Generic;

namespace GossipCheck.WebScraper.Models
{
    public class ArticleSearchResponse
    {
        public IEnumerable<Article> Articles { get; set; }
    }
}