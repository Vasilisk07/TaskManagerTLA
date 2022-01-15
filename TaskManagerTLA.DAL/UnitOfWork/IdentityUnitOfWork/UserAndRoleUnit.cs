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
        //шось мені здалось що данний підхід не зовсім правельний але як саме його переробити теж поки не в курсі
        //по перше певно не варто в двох властивостях Users і Roles викликати метод Save вони ж зсилаються на один і тойже контекст 
        //а по друге нам все ще доступні методи Save на рівні обєктів UserAndRoleUnit.Users.SaveAsync() і UserAndRoleUnit.Roles.SaveAsync()
        //
        public async Task SaveAsync()
        {
            await Users.SaveAsync();
            // await Roles.SaveAsync();
        }
    }
}
