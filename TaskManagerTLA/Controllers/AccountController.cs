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

        public AccountController(IIdentityServices identityService)
        {

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
            mapper = new MapperConfiguration(cfg => cfg.CreateMap<RegisterModel, UserDTO>()).CreateMapper();
            var userDTO = mapper.Map<UserDTO>(model);
            await identityService.MakeUserAsync(userDTO);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginModel { ReturnUrl = returnUrl });
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            mapper = new MapperConfiguration(cfg => cfg.CreateMap<LoginModel, LoginDTO>()).CreateMapper();
            LoginDTO loginDTO = mapper.Map<LoginDTO>(model);
            await identityService.LoginAsync(loginDTO);
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
        public async Task<IActionResult> ListUser()
        {
            IEnumerable<UserDTO> userDTO = await identityService.GetUsersAsync();
            mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserModel>()).CreateMapper();
            var usersModels = mapper.Map<IEnumerable<UserDTO>, List<UserModel>>(userDTO);
            return View(usersModels);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string UserId)
        {
            await identityService.DeleteUserAsync(UserId);
            return RedirectToAction("ListUser", "Account");
        }


        [Authorize(Roles = "Admin")]
        public IActionResult ListRole(string UserId)
        {
            ViewBag.UserId = UserId;
            var rolesDTO = identityService.GetRoles();
            mapper = new MapperConfiguration(cfg => cfg.CreateMap<RoleDTO, RoleModel>()).CreateMapper();
            var rolesModels = mapper.Map<IEnumerable<RoleDTO>, List<RoleModel>>(rolesDTO);
            return View(rolesModels);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeRole(string UserId, string role)
        {
            await identityService.EditUserRoleAsync(UserId, role);
            return RedirectToAction("ListUser", "Account");
        }
    }
}
