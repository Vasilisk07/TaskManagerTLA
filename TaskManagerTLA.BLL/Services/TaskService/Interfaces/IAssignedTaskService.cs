using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagerTLA.BLL.DTO;

namespace TaskManagerTLA.BLL.Services.TaskService.Interfaces
{
    public interface IAssignedTaskService
    {
        Task CreateAssignedTaskAsync(string userId, int taskId);
        Task DeleteAssignedTaskAsync(string userID, int globalId);
        Task<IEnumerable<AssignedTaskDTO>> GetAssignedTasksByUserNameAsync(string userName);
        Task UpdateDescriptionAsync(string userId, int globalTaskId, string description);
        Task UpdateElapsedTimeAssignedTaskAsync(string userId, int globalTaskId, int elapsedTime);
        Task<IEnumerable<AssignedTCommentsDTO>> GetAssignedTaskCommentsAsync(string userId, int globalTaskId);
    }
}
