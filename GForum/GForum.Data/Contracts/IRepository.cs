using System.Linq;

namespace GForum.Data.Contracts
{
    public interface IRepository<TEntity>
    {
        IQueryable<TEntity> QueryAll { get; }

        IQueryable<TEntity> QueryAllWithDeletd { get; }

        void Add(TEntity entity);

        void Remove(TEntity entity);

        void Restore(TEntity entity);
    }
}
