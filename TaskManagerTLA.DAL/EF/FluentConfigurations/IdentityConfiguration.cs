using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagerTLA.DAL.Identity.Entities;

namespace TaskManagerTLA.DAL.EF.FluentConfigurations
{
    public class IdentityConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("AspNetUsers");
            builder
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
        }
    }
}
