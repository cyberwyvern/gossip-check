using GossipCheck.BLL.Interface;
using GossipCheck.DAO.Interface;
using System;
using System.Collections.Generic;

namespace GossipCheck.BLL
{
    public class ReputabilityAlgorithm : IReputabilityAlgorithm
    {
        private readonly IGossipCheckUnitOfWork uow;

        private readonly Dictionary<Stance, double> stanceFactor = new Dictionary<Stance, double>
        {
            [Stance.Agree] = 1,
            [Stance.Disagree] = -1,
            [Stance.Unrelated] = 0,
            [Stance.Discuss] = .25
        };

        public ReputabilityAlgorithm(IGossipCheckUnitOfWork uow)
        {
            this.uow = uow;
        }

        public double GetScore(IEnumerable<KeyValuePair<string, Stance>> sourceStances)
        {
            return 0;
        }

        private string GetBaseUrl(string url) => new Uri(url).GetLeftPart(UriPartial.Authority).ToLower();
    }
}
