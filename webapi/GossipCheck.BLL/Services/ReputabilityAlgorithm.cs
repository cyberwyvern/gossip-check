using GossipCheck.BLL.Extensions;
using GossipCheck.BLL.Interface;
using GossipCheck.BLL.Models;
using GossipCheck.DAO.Entities;
using GossipCheck.DAO.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GossipCheck.BLL.Services
{
    public class ReputabilityAlgorithm : IReputabilityAlgorithm
    {
        private readonly IMbfcFacade mbfcFacade;

        private readonly Dictionary<Stance, double> stanceFactor = new Dictionary<Stance, double>
        {
            [Stance.Agree] = 1,
            [Stance.Disagree] = -1,
            [Stance.Unrelated] = 0,
            [Stance.Discuss] = .25
        };

        private readonly Dictionary<FactualReporting, double> factualityScores = new Dictionary<FactualReporting, double>
        {
            [FactualReporting.VeryHigh] = .9,
            [FactualReporting.High] = .8,
            [FactualReporting.MostlyFactual] = .7,
            [FactualReporting.Mixed] = .5,
            [FactualReporting.Low] = .3,
            [FactualReporting.VeryLow] = .1,
        };

        public ReputabilityAlgorithm(IGossipCheckUnitOfWork uow, IMbfcFacade mbfcFacade)
        {
            this.mbfcFacade = mbfcFacade;
        }

        public async Task<double> GetScore(IEnumerable<KeyValuePair<string, Stance>> sourceStances)
        {
            var reports = (await mbfcFacade.GetReportsAsync(sourceStances.Select(x => x.Key)))
                .Where(x => x.FactualReporting != FactualReporting.NA)
                .GroupBy(x => x.Source)
                .Select(x => x.First())
                .ToArray();

            var softmaxedReputations = reports.Select(CalculateReputation).Softmax().ToArray();
            var reputations = new Dictionary<string, double>();
            for (int i = 0; i < reports.Length; i++)
            {
                reputations.Add(reports[i].Source, softmaxedReputations[i]);
            }

            return sourceStances
                .Select(x => reputations.GetValueOrDefault(GetBaseUrl(x.Key)) * stanceFactor[x.Value])
                .Sum() + 1 / 2;
        }

        private string GetBaseUrl(string url)
        {
            return new Uri(url).GetLeftPart(UriPartial.Authority).ToLower();
        }

        private double CalculateReputation(MbfcReport report)
        {
            return factualityScores[report.FactualReporting];
        }
    }
}
