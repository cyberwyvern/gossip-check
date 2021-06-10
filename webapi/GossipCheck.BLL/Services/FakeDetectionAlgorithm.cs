using GossipCheck.BLL.Exceptions;
using GossipCheck.BLL.Extensions;
using GossipCheck.BLL.Interface;
using GossipCheck.BLL.Models;
using GossipCheck.DAO.Entities;
using System.Collections.Generic;
using System.Linq;

namespace GossipCheck.BLL.Services
{
    public class FakeDetectionAlgorithm : IFakeDetectionAlgorithm
    {
        private readonly Dictionary<Stance, double> stanceFactor = new Dictionary<Stance, double>
        {
            [Stance.Agree] = 1,
            [Stance.Disagree] = -1,
            [Stance.Unrelated] = 0,
            [Stance.Discuss] = .25
        };

        //sigmoid: 10/(1+e^-((x-2.5)))
        //x: 0, 1, 2, 3, 4, 5
        private readonly Dictionary<FactualReporting, double> reputationScores = new Dictionary<FactualReporting, double>
        {
            [FactualReporting.VeryHigh] = 9.241,
            [FactualReporting.High] = 8.176,
            [FactualReporting.MostlyFactual] = 6.225,
            [FactualReporting.Mixed] = 3.775,
            [FactualReporting.Low] = 1.824,
            [FactualReporting.VeryLow] = .759,
        };

        //sigmoid: 1/(1+e^-((x-1.5)))
        //x: 0, 1, 2, 3
        private readonly Dictionary<Verdict, double> verdictMinimalScores = new Dictionary<Verdict, double>
        {
            [Verdict.MostLikelyFake] = 0,
            [Verdict.LikelyFake] = .1824,
            [Verdict.Questionable] = .3775,
            [Verdict.LikelyTrue] = .6225,
            [Verdict.MostLikelyTrue] = .8176,
        };

        public Verdict GetVerdict(IEnumerable<RelatedArticleReport> relatedArticles)
        {
            var significantEntries = relatedArticles
                .Where(x => x.Factuality != FactualReporting.NA)
                .Where(x => x.Stance != Stance.Unrelated)
                .ToList();

            var highFactualityScore = this.reputationScores[FactualReporting.High];
            if (significantEntries.Count(x => this.reputationScores[x.Factuality] >= highFactualityScore) < 3)
            {
                throw new InsufficientDataAmountException();
            }

            var score = significantEntries.Select(x => this.reputationScores[x.Factuality])
                .Softmax()
                .Zip(significantEntries, (reputation, report) => reputation * this.stanceFactor[report.Stance])
                .Sum() + 1 / 2;

            return verdictMinimalScores
                .OrderByDescending(x => x.Value)
                .First(kv => kv.Value <= score)
                .Key;
        }
    }
}
