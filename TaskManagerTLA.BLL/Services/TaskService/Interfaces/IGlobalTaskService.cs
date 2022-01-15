using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagerTLA.BLL.DTO;

namespace TaskManagerTLA.BLL.Services.TaskService.Interfaces
{
    public interface IGlobalTaskService
    {
        Task AddGlobalTaskAsync(GlobalTaskDTO globalTaskDTO);
        Task DeleteGlobalTaskAsync(int globalTaskId);
        Task<GlobalTaskDTO> GetGlobalTaskAsync(int globalTaskId);
        Task<IEnumerable<GlobalTaskDTO>> GetGlobalTasksAsync();
        Task UpdateElapsedTimeGlobalTaskAsync(int globalTaskId, int elapsedTime);
        Task<IEnumerable<AssignedTaskDTO>> GetAssignedTasksByGlobalTaskIdAsync(int globalTaskId);
    }
}
