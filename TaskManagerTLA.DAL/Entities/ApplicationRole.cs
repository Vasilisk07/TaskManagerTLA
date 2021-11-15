using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TaskManagerTLA.DAL.Identity.Entities
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole(string name) : base(name)
        { }
        public ApplicationRole()
        { }
        public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        public List<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();
    }
}
