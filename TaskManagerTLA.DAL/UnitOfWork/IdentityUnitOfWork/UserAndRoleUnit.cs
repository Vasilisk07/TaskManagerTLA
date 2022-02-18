using System.Threading.Tasks;
using TaskManagerTLA.DAL.Identity.Entities;
using TaskManagerTLA.DAL.Repositories.Interfaces;
using TaskManagerTLA.DAL.UnitOfWork.IdentityUnitOfWork.Interfaces;

namespace TaskManagerTLA.DAL.UnitOfWork.IdentityUnitOfWork
{
    public class UserAndRoleUnit : IUserAndRoleUnit
    {
        public IUserRepository<ApplicationUser, string> Users { get; }
        public IRoleRepository<ApplicationRole, string> Roles { get; }

        public UserAndRoleUnit(IUserRepository<ApplicationUser, string> users, IRoleRepository<ApplicationRole, string> roles)
        {
            Users = users;
            Roles = roles;
        }

        public async Task SaveAsync()
        {
            await Users.SaveAsync();
            await Roles.SaveAsync();
        }
    }
}
