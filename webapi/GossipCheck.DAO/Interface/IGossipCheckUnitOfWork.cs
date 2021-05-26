using System;
using System.Threading.Tasks;

namespace GossipCheck.DAO.Interface
{
    public interface IGossipCheckUnitOfWork : IDisposable
    {
        ISourceReputationDAO SourceReputations { get; }

        ITransaction StartTransaction();

        Task<ITransaction> StartTransactionAsync();
    }
}