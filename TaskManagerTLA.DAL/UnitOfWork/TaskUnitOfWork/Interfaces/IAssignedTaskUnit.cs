using TaskManagerTLA.DAL.Entities;
using TaskManagerTLA.DAL.Repositories.Interfaces;

namespace TaskManagerTLA.DAL.UnitOfWork.TaskUnitOfWork.Interfaces
{
    public interface IAssignedTaskUnit
    {
        IRepository<AssignedTask, int> AssignedTasks { get; }
    }
}
