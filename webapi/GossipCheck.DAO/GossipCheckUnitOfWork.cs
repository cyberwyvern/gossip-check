using GossipCheck.DAO.ConcreteDAO;
using GossipCheck.DAO.Interface;
using System.Threading.Tasks;

namespace GossipCheck.DAO
{
    public sealed class GossipCheckUnitOfWork : IGossipCheckUnitOfWork
    {
        private readonly GossipCheckDBContext context;

        private IMbfcReportsDAO mbfcReports;

        public string ConnectionString { get; private set; }

        public IMbfcReportsDAO MbfcReports => mbfcReports ??= new MbfcReportsDAO(context);

        public GossipCheckUnitOfWork(GossipCheckDBContext context)
        {
            this.context = context;
        }

        public ITransaction StartTransaction()
        {
            Transaction transaction = new Transaction(context);
            transaction.Start();
            return transaction;
        }

        public async Task<ITransaction> StartTransactionAsync()
        {
            Transaction transaction = new Transaction(context);
            await transaction.StartAsync();
            return transaction;
        }

        public void Dispose()
        {
            context?.Dispose();
        }
    }
}