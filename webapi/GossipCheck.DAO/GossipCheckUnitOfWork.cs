using GossipCheck.DAO.ConcreteDAO;
using GossipCheck.DAO.Interface;
using System.Threading.Tasks;

namespace GossipCheck.DAO
{
    public sealed class GossipCheckUnitOfWork : IGossipCheckUnitOfWork
    {
        private readonly GossipCheckDBContext context;

        private ISourceReputationDAO sourceReputations;

        public string ConnectionString { get; private set; }

        public ISourceReputationDAO SourceReputations => this.sourceReputations ??= new SourceReputationDAO(context);

        public GossipCheckUnitOfWork(GossipCheckDBContext context)
        {
            this.context = context;
        }

        public ITransaction StartTransaction()
        {
            var transaction = new Transaction(this.context);
            transaction.Start();
            return transaction;
        }

        public async Task<ITransaction> StartTransactionAsync()
        {
            var transaction = new Transaction(this.context);
            await transaction.StartAsync();
            return transaction;
        }

        public void Dispose()
        {
            this.context?.Dispose();
        }
    }
}