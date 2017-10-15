using GForum.Data.Models;

namespace GForum.Services.Contracts
{
    public interface ICategoryService: IService<Category>
    {
        Category Create(string userId, string title);
    }
}