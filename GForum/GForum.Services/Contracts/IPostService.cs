using System;
using GForum.Data.Models;

namespace GForum.Services.Contracts
{
    public interface IPostService: IService<Post>
    {
        Post Submit(Guid categoryId, string userId, string title, string content);

        void Edit(Guid postId, string newContent);
    }
}