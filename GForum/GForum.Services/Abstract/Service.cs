using System;
using System.Linq;
using GForum.Data.Contracts;
using GForum.Data.Models.Contracts;
using GForum.Services.Contracts;

namespace GForum.Services.Abstract
{
    public abstract class Service<TEntity>: IService<TEntity>
        where TEntity : class, IEntity, IEntityWithGuid
    {
        protected readonly IUnitOfWork unitOfWork;
        protected readonly IRepository<TEntity> repository;

        public Service(
            IUnitOfWork unitOfWork,
            IRepository<TEntity> repository)
        {
            this.unitOfWork = unitOfWork;
            this.repository = repository;
        }

        public IQueryable<TEntity> GetAll(bool includeDeleted = false)
        {
            if (!includeDeleted)
            {
                return this.repository
                   .QueryAll;
            }
            else
            {
                return this.repository
                    .QueryAllWithDeletd;
            }
        }

        public IQueryable<TEntity> GetById(Guid id, bool includeDeleted = false)
        {
            if (!includeDeleted)
            {
                return this.repository
                    .QueryAll
                    .Where(x => x.Id == id);
            }
            else
            {
                return this.repository
                    .QueryAllWithDeletd
                    .Where(x => x.Id == id);
            }
        }

        public virtual void Delete(Guid id)
        {
            var entity = this.repository
                .QueryAll
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (entity == null)
            {
                return;
            }

            this.repository.Remove(entity);
            this.unitOfWork.Complete();
        }

        public void Restore(Guid id)
        {
            var entity = this.repository
                .QueryAllWithDeletd
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (entity == null)
            {
                return;
            }

            entity.IsDeleted = false;
            entity.DeletedOn = null;
            this.unitOfWork.Complete();
        }
    }
}
