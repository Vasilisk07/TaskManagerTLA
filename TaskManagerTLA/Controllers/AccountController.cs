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

        IMapper mapper;
        private IIdentityServices identityService;
        public AccountController(IIdentityServices identityService, IMapper mapper)
        {
            this.mapper = mapper;
            this.identityService = identityService;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
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
            await Login(new LoginModel { UserName = model.UserName, Password = model.Password });
            return RedirectToAction("Index", "Home");

        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // довго не міг зрозуміти як обійтись без логіки в данному методі, в результаті вирішив обійти це так.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            LoginDTO loginUser = mapper.Map<LoginDTO>(model);
            try
            {
                await identityService.Login(loginUser);
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
            await identityService.Logout();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ListUser()
        {
            IEnumerable<UserDTO> userDTO = identityService.GetUsers();
            var usersModels = mapper.Map<IEnumerable<UserDTO>, List<UserModel>>(userDTO);
            return View(usersModels);
        }


        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUser(string UserId)
        {
            identityService.DeleteUser(UserId);
            return RedirectToAction("ListUser", "Account");
        }


        [Authorize(Roles = "Admin")]
        public IActionResult ListRole(string UserId)
        {
            ViewBag.UserId = UserId;
            var rolesDTO = identityService.GetRoles();
            var rolesModels = mapper.Map<IEnumerable<RoleDTO>, List<RoleModel>>(rolesDTO);
            return View(rolesModels);
        }


        [Authorize(Roles = "Admin")]
        public IActionResult ChangeRole(string UserId, string roleId)
        {
            identityService.ChangeUserRole(UserId, roleId);
            return RedirectToAction("ListUser", "Account");
        }
    }
}
