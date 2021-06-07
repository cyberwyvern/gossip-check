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
            this.transaction = this.context.Database.BeginTransaction();
        }

        public void Commit()
        {
            this.context.SaveChanges();
            this.transaction?.Commit();
        }

        public void Rollback()
        {
            foreach (var entry in this.context.ChangeTracker.Entries())
            {
                entry.State = EntityState.Detached;
            }

            this.transaction?.Rollback();
        }

        public async Task StartAsync()
        {
            this.transaction = await this.context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await this.context.SaveChangesAsync();
            await this.transaction?.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            foreach (var entry in this.context.ChangeTracker.Entries())
            {
                entry.State = EntityState.Detached;
            }

            await this.transaction?.RollbackAsync();
        }

        public void Dispose()
        {
            this.transaction?.Dispose();
        }
    }
}