using System;
using System.Linq;
using GForum.Data.Contracts;
using GForum.Data.Models;
using GForum.Services.Contracts;

namespace GForum.Services
{
    public class CategoryService: ICategoryService
    {
        private readonly IRepository<Category> categories;
        private readonly IUnitOfWork unitOfWork;

        public CategoryService(
            IRepository<Category> categories,
            IUnitOfWork unitOfWork)
        {
            this.categories = categories;
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<Category> GetAll()
        {
            return this.categories.Query;
        }

        public IQueryable<Category> GetById(Guid id)
        {
            return this.categories.Query
                .Where(x => x.Id == id);
        }

        public Category Create(string userId, string title)
        {
            var category = new Category
            {
                AuthorId = userId,
                Title = title
            };

            this.categories.Add(category);
            this.unitOfWork.Complete();
            return category;
        }
    }
}
