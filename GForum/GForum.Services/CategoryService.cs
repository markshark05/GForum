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

        public CategoryService(IRepository<Category> categories)
        {
            this.categories = categories;
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
    }
}
