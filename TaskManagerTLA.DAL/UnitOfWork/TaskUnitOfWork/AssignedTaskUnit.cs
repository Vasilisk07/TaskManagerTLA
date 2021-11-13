using TaskManagerTLA.DAL.Entities;
using TaskManagerTLA.DAL.Repositories.Interfaces;
using TaskManagerTLA.DAL.UnitOfWork.TaskUnitOfWork.Interfaces;

namespace TaskManagerTLA.DAL.UnitOfWork.TaskUnitOfWork
{
    public class AssignedTaskUnit : IAssignedTaskUnit
    {
        public IRepository<AssignedTask, int> AssignedTasks { get; }
        public AssignedTaskUnit(IRepository<AssignedTask, int> assignedTasks)
        {
            AssignedTasks = assignedTasks;
        }
    }
}
