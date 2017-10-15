using System;
using System.Linq;
using GForum.Data.Models.Contracts;

namespace GForum.Services.Contracts
{
    public interface IService<TEntity>
        where TEntity : class, IEntity, IEntityWithGuid
    {
        IQueryable<TEntity> GetAll(bool includeDeleted = false);

        IQueryable<TEntity> GetById(Guid id, bool includeDeleted = false);

        void Delete(Guid id);

        void Restore(Guid id);
    }
}
