using AutoMapper;
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
    public class GlobalTaskService : IGlobalTaskService
    {
        private readonly IGlobalTaskUnit globalTaskUnit;
        private readonly IMapper mapper;
        public GlobalTaskService(IGlobalTaskUnit globalTaskUnit, IMapper mapper)
        {
            this.globalTaskUnit = globalTaskUnit;
            this.mapper = mapper;
        }

        public async Task AddGlobalTaskAsync(GlobalTaskDTO globalTaskDTO)
        {
            GlobalTask taskModel = mapper.Map<GlobalTask>(globalTaskDTO);
            await globalTaskUnit.GlobalTasks.CreateItemAsync(taskModel);
            await globalTaskUnit.GlobalTasks.SaveAsync();
        }

        public async Task DeleteGlobalTaskAsync(int? globalTaskId)
        {
            if (globalTaskId == null) throw new ServiceException("Не дійсне значення: globalTaskId = null");
            await globalTaskUnit.GlobalTasks.DeleteItemByIdAsync(globalTaskId.Value);
            await globalTaskUnit.GlobalTasks.SaveAsync();
        }

        public async Task<GlobalTaskDTO> GetGlobalTaskAsync(int? globalTaskId)
        {
            if (globalTaskId == null) throw new ServiceException("Не дійсне значення: globalTaskId = null");
            var globalTask = await globalTaskUnit.GlobalTasks.GetItemByIdAsync(globalTaskId.Value);
            return mapper.Map<GlobalTaskDTO>(globalTask);
        }

        public async Task<IEnumerable<GlobalTaskDTO>> GetGlobalTasksAsync()
        {
            var globalTaskList = await globalTaskUnit.GlobalTasks.GetAllItemsAsync();
            return mapper.Map<IEnumerable<GlobalTaskDTO>>(globalTaskList);
        }

        public async Task UpdateElapsedTimeGlobalTaskAsync(int? globalTaskId, int? elapsedTime)
        {
            if (globalTaskId == null) throw new ServiceException("Не дійсне значення: globalTaskId = null");
            var globalTask = await globalTaskUnit.GlobalTasks.GetItemByIdAsync(globalTaskId.Value);
            globalTask.TotalSpentHours = elapsedTime != null && elapsedTime.Value > 0 ? globalTask.TotalSpentHours + elapsedTime.Value : globalTask.TotalSpentHours;
            await globalTaskUnit.GlobalTasks.SaveAsync();
        }

        public async Task<IEnumerable<AssignedTaskDTO>> GetAssignedTasksByGlobalTaskIdAsync(int? globalTaskId)
        {
            if (globalTaskId == null) throw new ServiceException("Не дійсне значення: globalTaskId = null");
            var assignedTasks = (await globalTaskUnit.GlobalTasks.GetAllItemsAsync()).Where(p => p.Id == globalTaskId.Value).FirstOrDefault().AssignedTasks;
            return mapper.Map<IEnumerable<AssignedTaskDTO>>(assignedTasks);
        }
    }
}
