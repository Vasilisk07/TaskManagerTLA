using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<AssignedTaskDTO>> GetAssignedTasksByUserNameAsync(string username)
        {
            var assignedTaskList = (await DataBase.AssignedTasks.GetAllItemsAsync()).Where(p => p.User.UserName == username);
            return Mapper.Map<IEnumerable<AssignedTask>, List<AssignedTaskDTO>>(assignedTaskList);
        }

        public async Task CreateAssignedTaskAsync(string userId, int? globalTaskId)
        {
            if (userId == null || globalTaskId == null) throw new ServiceException("Не дійсне значення");
            await DataBase.AssignedTasks.CreateItemAsync(new AssignedTask { UserId = userId, GlobalTaskId = globalTaskId.Value });
            await DataBase.AssignedTasks.SaveAsync();
        }

        public async Task UpdateDescriptionAsync(string userId, int? globalTaskId, string description)
        {
            var asignedTask = (await DataBase.AssignedTasks.GetAllItemsAsync()).Where(p => (p.UserId == userId && p.GlobalTaskId == globalTaskId)).FirstOrDefault();
            asignedTask.Description = description != null ? $" {asignedTask.Description} | {DateTime.Now:dd.MM.yyyy} {description}" : asignedTask.Description;
            await DataBase.AssignedTasks.SaveAsync();
        }

        public async Task UpdateElapsedTimeAssignedTaskAsync(string userId, int? globalTaskId, int? elapsedTime)
        {
            if (globalTaskId == null || userId == null) throw new ServiceException("Не дійсне значення");
            var asignedTask = (await DataBase.AssignedTasks.GetAllItemsAsync()).Where(p => (p.UserId == userId && p.GlobalTaskId == globalTaskId)).FirstOrDefault();
            asignedTask.SpentHours = elapsedTime != null && elapsedTime.Value > 0 ? asignedTask.SpentHours + elapsedTime.Value : asignedTask.SpentHours;
            await DataBase.AssignedTasks.SaveAsync();
        }

        public async Task DeleteAssignedTaskAsync(string userId, int? globalTaskId)
        {
            var delasignedTask = (await DataBase.AssignedTasks.FindAsync(p => p.GlobalTaskId == globalTaskId && p.UserId == userId)).FirstOrDefault();
            if (delasignedTask == null) throw new ServiceException("Не дійсне значення");
            await DataBase.AssignedTasks.DeleteItemAsync(delasignedTask);
            await DataBase.AssignedTasks.SaveAsync();
        }
    }
}
