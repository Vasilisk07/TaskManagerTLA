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
        private readonly ITaskService taskServise;
        private readonly IIdentityServices identityService;
        private readonly IMapper mapper;

        public TaskController(IIdentityServices identityService, ITaskService taskService, IMapper mapper)
        {
            this.identityService = identityService;
            taskServise = taskService;
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
        public IActionResult CreateTask(TaskViewModel taskVievModel)
        {
            var taskDTO = mapper.Map<TaskDTO>(taskVievModel);
            taskServise.MakeTask(taskDTO);
            return RedirectToAction("TaskList", "Task");
        }

        [Authorize(Roles = "Admin, Manager")]
        public IActionResult TaskList()
        {
            var tasks = mapper.Map<IEnumerable<TaskDTO>, List<TaskViewModel>>(taskServise.GetTasks());
            return View(tasks);
        }

        [Authorize(Roles = "Admin, Manager")]
        public IActionResult DeleteTask(int? taskId)
        {
            taskServise.DeleteTask(taskId);
            return RedirectToAction("TaskList", "Task");
        }

        [Authorize(Roles = "Admin, Manager")]
        public IActionResult DetailsTask(int? taskId)
        {
            var selectedTeams = mapper.Map<IEnumerable<ActualTaskDTO>, List<ActualTaskViewModel>>(taskServise.GetDetailsTask(taskId));
            ViewBag.TaskName = taskServise.GetTask(taskId).TaskName;
            ViewBag.TaskId = taskId;
            return View(selectedTeams);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        public IActionResult AddUserInTask(int? taskId)
        {
            IEnumerable<UserDTO> usersDTO = identityService.GetUsers();
            var usersModel = mapper.Map<IEnumerable<UserDTO>, List<UserViewModel>>(usersDTO);
            usersModel = (from t in usersModel where t.UserRole == "Developer" select t).ToList();
            ViewBag.TaskId = taskId;
            ViewBag.TaskName = taskServise.GetTask(taskId).TaskName;
            return View(usersModel);
        }

        [Authorize(Roles = "Admin, Manager")]
        public IActionResult AddUser(int? taskId, string userName)
        {
            ActualTaskViewModel actualTaskModel = new ActualTaskViewModel() { TaskId = taskId.Value, UserName = userName, TaskName = taskServise.GetTask(taskId).TaskName };
            var taskDTO = mapper.Map<ActualTaskDTO>(actualTaskModel);
            taskServise.MakeActualTask(taskDTO);
            return RedirectToAction("DetailsTask", new { taskId });
        }

        [Authorize(Roles = "Admin, Manager")]
        public IActionResult DeleteUserInTask(int? actualTaskId, int? taskId)
        {
            taskServise.DeleteActualTask(actualTaskId);
            return RedirectToAction("DetailsTask", new { taskId });
        }

        [Authorize(Roles = "Admin, Manager, Developer")]
        public IActionResult ShowPersonalTask()
        {
            List<ActualTaskDTO> personalTasksDTOList = (from t in taskServise.GetActualTasks() where t.UserName == User.Identity.Name select t).ToList();
            var personalTasksViewList = mapper.Map<IEnumerable<ActualTaskDTO>, List<ActualTaskViewModel>>(personalTasksDTOList);
            return View(personalTasksViewList);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Manager, Developer")]
        public IActionResult EditPersonalTask(int? actualTaskId)
        {
            return View(new EditActualTaskViewModel { ActualTaskId = actualTaskId.Value });
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Manager, Developer")]
        public IActionResult EditPersonalTask(EditActualTaskViewModel EditModel)
        {
            taskServise.EditActualTask(EditModel.ActualTaskId, EditModel.ActTaskLeigth, EditModel.Description);
            return RedirectToAction("ShowPersonalTask", "Task");
        }
    }
}
