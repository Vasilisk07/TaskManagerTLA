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
        private readonly UserManager<IdentityUser> userManager;

        public TaskController(UserManager<IdentityUser> userManager, ITaskService serv)
        {
            this.userManager = userManager;
            taskServise = serv;
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
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<TModel, TaskDTO>()).CreateMapper();
            var tModel = mapper.Map<TaskDTO>(tVievModel);
            taskServise.MakeTask(tModel);

            return RedirectToAction("TaskList", "Task");
        }
        [Authorize(Roles = "Admin, Manager")]
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
        [Authorize(Roles = "Admin, Manager")]
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
        [Authorize(Roles = "Admin, Manager")]
        public async  Task <IActionResult> AddUserInTask(int? id)
        {

            if (id!=null)
            {
                List<IdentityUser> Users = userManager.Users.ToList();
                List<UserModel> users = new List<UserModel>();
                foreach (var item in Users)
                {
                    IEnumerable<string> roles = await userManager.GetRolesAsync(item);
                    string role = roles.First();
                    users.Add(new UserModel { UserName = item.UserName, Email = item.Email, UserRole = role });
                }
                users = (from t in users where t.UserRole == "Developer" select t).ToList();
                ViewBag.TaskId = id;
                ViewBag.TaskName = taskServise.GetTask(id).TaskName;
                return View(users);
            }

            return RedirectToAction("DetailsTask", "Task");
        }
        [Authorize(Roles = "Admin, Manager")]
        public IActionResult AddUser(int? id, string userName)
        {
            ATModel atmodel = new ATModel() { TaskId= (int)id, UserName = userName, TaskName= taskServise.GetTask((int)id).TaskName };

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ATModel, ActualTaskDTO>()).CreateMapper();
            var tModel = mapper.Map<ActualTaskDTO>(atmodel);

            taskServise.MakeActualTask(tModel);

            return RedirectToAction("DetailsTask",new { id } );
            

        }
        [Authorize(Roles = "Admin, Manager")]
        public IActionResult DeleteUserInTask(int? id, int? taskId)
        {
            taskServise.DeleteActualTask(id);

             return RedirectToAction("DetailsTask", new {id = taskId });
        }

        [Authorize(Roles = "Admin, Manager, Developer")]
        public IActionResult ShowPersonalTask()
        {
            List <ActualTaskDTO> curentTasksDTO = (from t in taskServise.GetActTasks() where t.UserName == User.Identity.Name select t).ToList();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ActualTaskDTO, ATModel>()).CreateMapper();
            var curentTasks = mapper.Map<IEnumerable<ActualTaskDTO>, List<ATModel>>(curentTasksDTO);

            return View(curentTasks);
        }


        [HttpGet]
        [Authorize(Roles = "Admin, Manager, Developer")]
        public IActionResult EditPersonalTask(int? id)
        {
           return View(new EditATModel { ActualTaskId=(int)id }) ;
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
