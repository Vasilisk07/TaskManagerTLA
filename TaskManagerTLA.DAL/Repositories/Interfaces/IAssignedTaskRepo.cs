using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagerTLA.DAL.Entities;

namespace TaskManagerTLA.DAL.Repositories.Interfaces
{
    public interface IAssignedTaskRepo<T, I> : IRepository<T, I>
    {
        Task<IEnumerable<AssignedTask>> GetItemsByUserNameAsync(string username);
        Task<AssignedTask> GetTaskForUserIdAsync(string userId, int taskId);
    }
}
