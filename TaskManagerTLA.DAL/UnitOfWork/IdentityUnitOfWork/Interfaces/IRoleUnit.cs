using TaskManagerTLA.DAL.Identity.Entities;
using TaskManagerTLA.DAL.Repositories.Interfaces;

namespace TaskManagerTLA.DAL.UnitOfWork.IdentityUnitOfWork.Interfaces
{
    public interface IRoleUnit
    {
        IRepository<ApplicationRole, string> Roles { get; }
    }
}
