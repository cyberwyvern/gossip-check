using GossipCheck.BLL.Models;
using System.Collections.Generic;

namespace GossipCheck.BLL.Interface
{
    public interface IFakeDetectionAlgorithm
    {
        Verdict GetVerdict(IEnumerable<RelatedArticleReport> relatedArticles);
    }
}
