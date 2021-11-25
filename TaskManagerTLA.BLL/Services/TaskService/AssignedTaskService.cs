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
        private readonly IAssignedTaskUnit assignedTaskUnit;
        private readonly IMapper mapper;

        public AssignedTaskService(IAssignedTaskUnit assignedTaskUnit, IMapper mapper)
        {
            this.assignedTaskUnit = assignedTaskUnit;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<AssignedTaskDTO>> GetAssignedTasksByUserNameAsync(string username)
        {
            var assignedTaskList = (await assignedTaskUnit.AssignedTasks.GetAllItemsAsync()).Where(p => p.User.UserName == username);
            return mapper.Map<IEnumerable<AssignedTask>, List<AssignedTaskDTO>>(assignedTaskList);
        }

        public async Task CreateAssignedTaskAsync(string userId, int? globalTaskId)
        {
            if (userId == null || globalTaskId == null) throw new ServiceException("Не дійсне значення: userId або globalTaskId = null");
            await assignedTaskUnit.AssignedTasks.CreateItemAsync(new AssignedTask { UserId = userId, GlobalTaskId = globalTaskId.Value });
            await assignedTaskUnit.AssignedTasks.SaveAsync();
        }

        public async Task UpdateDescriptionAsync(string userId, int? globalTaskId, string description)
        {
            var asignedTask = (await assignedTaskUnit.AssignedTasks.GetAllItemsAsync()).Where(p => (p.UserId == userId && p.GlobalTaskId == globalTaskId)).FirstOrDefault();
            asignedTask.Description = description != null ? $" {asignedTask.Description} | {DateTime.Now:dd.MM.yyyy} {description}" : asignedTask.Description;
            await assignedTaskUnit.AssignedTasks.SaveAsync();
        }

        public async Task UpdateElapsedTimeAssignedTaskAsync(string userId, int? globalTaskId, int? elapsedTime)
        {
            if (globalTaskId == null || userId == null) throw new ServiceException("Не дійсне значення: userId або globalTaskId = null");
            var asignedTask = (await assignedTaskUnit.AssignedTasks.GetAllItemsAsync()).Where(p => (p.UserId == userId && p.GlobalTaskId == globalTaskId)).FirstOrDefault();
            asignedTask.SpentHours = elapsedTime != null && elapsedTime.Value > 0 ? asignedTask.SpentHours + elapsedTime.Value : asignedTask.SpentHours;
            await assignedTaskUnit.AssignedTasks.SaveAsync();
        }

        public async Task DeleteAssignedTaskAsync(string userId, int? globalTaskId)
        {
            if (globalTaskId == null || userId == null) throw new ServiceException("Не дійсне значення: userId або globalTaskId = null");
            var delAsignedTask = (await assignedTaskUnit.AssignedTasks.FindAsync(p => p.GlobalTaskId == globalTaskId && p.UserId == userId)).FirstOrDefault();
            if (delAsignedTask == null) throw new ServiceException("Не знайдено відповідний запис в базі данних: AsignedTask = null");
            await assignedTaskUnit.AssignedTasks.DeleteItemAsync(delAsignedTask);
            await assignedTaskUnit.AssignedTasks.SaveAsync();
        }
    }
}
