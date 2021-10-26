using System;
using TaskManagerTLA.DAL.Entities;

namespace TaskManagerTLA.DAL.Interfaces
{
    // по суті тобі потрібен буде окремий unitOfWork на кожен тип операції, н.п. для роботи лише з тасками - TaskUnitOfWork, призначення тасок - уже треба і юзерів і тасок...
    public interface IUnitOfWork : IDisposable
    {
        IRepository<GlobalTask> GlobalTasks { get; }
        IRepository<AssignedTask> AssignedTasks { get; }
        void Save();
    }
}
