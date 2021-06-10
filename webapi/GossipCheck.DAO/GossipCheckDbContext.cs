using GossipCheck.DAO.Entities;
using Microsoft.EntityFrameworkCore;

namespace GossipCheck.DAO
{
    public sealed class GossipCheckDBContext : DbContext
    {
        public DbSet<MbfcReport> MbfcReports { get; set; }

        public GossipCheckDBContext(DbContextOptions<GossipCheckDBContext> options) : base(options)
        {
            this.Database.Migrate();
        }
    }
}
