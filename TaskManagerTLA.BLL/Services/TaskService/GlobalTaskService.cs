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
        private readonly IGlobalTaskUnit DataBase;
        private readonly IMapper Mapper;
        public GlobalTaskService(IGlobalTaskUnit dataBase, IMapper mapper)
        {
            this.DataBase = dataBase;
            this.Mapper = mapper;
        }

        public async Task AddGlobalTaskAsync(GlobalTaskDTO globalTaskDTO)
        {
            GlobalTask taskModel = Mapper.Map<GlobalTask>(globalTaskDTO);
            await DataBase.GlobalTasks.CreateItemAsync(taskModel);
            await DataBase.GlobalTasks.SaveAsync();
        }

        public async Task DeleteGlobalTaskAsync(int? globalTaskId)
        {
            if (globalTaskId == null) throw new ServiceException("Не дійсне значення");
            await DataBase.GlobalTasks.DeleteItemByIdAsync(globalTaskId.Value);
            await DataBase.GlobalTasks.SaveAsync();
        }

        public async Task<GlobalTaskDTO> GetGlobalTaskAsync(int? GlobalTaskId)
        {
            if (GlobalTaskId == null) throw new ServiceException("Не дійсне значення");
            var globalTask = await DataBase.GlobalTasks.GetItemByIdAsync(GlobalTaskId.Value);
            return Mapper.Map<GlobalTaskDTO>(globalTask);
        }

        public async Task<IEnumerable<GlobalTaskDTO>> GetGlobalTasksAsync()
        {
            var globalTaskList = await DataBase.GlobalTasks.GetAllItemsAsync();
            return Mapper.Map<IEnumerable<GlobalTaskDTO>>(globalTaskList);
        }

        public async Task UpdateElapsedTimeGlobalTaskAsync(int? globalTaskId, int? elapsedTime)
        {
            if (globalTaskId == null) throw new ServiceException("Не дійсне значення");
            var globalTask = await DataBase.GlobalTasks.GetItemByIdAsync(globalTaskId.Value);
            globalTask.TotalSpentHours = elapsedTime != null && elapsedTime.Value > 0 ? globalTask.TotalSpentHours + elapsedTime.Value : globalTask.TotalSpentHours;
            await DataBase.GlobalTasks.SaveAsync();
        }

        public async Task<IEnumerable<AssignedTaskDTO>> GetAssignedTasksByGlobalTaskIdAsync(int? globalTaskId)
        {
            if (globalTaskId == null) throw new ServiceException("Не дійсне значення");
            var assignedTasks = (await DataBase.GlobalTasks.GetAllItemsAsync()).Where(p => p.Id == globalTaskId.Value).FirstOrDefault().AssignedTasks;
            return Mapper.Map<IEnumerable<AssignedTaskDTO>>(assignedTasks);
        }
    }
}
