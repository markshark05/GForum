using System;
using GForum.Common.Enums;

namespace GForum.Services.Contracts
{
    public interface IVoteService
    {
        VoteType GetUserVoteTypeForPost(Guid postId, string userId);

        void ToggleVote(Guid postId, string userId, VoteType newVoteType);
    }
}
