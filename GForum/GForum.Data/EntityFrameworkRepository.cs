using System;
using System.Data.Entity;
using System.Linq;
using GForum.Data.Contracts;
using GForum.Data.Models.Contracts;

namespace GForum.Data
{
    public class EntityFrameworkRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {
        private readonly DbSet<TEntity> entities;

        public EntityFrameworkRepository(DbContext context)
        {
            this.entities = context.Set<TEntity>();
        }

        public IQueryable<TEntity> Query
        {
            get => this.entities.Where(e => !e.IsDeleted);
        }

        public void Add(TEntity entity)
        {
            this.entities.Add(entity);
        }

        public void Remove(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.Now;
        }
    }
}