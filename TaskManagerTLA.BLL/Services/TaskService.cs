using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagerTLA.BLL.DTO;
using TaskManagerTLA.BLL.Interfaces;
using TaskManagerTLA.DAL.Entities;
using TaskManagerTLA.DAL.Interfaces;

namespace TaskManagerTLA.BLL.Services
{
    public class TaskService : ITaskService
    {
        IUnitOfWork Database { get; set; }
        private bool disposed = false;



        public TaskService(IUnitOfWork DB)
        {
            Database = DB;
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Database.Dispose();
                }
                disposed = true;
            }
        }
        ~TaskService()
        {
            Dispose(false);
        }

        public IEnumerable<ActualTaskDTO> GetActTasks()
        {
            var maper = new MapperConfiguration(cfg => cfg.CreateMap<ActualTask, ActualTaskDTO>()).CreateMapper();
            return maper.Map<IEnumerable<ActualTask>, List<ActualTaskDTO>>(Database.ActualTasks.GetAll());
        }


        public ActualTaskDTO GetActualTask(int? id)
        {
            var ActlTask = Database.ActualTasks.Get(id.Value);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ActualTask, ActualTaskDTO>()).CreateMapper();
            var ActTask = mapper.Map<ActualTaskDTO>(ActlTask);
            return ActTask;
        }


        public TaskDTO GetTask(int? id)
        {
            var task = Database.Tasks.Get(id.Value);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<TaskModel, TaskDTO>()).CreateMapper();
            var Tmodel = mapper.Map<TaskDTO>(task);
            return Tmodel;
        }


        public IEnumerable<TaskDTO> GetTasks()
        {
            IEnumerable<TaskModel> Tlist = Database.Tasks.GetAll();
            var maper = new MapperConfiguration(cfg => cfg.CreateMap<TaskModel, TaskDTO>()).CreateMapper();
            return maper.Map<IEnumerable<TaskModel>, List<TaskDTO>>(Database.Tasks.GetAll());
        }


        public void MakeActualTask(ActualTaskDTO ActTaskDTO)
        {
            bool ifExist = false;
            var aTask = GetActTasks();
            foreach (var item in aTask)
            {
                if (item.TaskId == ActTaskDTO.TaskId && item.UserName == ActTaskDTO.UserName)
                {
                    ifExist = true;
                    break;
                }
            }
            if (!ifExist)
            {
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ActualTaskDTO, ActualTask>()).CreateMapper();
                var ActTask = mapper.Map<ActualTask>(ActTaskDTO);
                Database.ActualTasks.Create(ActTask);
                Database.Save();
            }
        }


        public void MakeTask(TaskDTO taskDTO)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<TaskDTO, TaskModel>()).CreateMapper();
            var Tmodel = mapper.Map<TaskModel>(taskDTO);
            Database.Tasks.Create(Tmodel);
            Database.Save();
        }


        public void DeleteTask(int? id)
        {
            Database.Tasks.Delete((int)id);
            var actTask = GetActTasks();
            foreach (var item in actTask)
            {
                if (item.TaskId == (int)id)
                {
                    Database.ActualTasks.Delete(item.ActualTaskId);
                }
            }
            Database.Save();
        }


        public void EditActualTask(int? id, int? time, string desk)
        {
            ActualTask EditsATask = Database.ActualTasks.Get((int)id);
            EditsATask.Description = desk != null ? $" {EditsATask.Description} | {DateTime.Now.ToString("dd.MM.yyyy")} {desk}" : EditsATask.Description;
            EditsATask.ActTaskLeigth = time != null && (int)time > 0 ? EditsATask.ActTaskLeigth + (int)time : EditsATask.ActTaskLeigth;
            TaskModel EditTask = Database.Tasks.Get(EditsATask.TaskId);
            EditTask.TaskLeigth = time != null && (int)time > 0 ? EditTask.TaskLeigth + (int)time : EditTask.TaskLeigth;
            Database.Save();
        }


        public void DeleteActualTask(int? id)
        {
            Database.ActualTasks.Delete((int)id);
            Database.Save();
        }

    }
}
