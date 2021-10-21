using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagerTLA.DAL.EF;
using TaskManagerTLA.DAL.Entities;
using TaskManagerTLA.DAL.Interfaces;

namespace TaskManagerTLA.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        // TODO відповідно до SOLID краще тут використовувати інтерфейси ITaskRepository, IActualTaskRepository..
        // це все має інжектнутись
        private readonly TaskContext db;
        private TaskRepositories globalTaskRepositories;
        private AssignedTaskRepositories assignedTaskRepositories;
        private bool disposed = false;

        public EFUnitOfWork(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TaskContext>();
            var options = optionsBuilder.UseSqlServer(connectionString).Options;
            db = new TaskContext(options);
        }

        public IRepository<GlobalTask> GlobalTasks
        {
            get
            {
                if (globalTaskRepositories == null)
                {
                    globalTaskRepositories = new TaskRepositories(db);
                }
                return globalTaskRepositories;
            }

        }

        public IRepository<AssignedTask> AssignedTasks
        {
            get
            {
                if (assignedTaskRepositories == null)
                {
                    assignedTaskRepositories = new AssignedTaskRepositories(db);
                }
                return assignedTaskRepositories;
            }

        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}
