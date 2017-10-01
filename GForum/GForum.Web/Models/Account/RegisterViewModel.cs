using System.ComponentModel.DataAnnotations;
using GForum.Common;

namespace GForum.Web.Models.Account
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [EmailAddress]
        [Display(Name = "Email (optional)")]
        public string Email { get; set; }

        [Required]
        [StringLength(100,
            ErrorMessage = "The {0} must be at least {2} characters long.",
            MinimumLength = Globals.RequiredPasswordLength)
        ]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
