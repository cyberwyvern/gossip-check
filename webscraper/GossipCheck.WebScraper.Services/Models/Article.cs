using System;

namespace GossipCheck.WebScraper.Services.Models
{
    public class Article
    {
        public string Link { get; set; }

        public DateTime PublishedDate { get; set; }

        public string Summary { get; set; }

        public string Title { get; set; }
    }
}