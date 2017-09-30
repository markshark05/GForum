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
            var role = new IdentityRole { Name = Globals.AdminRoleName };
            roleManager.Create(role);

            // Seed users
            var user = new ApplicationUser
            {
                UserName = Globals.DefaultAdminUsername,
                EmailConfirmed = true,
            };
            userManager.Create(user, Globals.DefaultAdminPassword);
            userManager.AddToRole(user.Id, Globals.AdminRoleName);

            // Seed catgeories
            var category = new Category
            {
                Title = Globals.DefaultCategoryTitle,
                Author = user,
            };
            context.Categories.Add(category);

            // Seed posts
            var post = new Post
            {
                Title = "README",
                Content =
                    $@"An admin account has been created with username - ""{Globals.DefaultAdminUsername}"" and " +
                    $@"password - ""{Globals.DefaultAdminPassword}"". Please change the passowrd ASAP!",
                Category = category,
                Author = user,
            };
            category.Posts.Add(post);

            // Complete
            context.SaveChanges();
            base.Seed(context);
        }
    }
}
