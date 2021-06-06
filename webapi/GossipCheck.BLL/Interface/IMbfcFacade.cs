using GossipCheck.DAO.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GossipCheck.BLL.Interface
{
    public interface IMbfcFacade
    {
        Task<IEnumerable<MbfcReport>> GetReportsAsync(IEnumerable<string> sourceUrls);
    }
}