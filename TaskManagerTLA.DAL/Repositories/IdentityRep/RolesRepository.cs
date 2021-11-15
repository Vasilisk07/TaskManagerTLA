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
    public class RolesRepository : IRepository<ApplicationRole, string>
    {

        private readonly IdentityContext dataBase;

        public RolesRepository(IdentityContext identityContext)
        {
            this.dataBase = identityContext;
        }

        public async Task<bool> CreateItemAsync(ApplicationRole newItem)
        {
            return await Task.Run(() =>
            {
                var res = dataBase.Roles.Add(newItem);
                return res.State == EntityState.Added;
            });
        }

        public async Task<bool> DeleteItemAsync(ApplicationRole item)
        {
            return await Task.Run(() =>
            {
                if (item != null)
                {
                    var res = dataBase.Roles.Remove(item);
                    return res.State == EntityState.Deleted;
                }
                return false;
            });
        }

        public async Task<bool> DeleteItemByIdAsync(string ItemId)
        {
            return await Task.Run(() =>
            {
                var item = dataBase.Roles.Find(ItemId);
                if (item != null)
                {
                    var res = dataBase.Roles.Remove(item);
                    return res.State == EntityState.Deleted;
                }
                return false;
            });
        }

        public async Task DeleteRangeAsync(IEnumerable<ApplicationRole> deletedList)
        {
            await Task.Run(() =>
            {
                dataBase.Roles.RemoveRange(deletedList);
            });
        }

        public async Task<IEnumerable<ApplicationRole>> GetAllItemsAsync()
        {
            return await Task.Run(() =>
            {
                return dataBase.Roles.Include(p => p.Users);
            });
        }

        public async Task<ApplicationRole> GetItemByIdAsync(string itemId)
        {
            return await Task.Run(() =>
            {
                return dataBase.Roles.Find(itemId);
            });
        }

        public async Task<IEnumerable<ApplicationRole>> FindAsync(Func<ApplicationRole, Boolean> predicate)
        {
            return await Task.Run(() =>
            {
                return dataBase.Roles.Where(predicate);
            });
        }

        public async Task UpdateItemAsunc(ApplicationRole item)
        {
            await Task.Run(() =>
            {
                dataBase.Roles.Update(item);
            });
        }

        public async Task SaveAsync()
        {
            await dataBase.SaveChangesAsync();
        }
    }
}
