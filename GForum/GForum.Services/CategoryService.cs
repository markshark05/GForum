using GForum.Data.Contracts;
using GForum.Data.Models;
using GForum.Services.Abstract;
using GForum.Services.Contracts;

namespace GForum.Services
{
    public class CategoryService : Service<Category>, ICategoryService
    {
        public CategoryService(
            IUnitOfWork unitOfWork,
            IRepository<Category> repository)
            : base(unitOfWork, repository)
        {
        }

        public Category Create(string userId, string title)
        {
            var category = new Category
            {
                AuthorId = userId,
                Title = title
            };

            this.repository.Add(category);
            this.unitOfWork.Complete();
            return category;
        }
    }
}
