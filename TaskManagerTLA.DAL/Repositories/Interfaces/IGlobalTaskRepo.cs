using System.Threading.Tasks;
using TaskManagerTLA.DAL.Entities;

namespace TaskManagerTLA.DAL.Repositories.Interfaces
{
    public interface IGlobalTaskRepo<T, I> : IRepository<T, I>
    {
        Task<GlobalTask> GetItemByGlobalTaskIdAsync(int taskId);
    }
}
