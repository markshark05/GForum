using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GForum.Data.Models;
using GForum.Data.Models.Contracts;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GForum.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer(new DbInitializer());
            this.Database.Initialize(true);
        }

        public virtual IDbSet<Post> Posts { get; set; }

        public virtual IDbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public override int SaveChanges()
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync()
        {
            this.ApplyAuditInfoRules();
            return base.SaveChangesAsync();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditInfoRules()
        {
            var entries = this.ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is IEntity && e.State == EntityState.Added);

            foreach (var entry in entries)
            {
                var entity = (IEntity)entry.Entity;
                if (entry.State == EntityState.Added || entity.CreatedOn == null)
                {
                    entity.CreatedOn = DateTime.Now;
                }
            }
        }
    }
}