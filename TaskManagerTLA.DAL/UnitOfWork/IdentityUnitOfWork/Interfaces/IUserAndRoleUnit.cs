using System.Threading.Tasks;
using TaskManagerTLA.DAL.Identity.Entities;
using TaskManagerTLA.DAL.Repositories.Interfaces;

namespace TaskManagerTLA.DAL.UnitOfWork.IdentityUnitOfWork.Interfaces
{
    public interface IUserAndRoleUnit
    {
        IUserRepository<ApplicationUser, string> Users { get; }
        IRoleRepository<ApplicationRole, string> Roles { get; }
        Task SaveAsync();
    }
}
