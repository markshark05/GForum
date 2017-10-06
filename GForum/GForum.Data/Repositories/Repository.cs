﻿using System;
using System.Data.Entity;
using System.Linq;
using GForum.Data.Models.Contracts;

namespace GForum.Data.Repositories
{
    public class Repository<TEntity>
        where TEntity : class, IEntity
    {
        private readonly DbSet<TEntity> entities;
        private readonly DbContext context;

        public Repository(DbContext context)
        {
            this.context = context;
            this.entities = context.Set<TEntity>();
        }

        public IQueryable<TEntity> Query => this.entities.Where(e => !e.IsDeleted);

        public void Add(TEntity entity)
        {
            var entry = this.context.Entry(entity);
            this.entities.Add(entity);
        }

        public void Remove(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.Now;
        }
    }
}