using System.Threading.Tasks;
using TaskManagerTLA.DAL.Identity.Entities;
using TaskManagerTLA.DAL.Repositories.Interfaces;

namespace TaskManagerTLA.DAL.UnitOfWork.IdentityUnitOfWork.Interfaces
{
    public interface IUserAndRoleUnit
    {
        IRepository<ApplicationUser, string> Users { get; }
        IRepository<ApplicationRole, string> Roles { get; }
        Task SaveAsync();
    }
}
