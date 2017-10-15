using System;
using GForum.Common.Enums;
using GForum.Data.Models;

namespace GForum.Services.Contracts
{
    public interface IVoteService: IService<Vote>
    {
        VoteType GetUserVoteTypeForPost(Guid postId, string userId);

        void ToggleVote(Guid postId, string userId, VoteType newVoteType);
    }
}
