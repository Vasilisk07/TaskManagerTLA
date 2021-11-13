using TaskManagerTLA.DAL.Entities;
using TaskManagerTLA.DAL.Repositories.Interfaces;
using TaskManagerTLA.DAL.UnitOfWork.TaskUnitOfWork.Interfaces;

namespace TaskManagerTLA.DAL.UnitOfWork.TaskUnitOfWork
{
    public class GlobalTaskUnit : IGlobalTaskUnit
    {
        public IRepository<GlobalTask, int> GlobalTasks { get; }
        public GlobalTaskUnit(IRepository<GlobalTask, int> globalTasks)
        {
            GlobalTasks = globalTasks;
        }
    }
}
