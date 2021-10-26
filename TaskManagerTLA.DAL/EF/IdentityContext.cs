using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagerTLA.DAL.Entities;
using TaskManagerTLA.DAL.Identity.Entities;

namespace TaskManagerTLA.DAL.EF
{
    public class IdentityContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, 
        IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        // тут виник такий нюанс що налаштування звязків прийшлось продублювати в обох контекстах,
        // можливо це так і треба, але можливо я шось криво реалізував 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");
            modelBuilder.Entity<ApplicationUser>()
                   .HasMany(u => u.Roles)
                   .WithMany(s => s.Users)
                   .UsingEntity<ApplicationUserRole>(
              j => j
                   .HasOne(pt => pt.Role)
                   .WithMany(t => t.UserRoles)
                   .HasForeignKey(pt => pt.RoleId),
              j => j
                   .HasOne(pt => pt.User)
                   .WithMany(t => t.UserRoles)
                   .HasForeignKey(pt => pt.UserId),
              j =>
              {
                  j.HasKey(t => new { t.RoleId, t.UserId });
                  j.ToTable("AspNetUserRoles");
              });

            modelBuilder.Entity<GlobalTask>()
                  .HasMany(u => u.Users)
                  .WithMany(s => s.GlobalTasks)
            .UsingEntity<AssignedTask>(
             j => j
                  .HasOne(pt => pt.User)
                  .WithMany(t => t.AssignedTasks)
                  .HasForeignKey(pt => pt.UserId),
             j => j
                  .HasOne(pt => pt.GlobalTask)
                  .WithMany(t => t.AssignedTasks)
                  .HasForeignKey(pt => pt.GlobalTaskId),
             j =>
             {
                 j.Property(pt => pt.SpentHours).HasDefaultValue(0);
                 j.Property(pt => pt.Description).HasDefaultValue("");
                 j.HasKey(t => new { t.GlobalTaskId, t.UserId });
                 j.ToTable("AssignedTask");

             });
            
        }
    }
}
