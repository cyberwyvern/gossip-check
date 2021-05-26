using GossipCheck.DAO.Entities;
using System.Collections.Generic;

namespace GossipCheck.DAO.Interface
{
    public interface ISourceReputationDAO : IDataAccessObject<SourceReputation, int>
    {

        public IEnumerable<SourceReputation> GetLatestByUrls(IEnumerable<string> urls);
    }
}
