using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagerTLA.DAL.Interfaces;

namespace TaskManagerTLA.DAL.Identity.Interfaces
{
    public interface IUnitOfWorkIdentity : IDisposable
    {
        UserManager<IdentityUser> UserManager { get; }
        RoleManager<IdentityRole> RoleManager { get; }
        SignInManager<IdentityUser> SignInManager { get; }

    }
}
