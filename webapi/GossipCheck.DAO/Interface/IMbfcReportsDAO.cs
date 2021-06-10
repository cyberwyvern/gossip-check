using GossipCheck.DAO.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GossipCheck.DAO.Interface
{
    public interface IMbfcReportsDAO : IDataAccessObject<MbfcReport, int>
    {
        Task<IEnumerable<MbfcReport>> GetLatestByUrlsAsync(IEnumerable<string> urls);
    }
}
