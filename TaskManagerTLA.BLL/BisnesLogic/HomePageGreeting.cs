using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskManagerTLA.BLL.DTO;
using TaskManagerTLA.BLL.Interfaces;

namespace TaskManagerTLA.BLL.BisnesLogic
{
   public class HomePageGreeting : IHomePageGreeting
    {
        private IIdentityServices identityService;
        public HomePageGreeting(IIdentityServices identityService)
        {
            this.identityService = identityService;
        }
         
        public string GetGreeting(HttpContext httpContext)
        {

            string resultGteting = "";
            if (httpContext.User.Identity.IsAuthenticated)
            {
                UserDTO user = identityService.GetUserByName(httpContext.User.Identity.Name);
                string role = identityService.GetUserRole(user.id);
                switch (role)
                {
                    case "Admin":
                        resultGteting = "Ви авторизувались як Адміністратор. Для вас доступний весь функціонал додатка.";
                        break;
                    case "Manager":
                        resultGteting = "Ви авторизувались як Адміністратор. Для вас доступний функціонал перегляду та редагування задач.";
                        break;
                    case "Developer":
                        resultGteting = "Ви авторизувались як Виконавець. Для вас доступні тільки ваші задачі.";
                        break;
                }
            }
            else
            {
                resultGteting = "На данный момент ви не авторизовані. Виконайте авхід або реєстрацію .";
            }

            return resultGteting;
        }


    }
}
