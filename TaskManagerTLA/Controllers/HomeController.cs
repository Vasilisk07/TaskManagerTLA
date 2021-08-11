using AutoMapper;
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
        public HomeController()
        {

        }


        public IActionResult Index()
        {
            ViewBag.Info = "";

            //if (HttpContext.User.Identity.IsAuthenticated)
            //{
            //    ApplicationUser user = userManager.FindByName(User.Identity.Name);
            //    string role = userManager.GetRoles(user.Id)[0];
            //    switch (role)
            //    {
            //        case "Admin":
            //            ViewBag.Info = "Вы вошли как администратор. Для вас доступен весь функционал приложения.";
            //            break;
            //        case "Manager":
            //            ViewBag.Info = "Вы вошли как Менеджер проекта. Для вас доступен функционал просмотра и редактирования задач.";
            //            break;
            //        case "Dev":
            //            ViewBag.Info = "Вы вошли как Исполнитель. Для вас доступны только вашы задачи.";
            //            break;
            //    }
            //}
            //else
            //{
            //    ViewBag.Info = "На данный момент вы не авторизованы. Выполните авторизацию или регистрацию для доступа к функционалу.";
            //}

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
