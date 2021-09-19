using System;
using System.Collections.Generic;
using System.Text;
using TaskManagerTLA.BLL.DTO;

namespace TaskManagerTLA.BLL.Interfaces
{
    public interface ITaskService
    {
        void MakeActualTask(ActualTaskDTO actualTaskDTO);
        void MakeTask(TaskDTO taskDTO);
        TaskDTO GetTask(int? taskId);
        void DeleteTask(int? taskId);
        void DeleteActualTask(int? actualTaskId);
        void DeleteActualTaskByUser(string userName);
        IEnumerable<TaskDTO> GetTasks();
        ActualTaskDTO GetActualTask(int? actualTaskId);
        IEnumerable<ActualTaskDTO> GetActualTasks();
        void EditActualTask(int? actualTaskId, int? elapsedTime, string description);
        IEnumerable<ActualTaskDTO> GetDetailsTask(int? taskId);


    }
}
