using System;

namespace GForum.Web.Areas.Admin.Models.Shared
{
    public class BtnPartialViewModel
    {
        public Guid Id { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }
    }
}