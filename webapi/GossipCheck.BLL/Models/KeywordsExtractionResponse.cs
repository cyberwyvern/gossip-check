using System.Collections.Generic;

namespace GossipCheck.BLL.Models
{
    internal class KeywordsExtractionResponse
    {
        public Language Language { get; set; }

        public ICollection<string> Keywords { get; set; }
    }
}
