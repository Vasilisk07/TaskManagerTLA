using AutoMapper;
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
    public class GlobalTaskService : IGlobalTaskService
    {
        private readonly IRepository<GlobalTask, int> globalTasRepository;
        private readonly IMapper mapper;
        public GlobalTaskService(IRepository<GlobalTask, int> globalTasRepository, IMapper mapper)
        {
            this.globalTasRepository = globalTasRepository;
            this.mapper = mapper;
        }

        public async Task AddGlobalTaskAsync(GlobalTaskDTO globalTaskDTO)
        {
            GlobalTask taskModel = mapper.Map<GlobalTask>(globalTaskDTO);
            await globalTasRepository.CreateItemAsync(taskModel);
            await globalTasRepository.SaveAsync();
        }

        public async Task DeleteGlobalTaskAsync(int? globalTaskId)
        {
            if (globalTaskId == null) throw new ServiceException("Не дійсне значення: globalTaskId = null");
            await globalTasRepository.DeleteItemByIdAsync(globalTaskId.Value);
            await globalTasRepository.SaveAsync();
        }

        public async Task<GlobalTaskDTO> GetGlobalTaskAsync(int? globalTaskId)
        {
            if (globalTaskId == null) throw new ServiceException("Не дійсне значення: globalTaskId = null");
            var globalTask = await globalTasRepository.GetItemByIdAsync(globalTaskId.Value);
            return mapper.Map<GlobalTaskDTO>(globalTask);
        }

        public async Task<IEnumerable<GlobalTaskDTO>> GetGlobalTasksAsync()
        {
            var globalTaskList = await globalTasRepository.GetAllItemsAsync();
            return mapper.Map<IEnumerable<GlobalTaskDTO>>(globalTaskList);
        }

        public async Task UpdateElapsedTimeGlobalTaskAsync(int? globalTaskId, int? elapsedTime)
        {
            if (globalTaskId == null) throw new ServiceException("Не дійсне значення: globalTaskId = null");
            var globalTask = await globalTasRepository.GetItemByIdAsync(globalTaskId.Value);
            globalTask.TotalSpentHours = elapsedTime != null && elapsedTime.Value > 0 ? globalTask.TotalSpentHours + elapsedTime.Value : globalTask.TotalSpentHours;
            await globalTasRepository.SaveAsync();
        }

        public async Task<IEnumerable<AssignedTaskDTO>> GetAssignedTasksByGlobalTaskIdAsync(int? globalTaskId)
        {
            if (globalTaskId == null) throw new ServiceException("Не дійсне значення: globalTaskId = null");
            var assignedTasks = (await globalTasRepository.GetAllItemsAsync()).Where(p => p.Id == globalTaskId.Value).FirstOrDefault().AssignedTasks;
            return mapper.Map<IEnumerable<AssignedTaskDTO>>(assignedTasks);
        }
    }
}
