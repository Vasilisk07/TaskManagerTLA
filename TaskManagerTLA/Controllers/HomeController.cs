using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TaskManagerTLA.BLL.DTO;
using TaskManagerTLA.BLL.Interfaces;
using TaskManagerTLA.Models;

namespace TaskManagerTLA.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        public HomeController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }


        public async Task< IActionResult> Index()
        {
            ViewBag.Info = "";
            if (HttpContext.User.Identity.IsAuthenticated)
            {
               IdentityUser user = await userManager.GetUserAsync(User);
                IEnumerable<string> roles = await userManager.GetRolesAsync(user);
                string role = roles.First();
                switch (role)
                {
                    case "Admin":
                        ViewBag.Info = "Ви авторизувались як Адміністратор. Для вас доступний весь функціонал додатка.";
                        break;
                    case "Manager":
                        ViewBag.Info = "Ви авторизувались як Адміністратор. Для вас доступний функціонал перегляду та редагування задач.";
                        break;
                    case "Developer":
                        ViewBag.Info = "Ви авторизувались як Виконавець. Для вас доступні тільки ваші задачі.";
                        break;
                }
            }
            else
            {
                ViewBag.Info = "На данный момент ви не авторизовані. Виконайте авхід або реєстрацію .";
            }
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
