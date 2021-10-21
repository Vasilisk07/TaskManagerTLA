using System.Collections.Generic;
using TaskManagerTLA.BLL.DTO;

namespace TaskManagerTLA.BLL.Interfaces
{
    public interface ITaskService
    {
        void CreateAssignedTask(string userName, int? taskId);
        void AddGlobalTask(GlobalTaskDTO globalTaskDTO);
        GlobalTaskDTO GetGlobalTask(int? globalTaskId);
        void DeleteGlobalTask(int? globalTaskId);
        void DeleteAssignedTask(string userID, int? globalId);
        IEnumerable<GlobalTaskDTO> GetGlobalTasks();
        IEnumerable<AssignedTaskDTO> GetAssignedTasksByUserName(string userName);
        void UpdateDescription(string userId, int? globalTaskId, string description);
        void UpdateElapsedTime(string userId, int? globalTaskId, int? elapsedTime);
        IEnumerable<AssignedTaskDTO> GetAssignedTasksByGlobalTaskId(int? globalTaskId);
    }
}
