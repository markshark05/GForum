﻿using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GForum.Models.IdentityModels
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}