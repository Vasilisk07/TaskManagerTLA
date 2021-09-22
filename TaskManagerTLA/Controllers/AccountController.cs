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
    public class AccountController : Controller
    {

        private readonly IMapper mapper;
        private readonly IIdentityServices identityService;
        private readonly ITaskService taskService;
        private readonly SignInManager<IdentityUser> signInManager;
        public AccountController(IIdentityServices identityService, IMapper mapper,SignInManager<IdentityUser> signInManager,ITaskService taskService)
        {
            this.mapper = mapper;
            this.identityService = identityService;
            this.signInManager = signInManager;
            this.taskService = taskService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            UserDTO userDTO = mapper.Map<UserDTO>(model);
            try
            {
                identityService.RegisterNewUser(userDTO);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
            await Login(new LoginViewModel { UserName = model.UserName, Password = model.Password });
            return RedirectToAction("Index", "Home");

        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // довго не міг зрозуміти як обійтись без логіки в данному методі, в результаті вирішив обійти це наступним чином.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            UserDTO loginUser = mapper.Map<UserDTO>(model);
            try
            {
                await identityService.Login(loginUser, signInManager);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await identityService.Logout(signInManager);
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ListUser()
        {
            IEnumerable<UserDTO> userDTO = identityService.GetUsers();
            var usersModels = mapper.Map<IEnumerable<UserDTO>, List<UserViewModel>>(userDTO);
            return View(usersModels);
        }


        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUser(string userId)
        {
            identityService.DeleteUser(userId, taskService);
            return RedirectToAction("ListUser", "Account");
        }


        [Authorize(Roles = "Admin")]
        public IActionResult ListRole(string userId)
        {
            ViewBag.UserId = userId;
            var roleModels = mapper.Map<IEnumerable<RoleDTO>, List<RoleViewModel>>(identityService.GetRoles());
            return View(roleModels);
        }


        [Authorize(Roles = "Admin")]
        public IActionResult ChangeRole(string userId, string roleId)
        {
            identityService.ChangeUserRole(userId, roleId);
            return RedirectToAction("ListUser", "Account");
        }
    }
}
