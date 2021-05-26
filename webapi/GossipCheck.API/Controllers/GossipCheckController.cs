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
        private readonly IStanceDetectorFascade stanceDetector;
        private readonly IReputabilityAlgorithm reputabilityAlgorithm;

        public GossipCheckController(IStanceDetectorFascade stanceDetector, IReputabilityAlgorithm reputabilityAlgorithm)
        {
            this.stanceDetector = stanceDetector;
            this.reputabilityAlgorithm = reputabilityAlgorithm;
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify(ArticleVerificationRequest request)
        {
            var stances = await this.stanceDetector.GetSourceStances(request.TextOrigin);
            var score = this.reputabilityAlgorithm.GetScore(stances);

            return Ok(new ArticleVerificationResponse
            {
                Score = score,
                SourceStances = stances
            });
        }
    }
}
