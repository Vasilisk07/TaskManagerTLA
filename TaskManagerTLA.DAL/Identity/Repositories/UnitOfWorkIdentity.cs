using System;
using TaskManagerTLA.DAL.EF;
using TaskManagerTLA.DAL.Identity.Entities;
using TaskManagerTLA.DAL.Identity.Interfaces;

namespace TaskManagerTLA.DAL.Identity.Repositories
{
    public class UnitOfWorkIdentity : IUnitOfWorkIdentity
    {
        // TODO ці всі поля мають інджектнутись

        // тут я не зрозумів як можна передати репозиторії через залежності
        // посуті ж  контролювати створеня репозиторыъв має UnitOfWork і надавати їм спільний доступ до бази данних
        // а якшо я буду їх інжектувати через залежності то вони будуть створюватись і передаватись без участі UnitOfWork

        // зараз ти всоедно береш IdentityContext з DI, тому еквівалентно можна зробити щось таке:
        //      services.AddTransient<IIdentityRepository<ApplicationUser>, IdentityUserRepository>>();
        // по замовчуванню DbContext налаштований як Scoped, тобто один dbContext на весь реквест
        private IIdentityRepository<ApplicationUser> userRepositories;
        private IIdentityRepository<ApplicationRole> rolesRepositories;
        private readonly IdentityContext IdentityDbContext;
        private bool disposed = false;

        public UnitOfWorkIdentity(IdentityContext database)
        {
            IdentityDbContext = database;
        }

        public IIdentityRepository<ApplicationUser> UsersRepository
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

        public IIdentityRepository<ApplicationRole> RolesRepository
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
