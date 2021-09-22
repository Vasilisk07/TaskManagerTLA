using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagerTLA.DAL.Interfaces;

namespace TaskManagerTLA.DAL.Identity.Interfaces
{
    public interface IUnitOfWorkIdentity : IDisposable
    {
        //TODO IIdentityRepository, UsersRepository....
        IIdentityRepositories<IdentityUser> UsersRepositories { get; }
        IIdentityRepositories<IdentityRole> RolesRepositories { get; }
        IIdentityRepositories<IdentityUserRole<string>> UserRolesRepositories { get; }
        public void Save();

    }
}
