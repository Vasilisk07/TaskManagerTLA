using Microsoft.AspNetCore.Identity;
using System;
using TaskManagerTLA.DAL.Identity.Entities;

namespace TaskManagerTLA.DAL.Identity.Interfaces
{
    public interface IUnitOfWorkIdentity : IDisposable
    {
        IIdentityRepository<ApplicationUser> UsersRepositories { get; }
        IIdentityRepository<IdentityRole> RolesRepositories { get; }
        IIdentityRepository<IdentityUserRole<string>> UserRolesRepositories { get; }
        public void Save();
    }
}
