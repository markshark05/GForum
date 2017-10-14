using System;
using System.Linq;
using GForum.Data.Models;

namespace GForum.Services.Contracts
{
    public interface ICategoryService
    {
        IQueryable<Category> GetAll();

        IQueryable<Category> GetById(Guid id);

        Category Create(string userId, string title);
    }
}