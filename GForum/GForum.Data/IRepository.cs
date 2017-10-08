using System.Linq;

namespace GForum.Data
{
    public interface IRepository<TEntity>
    {
        IQueryable<TEntity> Query { get; }

        void Add(TEntity entity);

        void Remove(TEntity entity);
    }
}
