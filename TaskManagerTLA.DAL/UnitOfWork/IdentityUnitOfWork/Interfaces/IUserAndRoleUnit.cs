using System.Threading.Tasks;
using TaskManagerTLA.DAL.Identity.Entities;
using TaskManagerTLA.DAL.Repositories.Interfaces;

namespace TaskManagerTLA.DAL.UnitOfWork.IdentityUnitOfWork.Interfaces
{
    // в юніта має бути метод Save() це основне його призначення
    public interface IUserAndRoleUnit
    {
        IRepository<ApplicationUser, string> Users { get; }
        IRepository<ApplicationRole, string> Roles { get; }
    }
}
