using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerTLA.BLL.Interfaces
{
    public interface IHomePageGreeting
    {
        Task<string> GetGreeting(HttpContext httpContext);

    }
}
