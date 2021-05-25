using GossipCheck.WebScraper.Models;
using GossipCheck.WebScraper.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace GossipCheck.WebScraper.Controllers
{
    [ApiController]
    [Route("webscraper")]
    public class WebScraperController : ControllerBase
    {
        private readonly INLUService nLUService;
        private readonly IWebScraperService webScraper;

        public WebScraperController(INLUService nLUService, IWebScraperService webScraper)
        {
            this.nLUService = nLUService;
            this.webScraper = webScraper;
        }

        [HttpPost("extract-keywords")]
        public IActionResult ExtractKeywords(KeywordsExtractionRequest request)
        {
            var (language, keywords) = this.nLUService.ExtractKeywords(request.TextOrigin);

            return Ok(new KeywordsExtractionResponse
            {
                Language = language,
                Keywords = keywords.ToArray()
            });
        }

        [HttpPost("search-articles")]
        public async Task<IActionResult> SearchArticles(ArticleSearchRequest request)
        {
            var articles = await this.webScraper.SearchArticles(request.Language, request.Keywords);

            return Ok(new ArticleSearchResponse
            {
                Articles = articles
            });
        }
    }
}