using System.ComponentModel.DataAnnotations;

namespace GForum.Web.Models.Manage
{
    public class ChangeEmailViewModel
    {
        [Display(Name = "Current Email")]
        public string CurrentEmail { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "New Email")]
        public string NewEmail { get; set; }
    }
}