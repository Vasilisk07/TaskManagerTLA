using System;
using TaskManagerTLA.DAL.Entities;

namespace TaskManagerTLA.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<GlobalTask> GlobalTasks { get; }
        IRepository<AssignedTask> AssignedTasks { get; }
        void Save();
    }
}
