using System;
using System.Linq;
using GForum.Common.Enums;
using GForum.Data;
using GForum.Data.Models;

namespace GForum.Services
{
    public class PostService
    {
        private readonly ApplicationData data;

        public PostService(ApplicationData data)
        {
            this.data = data;
        }

        public IQueryable<Post> GetQueryable()
        {
            return this.data.Posts.Query;
        }

        public Post GetById(Guid id)
        {
            return this.data.Posts.Query
                .FirstOrDefault(x => x.Id == id);
        }

        public VoteType GetUserVoteTypeForPost(Guid postId, string userId)
        {
            var vote = this.data.Votes.Query
                .FirstOrDefault(x => x.UserId == userId && x.PostId == postId);
            return vote != null ? vote.VoteType : VoteType.None;
        }

        public void Submit(Post post)
        {
            this.data.Posts.Add(post);
            this.data.Complete();
        }

        public void ToggleVote(Guid postId, Vote vote)
        {
            var post = this.GetById(postId);
            var prevUserVote = this.data.Votes.Query
                .FirstOrDefault(x => x.UserId == vote.UserId && x.PostId == postId);

            if (prevUserVote == null)
            {
                post.Votes.Add(vote);
                post.VoteCount += (int)vote.VoteType;
            }
            else
            {
                this.data.Votes.Remove(prevUserVote);
                post.VoteCount -= (int)prevUserVote.VoteType;

                if (prevUserVote.VoteType != vote.VoteType)
                {
                    post.Votes.Add(vote);
                    post.VoteCount += (int)vote.VoteType;
                }
            }
            this.data.Complete();
        }

        public void Edit(Guid postId, string newContent)
        {
            var post = this.GetById(postId);
            post.Content = newContent;
            post.EditedOn = DateTime.Now;
            this.data.Complete();
        }
    }
}