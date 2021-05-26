using System.Collections.Generic;

namespace GossipCheck.BLL.Models
{
    public class ArticleSearchResponse
    {
        public IEnumerable<Article> Articles { get; set; }
    }
}