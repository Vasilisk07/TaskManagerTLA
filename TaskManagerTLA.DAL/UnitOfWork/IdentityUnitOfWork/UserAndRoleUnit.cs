using TaskManagerTLA.DAL.Identity.Entities;
using TaskManagerTLA.DAL.Repositories.Interfaces;
using TaskManagerTLA.DAL.UnitOfWork.IdentityUnitOfWork.Interfaces;

namespace TaskManagerTLA.DAL.UnitOfWork.IdentityUnitOfWork
{
    public class UserAndRoleUnit : IUserAndRoleUnit
    {
        public IRepository<ApplicationUser, string> Users { get; }
        public IRepository<ApplicationRole, string> Roles { get; }
        public UserAndRoleUnit(IRepository<ApplicationUser, string> users, IRepository<ApplicationRole, string> roles)
        {
            Users = users;
            Roles = roles;
        }
    }
}
