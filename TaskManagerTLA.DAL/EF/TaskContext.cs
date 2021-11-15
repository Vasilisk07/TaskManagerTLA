using Microsoft.EntityFrameworkCore;
using TaskManagerTLA.DAL.EF.FluentConfigurations;
using TaskManagerTLA.DAL.Entities;

namespace TaskManagerTLA.DAL.EF
{
    public class TaskContext : DbContext
    {
        public DbSet<GlobalTask> GlobalTask { get; set; }
        public DbSet<AssignedTask> AssignedTask { get; set; }
        public TaskContext(DbContextOptions<TaskContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new IdentityConfiguration());
            modelBuilder.ApplyConfiguration(new TaskConfiguration());
        }
    }
}
