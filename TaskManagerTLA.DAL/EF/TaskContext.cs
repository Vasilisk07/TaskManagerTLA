using Microsoft.EntityFrameworkCore;
using TaskManagerTLA.DAL.Entities;
using TaskManagerTLA.DAL.Identity.Entities;

namespace TaskManagerTLA.DAL.EF
{
    class TaskContext : DbContext
    {
        public DbSet<GlobalTask> GlobalTask { get; set; }
        public DbSet<AssignedTask> AssignedTask { get; set; }
        public TaskContext(DbContextOptions<TaskContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        // тут виник такий нюанс що налаштування звязків прийшлось продублювати в обох контекстах,
        // можливо це так і треба, але можливо я шось криво реалізував 
        // чого воно працює саме так питання для мене так і залишилось відкритим...
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");
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

            base.OnModelCreating(modelBuilder);
        }
    }
}
