using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagerTLA.BLL.DTO;
using TaskManagerTLA.BLL.Exeption;
using TaskManagerTLA.BLL.Services.TaskService.Interfaces;
using TaskManagerTLA.DAL.Entities;
using TaskManagerTLA.DAL.Repositories.Interfaces;

namespace TaskManagerTLA.BLL.Services.TaskService
{
    public class AssignedTaskService : IAssignedTaskService
    {
        private readonly IRepository<AssignedTask, int> assignedTaskRepository;
        private readonly IMapper mapper;

        public AssignedTaskService(IRepository<AssignedTask, int> assignedTaskRepository, IMapper mapper)
        {
            this.assignedTaskRepository = assignedTaskRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<AssignedTaskDTO>> GetAssignedTasksByUserNameAsync(string username)
        {
            var assignedTaskList = (await assignedTaskRepository.GetAllItemsAsync()).Where(p => p.User.UserName == username);
            return mapper.Map<IEnumerable<AssignedTask>, List<AssignedTaskDTO>>(assignedTaskList);
        }

        public async Task CreateAssignedTaskAsync(string userId, int? globalTaskId)
        {
            if (userId == null || globalTaskId == null) throw new ServiceException("Не дійсне значення: userId або globalTaskId = null");
            await assignedTaskRepository.CreateItemAsync(new AssignedTask { UserId = userId, GlobalTaskId = globalTaskId.Value });
            await assignedTaskRepository.SaveAsync();
        }

        public async Task UpdateDescriptionAsync(string userId, int? globalTaskId, string description)
        {
            var asignedTask = (await assignedTaskRepository.GetAllItemsAsync()).Where(p => (p.UserId == userId && p.GlobalTaskId == globalTaskId)).FirstOrDefault();
            asignedTask.Description = description != null ? $" {asignedTask.Description} | {DateTime.Now:dd.MM.yyyy} {description}" : asignedTask.Description;
            await assignedTaskRepository.SaveAsync();
        }

        public async Task UpdateElapsedTimeAssignedTaskAsync(string userId, int? globalTaskId, int? elapsedTime)
        {
            if (globalTaskId == null || userId == null) throw new ServiceException("Не дійсне значення: userId або globalTaskId = null");
            var asignedTask = (await assignedTaskRepository.GetAllItemsAsync()).Where(p => (p.UserId == userId && p.GlobalTaskId == globalTaskId)).FirstOrDefault();
            asignedTask.SpentHours = elapsedTime != null && elapsedTime.Value > 0 ? asignedTask.SpentHours + elapsedTime.Value : asignedTask.SpentHours;
            await assignedTaskRepository.SaveAsync();
        }

        public async Task DeleteAssignedTaskAsync(string userId, int? globalTaskId)
        {
            if (globalTaskId == null || userId == null) throw new ServiceException("Не дійсне значення: userId або globalTaskId = null");
            var delAsignedTask = await assignedTaskRepository.FindItemAsync(p => p.GlobalTaskId == globalTaskId && p.UserId == userId);
            if (delAsignedTask == null) throw new ServiceException("Не знайдено відповідний запис в базі данних: AsignedTask = null");
            await assignedTaskRepository.DeleteItemAsync(delAsignedTask);
            await assignedTaskRepository.SaveAsync();
        }
    }
}
