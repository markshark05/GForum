using System.Linq;
using GForum.Data.Models;

namespace GForum.Web.Areas.Admin.Models.Categories
{
    public class CategoriesViewModel
    {
        public CategoryAddViewModel CategoryAdd { get; set; }

        public IQueryable<Category> CategoriesQueriable { get; set; }
    }
}