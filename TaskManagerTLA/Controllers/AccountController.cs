using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagerTLA.BLL.DTO;
using TaskManagerTLA.BLL.Exeption;
using TaskManagerTLA.BLL.Interfaces;
using TaskManagerTLA.Models;

namespace TaskManagerTLA.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMapper mapper;
        private readonly IIdentityService identityService;

        public AccountController(IIdentityService identityService, IMapper mapper)
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
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            UserDTO userDTO = mapper.Map<UserDTO>(model);
            try
            {
                identityService.CreateUserAndRole(userDTO);
            }
            catch (MyException ex)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            UserDTO loginUser = mapper.Map<UserDTO>(model);
            try
            {
                await identityService.Login(loginUser);
            }
            catch (MyException ex)
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
            var usersModels = mapper.Map<IEnumerable<UserDTO>, List<UserViewModel>>(userDTO);
            return View(usersModels);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUser(string userId)
        {
            identityService.DeleteUser(userId);
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
            identityService.UpdateUserRole(userId, roleId);
            return RedirectToAction("ListUser", "Account");
        }
    }
}
