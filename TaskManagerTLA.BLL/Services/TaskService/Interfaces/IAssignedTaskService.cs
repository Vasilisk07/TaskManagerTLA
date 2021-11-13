using System.Collections.Generic;
using TaskManagerTLA.BLL.DTO;

namespace TaskManagerTLA.BLL.Services.TaskService.Interfaces
{
    public interface IAssignedTaskService
    {
        void CreateAssignedTask(string userId, int? taskId);
        void DeleteAssignedTask(string userID, int? globalId);
        IEnumerable<AssignedTaskDTO> GetAssignedTasksByUserName(string userName);
        void UpdateDescription(string userId, int? globalTaskId, string description);
        void UpdateElapsedTimeAssignedTask(string userId, int? globalTaskId, int? elapsedTime);
    }
}
