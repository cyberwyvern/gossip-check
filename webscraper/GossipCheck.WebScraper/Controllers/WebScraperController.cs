using GossipCheck.WebScraper.Models;
using GossipCheck.WebScraper.Services.Services;
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
        private readonly IArticleSearchEngine webScraper;
        private readonly IMbfcCrawler mbfcCrawler;

        public WebScraperController(INLUService nLUService, IArticleSearchEngine webScraper, IMbfcCrawler mbfcCrawler)
        {
            this.nLUService = nLUService;
            this.webScraper = webScraper;
            this.mbfcCrawler = mbfcCrawler;
        }

        [HttpPost("extract-keywords")]
        public IActionResult ExtractKeywords(KeywordsExtractionRequest request)
        {
            var keywords = this.nLUService.ExtractKeywords(request.TextOrigin, request.Limit, out var language);

            return this.Ok(new KeywordsExtractionResponse
            {
                Language = language,
                Keywords = keywords.ToArray()
            });
        }

        [HttpPost("search-articles")]
        public async Task<IActionResult> SearchArticles(ArticleSearchRequest request)
        {
            var articles = await this.webScraper.SearchArticles(request.Keywords, request.Language);

            return this.Ok(new ArticleSearchResponse
            {
                Articles = articles
            });
        }

        [HttpGet("mbfc-report")]
        public async Task<IActionResult> GetMbfcReport([FromQuery] string sourceUrl)
        {
            var result = await this.mbfcCrawler.GetMbfcReport(sourceUrl);

            if (result == null)
            {
                return this.NotFound();
            }
            else
            {
                return this.Ok(result);
            }
        }
    }
}