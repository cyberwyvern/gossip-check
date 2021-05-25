using System;

namespace GossipCheck.WebScraper.Models
{
    public class ScraperArticleModel
    {
        public string _id { get; set; }
        
        public double _score { get; set; }
        
        public string author { get; set; }
        
        public string clean_url { get; set; }
        
        public string country { get; set; }
        
        public string language { get; set; }
        
        public string link { get; set; }
        
        public DateTime published_date { get; set; }
        
        public int rank { get; set; }
        
        public string rights { get; set; }
        
        public string summary { get; set; }
        
        public string title { get; set; }
        
        public string topic { get; set; }
    }
}