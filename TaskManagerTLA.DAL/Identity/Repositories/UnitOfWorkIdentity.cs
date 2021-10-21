
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagerTLA.DAL.EF;
using TaskManagerTLA.DAL.Identity.Entities;
using TaskManagerTLA.DAL.Identity.Interfaces;
using TaskManagerTLA.DAL.Interfaces;

namespace TaskManagerTLA.DAL.Identity.Repositories
{
    public class UnitOfWorkIdentity : IUnitOfWorkIdentity
    {
        // TODO ці всі поля мають інджектнутись
        private IdentityUserRepository userRepositories;
        private IdentityRoleRepository rolesRepositories;
        private IdentityUserRolesRepository userRolesRepositories;
        private readonly IdentityContext IdentityDbContext;
        private bool disposed = false;

        public UnitOfWorkIdentity(string connectionString )
        {
            var optionsBuilder = new DbContextOptionsBuilder<IdentityContext>();
            var options = optionsBuilder.UseSqlServer(connectionString).Options;
            IdentityDbContext = new IdentityContext(options);
            
        }

        public IIdentityRepository<ApplicationUser> UsersRepositories
        {
            get
            {
                if (userRepositories == null)
                {
                    userRepositories = new IdentityUserRepository(IdentityDbContext);
                }
                return userRepositories;
            }
        }
        public IIdentityRepository<IdentityRole> RolesRepositories
        {
            get
            {
                if (rolesRepositories == null)
                {
                    rolesRepositories = new IdentityRoleRepository(IdentityDbContext);
                }
                return rolesRepositories;
            }
        }

        public IIdentityRepository<IdentityUserRole<string>> UserRolesRepositories
        {
            get
            {
                if (userRolesRepositories == null)
                {
                    userRolesRepositories = new IdentityUserRolesRepository(IdentityDbContext);
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
