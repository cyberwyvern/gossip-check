using GossipCheck.WebScraper.Services;
using System.Collections.Generic;

namespace GossipCheck.WebScraper.Models
{
    public class ArticleSearchRequest
    {
        public Language? Language { get; set; }

        public ICollection<string> Keywords { get; set; }
    }
}