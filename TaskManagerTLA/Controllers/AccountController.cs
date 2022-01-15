using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IUserService userService;
        public AccountController(IAuthService loginService, IRoleService rolesService, IUserService userService, IMapper mapper)
        {
            this.mapper = mapper;
            this.loginService = loginService;
            this.rolesService = rolesService;
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
        {
            UserDTO userDTO = mapper.Map<UserDTO>(model);
            try
            {
                await userService.CreateUserAndRoleAsync(userDTO, null);
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
                await loginService.LogoutAsync();
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
        public async Task<RedirectToActionResult> LogoutAsync()
        {
            await loginService.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserViewModel>>> ListUserAsync()
        {
            IEnumerable<UserDTO> userDTO = await userService.GetUsersAsync();
            var usersModels = mapper.Map<IEnumerable<UserDTO>, List<UserViewModel>>(userDTO);
            return View(usersModels);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ActionName("DeleteUser")]
        public async Task<IActionResult> ConfirmDeleteUserAsync(string id)
        {
            var userDTO = (await userService.GetUsersAsync()).Where(p => p.Id == id).FirstOrDefault();
            var userModel = mapper.Map<UserViewModel>(userDTO);
            return View(userModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<RedirectToActionResult> DeleteUserAsync(string id)
        {
            await userService.DeleteUserAsync(id);
            TempData["UserInfo"] = $"Користувача видалено.";
            return RedirectToAction("ListUser", "Account");
        }


        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<RoleViewModel>>> ListRoleAsync(string userId)
        {
            ViewBag.UserId = userId;
            var listRole = await rolesService.GetRolesAsync();
            var roleModels = mapper.Map<IEnumerable<RoleDTO>, List<RoleViewModel>>(listRole);
            return View(roleModels);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserViewModel>>> UserEditingAsync(string userId)
        {
            var user = await userService.GetUserByIdAsync(userId);
            var roles = await userService.GetUserRolesAsync(userId);
            var userView = mapper.Map<UserViewModel>(user);
            userView.Roles = mapper.Map<IEnumerable<RoleDTO>, List<RoleViewModel>>(roles);
            return View(userView);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserEditingAsync(UserViewModel userEdit)
        {
            try
            {
                UserDTO userDto = mapper.Map<UserDTO>(userEdit);
                await userService.UserUpdateAsync(userDto);
                TempData["UserInfo"] = $"Здійснена успішна зміна персональних данних користувача.";
            }
            catch (ServiceException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(userEdit);
            }
            return RedirectToAction("ListUser", "Account");
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeRoleAsync(string id)
        {
            ViewBag.UserId = id;
            var userRolesDb = await userService.GetUserRolesAsync(id);
            var userRolesView = mapper.Map<IEnumerable<RoleDTO>, List<RoleViewModel>>(userRolesDb);
            return View(userRolesView);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ChangeRoleAsync(string[] roles, string userId)
        {

            await userService.ChangeUserRolesAsync(userId, roles);
            TempData["RoleInfo"] = $"Здійснена успішна зміна ролей користувача.";
            return RedirectToAction("UserEditing", "Account", new { userId });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddNewUser()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewUser(RegisterViewModel model, string role)
        {
            UserDTO userDTO = mapper.Map<UserDTO>(model);
            try
            {
                await userService.CreateUserAndRoleAsync(userDTO, role);
                TempData["UserInfo"] = $"Користувач під ніком {userDTO.UserName} успішно створений.";
            }
            catch (LoginException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }

            return RedirectToAction("ListUser", "Account");
        }
    }
}
