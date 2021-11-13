using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TaskManagerTLA.BLL.DTO;
using TaskManagerTLA.BLL.Exeption;
using TaskManagerTLA.BLL.Services.IdentityService.Interfaces;
using TaskManagerTLA.BLL.Services.TaskService.Interfaces;
using TaskManagerTLA.Models;

namespace TaskManagerTLA.Controllers
{
    public class TaskController : Controller
    {
        //! taskService => TaskService; - з маленької щащвиай пишуть лише аргументи функцій і локальні змінні https://docs.microsoft.com/ru-ru/dotnet/csharp/fundamentals/coding-style/coding-conventions
        private readonly IGlobalTaskService GlobalTaskaskService;
        private readonly IAssignedTaskService AssignedTaskService;
        private readonly IUserService UserService;
        private readonly IMapper mapper;

        public TaskController(IGlobalTaskService globalTaskService, IAssignedTaskService assignedTaskService,IUserService userService, IMapper mapper)
        {
            GlobalTaskaskService = globalTaskService;
            AssignedTaskService = assignedTaskService;
            UserService = userService;
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
            // async
            GlobalTaskaskService.AddGlobalTask(taskDTO);
            return RedirectToAction("TaskList", "Task");
        }

        [Authorize(Roles = "Admin, Manager")]
        public IActionResult TaskList()
        {
            var tasks = mapper.Map<IEnumerable<GlobalTaskDTO>, List<GlobalTaskViewModel>>(GlobalTaskaskService.GetGlobalTasks());
            return View(tasks);
        }

        [Authorize(Roles = "Admin, Manager")]
        public IActionResult DeleteTask(int? globalTaskId)
        {
            try
            {
                // async
                GlobalTaskaskService.DeleteGlobalTask(globalTaskId);
                return RedirectToAction("TaskList", "Task");
            }
            catch (ServiceException ex)
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
                var selectedTeams = mapper.Map<IEnumerable<AssignedTaskDTO>, List<AssignedTaskViewModel>>(GlobalTaskaskService.GetAssignedTasksByGlobalTaskId(globalTaskId));
                // async
                ViewBag.TaskName = GlobalTaskaskService.GetGlobalTask(globalTaskId).Name;
                ViewBag.globalTaskId = globalTaskId;
                return View(selectedTeams);
            }
            catch (ServiceException ex)
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
                IEnumerable<UserDTO> usersDTO = UserService.GetUsersWhoAreNotAssignedTask(globalTaskId);
                var usersModel = mapper.Map<IEnumerable<UserDTO>, List<UserViewModel>>(usersDTO);
                ViewBag.globalTaskId = globalTaskId;
                // async
                ViewBag.TaskName = GlobalTaskaskService.GetGlobalTask(globalTaskId).Name;
                return View(usersModel);
            }
            catch (ServiceException ex)
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
                // async
                AssignedTaskService.CreateAssignedTask(userId, globalTaskId);
                return RedirectToAction("DetailAboutTask", new { globalTaskId });
            }
            catch (ServiceException ex)
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
                // async
                AssignedTaskService.DeleteAssignedTask(userId, globalTaskId);
                return RedirectToAction("DetailAboutTask", new { globalTaskId });
            }
            catch (ServiceException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        [Authorize(Roles = "Admin, Manager, Developer")]
        public IActionResult ShowAssignedTaskCurrentUser()
        {
                // async
            var personalTasksDTOList = AssignedTaskService.GetAssignedTasksByUserName(User.Identity.Name).ToList();
            var personalTasksViewList = mapper.Map<IEnumerable<AssignedTaskDTO>, List<AssignedTaskViewModel>>(personalTasksDTOList);
            return View(personalTasksViewList);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Manager, Developer")]
        public IActionResult EnterDataIntoAssignedTask(string userId, int? globalTaskId)
        {
            try
            {
                // async
                ViewBag.TaskName = GlobalTaskaskService.GetGlobalTask(globalTaskId).Name;
                return View(new EditAssignedTaskViewModel { UserId = userId, GlobalTaskId = globalTaskId.Value });
            }
            catch (ServiceException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Manager, Developer")]
        public IActionResult EnterDataIntoAssignedTask(EditAssignedTaskViewModel EditModel) // краще назвати UpdateAssignedTask
        {
            // async
            AssignedTaskService.UpdateDescription(EditModel.UserId, EditModel.GlobalTaskId, EditModel.Description);
            // async
            AssignedTaskService.UpdateElapsedTimeAssignedTask(EditModel.UserId, EditModel.GlobalTaskId, EditModel.SpentHours);
            GlobalTaskaskService.UpdateElapsedTimeGlobalTask(EditModel.GlobalTaskId, EditModel.SpentHours);

            return RedirectToAction("ShowAssignedTaskCurrentUser", "Task");
        }
    }
}
