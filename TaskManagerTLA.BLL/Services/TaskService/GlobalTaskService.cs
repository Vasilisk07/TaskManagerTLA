using AutoMapper;
using System.Collections.Generic;
using System.Linq;
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

        public void AddGlobalTask(GlobalTaskDTO globalTaskDTO)
        {
            GlobalTask taskModel = Mapper.Map<GlobalTask>(globalTaskDTO);
            DataBase.GlobalTasks.CreateItem(taskModel);
            DataBase.GlobalTasks.Save();
        }

        public void DeleteGlobalTask(int? globalTaskId)
        {
            if (globalTaskId == null) throw new ServiceException("Не дійсне значення");
            DataBase.GlobalTasks.DeleteItemById(globalTaskId.Value);
            DataBase.GlobalTasks.Save();
        }

        public GlobalTaskDTO GetGlobalTask(int? GlobalTaskId)
        {
            if (GlobalTaskId == null) throw new ServiceException("Не дійсне значення");
            var globalTask = DataBase.GlobalTasks.GetItemById(GlobalTaskId.Value);
            return Mapper.Map<GlobalTaskDTO>(globalTask);
        }

        public IEnumerable<GlobalTaskDTO> GetGlobalTasks()
        {
            var globalTaskList = DataBase.GlobalTasks.GetAllItems();
            return Mapper.Map<IEnumerable<GlobalTaskDTO>>(globalTaskList);
        }

        public void UpdateElapsedTimeGlobalTask(int? globalTaskId, int? elapsedTime)
        {
            if (globalTaskId == null) throw new ServiceException("Не дійсне значення");
            var globalTask = DataBase.GlobalTasks.GetItemById(globalTaskId.Value);
            globalTask.TotalSpentHours = elapsedTime != null && elapsedTime.Value > 0 ? globalTask.TotalSpentHours + elapsedTime.Value : globalTask.TotalSpentHours;
            DataBase.GlobalTasks.Save();
        }

        public IEnumerable<AssignedTaskDTO> GetAssignedTasksByGlobalTaskId(int? globalTaskId)
        {
            if (globalTaskId == null) throw new ServiceException("Не дійсне значення");
            var assignedTasks = DataBase.GlobalTasks.GetAllItems().Where(p=>p.Id == globalTaskId.Value).FirstOrDefault().AssignedTasks;
            return Mapper.Map<IEnumerable<AssignedTaskDTO>>(assignedTasks);
        }
    }
}
