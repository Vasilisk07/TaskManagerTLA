using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManagerTLA.DAL.EF;
using TaskManagerTLA.DAL.Identity.Entities;
using TaskManagerTLA.DAL.Repositories.Interfaces;

namespace TaskManagerTLA.DAL.Repositories.IdentityRep
{
    public class UsersRepository : IUserRepository<ApplicationUser, string>
    {
        private readonly ApplicationContext dataBase;

        public UsersRepository(ApplicationContext identityContext)
        {
            this.dataBase = identityContext;
        }

        public async Task<bool> CreateItemAsync(ApplicationUser newItem)
        {
            var res = await dataBase.Users.AddAsync(newItem);
            return res.State == EntityState.Added;
        }

        public async Task<bool> AnyThereUsersAsync()
        {
            return await Task.Run(() =>
            {
                return dataBase.Set<ApplicationUser>().Count() < 1;
            });
        }


        public async Task<bool> DeleteItemAsync(ApplicationUser item)
        {
            dataBase.Users.Remove(item);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemByIdAsync(string itemId)
        {
            dataBase.Users.Remove(dataBase.Users.Find(itemId));
            return await Task.FromResult(true);
        }


        public async Task<IEnumerable<ApplicationUser>> GetAllItemsAsync()
        {
            return await Task.Run(() =>
            {
                return dataBase.Users.Include(c => c.GlobalTasks).Include(c => c.AssignedTasks).Include(c => c.Roles);
            });
        }

        public async Task<ApplicationUser> GetItemByIdAsync(string itemId)
        {
            return await Task.Run(() =>
            {
                return dataBase.Users.Where(p => p.Id == itemId)
                    .Include(c => c.GlobalTasks)
                    .Include(c => c.AssignedTasks)
                    .Include(c => c.Roles)
                    .FirstOrDefault();
            });

        }

        public async Task<ApplicationUser> FindFirstItemAsync(Expression<Func<ApplicationUser, bool>> predicate)
        {
            return await dataBase.Users.FirstOrDefaultAsync(predicate);
        }

        public async Task SaveAsync()
        {
            await dataBase.SaveChangesAsync();
        }
    }
}
