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

        public IQueryable<Post> GetAll()
        {
            return this.data.Posts.Query;
        }

        public Post GetById(Guid id)
        {
            return this.data.Posts.Query
                .FirstOrDefault(x => x.Id == id);
        }

        public void Submit(Post post)
        {
            this.data.Posts.Add(post);
            this.data.Complete();
        }

        public void ToggleVote(Post post, Vote vote)
        {
            var prevUserVote = post.Votes
                .FirstOrDefault(x => x.IsDeleted == false && x.UserId == vote.UserId);

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
    }
}
