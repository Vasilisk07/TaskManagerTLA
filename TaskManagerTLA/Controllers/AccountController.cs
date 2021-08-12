using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagerTLA.Models;

namespace TaskManagerTLA.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser { Email = model.Email, UserName = model.Name};
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Developer");
                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

            }
            return View(model);
        }


        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await signInManager.PasswordSignInAsync(model.Name, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                   
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Невірний логін або пароль");
                }
            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ListUser()
        {


            List<IdentityUser> Users = userManager.Users.ToList();
            List<UserModel> users= new List<UserModel>();
            foreach (var item in Users)
            {
                IEnumerable<string> roles = await userManager.GetRolesAsync(item);
                string role = roles.First();
                users.Add(new UserModel { UserName = item.UserName, Email=item.Email, UserRole = role });
            }
            return View(users);
        }



        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string name)
        {
            if(name!=User.Identity.Name)
            {
                await userManager.DeleteAsync(await userManager.FindByNameAsync(name));
            }
            return RedirectToAction("ListUser", "Account");
        }



        [Authorize(Roles = "Admin")]
        public  IActionResult ListRole(string name)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<IdentityRole,Role >().ForMember("UserRole",opt =>opt.MapFrom(c=>c.Name))).CreateMapper();
            var roles = mapper.Map<IEnumerable<IdentityRole>, List<Role>>(roleManager.Roles);
            ViewBag.UserName = name;
            return View(roles);
        }




        [Authorize(Roles = "Admin")]
        public async Task <IActionResult> ChangeRole(string name,string role)
        {
            IdentityUser User = await userManager.FindByNameAsync(name);
            await userManager.RemoveFromRoleAsync(User, (await userManager.GetRolesAsync(User)).First());
            await userManager.AddToRoleAsync(User, role);
            return RedirectToAction("ListUser", "Account");
        }


      


        public IActionResult Index()
        {
            return View();
        }
    }
}
