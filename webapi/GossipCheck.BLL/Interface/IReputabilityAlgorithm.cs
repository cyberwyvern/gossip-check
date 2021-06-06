using GossipCheck.BLL.Models;
using System.Collections.Generic;

namespace GossipCheck.BLL.Interface
{
    public interface IReputabilityAlgorithm
    {
        double GetScore(IEnumerable<KeyValuePair<string, Stance>> sourceStances);
    }
}
