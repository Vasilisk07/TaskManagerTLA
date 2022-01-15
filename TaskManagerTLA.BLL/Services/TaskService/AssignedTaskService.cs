using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TaskManagerTLA.BLL.DTO;
using TaskManagerTLA.BLL.Exeption;
using TaskManagerTLA.BLL.Services.TaskService.Interfaces;
using TaskManagerTLA.DAL.Entities;
using TaskManagerTLA.DAL.Repositories.Interfaces;

namespace TaskManagerTLA.BLL.Services.TaskService
{
    public class AssignedTaskService : IAssignedTaskService
    {
        private readonly IAssignedTaskRepo<AssignedTask, int> assignedTaskRepository;
        private readonly IMapper mapper;

        public AssignedTaskService(IAssignedTaskRepo<AssignedTask, int> assignedTaskRepository, IMapper mapper)
        {
            this.assignedTaskRepository = assignedTaskRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<AssignedTaskDTO>> GetAssignedTasksByUserNameAsync(string username)
        {
            var assignedTaskList = await assignedTaskRepository.GetItemsByUserNameAsync(username);
            return mapper.Map<IEnumerable<AssignedTask>, List<AssignedTaskDTO>>(assignedTaskList);
        }

        public async Task CreateAssignedTaskAsync(string userId, int globalTaskId)
        {
            if (userId == null)
                throw new ArgumentNullException("userId не може бути пустим");

            await assignedTaskRepository.CreateItemAsync(new AssignedTask { UserId = userId, GlobalTaskId = globalTaskId });
            await assignedTaskRepository.SaveAsync();
        }

        public async Task UpdateDescriptionAsync(string userId, int globalTaskId, string description)
        {
            var asignedTask = await assignedTaskRepository.GetTaskForUserIdAsync(userId, globalTaskId);
            asignedTask.Comments.Add(new AssignedTComments { DateModified = DateTime.Now, Comments = description });
            await assignedTaskRepository.SaveAsync();
        }

        public async Task UpdateElapsedTimeAssignedTaskAsync(string userId, int globalTaskId, int elapsedTime)
        {
            var asignedTask = await assignedTaskRepository.GetTaskForUserIdAsync(userId, globalTaskId);
            asignedTask.SpentHours = elapsedTime > 0 ? asignedTask.SpentHours + elapsedTime : asignedTask.SpentHours;
            await assignedTaskRepository.SaveAsync();
        }


        public async Task DeleteAssignedTaskAsync(string userId, int globalTaskId)
        {
            if (userId == null)
                throw new ServiceException("Не дійсне значення: userId = null");

            var delAsignedTask = await assignedTaskRepository.FindFirstItemAsync(p => p.GlobalTaskId == globalTaskId && p.UserId == userId);

            if (delAsignedTask == null)
                throw new ServiceException("Не знайдено відповідний запис в базі данних: AsignedTask = null");

            await assignedTaskRepository.DeleteItemAsync(delAsignedTask);
            await assignedTaskRepository.SaveAsync();
        }

        public async Task<IEnumerable<AssignedTCommentsDTO>> GetAssignedTaskCommentsAsync(string userId, int globalTaskId)
        {
            var assignedTask = (await assignedTaskRepository.FindFirstItemAsync(p => p.GlobalTaskId == globalTaskId && p.UserId == userId));
            return mapper.Map<IEnumerable<AssignedTComments>, List<AssignedTCommentsDTO>>(assignedTask.Comments);
        }
    }
}
