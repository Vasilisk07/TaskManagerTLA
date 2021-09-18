using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        private IIdentityServices identityService;
        IMapper mapper;

        public TaskController(IIdentityServices identityService, ITaskService serv,IMapper mapper)
        {
            this.identityService = identityService;
            taskServise = serv;
            this.mapper = mapper;
        }


        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        public IActionResult CreateTask()
        {
            return View();
        }


        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public IActionResult CreateTask(TModel tVievModel)
        {
           // mapper = new MapperConfiguration(cfg => cfg.CreateMap<TModel, TaskDTO>()).CreateMapper();
            var tModel = mapper.Map<TaskDTO>(tVievModel);
            taskServise.MakeTask(tModel);
            return RedirectToAction("TaskList", "Task");
        }


        [Authorize(Roles = "Admin, Manager")]
        public IActionResult TaskList()
        {
            IEnumerable<TaskDTO> taskDtos = taskServise.GetTasks();
           // mapper = new MapperConfiguration(cfg => cfg.CreateMap<TaskDTO, TModel>()).CreateMapper();
            var tasks = mapper.Map<IEnumerable<TaskDTO>, List<TModel>>(taskDtos);
            return View(tasks);
        }


        [Authorize(Roles = "Admin, Manager")]
        public IActionResult DeleteTask(int? id)
        {
            taskServise.DeleteTask(id);
            return RedirectToAction("TaskList", "Task");
        }


        [Authorize(Roles = "Admin, Manager")]
        public IActionResult DetailsTask(int? id)
        {
            var ATasks = taskServise.GetActTasks();
           // mapper = new MapperConfiguration(cfg => cfg.CreateMap<ActualTaskDTO, ATModel>()).CreateMapper();
            var tasks = mapper.Map<IEnumerable<ActualTaskDTO>, List<ActualTaskViewModel>>(ATasks);
            var selectedTeams = from t in tasks where t.TaskId == id select t;
            ViewBag.TaskName = taskServise.GetTask(id).TaskName;
            ViewBag.TaskId = id;
            return View(selectedTeams);
        }


        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        public IActionResult AddUserInTask(int? id)
        {
            IEnumerable<UserDTO> usersDTO = identityService.GetUsers();
           // mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserModel>()).CreateMapper();
            var usersModel = mapper.Map<IEnumerable<UserDTO>, List<UserModel>>(usersDTO);
            usersModel = (from t in usersModel where t.UserRole == "Developer" select t).ToList();
            ViewBag.TaskId = id;
            ViewBag.TaskName = taskServise.GetTask(id).TaskName;
            return View(usersModel);
        }


        [Authorize(Roles = "Admin, Manager")]
        public IActionResult AddUser(int? id, string userName)
        {
            ActualTaskViewModel atmodel = new ActualTaskViewModel() { TaskId = (int)id, UserName = userName, TaskName = taskServise.GetTask((int)id).TaskName };
           // mapper = new MapperConfiguration(cfg => cfg.CreateMap<ATModel, ActualTaskDTO>()).CreateMapper();
            var tModel = mapper.Map<ActualTaskDTO>(atmodel);
            taskServise.MakeActualTask(tModel);
            return RedirectToAction("DetailsTask", new { id });
        }


        [Authorize(Roles = "Admin, Manager")]
        public IActionResult DeleteUserInTask(int? id, int? taskId)
        {
            taskServise.DeleteActualTask(id);
            return RedirectToAction("DetailsTask", new { id = taskId });
        }


        [Authorize(Roles = "Admin, Manager, Developer")]
        public IActionResult ShowPersonalTask()
        {
            List<ActualTaskDTO> curentTasksDTO = (from t in taskServise.GetActTasks() where t.UserName == User.Identity.Name select t).ToList();
           // mapper = new MapperConfiguration(cfg => cfg.CreateMap<ActualTaskDTO, ATModel>()).CreateMapper();
            var curentTasks = mapper.Map<IEnumerable<ActualTaskDTO>, List<ActualTaskViewModel>>(curentTasksDTO);
            return View(curentTasks);
        }


        [HttpGet]
        [Authorize(Roles = "Admin, Manager, Developer")]
        public IActionResult EditPersonalTask(int? id)
        {
            return View(new EditATModel { ActualTaskId = (int)id });
        }


        [HttpPost]
        [Authorize(Roles = "Admin, Manager, Developer")]
        public IActionResult EditPersonalTask(EditATModel EditModel)
        {
            taskServise.EditActualTask(EditModel.ActualTaskId, EditModel.ActTaskLeigth, EditModel.Description);
            return RedirectToAction("ShowPersonalTask", "Task");
        }
    }
}
