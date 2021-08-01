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

        private TaskContext db;
        private TaskRepositories taskRepositories;
        private ActualTaskRepositories actualTaskRepositories;
        private bool disposed = false;

        public IRepository<TaskModel> Tasks
        {
            get
            {
                if (taskRepositories == null)
                {
                    taskRepositories = new TaskRepositories(db);
                }
                return taskRepositories;
            }

        }
        public IRepository<ActualTask> ActualTasks
        {
            get
            {
                if (actualTaskRepositories == null)
                {
                    actualTaskRepositories = new ActualTaskRepositories(db);
                }
                return actualTaskRepositories;
            }

        }

        public EFUnitOfWork(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TaskContext>();
            var options = optionsBuilder.UseSqlServer(connectionString).Options;
            db = new TaskContext(options);
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
