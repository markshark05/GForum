using System;
using System.Linq;
using GForum.Data.Contracts;
using GForum.Data.Models;
using GForum.Services.Abstract;
using GForum.Services.Contracts;

namespace GForum.Services
{
    public class CommentService : Service<Comment>, ICommentService
    {
        private readonly IRepository<Post> postsRepository;

        public CommentService(
            IUnitOfWork unitOfWork,
            IRepository<Comment> repository,
            IRepository<Post> postsRepository)
            : base(unitOfWork, repository)
        {
            this.postsRepository = postsRepository;
        }

        public void AddCommentToPost(Guid postId, string userId, string content)
        {
            var post = this.postsRepository
                .QueryAll
                .Where(x => x.Id == postId)
                .FirstOrDefault();

            if (post == null)
            {
                return;
            }

            var comment = new Comment
            {
                AuthorId = userId,
                Content = content,
            };

            post.Comments.Add(comment);
            post.CommentCount++;
            this.unitOfWork.Complete();
        }
    }
}
