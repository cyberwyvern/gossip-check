using System;
using System.Threading.Tasks;

namespace GossipCheck.DAO.Interface
{
    public interface IGossipCheckUnitOfWork : IDisposable
    {
        IMbfcReportsDAO MbfcReports { get; }

        ITransaction StartTransaction();

        Task<ITransaction> StartTransactionAsync();
    }
}