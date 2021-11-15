using TaskManagerTLA.DAL.Entities;
using TaskManagerTLA.DAL.Repositories.Interfaces;

namespace TaskManagerTLA.DAL.UnitOfWork.TaskUnitOfWork.Interfaces
{
    public interface IGlobalTaskUnit
    {
        IRepository<GlobalTask, int> GlobalTasks { get; }
    }
}
