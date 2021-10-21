using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TaskManagerTLA.BLL.DTO;
using TaskManagerTLA.BLL.Exeption;
using TaskManagerTLA.BLL.Interfaces;
using TaskManagerTLA.Models;

namespace TaskManagerTLA.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskService taskService;
        private readonly IIdentityService identityService;
        private readonly IMapper mapper;

        public TaskController(IIdentityService identityService, ITaskService taskService, IMapper mapper)
        {
            this.identityService = identityService;
            this.taskService = taskService;
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
        public IActionResult CreateTask(GlobalTaskViewModel taskVievModel)
        {
            var taskDTO = mapper.Map<GlobalTaskDTO>(taskVievModel);
            taskService.AddGlobalTask(taskDTO);
            return RedirectToAction("TaskList", "Task");
        }

        [Authorize(Roles = "Admin, Manager")]
        public IActionResult TaskList()
        {
            var tasks = mapper.Map<IEnumerable<GlobalTaskDTO>, List<GlobalTaskViewModel>>(taskService.GetGlobalTasks());
            return View(tasks);
        }

        [Authorize(Roles = "Admin, Manager")]
        public IActionResult DeleteTask(int? globalTaskId)
        {
            try
            {
                taskService.DeleteGlobalTask(globalTaskId);
                return RedirectToAction("TaskList", "Task");
            }
            catch (MyException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        [Authorize(Roles = "Admin, Manager")]
        public IActionResult DetailAboutTask(int? globalTaskId)
        {
            try
            {
                var selectedTeams = mapper.Map<IEnumerable<AssignedTaskDTO>, List<AssignedTaskViewModel>>(taskService.GetAssignedTasksByGlobalTaskId(globalTaskId));
                ViewBag.TaskName = taskService.GetGlobalTask(globalTaskId).Name;
                ViewBag.globalTaskId = globalTaskId;
                return View(selectedTeams);
            }
            catch (MyException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        public IActionResult SelectUserToAssignTask(int? globalTaskId)
        {
            try
            {
                IEnumerable<UserDTO> usersDTO = identityService.GetUsersWhoAreNotAssignedTask(globalTaskId);
                var usersModel = mapper.Map<IEnumerable<UserDTO>, List<UserViewModel>>(usersDTO);
                ViewBag.globalTaskId = globalTaskId;
                ViewBag.TaskName = taskService.GetGlobalTask(globalTaskId).Name;
                return View(usersModel);
            }
            catch (MyException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }

        }

        [Authorize(Roles = "Admin, Manager")]
        public IActionResult AssignTaskToUser(string userId, int? globalTaskId)
        {
            try
            {
                taskService.CreateAssignedTask(userId, globalTaskId);
                return RedirectToAction("DetailAboutTask", new { globalTaskId });
            }
            catch (MyException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        [Authorize(Roles = "Admin, Manager")]
        public IActionResult CancelAssignedTask(string userId, int? globalTaskId)
        {
            try
            {
                taskService.DeleteAssignedTask(userId, globalTaskId);
                return RedirectToAction("DetailAboutTask", new { globalTaskId });
            }
            catch (MyException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        [Authorize(Roles = "Admin, Manager, Developer")]
        public IActionResult ShowAssignedTaskCurrentUser()
        {
            var personalTasksDTOList = taskService.GetAssignedTasksByUserName(User.Identity.Name).ToList();
            var personalTasksViewList = mapper.Map<IEnumerable<AssignedTaskDTO>, List<AssignedTaskViewModel>>(personalTasksDTOList);
            return View(personalTasksViewList);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Manager, Developer")]
        public IActionResult EnterDataIntoAssignedTask(string userId, int? globalTaskId)
        {
            try
            {
                ViewBag.TaskName = taskService.GetGlobalTask(globalTaskId).Name;
                return View(new EditAssignedTaskViewModel { UserId = userId, GlobalTaskId = globalTaskId.Value });
            }
            catch (MyException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Manager, Developer")]
        public IActionResult EnterDataIntoAssignedTask(EditAssignedTaskViewModel EditModel)
        {
            taskService.UpdateDescription(EditModel.UserId, EditModel.GlobalTaskId, EditModel.Description);
            taskService.UpdateElapsedTime(EditModel.UserId, EditModel.GlobalTaskId, EditModel.SpentHours);
            return RedirectToAction("ShowAssignedTaskCurrentUser", "Task");
        }
    }
}
