using System;
using System.Linq;
using GForum.Data.Contracts;
using GForum.Data.Models;
using GForum.Services.Abstract;
using GForum.Services.Contracts;

namespace GForum.Services
{
    public class PostService : Service<Post>, IPostService
    {
        public PostService(
            IUnitOfWork unitOfWork,
            IRepository<Post> repository)
            : base(unitOfWork, repository)
        {
        }

        public Post Submit(Guid categoryId, string userId, string title, string content)
        {
            var post = new Post
            {
                CategoryId = categoryId,
                AuthorId = userId,
                Title = title,
                Content = content
            };

            this.repository.Add(post);
            this.unitOfWork.Complete();
            return post;
        }

        public void Edit(Guid postId, string newContent)
        {
            var post = this.GetById(postId).FirstOrDefault();
            post.Content = newContent;
            post.EditedOn = DateTime.Now;

            this.unitOfWork.Complete();
        }

        public IQueryable<Post> GetRecent(int count)
        {
            return this.repository
                .QueryAll
                .OrderByDescending(x => x.CreatedOn)
                .Take(count);
        }

        public IQueryable<Post> GetTopRated(int count)
        {
            return this.repository
                .QueryAll
                .OrderByDescending(x => x.VoteCount)
                .ThenByDescending(x => x.CreatedOn)
                .Take(count);
        }
    }
}