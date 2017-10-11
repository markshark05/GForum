using System;

namespace GForum.Web.Models.Forum
{
    public class SubmittedByPartialViewModel
    {
        public AuthorViewModel Author { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}