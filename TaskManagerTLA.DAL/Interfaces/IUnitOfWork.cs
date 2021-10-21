using System;
using System.Collections.Generic;
using System.Text;
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
