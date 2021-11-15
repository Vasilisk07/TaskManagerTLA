using Microsoft.AspNetCore.Identity;

namespace TaskManagerTLA.DAL.Identity.Entities
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public ApplicationUser User { get; set; }
        public ApplicationRole Role { get; set; }
    }
}
