using System;
using System.Linq;
using GForum.Data.Contracts;
using GForum.Data.Models;
using GForum.Services.Abstract;
using GForum.Services.Contracts;

namespace GForum.Services
{
    public class CategoryService : Service<Category>, ICategoryService
    {
        private readonly IRepository<Post> postsRepository;

        public CategoryService(
            IUnitOfWork unitOfWork,
            IRepository<Category> repository,
            IRepository<Post> postsRepository)
            : base(unitOfWork, repository)
        {
            this.postsRepository = postsRepository;
        }

        public Category Create(string userId, string title)
        {
            var category = new Category
            {
                AuthorId = userId,
                Title = title
            };

            this.repository.Add(category);
            this.unitOfWork.Complete();
            return category;
        }

        public override void Delete(Guid id)
        {
            var category = this.GetById(id).FirstOrDefault();

            if (category == null)
            {
                return;
            }

            foreach (var post in category.Posts)
            {
                this.postsRepository.Remove(post);
            }

            base.Delete(id);
        }
    }
}
