using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TaskManagerTLA.DAL.EF;
using TaskManagerTLA.DAL.Identity.Entities;
using TaskManagerTLA.DAL.Repositories.Interfaces;

namespace TaskManagerTLA.DAL.Repositories.IdentityRep
{
    public class RolesRepository : IRoleRepository<ApplicationRole, string>
    {

        private readonly ApplicationContext dataBase;

        public RolesRepository(ApplicationContext identityContext)
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
            dataBase.Roles.Remove(item);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemByIdAsync(string itemId)
        {
            dataBase.Roles.Remove(dataBase.Roles.Find(itemId));
            return await Task.FromResult(true);
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
        public async Task<ApplicationRole> GetItemByNameAsync(string itemName)
        {
            return await Task.Run(() =>
            {
                return dataBase.Roles.Where(p => p.Name == itemName).Include(p => p.Users).FirstOrDefault();
            });
        }

        public async Task<ApplicationRole> FindFirstItemAsync(Expression<Func<ApplicationRole, bool>> predicate)
        {
            return await Task.Run(() =>
            {
                return dataBase.Roles.FirstOrDefault(predicate);
            });
        }

        public async Task SaveAsync()
        {
            await dataBase.SaveChangesAsync();
        }
    }
}
