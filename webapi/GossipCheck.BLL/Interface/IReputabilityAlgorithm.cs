using GossipCheck.BLL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GossipCheck.BLL.Interface
{
    public interface IReputabilityAlgorithm
    {
        Task<double> GetScore(IEnumerable<KeyValuePair<string, Stance>> sourceStances);
    }
}
