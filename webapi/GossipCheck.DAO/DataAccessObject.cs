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
            this.context = dbContext;
            this.table = this.context.Set<TEntity>();
        }

        public virtual UId Create(TEntity item)
        {
            this.table.Add(item);
            this.context.SaveChanges();
            return item.Id;
        }

        public virtual IEnumerable<TEntity> CreateMultiple(IEnumerable<TEntity> items)
        {
            this.table.AddRange(items);
            this.context.SaveChanges();
            return items;
        }

        public virtual TEntity Read(UId id)
        {
            return this.table.Find(id);
        }

        public virtual IEnumerable<TEntity> ReadMultiple(IEnumerable<UId> Ids)
        {
            return this.table.Where(e => Ids.Contains(e.Id)).ToList();
        }

        public virtual int Update(TEntity item)
        {
            if (this.table.Local.Contains(item))
            {
                this.context.Entry(item).State = EntityState.Detached;
            }

            this.context.Update(item);
            return this.context.SaveChanges();
        }

        public virtual int UpdateMultiple(IEnumerable<TEntity> items)
        {
            foreach (var item in items)
            {
                if (this.table.Local.Contains(item))
                {
                    this.context.Entry(item).State = EntityState.Detached;
                }
            }

            this.context.UpdateRange(items);
            return this.context.SaveChanges();
        }

        public virtual int Delete(UId itemId)
        {
            var tracked = this.table.Local.SingleOrDefault(i => i.Id.CompareTo(itemId) == 0);

            this.table.Remove(tracked ?? new TEntity { Id = itemId });
            return this.context.SaveChanges();
        }

        public virtual int DeleteMultiple(IEnumerable<UId> itemsId)
        {
            var tracked = new List<TEntity>();
            foreach (var id in itemsId)
            {
                var item = this.table.Local.SingleOrDefault(i => i.Id.CompareTo(id) == 0);
                tracked.Add(item ?? new TEntity { Id = id });
            }

            this.table.RemoveRange(tracked);
            return this.context.SaveChanges();
        }

        public virtual async Task<UId> CreateAsync(TEntity item)
        {
            this.table.Add(item);
            await this.context.SaveChangesAsync();
            return item.Id;
        }

        public virtual async Task<IEnumerable<TEntity>> CreateMultipleAsync(IEnumerable<TEntity> items)
        {
            this.table.AddRange(items);
            await this.context.SaveChangesAsync();
            return items;
        }

        public virtual async Task<TEntity> ReadAsync(UId id)
        {
            return await this.table.FindAsync(id);
        }

        public virtual async Task<IEnumerable<TEntity>> ReadMultipleAsync(IEnumerable<UId> Ids)
        {
            return await this.table.Where(e => Ids.Contains(e.Id)).ToListAsync();
        }

        public virtual async Task<int> UpdateAsync(TEntity item)
        {
            if (this.table.Local.Contains(item))
            {
                this.context.Entry(item).State = EntityState.Detached;
            }

            this.context.Update(item);
            return await this.context.SaveChangesAsync();
        }

        public virtual async Task<int> UpdateMultipleAsync(IEnumerable<TEntity> items)
        {
            foreach (var item in items)
            {
                if (this.table.Local.Contains(item))
                {
                    this.context.Entry(item).State = EntityState.Detached;
                }
            }

            this.context.UpdateRange(items);
            return await this.context.SaveChangesAsync();
        }

        public virtual async Task<int> DeleteAsync(UId itemId)
        {
            var tracked = this.table.Local.SingleOrDefault(i => i.Id.CompareTo(itemId) == 0);

            this.table.Remove(tracked ?? new TEntity { Id = itemId });
            return await this.context.SaveChangesAsync();
        }

        public virtual async Task<int> DeleteMultipleAsync(IEnumerable<UId> itemsId)
        {
            var tracked = new List<TEntity>();
            foreach (var id in itemsId)
            {
                var item = this.table.Local.SingleOrDefault(i => i.Id.CompareTo(id) == 0);
                tracked.Add(item ?? new TEntity { Id = id });
            }

            this.table.RemoveRange(tracked);
            return await this.context.SaveChangesAsync();
        }
    }
}