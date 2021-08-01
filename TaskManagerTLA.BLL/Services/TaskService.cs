using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagerTLA.BLL.DTO;
using TaskManagerTLA.BLL.Infrastructure;
using TaskManagerTLA.BLL.Interfaces;
using TaskManagerTLA.DAL.Entities;
using TaskManagerTLA.DAL.Interfaces;

namespace TaskManagerTLA.BLL.Services
{
    public class TaskService : ITaskService
    {
        IUnitOfWork Database { get; set; }

        public TaskService(IUnitOfWork DB)
        {
            Database = DB;
        }
        public void Dispose()
        {
            Database.Dispose();
        }

        public IEnumerable<ActualTaskDTO> GetActTasks()
        {
            var maper = new MapperConfiguration(cfg => cfg.CreateMap<ActualTask, ActualTaskDTO>()).CreateMapper();
            return maper.Map<IEnumerable<ActualTask>, List<ActualTaskDTO>>(Database.ActualTasks.GetAll());


        }

        public ActualTaskDTO GetActualTask(int? id)
        {
            if (id == null)
            {
                throw new ValidationException("Not valid id", "");
            }
            var ActlTask = Database.ActualTasks.Get(id.Value);
            if (ActlTask == null)
            {
                throw new ValidationException("Task is empty", "");
            }
            return new ActualTaskDTO
            {
                ActTaskLeigth = ActlTask.ActTaskLeigth,
                ActualTaskId = ActlTask.ActualTaskId,
                Description = ActlTask.Description,
                TaskId = ActlTask.TaskId,
                TaskName = ActlTask.TaskName,
                UserName = ActlTask.UserName
            };
        }

        public TaskDTO GetTask(int? id)
        {
            if (id == null)
            {
                throw new ValidationException("Not valid id", "");
            }
            var task = Database.Tasks.Get(id.Value);
            if (task == null)
            {
                throw new ValidationException("Task is empty", "");
            }
            return new TaskDTO
            {
                TaskBegin = task.TaskBegin,
                TaskDescription = task.TaskDescription,
                TaskName = task.TaskName,
                TaskId = task.TaskId,
                TaskEnd = task.TaskEnd,
                TaskLeigth = task.TaskLeigth
            };
        }

        public IEnumerable<TaskDTO> GetTasks()
        {
            var maper = new MapperConfiguration(cfg => cfg.CreateMap<TaskModel, TaskDTO>()).CreateMapper();
            return maper.Map<IEnumerable<TaskModel>, List<TaskDTO>>(Database.Tasks.GetAll());
        }

        public void MakeActualTask(ActualTaskDTO ActTaskDTO)
        {
            TaskModel Tmodel = Database.Tasks.Get(ActTaskDTO.TaskId);
            if (Tmodel == null)
            {
                throw new ValidationException("Task is empty", "");
            }
            ActualTask ActTask = new ActualTask
            {
                TaskId = ActTaskDTO.TaskId,
                ActTaskLeigth = ActTaskDTO.ActTaskLeigth,
                ActualTaskId = ActTaskDTO.ActualTaskId,
                Description = ActTaskDTO.Description,
                TaskName = ActTaskDTO.TaskName,
                UserName = ActTaskDTO.UserName
            };
            Database.ActualTasks.Create(ActTask);
            Database.Save();
        }

        public void MakeTask(TaskDTO taskDTO)
        {
            TaskModel Tmodel = new TaskModel
            {
                TaskId = taskDTO.TaskId,
                TaskName = taskDTO.TaskName,
                TaskDescription = taskDTO.TaskDescription,
                TaskLeigth = taskDTO.TaskLeigth,
                TaskBegin = taskDTO.TaskBegin,
                TaskEnd = taskDTO.TaskEnd
            };
            Database.Tasks.Create(Tmodel);
            Database.Save();
        }
    }
}
