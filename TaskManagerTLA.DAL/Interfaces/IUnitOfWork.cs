using System;
using System.Collections.Generic;
using System.Text;
using TaskManagerTLA.DAL.Entities;

namespace TaskManagerTLA.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TaskModel> Tasks { get; }
        IRepository<ActualTask> ActualTasks { get; }
        void Save();
    }
}
