using System;
using System.Linq;
using GForum.Common.Enums;
using GForum.Data;
using GForum.Data.Models;
using GForum.Services.Contracts;

namespace GForum.Services
{
    public class PostService: IPostService
    {
        private readonly UnitOfWork unitOfWork;
        private readonly IRepository<Category> category;
        private readonly IRepository<Post> posts;
        private readonly IRepository<Vote> votes;

        public PostService(
            UnitOfWork data,
            IRepository<Category> category,
            IRepository<Post> posts,
            IRepository<Vote> votes)
        {
            this.unitOfWork = data;
            this.category = category;
            this.posts = posts;
            this.votes = votes;
        }

        public IQueryable<Post> GetAll()
        {
            return this.posts.Query;
        }

        public IQueryable<Post> GetById(Guid id)
        {
            return this.posts.Query
                .Where(x => x.Id == id);
        }

        public VoteType GetUserVoteTypeForPost(Guid postId, string userId)
        {
            var vote = this.votes.Query
                .FirstOrDefault(x => x.UserId == userId && x.PostId == postId);

            return vote != null ? vote.VoteType : VoteType.None;
        }

        public void Submit(Post post)
        {
            this.posts.Add(post);
            this.unitOfWork.Complete();
        }

        public void ToggleVote(Guid postId, Vote vote)
        {
            var post = this.GetById(postId).FirstOrDefault();
            var prevUserVote = this.votes.Query
                .FirstOrDefault(x => x.UserId == vote.UserId && x.PostId == postId);

            if (prevUserVote == null)
            {
                post.Votes.Add(vote);
                post.VoteCount += (int)vote.VoteType;
            }
            else
            {
                this.votes.Remove(prevUserVote);
                post.VoteCount -= (int)prevUserVote.VoteType;

                if (prevUserVote.VoteType != vote.VoteType)
                {
                    post.Votes.Add(vote);
                    post.VoteCount += (int)vote.VoteType;
                }
            }
            this.unitOfWork.Complete();
        }

        public void Edit(Guid postId, string newContent)
        {
            var post = this.GetById(postId).FirstOrDefault();
            post.Content = newContent;
            post.EditedOn = DateTime.Now;
            this.unitOfWork.Complete();
        }

        public void Delete(Guid postId)
        {
            var post = this.GetById(postId).FirstOrDefault();
            this.posts.Remove(post);
            this.unitOfWork.Complete();
        }
    }
}