using System.Collections.Generic;

namespace GossipCheck.BLL.Models
{
    internal class StanceDetectionRequest
    {
        public string Headline { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Bodies { get; set; }
    }
}
