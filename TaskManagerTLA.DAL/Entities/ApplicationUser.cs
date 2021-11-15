using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using TaskManagerTLA.DAL.Entities;

namespace TaskManagerTLA.DAL.Identity.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public List<ApplicationRole> Roles { get; set; } = new List<ApplicationRole>();
        public List<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();

        // таски не мають відношення до identity, але ок)
        public List<GlobalTask> GlobalTasks { get; set; } = new List<GlobalTask>();
        public List<AssignedTask> AssignedTasks { get; set; } = new List<AssignedTask>();
    }
}
