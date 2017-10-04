using System.ComponentModel.DataAnnotations;
using GForum.Common;

namespace GForum.Web.Models.Forum
{
    public class PostSubmitViewModel
    {
        public CategoryViewModel Category { get; set; }

        [Required]
        [StringLength(Globals.PostTitleLength)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
    }
}