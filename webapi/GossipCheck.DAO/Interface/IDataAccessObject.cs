using GossipCheck.DAO.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GossipCheck.DAO.Interface
{
    public interface IDataAccessObject<TEntity, UId>
        where UId : struct, IComparable
        where TEntity : class, IEntity<UId>, new()
    {
        UId Create(TEntity item);

        IEnumerable<TEntity> CreateMultiple(IEnumerable<TEntity> items);

        TEntity Read(UId id);

        IEnumerable<TEntity> ReadMultiple(IEnumerable<UId> Ids);

        int Update(TEntity item);

        int UpdateMultiple(IEnumerable<TEntity> items);

        int Delete(UId itemId);

        int DeleteMultiple(IEnumerable<UId> itemsId);

        Task<UId> CreateAsync(TEntity item);

        Task<IEnumerable<TEntity>> CreateMultipleAsync(IEnumerable<TEntity> items);

        Task<TEntity> ReadAsync(UId id);

        Task<IEnumerable<TEntity>> ReadMultipleAsync(IEnumerable<UId> Ids);

        Task<int> UpdateAsync(TEntity item);

        Task<int> UpdateMultipleAsync(IEnumerable<TEntity> items);

        Task<int> DeleteAsync(UId itemId);

        Task<int> DeleteMultipleAsync(IEnumerable<UId> itemsId);
    }
}