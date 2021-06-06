using GossipCheck.DAO.Entities;
using GossipCheck.DAO.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GossipCheck.DAO.ConcreteDAO
{
    internal sealed class MbfcReportsDAO : DataAccessObject<MbfcReport, int>, IMbfcReportsDAO
    {
        public MbfcReportsDAO(DbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<MbfcReport>> GetLatestByUrlsAsync(IEnumerable<string> urls)
        {
            return await table
                .Where(x => urls.Contains(x.Source))
                .GroupBy(x => x.Source)
                .Select(x => x.FirstOrDefault())
                .ToListAsync();
        }
    }
}
