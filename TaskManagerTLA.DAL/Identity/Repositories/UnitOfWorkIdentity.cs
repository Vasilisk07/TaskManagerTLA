
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagerTLA.DAL.EF;
using TaskManagerTLA.DAL.Identity.Interfaces;

namespace TaskManagerTLA.DAL.Identity.Repositories
{
    public class UnitOfWorkIdentity : IUnitOfWorkIdentity
    {
        private IdentityUserRepositories userRepositories;
        private IdentityRoleRepositories roleRepositories;
        private IdentityUserRolesRepositories userRolesRepositories;
        private readonly IdentityContext IdentityDbContext;
        private bool disposed = false;

        public UnitOfWorkIdentity(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<IdentityContext>();
            var options = optionsBuilder.UseSqlServer(connectionString).Options;
            IdentityDbContext = new IdentityContext(options);
        }

        public IIdentityRepositories<IdentityUser> UsersRepositories
        {
            get
            {
                if (userRepositories == null)
                {
                    userRepositories = new IdentityUserRepositories(IdentityDbContext);
                }
                return userRepositories;
            }
        }

        public IIdentityRepositories<IdentityRole> RolesRepositories
        {
            get
            {
                if (roleRepositories == null)
                {
                    roleRepositories = new IdentityRoleRepositories(IdentityDbContext);
                }
                return roleRepositories;
            }
        }

        public IIdentityRepositories<IdentityUserRole<string>> UserRolesRepositories
        {
            get
            {
                if (userRolesRepositories == null)
                {
                    userRolesRepositories = new IdentityUserRolesRepositories(IdentityDbContext);
                }
                return userRolesRepositories;
            }
        }

        public virtual void Dispose(bool disposing)
        {

            if (!disposed)
            {
                if (disposing)
                {
                    IdentityDbContext.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            IdentityDbContext.SaveChanges();
        }
    }
}
