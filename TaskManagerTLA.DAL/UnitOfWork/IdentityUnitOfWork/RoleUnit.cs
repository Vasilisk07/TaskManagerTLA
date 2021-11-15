using TaskManagerTLA.DAL.Identity.Entities;
using TaskManagerTLA.DAL.Repositories.Interfaces;
using TaskManagerTLA.DAL.UnitOfWork.IdentityUnitOfWork.Interfaces;

namespace TaskManagerTLA.DAL.UnitOfWork.IdentityUnitOfWork
{
    public class RoleUnit : IRoleUnit
    {
        public IRepository<ApplicationRole, string> Roles { get; }
        public RoleUnit(IRepository<ApplicationRole, string> roles)
        {
            Roles = roles;
        }
    }
}
