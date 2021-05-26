using GossipCheck.DAO.Entities;
using Microsoft.EntityFrameworkCore;

namespace GossipCheck.DAO
{
    public sealed class GossipCheckDBContext : DbContext
    {
        public DbSet<SourceReputation> SourceReputations { get; set; }

        public GossipCheckDBContext(DbContextOptions<GossipCheckDBContext> options) : base(options)
        {
            Database.Migrate();
        }
    }
}
