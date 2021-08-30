
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagerTLA.DAL.EF;
using TaskManagerTLA.DAL.Identity.Interfaces;

namespace TaskManagerTLA.DAL.Identity
{
    public class UnitOfWorkIdentity : IUnitOfWorkIdentity
    {

        public UserManager<IdentityUser> UserManager { get;  }
        public RoleManager<IdentityRole> RoleManager { get;  }
        public SignInManager<IdentityUser> SignInManager { get; }

        private bool disposed = false;

        public UnitOfWorkIdentity(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            SignInManager = signInManager;
        }




        public virtual void Dispose(bool disposing)
        {

            if (!disposed)
            {
                if (disposing)
                {
                    UserManager.Dispose();
                    RoleManager.Dispose();
                    
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }





    }
}
