using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagerTLA.BLL.DTO;
using TaskManagerTLA.BLL.Exeption;
using TaskManagerTLA.BLL.Services.IdentityService.Interfaces;
using TaskManagerTLA.Models;

namespace TaskManagerTLA.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMapper mapper;
        private readonly IAuthService loginService;
        private readonly IRoleService rolesService;
        private readonly IUserRoleService userRoleService;
        private readonly IUserService userService;

        public AccountController(IAuthService loginService, IRoleService rolesService, IUserRoleService userRoleService, IUserService userService, IMapper mapper)
        {
            this.mapper = mapper;
            this.loginService = loginService;
            this.rolesService = rolesService;
            this.userRoleService = userRoleService;
            this.userService = userService;
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
                // бажано зробити цей виклик async
                var roleDTO = rolesService.GetRoleByName("Developer");
                userService.CreateUserAndRole(userDTO, roleDTO);
            }
            catch (LoginException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
            await LoginAsync(new LoginViewModel { UserName = model.UserName, Password = model.Password });
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
        public async Task<IActionResult> LoginAsync(LoginViewModel model)
        {
            UserDTO loginUser = mapper.Map<UserDTO>(model);
            try
            {
                //! асинхронні методи повинні закінчуватись на Async => LoginAsync()
                //
                await loginService.LoginAsync(loginUser);
            }
            catch (LoginException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogoutAsync()
        {
            // асинхронні методи повинні закінчуватись на Async
            await loginService.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ListUser()
        {
            IEnumerable<UserDTO> userDTO = userService.GetUsers();
            var usersModels = mapper.Map<IEnumerable<UserDTO>, List<UserViewModel>>(userDTO);
            return View(usersModels);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUser(string userId)
        {
            // може бути async
            userService.DeleteUser(userId);
            return RedirectToAction("ListUser", "Account");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ListRole(string userId)
        {
            ViewBag.UserId = userId;
            var roleModels = mapper.Map<IEnumerable<RoleDTO>, List<RoleViewModel>>(rolesService.GetRoles());
            return View(roleModels);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ChangeRole(string userId, string roleId)
        {
            // може бути async
            userRoleService.UpdateUserRole(userId, roleId);
            return RedirectToAction("ListUser", "Account");
        }
    }
}
