using System.Collections.Generic;
using TaskManagerTLA.BLL.DTO;

namespace TaskManagerTLA.BLL.Services.TaskService.Interfaces
{
    public interface IGlobalTaskService
    {
        void AddGlobalTask(GlobalTaskDTO globalTaskDTO);
        void DeleteGlobalTask(int? globalTaskId);
        GlobalTaskDTO GetGlobalTask(int? globalTaskId);
        IEnumerable<GlobalTaskDTO> GetGlobalTasks();
        void UpdateElapsedTimeGlobalTask(int? globalTaskId, int? elapsedTime);
        IEnumerable<AssignedTaskDTO> GetAssignedTasksByGlobalTaskId(int? globalTaskId);
    }
}
