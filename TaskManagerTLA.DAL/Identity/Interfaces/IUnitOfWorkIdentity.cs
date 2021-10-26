using System;
using TaskManagerTLA.DAL.Identity.Entities;

namespace TaskManagerTLA.DAL.Identity.Interfaces
{
    public interface IUnitOfWorkIdentity : IDisposable
    {
        IIdentityRepository<ApplicationUser> UsersRepository { get; }
        IIdentityRepository<ApplicationRole> RolesRepository { get; }
        public void Save();
    }
}
