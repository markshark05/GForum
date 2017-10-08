using System;
using System.Linq;
using GForum.Data;
using GForum.Data.Models;
using GForum.Services.Contracts;

namespace GForum.Services
{
    public class CategoryService: ICategoryService
    {
        private readonly UnitOfWork unitOfWork;
        private readonly IRepository<Category> categories;

        public CategoryService(
            UnitOfWork unitOfWork, 
            IRepository<Category> categories)
        {
            this.unitOfWork = unitOfWork;
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
