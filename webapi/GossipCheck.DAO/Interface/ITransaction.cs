using System;
using System.Threading.Tasks;

namespace GossipCheck.DAO.Interface
{
    public interface ITransaction : IDisposable
    {
        void Start();

        void Commit();

        void Rollback();

        Task StartAsync();

        Task CommitAsync();

        Task RollbackAsync();
    }
}