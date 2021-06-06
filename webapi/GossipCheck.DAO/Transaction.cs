using GossipCheck.DAO.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace GossipCheck.DAO
{
    internal sealed class Transaction : ITransaction
    {
        private readonly DbContext context;
        private IDbContextTransaction transaction;

        public Transaction(DbContext context)
        {
            this.context = context;
        }

        public void Start()
        {
            transaction = context.Database.BeginTransaction();
        }

        public void Commit()
        {
            context.SaveChanges();
            transaction?.Commit();
        }

        public void Rollback()
        {
            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry in context.ChangeTracker.Entries())
            {
                entry.State = EntityState.Detached;
            }

            transaction?.Rollback();
        }

        public async Task StartAsync()
        {
            transaction = await context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await context.SaveChangesAsync();
            await transaction?.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry in context.ChangeTracker.Entries())
            {
                entry.State = EntityState.Detached;
            }

            await transaction?.RollbackAsync();
        }

        public void Dispose()
        {
            transaction?.Dispose();
        }
    }
}