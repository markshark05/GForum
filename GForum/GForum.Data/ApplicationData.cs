using System;
using GForum.Data.Models;
using GForum.Data.Repositories;

namespace GForum.Data
{
    public class ApplicationData : IDisposable
    {
        private readonly ApplicationDbContext context;

        public ApplicationData(ApplicationDbContext context)
        {
            this.context = context;
            this.Users = new Repository<ApplicationUser>(context);
            this.Categories = new Repository<Category>(context);
            this.Posts = new Repository<Post>(context);
            this.Votes = new Repository<Vote>(context);
        }

        public Repository<ApplicationUser> Users { get; }

        public Repository<Category> Categories { get; }

        public Repository<Post> Posts { get; }

        public Repository<Vote> Votes { get; }

        public int Complete()
        {
            return this.context.SaveChanges();
        }

        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}
