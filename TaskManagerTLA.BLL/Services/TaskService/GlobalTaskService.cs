using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagerTLA.BLL.DTO;
using TaskManagerTLA.BLL.Services.TaskService.Interfaces;
using TaskManagerTLA.DAL.Entities;
using TaskManagerTLA.DAL.Repositories.Interfaces;

namespace TaskManagerTLA.BLL.Services.TaskService
{
    public class GlobalTaskService : IGlobalTaskService
    {
        private readonly IGlobalTaskRepo<GlobalTask, int> globalTasRepository;
        private readonly IMapper mapper;
        public GlobalTaskService(IGlobalTaskRepo<GlobalTask, int> globalTasRepository, IMapper mapper)
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

        public async Task DeleteGlobalTaskAsync(int globalTaskId)
        {
            // а для чого ти робиш int?, якщо null не валідне значення?
            /// добав валідацію на 
            //VB десь прочитав що якщо використовуєш в ролі ID int то він обовязково має бути Nullable type з ним типу простіше працювати

            await globalTasRepository.DeleteItemByIdAsync(globalTaskId);
            await globalTasRepository.SaveAsync();
        }

        public async Task<GlobalTaskDTO> GetGlobalTaskAsync(int globalTaskId)
        {
            var globalTask = await globalTasRepository.GetItemByIdAsync(globalTaskId);
            return mapper.Map<GlobalTaskDTO>(globalTask);
        }

        public async Task<IEnumerable<GlobalTaskDTO>> GetGlobalTasksAsync()
        {
            var globalTaskList = await globalTasRepository.GetAllItemsAsync();
            return mapper.Map<IEnumerable<GlobalTaskDTO>>(globalTaskList);
        }

        public async Task UpdateElapsedTimeGlobalTaskAsync(int globalTaskId, int elapsedTime)
        {
            var globalTask = await globalTasRepository.GetItemByIdAsync(globalTaskId);
            globalTask.TotalSpentHours = elapsedTime > 0 ? globalTask.TotalSpentHours + elapsedTime : globalTask.TotalSpentHours;
            await globalTasRepository.SaveAsync();
        }

        public async Task<IEnumerable<AssignedTaskDTO>> GetAssignedTasksByGlobalTaskIdAsync(int globalTaskId)
        {
            var assignedTasks = (await globalTasRepository.GetItemByGlobalTaskIdAsync(globalTaskId)).AssignedTasks;
            return mapper.Map<IEnumerable<AssignedTaskDTO>>(assignedTasks);
        }
    }
}
