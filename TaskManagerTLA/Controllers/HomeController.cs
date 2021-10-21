using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TaskManagerTLA.Models;

namespace TaskManagerTLA.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.UserName = User.Identity.Name;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
