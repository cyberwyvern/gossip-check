using System;

namespace GossipCheck.BLL.Models
{
    public class Article
    {
        public string Link { get; set; }

        public DateTime PublishedDate { get; set; }

        public string Summary { get; set; }

        public string Title { get; set; }
    }
}