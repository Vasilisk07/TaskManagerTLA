using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagerTLA.DAL.EF;
using TaskManagerTLA.DAL.Identity.Entities;
using TaskManagerTLA.DAL.Repositories.Interfaces;

namespace TaskManagerTLA.DAL.Repositories.IdentityRep
{
    public class UsersRepository : IRepository<ApplicationUser, string>
    {
        private readonly IdentityContext dataBase;

        public UsersRepository(IdentityContext identityContext)
        {
            this.dataBase = identityContext;
        }

        public async Task<bool> CreateItemAsync(ApplicationUser newItem)
        {
            return await Task.Run(() =>
            {
                var res = dataBase.Users.Add(newItem);
                return res.State == EntityState.Added;
            });
        }

        public async Task<bool> DeleteItemAsync(ApplicationUser item)
        {
            return await Task.Run(() =>
            {
                if (item != null)
                {
                    var res = dataBase.Users.Remove(item);
                    return res.State == EntityState.Deleted;
                }
                return false;
            });
        }

        public async Task<bool> DeleteItemByIdAsync(string itemId)
        {
            return await Task.Run(() =>
            {
                var item = dataBase.Users.Find(itemId);
                if (item != null)
                {
                    var res = dataBase.Users.Remove(item);
                    return res.State == EntityState.Deleted;
                }
                return false;
            });
        }

        public async Task DeleteRangeAsync(IEnumerable<ApplicationUser> deletedList)
        {
            await Task.Run(() =>
            {
                dataBase.Users.RemoveRange(deletedList);
            });
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllItemsAsync()
        {
            return await Task.Run(() =>
            {
                var users = dataBase.Users.Include(c => c.GlobalTasks).Include(c => c.AssignedTasks).Include(c => c.Roles);
                return users;
            });
        }

        public async Task<ApplicationUser> GetItemByIdAsync(string itemId)
        {
            return await Task.Run(() =>
            {
                return dataBase.Users.Find(itemId);
            });
        }

        public async Task<IEnumerable<ApplicationUser>> FindAsync(Func<ApplicationUser, Boolean> predicate)
        {
            return await Task.Run(() =>
            {
                return dataBase.Users.Where(predicate);
            });
        }

        public async Task UpdateItemAsunc(ApplicationUser item)
        {
            await Task.Run(() =>
            {
                dataBase.Users.Update(item);
            });
        }

        public async Task SaveAsync()
        {
            await dataBase.SaveChangesAsync();
        }
    }
}
