using System;
using System.Linq;
using GForum.Data.Models;

namespace GForum.Services.Contracts
{
    public interface IPostService
    {
        IQueryable<Post> GetAll();

        IQueryable<Post> GetById(Guid id);

        Post Submit(Guid categoryId, string userId, string title, string content);

        void Edit(Guid postId, string newContent);

        void Delete(Guid postId);
    }
}