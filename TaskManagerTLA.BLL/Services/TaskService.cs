using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagerTLA.BLL.DTO;
using TaskManagerTLA.BLL.Exeption;
using TaskManagerTLA.BLL.Interfaces;
using TaskManagerTLA.DAL.Entities;
using TaskManagerTLA.DAL.Interfaces;

namespace TaskManagerTLA.BLL.Services
{
    // розділити на TaskService, GlobalTaskService
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork dataBase;
        private readonly IMapper mapper;

        public TaskService(IUnitOfWork dataBase, IMapper mapper)
        {
            this.dataBase = dataBase;
            this.mapper = mapper;
        }

        public IEnumerable<AssignedTaskDTO> GetAssignedTasksByUserName(string username)
        {
            var assignedTaskList = dataBase.AssignedTasks.GetAll().TakeWhile(p => p.User.UserName == username);
            return mapper.Map<IEnumerable<AssignedTask>, List<AssignedTaskDTO>>(assignedTaskList);
        }

        public GlobalTaskDTO GetGlobalTask(int? GlobalTaskId)
        {
            if (GlobalTaskId == null) throw new MyException("Не дійсне значення");
            var globalTask = dataBase.GlobalTasks.Get(GlobalTaskId.Value);
            return mapper.Map<GlobalTaskDTO>(globalTask);
        }

        public IEnumerable<AssignedTaskDTO> GetAssignedTasksByGlobalTaskId(int? globalTaskId)
        {
            if (globalTaskId == null) throw new MyException("Не дійсне значення");
            var globalTask = dataBase.GlobalTasks.GetAll().Where(p => p.Id == globalTaskId).Single();
            var assignedTask = globalTask.AssignedTasks;
            return mapper.Map<IEnumerable<AssignedTaskDTO>>(assignedTask);
        }

        public IEnumerable<GlobalTaskDTO> GetGlobalTasks()
        {
            var globalTaskList = dataBase.GlobalTasks.GetAll();
            return mapper.Map<IEnumerable<GlobalTaskDTO>>(globalTaskList);
        }

        public void CreateAssignedTask(string userId, int? globalTaskId)
        {
            var globalTask = dataBase.GlobalTasks.Get(globalTaskId.Value);
            globalTask.AssignedTasks.Add(new AssignedTask { GlobalTaskId = globalTaskId.Value, UserId = userId });
            dataBase.Save();
        }

        public void AddGlobalTask(GlobalTaskDTO globalTaskDTO)
        {
            GlobalTask taskModel = mapper.Map<GlobalTask>(globalTaskDTO);
            dataBase.GlobalTasks.Create(taskModel);
            dataBase.Save();
        }

        public void DeleteGlobalTask(int? globalTaskId)
        {
            if (globalTaskId == null) throw new MyException("Не дійсне значення");
            dataBase.GlobalTasks.Delete(globalTaskId.Value);
            dataBase.Save();
        }

        public void UpdateDescription(string userId, int? globalTaskId, string description)
        {
            var asignedTask = dataBase.AssignedTasks.GetAll().Where(p => (p.UserId == userId && p.GlobalTaskId == globalTaskId)).FirstOrDefault();
            asignedTask.Description = description != null ? $" {asignedTask.Description} | {DateTime.Now:dd.MM.yyyy} {description}" : asignedTask.Description;
            dataBase.Save();
        }

        public void UpdateElapsedTime(string userId, int? globalTaskId, int? elapsedTime)
        {
            if (globalTaskId == null && userId == null) throw new MyException("Не дійсне значення");
            var asignedTask = dataBase.AssignedTasks.GetAll().Where(p => (p.UserId == userId && p.GlobalTaskId == globalTaskId)).FirstOrDefault();
            asignedTask.SpentHours = elapsedTime != null && elapsedTime.Value > 0 ? asignedTask.SpentHours + elapsedTime.Value : asignedTask.SpentHours;
            var globalTask = dataBase.GlobalTasks.Get(globalTaskId.Value);
            globalTask.TotalSpentHours = elapsedTime != null && elapsedTime.Value > 0 ? globalTask.TotalSpentHours + elapsedTime.Value : globalTask.TotalSpentHours;
            dataBase.Save();
        }

        public void DeleteAssignedTask(string userId, int? globalTaskId)
        {
            //в данній реалізації видаляю саме присвоєну певному юзеру таску з основної
            //щоб не тягнути сюди ще і IdentityService
            if (globalTaskId == null && userId == null) throw new MyException("Не дійсне значення");
            var globalTask = dataBase.GlobalTasks.Find(p => p.Id == globalTaskId).FirstOrDefault();
            // це просить методу в globalTask
            globalTask.AssignedTasks.Remove(globalTask.AssignedTasks.Where(p => p.UserId == userId).FirstOrDefault());
            dataBase.Save();
        }
    }
}
