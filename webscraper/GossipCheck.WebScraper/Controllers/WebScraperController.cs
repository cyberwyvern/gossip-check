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
        private readonly IWebScraperService webScraper;
        private readonly IMbfcCrawler mbfcCrawler;

        public WebScraperController(INLUService nLUService, IWebScraperService webScraper, IMbfcCrawler mbfcCrawler)
        {
            this.nLUService = nLUService;
            this.webScraper = webScraper;
            this.mbfcCrawler = mbfcCrawler;
        }

        [HttpPost("extract-keywords")]
        public IActionResult ExtractKeywords(KeywordsExtractionRequest request)
        {
            (Services.Language language, System.Collections.Generic.IEnumerable<string> keywords) = nLUService.ExtractKeywords(request.TextOrigin);

            return Ok(new KeywordsExtractionResponse
            {
                Language = language,
                Keywords = keywords.ToArray()
            });
        }

        [HttpPost("search-articles")]
        public async Task<IActionResult> SearchArticles(ArticleSearchRequest request)
        {
            System.Collections.Generic.IEnumerable<Services.Models.Article> articles = await webScraper.SearchArticles(request.Language, request.Keywords);

            return Ok(new ArticleSearchResponse
            {
                Articles = articles
            });
        }

        [HttpGet("mbfc-report")]
        public async Task<IActionResult> GetMbfcReport([FromQuery] string sourceUrl)
        {
            var result = await mbfcCrawler.GetMbfcReport(sourceUrl);

            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }
    }
}