using System.ComponentModel.DataAnnotations;
using GForum.Common;

namespace GForum.Web.Areas.Admin.Models
{
    public class CategoryAddViewModel
    {
        [Required]
        [StringLength(Globals.CategoryTitleLength)]
        public string Title { get; set; }
    }
}