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

        Post Submit(Guid categoryId, string userId, string title, string content);

        void ToggleVote(Guid postId, string userId, VoteType voteType);

        void Edit(Guid postId, string newContent);

        void Delete(Guid postId);
    }
}