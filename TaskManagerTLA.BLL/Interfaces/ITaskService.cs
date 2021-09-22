using System;
using System.Collections.Generic;
using System.Text;
using TaskManagerTLA.BLL.DTO;

namespace TaskManagerTLA.BLL.Interfaces
{
    public interface ITaskService
    {
        // TODO `Make` значить щось зробити, створити
        // назви `AddActualTask`
        // я б розділив це на два сервіси TaskService i ActualTaskService
        void MakeActualTask(ActualTaskDTO actualTaskDTO);
        void MakeTask(TaskDTO taskDTO);
        TaskDTO GetTask(int? taskId);
        void DeleteTask(int? taskId);
        void DeleteActualTask(int? actualTaskId);
        void DeleteActualTaskByUser(string userName);
        IEnumerable<TaskDTO> GetTasks();
        ActualTaskDTO GetActualTask(int? actualTaskId);
        IEnumerable<ActualTaskDTO> GetActualTasks();
        // TODO якщо так то розділи на два методи UpdateElapsedTime, UpdateDescription.
        void EditActualTask(int? actualTaskId, int? elapsedTime, string description);
        // TODO що таке 'DetailsTask' ?) Ти вертаєш ActualTaskDTO, краще так і назви GetActualTasks чи GetActualTasksByTaskId
        IEnumerable<ActualTaskDTO> GetDetailsTask(int? taskId);
    }
}
