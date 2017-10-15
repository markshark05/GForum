using System;
using System.Linq;
using GForum.Common.Enums;
using GForum.Data.Contracts;
using GForum.Data.Models;
using GForum.Services.Abstract;
using GForum.Services.Contracts;

namespace GForum.Services
{
    public class VoteService : Service<Vote>, IVoteService
    {
        private readonly IRepository<Post> postsRepository;

        public VoteService(
            IUnitOfWork unitOfWork,
            IRepository<Vote> repository,
            IRepository<Post> postsRepository)
            : base(unitOfWork, repository)
        {
            this.postsRepository = postsRepository;
        }

        public VoteType GetUserVoteTypeForPost(Guid postId, string userId)
        {
            var vote = this.repository.QueryAll
                .FirstOrDefault(x => x.UserId == userId && x.PostId == postId);

            return vote != null ? vote.VoteType : VoteType.None;
        }

        public void ToggleVote(Guid postId, string userId, VoteType newVoteType)
        {
            var post = this.postsRepository.QueryAll
                .FirstOrDefault(x => x.Id == postId);

            if (post == null)
            {
                return;
            }

            var prevUserVote = this.repository.QueryAll
                .FirstOrDefault(x => x.PostId == postId && x.UserId == userId);

            var newVote = new Vote
            {
                VoteType = newVoteType,
                UserId = userId,
                PostId = postId,
            };

            if (prevUserVote == null)
            {
                this.repository.Add(newVote);
                post.VoteCount += (int)newVote.VoteType;
            }
            else
            {
                this.repository.Remove(prevUserVote);
                post.VoteCount -= (int)prevUserVote.VoteType;

                if (prevUserVote.VoteType != newVote.VoteType)
                {
                    this.repository.Add(newVote);
                    post.VoteCount += (int)newVote.VoteType;
                }
            }

            this.unitOfWork.Complete();
        }
    }
}
