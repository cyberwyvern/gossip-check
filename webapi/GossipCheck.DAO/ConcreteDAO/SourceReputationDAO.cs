using GossipCheck.DAO.Entities;
using GossipCheck.DAO.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GossipCheck.DAO.ConcreteDAO
{
    internal sealed class SourceReputationDAO : DataAccessObject<SourceReputation, int>, ISourceReputationDAO
    {
        public SourceReputationDAO(DbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<SourceReputation> GetLatestByUrls(IEnumerable<string> urls)
        {
            return this.table
                .Where(x => urls.Contains(x.BaseUrl))
                .AsEnumerable()
                .GroupBy(x => x.BaseUrl)
                .Select(x => x.OrderByDescending(x => x.Date).First())
                .ToList();
        }
    }
}
