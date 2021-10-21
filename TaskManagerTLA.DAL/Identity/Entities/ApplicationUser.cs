using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using TaskManagerTLA.DAL.Entities;

namespace TaskManagerTLA.DAL.Identity.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public List<IdentityRole> Roles { get; set; } = new List<IdentityRole>();
        public List<GlobalTask> GlobalTasks { get; set; } = new List<GlobalTask>();
        public List<AssignedTask> AssignedTasks { get; set; } = new List<AssignedTask>();
    }
}
