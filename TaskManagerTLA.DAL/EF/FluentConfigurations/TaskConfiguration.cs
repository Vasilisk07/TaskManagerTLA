using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagerTLA.DAL.Entities;

namespace TaskManagerTLA.DAL.EF.FluentConfigurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<GlobalTask>
    {
        public void Configure(EntityTypeBuilder<GlobalTask> builder)
        {
            builder
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
