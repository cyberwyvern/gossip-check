using System.Collections.Generic;

namespace GossipCheck.BLL.Models
{
    public class ArticleSearchRequest
    {
        public Language? Language { get; set; }

        public ICollection<string> Keywords { get; set; }
    }
}