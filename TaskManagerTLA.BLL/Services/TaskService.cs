using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagerTLA.BLL.DTO;
using TaskManagerTLA.BLL.Interfaces;
using TaskManagerTLA.DAL.Entities;
using TaskManagerTLA.DAL.Interfaces;

namespace TaskManagerTLA.BLL.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork dataBase;
        private readonly IMapper mapper;

        public TaskService(IUnitOfWork dataBase, IMapper mapper)
        {
            this.dataBase = dataBase;
            this.mapper = mapper;
        }

        public IEnumerable<ActualTaskDTO> GetActualTasks()
        {

            return mapper.Map<IEnumerable<ActualTask>, List<ActualTaskDTO>>(dataBase.ActualTasks.GetAll());
        }

        public ActualTaskDTO GetActualTask(int? actualTaskId)
        {
            ActualTaskDTO actualTaskDTO = mapper.Map<ActualTaskDTO>(dataBase.ActualTasks.Get(actualTaskId.Value));
            return actualTaskDTO;
        }

        public TaskDTO GetTask(int? taskId)
        {
            // int? треба перевірити на null перед тим як робити taskId.Value, будеш мати ексепшн
            TaskDTO taskDTO = mapper.Map<TaskDTO>(dataBase.Tasks.Get(taskId.Value));
            return taskDTO;
        }

        public IEnumerable<ActualTaskDTO> GetDetailsTask(int? taskId)
        {
            // TODO делегувати запит sql серверу
            // в тебе навіть метод є в репозиторіх готовий        IEnumerable<T> Find(Func<T, Boolean> predicate);

            return from t in GetActualTasks() where t.TaskId == taskId.Value select t;
        }

        public IEnumerable<TaskDTO> GetTasks()
        {
            IEnumerable<TaskDTO> taskListDTO = mapper.Map<IEnumerable<TaskModel>, List<TaskDTO>>(dataBase.Tasks.GetAll());
            return taskListDTO;
        }

        public void MakeActualTask(ActualTaskDTO actualTaskDTO)
        {
            bool ifExist = false;
            var aTask = GetActualTasks();
            foreach (var item in aTask)
            {
                // TODO це правило унікальності можна описати в EF https://stackoverflow.com/questions/18889218/unique-key-constraints-for-multiple-columns-in-entity-framework
                if (item.TaskId == actualTaskDTO.TaskId && item.UserName == actualTaskDTO.UserName)
                {
                    ifExist = true;
                    break;
                }
            }
            // TODO якщо такий таск уже є - ми повинні якось повідомити користувача?
            if (!ifExist)
            {
                ActualTask actualTask = mapper.Map<ActualTask>(actualTaskDTO);
                dataBase.ActualTasks.Create(actualTask);
                dataBase.Save();
            }
        }

        public void MakeTask(TaskDTO taskDTO)
        {
            TaskModel taskModel = mapper.Map<TaskModel>(taskDTO);
            dataBase.Tasks.Create(taskModel);
            dataBase.Save();
        }

        public void DeleteTask(int? taskId)
        {
            dataBase.Tasks.Delete(taskId.Value);
            var actualTaskList = GetActualTasks();
            // TODO потрібно витягти лише один запис з контексту і його видалити
            // маєш в репозиторії готовий метод IEnumerable<T> Find(Func<T, Boolean> predicate);
            foreach (var item in actualTaskList)
            {
                if (item.TaskId == taskId.Value)
                {
                    dataBase.ActualTasks.Delete(item.ActualTaskId);
                }
            }
            dataBase.Save();
        }

        public void EditActualTask(int? actualTaskId, int? elapsedTime, string description)
        {
            ActualTask EditsActualTask = dataBase.ActualTasks.Get(actualTaskId.Value);
            EditsActualTask.Description = description != null ? $" {EditsActualTask.Description} | {DateTime.Now:dd.MM.yyyy} {description}" : EditsActualTask.Description;
            EditsActualTask.ActTaskLeigth = elapsedTime != null && elapsedTime.Value > 0 ? EditsActualTask.ActTaskLeigth + elapsedTime.Value : EditsActualTask.ActTaskLeigth;
            TaskModel EditTask = dataBase.Tasks.Get(EditsActualTask.TaskId);
            EditTask.TaskLeigth = elapsedTime != null && elapsedTime.Value > 0 ? EditTask.TaskLeigth + elapsedTime.Value : EditTask.TaskLeigth;
            dataBase.Save();
        }

        public void DeleteActualTaskByUser(string userName)
        {
            // TODO делегувати запит sql серверу

            foreach (var item in dataBase.ActualTasks.GetAll())
            {
                if (item.UserName == userName)
                {
                    dataBase.ActualTasks.Delete(item.ActualTaskId);
                }
            }
            dataBase.Save();
        }

        public void DeleteActualTask(int? actualTaskId)
        {
            dataBase.ActualTasks.Delete(actualTaskId.Value);
            dataBase.Save();
        }

    }
}
