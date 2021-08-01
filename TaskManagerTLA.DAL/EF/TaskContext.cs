using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagerTLA.DAL.Entities;


namespace TaskManagerTLA.DAL.EF
{
    class TaskContext : DbContext
    {

        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<ActualTask> ActualTasks { get; set; }



        public TaskContext(DbContextOptions<TaskContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

    }
   





}
