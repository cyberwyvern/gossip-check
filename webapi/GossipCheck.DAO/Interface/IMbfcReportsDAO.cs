using GossipCheck.DAO.Entities;
using System.Collections.Generic;

namespace GossipCheck.DAO.Interface
{
    public interface IMbfcReportsDAO : IDataAccessObject<MbfcReport, int>
    {

        public IEnumerable<MbfcReport> GetLatestByUrls(IEnumerable<string> urls);
    }
}
