using System.Linq;
using GForum.Data.Models;

namespace GForum.Web.Areas.Admin.Models
{
    public class AdminCategoriesViewModel
    {
        public CategoryAddViewModel CategoryAdd { get; set; }

        public IQueryable<Category> CategoriesQueriable { get; set; }
    }
}