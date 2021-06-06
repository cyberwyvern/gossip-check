using GossipCheck.BLL.Models;
using System.Collections.Generic;

namespace GossipCheck.API.Models
{
    public class ArticleVerificationResponse
    {
        public double Score { get; set; }

        public IEnumerable<KeyValuePair<string, Stance>> SourceStances { get; set; }
    }
}