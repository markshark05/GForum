using System;
using System.Linq;
using GForum.Common.Enums;
using GForum.Data.Contracts;
using GForum.Data.Models;
using GForum.Services.Contracts;

namespace GForum.Services
{
    public class VoteService: IVoteService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepository<Post> posts;
        private readonly IRepository<Vote> votes;

        public VoteService(
            IUnitOfWork unitOfWork,
            IRepository<Post> posts,
            IRepository<Vote> votes)
        {
            this.unitOfWork = unitOfWork;
            this.posts = posts;
            this.votes = votes;
        }

        public VoteType GetUserVoteTypeForPost(Guid postId, string userId)
        {
            var vote = this.votes.Query
                .FirstOrDefault(x => x.UserId == userId && x.PostId == postId);

            return vote != null ? vote.VoteType : VoteType.None;
        }

        public void ToggleVote(Guid postId, string userId, VoteType newVoteType)
        {
            var post = this.posts.Query
                .FirstOrDefault(x => x.Id == postId);

            if (post == null)
            {
                throw new ArgumentException("Post does not exist.");
            }

            var prevUserVote = this.votes.Query
                .FirstOrDefault(x => x.PostId == postId && x.UserId == userId);

            var newVote = new Vote
            {
                VoteType = newVoteType,
                UserId = userId,
                PostId = postId,
            };

            if (prevUserVote == null)
            {
                this.votes.Add(newVote);
                post.VoteCount += (int)newVote.VoteType;
            }
            else
            {
                this.votes.Remove(prevUserVote);
                post.VoteCount -= (int)prevUserVote.VoteType;

                if (prevUserVote.VoteType != newVote.VoteType)
                {
                    this.votes.Add(newVote);
                    post.VoteCount += (int)newVote.VoteType;
                }
            }

            this.unitOfWork.Complete();
        }
    }
}
