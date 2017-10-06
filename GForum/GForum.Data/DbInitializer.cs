using System.Data.Entity;
using GForum.Common;
using GForum.Data.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GForum.Data
{
    internal class DbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            // Seed roles
            var adminRole = new IdentityRole { Name = Globals.AdminRoleName };
            roleManager.Create(adminRole);

            // Seed users
            var admin = new ApplicationUser
            {
                UserName = Globals.DefaultAdminUsername,
                EmailConfirmed = true,
            };
            userManager.Create(admin, Globals.DefaultAdminPassword);
            userManager.AddToRole(admin.Id, Globals.AdminRoleName);

            // Seed catgeories
            var category = new Category
            {
                Title = Globals.DefaultCategoryTitle,
                Author = admin,
            };
            context.Categories.Add(category);

            // Seed posts
            var post = new Post
            {
                Title = "README",
                Content =
                    $"An admin account has been created " +
                    $"with username - *{Globals.DefaultAdminUsername}* and password - *{Globals.DefaultAdminPassword}*.  \n" +
                    $"**Please change the passowrd ASAP!**",
                Category = category,
                Author = admin,
            };
            category.Posts.Add(post);

            // Complete
            context.SaveChanges();
            base.Seed(context);
        }
    }
}
