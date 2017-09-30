using System;
using System.Linq;
using GForum.Data;
using GForum.Data.Models;

namespace GForum.Services
{
    public class CategoryService
    {
        private readonly ApplicationData data;

        public CategoryService(ApplicationData data)
        {
            this.data = data;
        }

        public IQueryable<Category> GetAll()
        {
            return this.data.Categories.GetAll();
        }

        public Category GetById(Guid id)
        {
            return this.data.Categories
                .GetAll()
                .FirstOrDefault(x => x.Id == id);
        }
    }
}
