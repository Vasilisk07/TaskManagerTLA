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
    // створи базовий репозиторій Repository<T, string>, реалізуй в ньому усі ці базові методи
    // тут в RolesRepository наслідуй Repository<T, string>, якщщо потрібно добавляй специфічні для ролей методи
    // ці методи занеси в інтерфейс IRolesRepository який всюди будеш інджектити (IRepository<ApplicationRole, string>)
    public class RolesRepository : IRepository<ApplicationRole,string>
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

        public async Task<ApplicationRole> FindItemAsync(Expression<Func<ApplicationRole, bool>> predicate)
        {
            return await Task.Run(() =>
            {
                return dataBase.Roles.FirstOrDefault(predicate);
            });
        }

        public async Task<IEnumerable<ApplicationRole>> FindRangeAsync(Expression<Func<ApplicationRole, bool>> predicate)
        {
            return await Task.Run(() =>
            {
                return dataBase.Roles.Where(predicate);
            });
        }

        public async Task UpdateItemAsync(ApplicationRole item)
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
