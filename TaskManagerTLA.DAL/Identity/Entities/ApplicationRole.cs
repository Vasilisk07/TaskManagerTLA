using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TaskManagerTLA.DAL.Identity.Entities
{
    public class ApplicationRole : IdentityRole<string>
    {
        public ApplicationRole(string name) : base(name)
        { }
        public ApplicationRole()
        { }
        public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    }
}
