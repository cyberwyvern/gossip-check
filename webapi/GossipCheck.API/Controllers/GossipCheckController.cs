using GossipCheck.API.Extensions;
using GossipCheck.API.Models;
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
            var sourceStances = await this.stanceDetector.GetSourceStances(request.TextOrigin);
            var reports = await this.mbfcReporting.GetReportsAsync(sourceStances.Select(x => x.Key.ToAuthorityUrl()));
            var reportStances = reports.Join(
                sourceStances,
                x => x.Source.ToAuthorityUrl(),
                x => x.Key.ToAuthorityUrl(),
                (x, y) => new KeyValuePair<MbfcReport, Stance>(x, y.Value))
                .ToList();

            var verdict = this.fakeDetectionAlgorithm.GetVerdict(reportStances);

            var relatedArticles = reportStances.Join(
                sourceStances,
                x => x.Key.Source.ToAuthorityUrl(),
                x => x.Key.ToAuthorityUrl(),
                (x, y) => x.Key.ToRelatedArticle(y.Key, x.Value))
                .OrderBy(x => x.Stance)
                .ToList();


            return this.Ok(new ArticleAnalysisResponse
            {
                Verdict = verdict,
                RelatedArticles = relatedArticles
            });
        }
    }
}
