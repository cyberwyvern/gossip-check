using GossipCheck.BLL.Models;
using GossipCheck.DAO.Entities;
using System.Collections.Generic;

namespace GossipCheck.BLL.Interface
{
    public interface IFakeDetectionAlgorithm
    {
        Verdict GetVerdict(IEnumerable<KeyValuePair<MbfcReport, Stance>> reportStances);
    }
}
