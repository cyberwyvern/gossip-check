using GossipCheck.API.Models;
using GossipCheck.BLL.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GossipCheck.API.Controllers
{
    [Route("gossip-check")]
    [ApiController]
    public class GossipCheckController : ControllerBase
    {
        private readonly IStanceDetectorFacade stanceDetector;
        private readonly IReputabilityAlgorithm reputabilityAlgorithm;

        public GossipCheckController(IStanceDetectorFacade stanceDetector, IReputabilityAlgorithm reputabilityAlgorithm)
        {
            this.stanceDetector = stanceDetector;
            this.reputabilityAlgorithm = reputabilityAlgorithm;
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify(ArticleVerificationRequest request)
        {
            System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, BLL.Models.Stance>> stances = await stanceDetector.GetSourceStances(request.TextOrigin);
            double score = await reputabilityAlgorithm.GetScore(stances);

            return Ok(new ArticleVerificationResponse
            {
                Score = score,
                SourceStances = stances
            });
        }
    }
}
