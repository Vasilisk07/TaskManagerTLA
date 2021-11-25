using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagerTLA.BLL.DTO;
using TaskManagerTLA.BLL.Exeption;
using TaskManagerTLA.BLL.Services.IdentityService.Interfaces;
using TaskManagerTLA.BLL.Services.TaskService.Interfaces;
using TaskManagerTLA.Models;

namespace TaskManagerTLA.Controllers
{
    public class TaskController : Controller
    {

        private readonly IGlobalTaskService GlobalTaskaskService;
        private readonly IAssignedTaskService AssignedTaskService;
        private readonly IUserService UserService;
        private readonly IMapper Mapper;

        public TaskController(IGlobalTaskService globalTaskService, IAssignedTaskService assignedTaskService, IUserService userService, IMapper mapper)
        {
            GlobalTaskaskService = globalTaskService;
            AssignedTaskService = assignedTaskService;
            UserService = userService;
            this.Mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        public IActionResult CreateTask()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> CreateTaskAsync(GlobalTaskViewModel taskVievModel)
        {
            var taskDTO = Mapper.Map<GlobalTaskDTO>(taskVievModel);
            await GlobalTaskaskService.AddGlobalTaskAsync(taskDTO);
            return RedirectToAction("TaskList", "Task");
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<IEnumerable<GlobalTaskViewModel>>> TaskListAsync()
        {
            var globalTaskDTO = await GlobalTaskaskService.GetGlobalTasksAsync();
            var tasks = Mapper.Map<IEnumerable<GlobalTaskDTO>, List<GlobalTaskViewModel>>(globalTaskDTO);
            return View(tasks);
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> DeleteTaskAsync(int? globalTaskId)
        {
            try
            {

                await GlobalTaskaskService.DeleteGlobalTaskAsync(globalTaskId);
                return RedirectToAction("TaskList", "Task");
            }
            catch (ServiceException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<IEnumerable<AssignedTaskViewModel>>> DetailAboutTaskAsync(int? globalTaskId)
        {
            try
            {
                var selectedTeamsDTO = await GlobalTaskaskService.GetAssignedTasksByGlobalTaskIdAsync(globalTaskId);
                var selectedTeams = Mapper.Map<IEnumerable<AssignedTaskDTO>, List<AssignedTaskViewModel>>(selectedTeamsDTO);
                ViewBag.TaskName = (await GlobalTaskaskService.GetGlobalTaskAsync(globalTaskId)).Name;
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
        public async Task<ActionResult<IEnumerable<UserViewModel>>> SelectUserToAssignTaskAsync(int? globalTaskId)
        {
            try
            {
                IEnumerable<UserDTO> usersDTO = await UserService.GetUsersWhoAreNotAssignedTaskAsync(globalTaskId);
                var usersModel = Mapper.Map<IEnumerable<UserDTO>, List<UserViewModel>>(usersDTO);
                ViewBag.globalTaskId = globalTaskId;
                ViewBag.TaskName = (await GlobalTaskaskService.GetGlobalTaskAsync(globalTaskId)).Name;
                return View(usersModel);
            }
            catch (ServiceException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }

        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> AssignTaskToUserAsync(string userId, int? globalTaskId)
        {
            try
            {
                await AssignedTaskService.CreateAssignedTaskAsync(userId, globalTaskId);
                return RedirectToAction("DetailAboutTask", new { globalTaskId });
            }
            catch (ServiceException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> CancelAssignedTaskAsync(string userId, int? globalTaskId)
        {
            try
            {
                await AssignedTaskService.DeleteAssignedTaskAsync(userId, globalTaskId);
                return RedirectToAction("DetailAboutTask", new { globalTaskId });
            }
            catch (ServiceException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        [Authorize(Roles = "Admin, Manager, Developer")]
        public async Task<ActionResult<IEnumerable<AssignedTaskViewModel>>> ShowAssignedTaskCurrentUserAsync()
        {
            var personalTasksDTOList = (await AssignedTaskService.GetAssignedTasksByUserNameAsync(User.Identity.Name)).ToList();
            var personalTasksViewList = Mapper.Map<IEnumerable<AssignedTaskDTO>, List<AssignedTaskViewModel>>(personalTasksDTOList);
            return View(personalTasksViewList);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Manager, Developer")]
        public async Task<ActionResult<EditAssignedTaskViewModel>> EnterDataIntoAssignedTaskAsync(string userId, int? globalTaskId)
        {
            try
            {
                ViewBag.TaskName = (await GlobalTaskaskService.GetGlobalTaskAsync(globalTaskId)).Name;
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
        public async Task<IActionResult> UpdateAssignedTask(EditAssignedTaskViewModel EditModel)
        {
            await AssignedTaskService.UpdateDescriptionAsync(EditModel.UserId, EditModel.GlobalTaskId, EditModel.Description);
            await AssignedTaskService.UpdateElapsedTimeAssignedTaskAsync(EditModel.UserId, EditModel.GlobalTaskId, EditModel.SpentHours);
            await GlobalTaskaskService.UpdateElapsedTimeGlobalTaskAsync(EditModel.GlobalTaskId, EditModel.SpentHours);
            return RedirectToAction("ShowAssignedTaskCurrentUser", "Task");
        }
    }
}
