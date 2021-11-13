using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagerTLA.BLL.DTO;
using TaskManagerTLA.BLL.Exeption;
using TaskManagerTLA.BLL.Services.TaskService.Interfaces;
using TaskManagerTLA.DAL.Entities;
using TaskManagerTLA.DAL.UnitOfWork.TaskUnitOfWork.Interfaces;

namespace TaskManagerTLA.BLL.Services.TaskService
{
    public class AssignedTaskService : IAssignedTaskService
    {
        private readonly IAssignedTaskUnit DataBase;
        private readonly IMapper Mapper;

        public AssignedTaskService(IAssignedTaskUnit dataBase, IMapper mapper)
        {
            DataBase = dataBase;
            Mapper = mapper;
        }

        public IEnumerable<AssignedTaskDTO> GetAssignedTasksByUserName(string username)
        {
            var assignedTaskList = DataBase.AssignedTasks.GetAllItems().Where(p => p.User.UserName == username);
            return Mapper.Map<IEnumerable<AssignedTask>, List<AssignedTaskDTO>>(assignedTaskList);
        }

        public void CreateAssignedTask(string userId, int? globalTaskId)
        {
            if (userId==null|| globalTaskId==null) throw new ServiceException("Не дійсне значення");
            DataBase.AssignedTasks.CreateItem(new AssignedTask { UserId = userId, GlobalTaskId = globalTaskId.Value });
            DataBase.AssignedTasks.Save();
        }

        public void UpdateDescription(string userId, int? globalTaskId, string description)
        {
            var asignedTask = DataBase.AssignedTasks.GetAllItems().Where(p => (p.UserId == userId && p.GlobalTaskId == globalTaskId)).FirstOrDefault();
            asignedTask.Description = description != null ? $" {asignedTask.Description} | {DateTime.Now:dd.MM.yyyy} {description}" : asignedTask.Description;
            DataBase.AssignedTasks.Save();
        }

        public void UpdateElapsedTimeAssignedTask(string userId, int? globalTaskId, int? elapsedTime)
        {
            if (globalTaskId == null || userId == null) throw new ServiceException("Не дійсне значення");
            var asignedTask = DataBase.AssignedTasks.GetAllItems().Where(p => (p.UserId == userId && p.GlobalTaskId == globalTaskId)).FirstOrDefault();
            asignedTask.SpentHours = elapsedTime != null && elapsedTime.Value > 0 ? asignedTask.SpentHours + elapsedTime.Value : asignedTask.SpentHours;
            DataBase.AssignedTasks.Save();
        }

        public void DeleteAssignedTask(string userId, int? globalTaskId)
        {
            var delasignedTask = DataBase.AssignedTasks.Find(p => p.GlobalTaskId == globalTaskId && p.UserId == userId).FirstOrDefault();
            if (delasignedTask==null) throw new ServiceException("Не дійсне значення");
            DataBase.AssignedTasks.DeleteItem(delasignedTask);
            DataBase.AssignedTasks.Save();
        }
    }
}
