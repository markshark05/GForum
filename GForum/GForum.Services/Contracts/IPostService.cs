using System;
using System.Linq;
using GForum.Common.Enums;
using GForum.Data.Models;

namespace GForum.Services.Contracts
{
    public interface IPostService
    {
        IQueryable<Post> GetAll();

        IQueryable<Post> GetById(Guid id);

        VoteType GetUserVoteTypeForPost(Guid postId, string userId);

        void Submit(Post post);

        void ToggleVote(Guid postId, Vote vote);

        void Edit(Guid postId, string newContent);

        void Delete(Guid postId);
    }
}