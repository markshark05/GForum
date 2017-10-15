using System;
using System.Linq;
using GForum.Data.Models;

namespace GForum.Services.Contracts
{
    public interface IPostService: IService<Post>
    {
        IQueryable<Post> GetRecent(int count);

        IQueryable<Post> GetTopRated(int count);

        Post Submit(Guid categoryId, string userId, string title, string content);

        void Edit(Guid postId, string newContent);
    }
}