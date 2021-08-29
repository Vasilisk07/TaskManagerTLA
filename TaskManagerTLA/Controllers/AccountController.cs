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


            //if (ModelState.IsValid)
            //{
            //    IdentityUser user = new IdentityUser { Email = model.Email, UserName = model.Name };
            //    var result = await userManager.CreateAsync(user, model.Password);
            //    if (result.Succeeded)
            //    {
            //        await userManager.AddToRoleAsync(user, "Developer");
            //        await signInManager.SignInAsync(user, false);
            //        return RedirectToAction("Index", "Home");
            //    }
            //    else
            //    {
            //        foreach (var error in result.Errors)
            //        {
            //            ModelState.AddModelError(string.Empty, error.Description);
            //        }
            //    }
            //}
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

            //if (ModelState.IsValid)
            //{
            //    var result =
            //        await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
            //    if (result.Succeeded)
            //    {
            //        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            //        {
            //            return Redirect(model.ReturnUrl);
            //        }
            //        else
            //        {
            //            return RedirectToAction("Index", "Home");
            //        }
            //    }
            //    else
            //    {
            //        ModelState.AddModelError("", "Невірний логін або пароль");
            //    }
            //}
            //return View(model);
            // 
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


            //List<IdentityUser> Users = userManager.Users.ToList();
            //List<UserModel> users = new List<UserModel>();
            //foreach (var item in Users)
            //{
            //    IEnumerable<string> roles = await userManager.GetRolesAsync(item);
            //    string role = roles.First();
            //    users.Add(new UserModel { UserName = item.UserName, Email = item.Email, UserRole = role });
            //}
            return View(usersModels);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string UserId)
        {
            await identityService.DeleteUserAsync(UserId);


            //if (name != User.Identity.Name)
            //{
            //    await userManager.DeleteAsync(await userManager.FindByNameAsync(name));
            //}
            return RedirectToAction("ListUser", "Account");
        }


        [Authorize(Roles = "Admin")]
        public IActionResult ListRole(string UserId)
        {
            ViewBag.UserId = UserId;
            var rolesDTO = identityService.GetRoles();
            mapper = new MapperConfiguration(cfg => cfg.CreateMap<RoleDTO, RoleModel>()).CreateMapper();
            var rolesModels = mapper.Map<IEnumerable<RoleDTO>, List<RoleModel>>(rolesDTO);

            //var mapper = new MapperConfiguration(cfg => cfg.CreateMap<IdentityRole, Role>().ForMember("UserRole", opt => opt.MapFrom(c => c.Name))).CreateMapper();
            //var roles = mapper.Map<IEnumerable<IdentityRole>, List<Role>>(roleManager.Roles);
            //ViewBag.UserId = UserId;
            return View(rolesModels);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeRole(string UserId, string role)
        {

            await identityService.EditUserRoleAsync(UserId, role);

            //IdentityUser User = await userManager.FindByNameAsync(name);
            //await userManager.RemoveFromRoleAsync(User, (await userManager.GetRolesAsync(User)).First());
            //await userManager.AddToRoleAsync(User, role);
            return RedirectToAction("ListUser", "Account");
        }
    }
}
