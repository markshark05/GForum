using System;
using System.Linq;
using GForum.Data.Contracts;
using GForum.Data.Models;
using GForum.Services.Contracts;

namespace GForum.Services
{
    public class PostService : IPostService
    {
        private readonly IRepository<Post> posts;
        private readonly IUnitOfWork unitOfWork;

        public PostService(
            IRepository<Post> posts,
            IUnitOfWork unitOfWork)
        {
            this.posts = posts;
            this.unitOfWork = unitOfWork;
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

        public Post Submit(Guid categoryId, string userId, string title, string content)
        {
            var post = new Post
            {
                CategoryId = categoryId,
                AuthorId = userId,
                Title = title,
                Content = content
            };

            this.posts.Add(post);
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

        public void Delete(Guid postId)
        {
            var post = this.GetById(postId).FirstOrDefault();
            this.posts.Remove(post);
            this.unitOfWork.Complete();
        }
    }
}