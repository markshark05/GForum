using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace GForum.Data.Repositories
{
    public class Repository<TEntity>
        where TEntity : class
    {
        private readonly DbSet<TEntity> entities;

        public Repository(DbContext context)
        {
            this.entities = context.Set<TEntity>();
        }

        public TEntity Get(int id)
        {
            return this.entities.Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return this.entities.ToList();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return this.entities.Where(predicate);
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return this.entities.SingleOrDefault(predicate);
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
            this.entities.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            this.entities.RemoveRange(entities);
        }
    }
}