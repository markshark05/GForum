using System;

namespace GForum.Web.Models.Shared
{
    public class SubmittedByPartialViewModel
    {
        public AuthorViewModel Author { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}