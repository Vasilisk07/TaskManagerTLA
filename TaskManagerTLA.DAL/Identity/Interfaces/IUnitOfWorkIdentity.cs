using System;
using TaskManagerTLA.DAL.Identity.Entities;

namespace TaskManagerTLA.DAL.Identity.Interfaces
{
    public interface IUnitOfWorkIdentity : IDisposable
    {
        IIdentityRepository<ApplicationUser> UsersRepositories { get; }
        IIdentityRepository<ApplicationRole> RolesRepositories { get; }
        public void Save();
    }
}
