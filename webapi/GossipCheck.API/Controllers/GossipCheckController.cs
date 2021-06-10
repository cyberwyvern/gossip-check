using GossipCheck.API.Extensions;
using GossipCheck.API.Models;
using GossipCheck.BLL.Exceptions;
using GossipCheck.BLL.Extensions;
using GossipCheck.BLL.Interface;
using GossipCheck.BLL.Models;
using GossipCheck.DAO.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GossipCheck.API.Controllers
{
    [Route("gossip-check")]
    [ApiController]
    public class GossipCheckController : ControllerBase
    {
        private readonly IStanceDetectionService stanceDetector;
        private readonly IFakeDetectionAlgorithm fakeDetectionAlgorithm;
        private readonly IMbfcReportingService mbfcReporting;

        public GossipCheckController(
            IStanceDetectionService stanceDetector,
            IFakeDetectionAlgorithm reputabilityAlgorithm,
            IMbfcReportingService mbfcReporting)
        {
            this.stanceDetector = stanceDetector;
            this.fakeDetectionAlgorithm = reputabilityAlgorithm;
            this.mbfcReporting = mbfcReporting;
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify(ArticleAnalysisRequest request)
        {
            var articleStances = (await this.stanceDetector.GetArticleStances(request.TextOrigin));
            var sourceUrls = articleStances.Select(x => x.Key.ToAuthorityUrl());
            var reports = (await this.mbfcReporting.GetReportsAsync(sourceUrls)).Join(
                articleStances,
                x => x.Source,
                x => x.Key.ToAuthorityUrl(),
                (x, y) => x.ToRelatedArticleReport(y.Key, y.Value))
                .Where(x => x.Factuality != FactualReporting.NA)
                .Where(x => x.Stance != Stance.Unrelated)
                .ToList();

            Verdict verdict;
            try
            {
                verdict = this.fakeDetectionAlgorithm.GetVerdict(reports);
            }
            catch (InsufficientDataAmountException)
            {
                verdict = Verdict.CouldNotDetermine;
            }

            return this.Ok(new ArticleAnalysisResponse
            {
                Verdict = verdict,
                RelatedArticles = reports.Select(x => x.ToResponseModel())
            });
        }
    }
}
