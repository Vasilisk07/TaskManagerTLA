using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagerTLA.DAL.EF.FluentConfigurations;
using TaskManagerTLA.DAL.Entities;
using TaskManagerTLA.DAL.Identity.Entities;

namespace TaskManagerTLA.DAL.EF
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser, ApplicationRole, string,
        IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public DbSet<GlobalTask> GlobalTask { get; set; }
        public DbSet<AssignedTask> AssignedTask { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            // Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new IdentityConfiguration());
            modelBuilder.ApplyConfiguration(new TaskConfiguration());

            //Ініціалізуєм базу данних ролями. (При виконанні міграцій вони будуть додані в базу)
            modelBuilder.Entity<ApplicationRole>().HasData(
                new ApplicationRole[]
                {
                    new ApplicationRole { Name = "Admin"},
                    new ApplicationRole { Name = "Manager"},
                    new ApplicationRole { Name = "Developer"}
                });
        }
    }
}
