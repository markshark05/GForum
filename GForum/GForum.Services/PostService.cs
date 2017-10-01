using System;
using System.Linq;
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
            return this.data.Posts.Query();
        }

        public Post GetById(Guid id)
        {
            return this.data.Posts
                .Query()
                .FirstOrDefault(x => x.Id == id);
        }
    }
}
