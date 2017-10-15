using System;
using GForum.Data.Models;

namespace GForum.Services.Contracts
{
    public interface ICommentService: IService<Comment>
    {
        void AddCommentToPost(Guid postId, string userId, string content);
    }
}
