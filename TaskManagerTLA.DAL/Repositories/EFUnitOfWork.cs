using System;
using TaskManagerTLA.DAL.EF;
using TaskManagerTLA.DAL.Entities;
using TaskManagerTLA.DAL.Interfaces;

namespace TaskManagerTLA.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {

        // це все має інжектнутись

        // тут я не зрозумів як можна передати репозиторії через залежності
        // посуті ж  контролювати створеня репозиторыъв має UnitOfWork і надавати їм спільний доступ до бази данних
        // а якшо я буду їх інжектувати через залежності то вони будуть створюватись і передаватись без участі UnitOfWork

        private readonly TaskContext db;
        private IRepository<GlobalTask> globalTaskRepositories;
        private IRepository<AssignedTask> assignedTaskRepositories;
        private bool disposed = false;

        public EFUnitOfWork(TaskContext database)
        {
            db = database;
        }

        public IRepository<GlobalTask> GlobalTasks
        {
            get
            {
                if (globalTaskRepositories == null)
                {
                    globalTaskRepositories = new TaskRepository(db);
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
                    assignedTaskRepositories = new AssignedTaskRepository(db);
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
