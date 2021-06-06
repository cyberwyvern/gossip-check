using GossipCheck.DAO.Entities;
using GossipCheck.DAO.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GossipCheck.DAO
{
    public class DataAccessObject<TEntity, UId> : IDataAccessObject<TEntity, UId>
        where UId : struct, IComparable
        where TEntity : class, IEntity<UId>, new()
    {
        protected DbContext context;
        protected DbSet<TEntity> table;

        public DataAccessObject(DbContext dbContext)
        {
            context = dbContext;
            table = context.Set<TEntity>();
        }

        public virtual UId Create(TEntity item)
        {
            table.Add(item);
            context.SaveChanges();
            return item.Id;
        }

        public virtual IEnumerable<TEntity> CreateMultiple(IEnumerable<TEntity> items)
        {
            table.AddRange(items);
            context.SaveChanges();
            return items;
        }

        public virtual TEntity Read(UId id)
        {
            return table.Find(id);
        }

        public virtual IEnumerable<TEntity> ReadMultiple(IEnumerable<UId> Ids)
        {
            return table.Where(e => Ids.Contains(e.Id)).ToList();
        }

        public virtual int Update(TEntity item)
        {
            if (table.Local.Contains(item))
            {
                context.Entry(item).State = EntityState.Detached;
            }

            context.Update(item);
            return context.SaveChanges();
        }

        public virtual int UpdateMultiple(IEnumerable<TEntity> items)
        {
            foreach (TEntity item in items)
            {
                if (table.Local.Contains(item))
                {
                    context.Entry(item).State = EntityState.Detached;
                }
            }

            context.UpdateRange(items);
            return context.SaveChanges();
        }

        public virtual int Delete(UId itemId)
        {
            TEntity tracked = table.Local.SingleOrDefault(i => i.Id.CompareTo(itemId) == 0);

            table.Remove(tracked ?? new TEntity { Id = itemId });
            return context.SaveChanges();
        }

        public virtual int DeleteMultiple(IEnumerable<UId> itemsId)
        {
            List<TEntity> tracked = new List<TEntity>();
            foreach (UId id in itemsId)
            {
                TEntity item = table.Local.SingleOrDefault(i => i.Id.CompareTo(id) == 0);
                tracked.Add(item ?? new TEntity { Id = id });
            }

            table.RemoveRange(tracked);
            return context.SaveChanges();
        }

        public virtual async Task<UId> CreateAsync(TEntity item)
        {
            table.Add(item);
            await context.SaveChangesAsync();
            return item.Id;
        }

        public virtual async Task<IEnumerable<TEntity>> CreateMultipleAsync(IEnumerable<TEntity> items)
        {
            table.AddRange(items);
            await context.SaveChangesAsync();
            return items;
        }

        public virtual async Task<TEntity> ReadAsync(UId id)
        {
            return await table.FindAsync(id);
        }

        public virtual async Task<IEnumerable<TEntity>> ReadMultipleAsync(IEnumerable<UId> Ids)
        {
            return await table.Where(e => Ids.Contains(e.Id)).ToListAsync();
        }

        public virtual async Task<int> UpdateAsync(TEntity item)
        {
            if (table.Local.Contains(item))
            {
                context.Entry(item).State = EntityState.Detached;
            }

            context.Update(item);
            return await context.SaveChangesAsync();
        }

        public virtual async Task<int> UpdateMultipleAsync(IEnumerable<TEntity> items)
        {
            foreach (TEntity item in items)
            {
                if (table.Local.Contains(item))
                {
                    context.Entry(item).State = EntityState.Detached;
                }
            }

            context.UpdateRange(items);
            return await context.SaveChangesAsync();
        }

        public virtual async Task<int> DeleteAsync(UId itemId)
        {
            TEntity tracked = table.Local.SingleOrDefault(i => i.Id.CompareTo(itemId) == 0);

            table.Remove(tracked ?? new TEntity { Id = itemId });
            return await context.SaveChangesAsync();
        }

        public virtual async Task<int> DeleteMultipleAsync(IEnumerable<UId> itemsId)
        {
            List<TEntity> tracked = new List<TEntity>();
            foreach (UId id in itemsId)
            {
                TEntity item = table.Local.SingleOrDefault(i => i.Id.CompareTo(id) == 0);
                tracked.Add(item ?? new TEntity { Id = id });
            }

            table.RemoveRange(tracked);
            return await context.SaveChangesAsync();
        }
    }
}