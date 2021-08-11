using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagerTLA.BLL.DTO;
using TaskManagerTLA.BLL.Interfaces;
using TaskManagerTLA.Models;

namespace TaskManagerTLA.Controllers
{
    public class TaskController : Controller
    {
        ITaskService taskServise;
        

        public TaskController(ITaskService serv)
        {
            
            taskServise = serv;
        }
        [HttpGet]
        public IActionResult CreateTask()
        {

            return View();
        }
        [HttpPost]
        public IActionResult CreateTask(TModel tVievModel)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<TModel, TaskDTO>()).CreateMapper();
            var tModel = mapper.Map<TaskDTO>(tVievModel);
            taskServise.MakeTask(tModel);

            return RedirectToAction("TaskList", "Task");
        }

        public IActionResult TaskList()
        {
            IEnumerable<TaskDTO> taskDtos = taskServise.GetTasks();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<TaskDTO, TModel>()).CreateMapper();
            var tasks = mapper.Map<IEnumerable<TaskDTO>, List<TModel>>(taskDtos);

            return View(tasks);
        }

        public IActionResult DeleteTask(int? id)
        {
            if (id!=null)
            {
                taskServise.DeleteTask(id);
            }
            return RedirectToAction("TaskList", "Task");
        }

        public IActionResult DetailsTask(int? id)
        {
           
            
            if (id!=null)
            {
                var ATasks = taskServise.GetActTasks();
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ActualTaskDTO, ATModel>()).CreateMapper();
                var tasks = mapper.Map<IEnumerable<ActualTaskDTO>, List<ATModel>>(ATasks);
                var selectedTeams = from t in tasks where t.TaskId == id select t;
                ViewBag.TaskName = taskServise.GetTask(id).TaskName;
                ViewBag.TaskId = id;

                return View(selectedTeams);
            }


            return RedirectToAction("TaskList", "Task");
        }

        [HttpGet]
        public IActionResult AddUserInTask(int? id)
        {

            if (id!=null)
            {



                return View();
            }




            return RedirectToAction("DetailsTask", "Task");
        }



    }
}
