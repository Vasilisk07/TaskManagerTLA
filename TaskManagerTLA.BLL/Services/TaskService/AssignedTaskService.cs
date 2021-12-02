using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IRepository<AssignedTask, int> assignedTaskRepository;
        private readonly IMapper mapper;

        public AssignedTaskService(IRepository<AssignedTask, int> assignedTaskRepository, IMapper mapper)
        {
            this.assignedTaskRepository = assignedTaskRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<AssignedTaskDTO>> GetAssignedTasksByUserNameAsync(string username)
        {
            // не ефективно
            var assignedTaskList = (await assignedTaskRepository.GetAllItemsAsync()).Where(p => p.User.UserName == username);
            return mapper.Map<IEnumerable<AssignedTask>, List<AssignedTaskDTO>>(assignedTaskList);
        }

        public async Task CreateAssignedTaskAsync(string userId, int? globalTaskId)
        {
            // так точно буде видно що прийшло пустим
            if (userId == null)
                throw new ArgumentNullException("userId не може бути пустим");

            if (globalTaskId == null)
                throw new ArgumentNullException("globalTaskId не може бути пустим");

            await assignedTaskRepository.CreateItemAsync(new AssignedTask { UserId = userId, GlobalTaskId = globalTaskId.Value });
            await assignedTaskRepository.SaveAsync();
        }

        public async Task UpdateDescriptionAsync(string userId, int? globalTaskId, string description)
        {
            // не ефективно
            //  var asignedTask = await assignedTaskRepository.GetTaskForUserAsync(userId,globalTaskId);
            var asignedTask = (await assignedTaskRepository.GetAllItemsAsync()).Where(p => (p.UserId == userId && p.GlobalTaskId == globalTaskId)).FirstOrDefault();
            asignedTask.Description = description != null ? $" {asignedTask.Description} | {DateTime.Now:dd.MM.yyyy} {description}" : asignedTask.Description;
            // думаю тут з кожнним апдейтом буде добавлятись | {DateTime.Now:dd.MM.yyyy} {description}
            // краще збережи asignedTaskю.UpdatedAt = DateTime.Now:dd.MM.yyyy і показуй його на формі
            await assignedTaskRepository.SaveAsync();
        }

        public async Task UpdateElapsedTimeAssignedTaskAsync(string userId, int? globalTaskId, int? elapsedTime)
        {
            if (globalTaskId == null || userId == null)
                throw new ServiceException("Не дійсне значення: userId або globalTaskId = null");

            var asignedTask = (await assignedTaskRepository.GetAllItemsAsync()).Where(p => (p.UserId == userId && p.GlobalTaskId == globalTaskId)).FirstOrDefault();
            // тут я не можу зрозуміти як воно має работати :-)
            asignedTask.SpentHours = elapsedTime != null && elapsedTime.Value > 0 ? asignedTask.SpentHours + elapsedTime.Value : asignedTask.SpentHours;
            await assignedTaskRepository.SaveAsync();
        }

        public async Task DeleteAssignedTaskAsync(string userId, int? globalTaskId)
        {
            if (globalTaskId == null || userId == null)
                throw new ServiceException("Не дійсне значення: userId або globalTaskId = null");

            var delAsignedTask = await assignedTaskRepository.FindItemAsync(p => p.GlobalTaskId == globalTaskId && p.UserId == userId);

            if (delAsignedTask == null)
                throw new ServiceException("Не знайдено відповідний запис в базі данних: AsignedTask = null");
            // пару пустий ліній і код читаєтьяс набагато лешге

            await assignedTaskRepository.DeleteItemAsync(delAsignedTask);
            await assignedTaskRepository.SaveAsync();
        }
    }
}
