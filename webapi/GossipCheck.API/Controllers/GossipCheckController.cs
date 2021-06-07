using GossipCheck.API.Models;
using GossipCheck.BLL.Interface;
using GossipCheck.BLL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
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
            var stances = await this.stanceDetector.GetSourceStances(request.TextOrigin);
            var score = await this.reputabilityAlgorithm.GetScore(stances.Where(x => x.Value != Stance.Unrelated));

            return this.Ok(new ArticleVerificationResponse
            {
                Score = score,
                SourceStances = stances
            });
        }
    }
}
