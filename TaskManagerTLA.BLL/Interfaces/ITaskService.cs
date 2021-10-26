using System.Collections.Generic;
using TaskManagerTLA.BLL.DTO;

namespace TaskManagerTLA.BLL.Interfaces
{
    public interface ITaskService
    {
        // ці всі методи можуть бути async, а async це дуже круто, особливо для нас, коли треба чекати на відповідь з бд
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
