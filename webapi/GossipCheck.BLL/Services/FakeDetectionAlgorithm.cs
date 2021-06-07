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

        private readonly Dictionary<FactualReporting, double> reputationScores = new Dictionary<FactualReporting, double>
        {
            [FactualReporting.VeryHigh] = 1,
            [FactualReporting.High] = .8,
            [FactualReporting.MostlyFactual] = .6,
            [FactualReporting.Mixed] = .4,
            [FactualReporting.Low] = .2,
            [FactualReporting.VeryLow] = 0,
        };

        private readonly Dictionary<Verdict, double> verdictMinimalScores = new Dictionary<Verdict, double>
        {
            [Verdict.MostLikelyFake] = 0,
            [Verdict.LikelyFake] = .2,
            [Verdict.HardToSay] = .4,
            [Verdict.LikelyTrue] = .6,
            [Verdict.MostLikelyTrue] = .8,
        };

        public Verdict GetVerdict(IEnumerable<KeyValuePair<MbfcReport, Stance>> reportStances)
        {
            var significantEntries = reportStances
                .Where(x => x.Key.FactualReporting != FactualReporting.NA)
                .Where(x => x.Value != Stance.Unrelated)
                .ToList();

            if (significantEntries.Count < 3)
            {
                return Verdict.UnableToDetermine;
            }

            var score = significantEntries.Select(x => this.ExtractReputationFromReport(x.Key))
                .Softmax()
                .Zip(significantEntries, (reputation, reportStance) => new { Stance = reportStance.Value, Reputation = reputation })
                .Select(x => x.Reputation * this.stanceFactor[x.Stance])
                .Sum() + 1 / 2;

            foreach (var kv in this.verdictMinimalScores.OrderByDescending(x => x.Value))
            {
                if (kv.Value <= score)
                {
                    return kv.Key;
                }
            }

            return Verdict.UnableToDetermine;
        }

        private double ExtractReputationFromReport(MbfcReport report)
        {
            return this.reputationScores[report.FactualReporting];
        }
    }
}
