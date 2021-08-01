using System;
using System.Collections.Generic;
using System.Text;
using TaskManagerTLA.BLL.DTO;

namespace TaskManagerTLA.BLL.Interfaces
{
    public interface ITaskService
    {
        void MakeActualTask(ActualTaskDTO ActTaskDTO);
        void MakeTask(TaskDTO taskDTO);
        TaskDTO GetTask(int? id);
        IEnumerable<TaskDTO> GetTasks();
        ActualTaskDTO GetActualTask(int? id);
        IEnumerable<ActualTaskDTO> GetActTasks();
        void Dispose();








    }
}
