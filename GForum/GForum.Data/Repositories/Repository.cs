using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using GForum.Data.Models.Contracts;

namespace GForum.Data.Repositories
{
    public class Repository<TEntity>
        where TEntity : class, IEntity
    {
        private readonly DbSet<TEntity> entities;

        public Repository(DbContext context)
        {
            this.entities = context.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll()
        {
            return this.entities.Where(e => !e.IsDeleted);
        }

        public void Add(TEntity entity)
        {
            this.entities.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            this.entities.AddRange(entities);
        }

        public void Remove(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.Now;
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
                entity.DeletedOn = DateTime.Now;
            }
        }
    }
}