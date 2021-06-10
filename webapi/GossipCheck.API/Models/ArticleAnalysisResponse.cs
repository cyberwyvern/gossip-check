using GossipCheck.BLL.Models;
using System.Collections.Generic;

namespace GossipCheck.API.Models
{
    public class ArticleAnalysisResponse
    {
        public Verdict Verdict { get; set; }

        public IEnumerable<RelatedArticleResponseModel> RelatedArticles { get; set; }
    }
}