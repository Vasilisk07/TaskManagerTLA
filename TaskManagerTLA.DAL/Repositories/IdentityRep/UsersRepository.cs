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
    public class UsersRepository : IRepository<ApplicationUser, string>
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
            //if (item != null) // така перевірка дуже небезпечна тим, що ти ніколи не дізнаєшся що в тебе в коді щось зламалось і прийшов item==null
            //{
            //    var res = dataBase.Users.Remove(item);
            //    return res.State == EntityState.Deleted;
            //}
            dataBase.Users.Remove(item);
            return await Task.FromResult(true); // так можна заткнути компілятор щоб не ругався warning CS1998: This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.

        }

        public async Task<bool> DeleteItemByIdAsync(string itemId)
        {
            dataBase.Users.Remove(dataBase.Users.Find(itemId));
            return await Task.FromResult(true);
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
                return dataBase.Users.Include(c => c.GlobalTasks).Include(c => c.AssignedTasks).Include(c => c.Roles);
            });
        }

        public async Task<ApplicationUser> GetItemByIdAsync(string itemId)
        {
            return await dataBase.Users.FindAsync(itemId);
        }

        public async Task<ApplicationUser> FindItemAsync(Expression<Func<ApplicationUser, bool>> predicate)
        {
            // linq може перетворити Expression на sql запит і послати його на сервер, Func доведеться запукати уже локально тут для ВСІХ дкументів що прийдуть з бд
            return await dataBase.Users.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<ApplicationUser>> FindRangeAsync(Expression<Func<ApplicationUser, bool>> predicate)
        {
            return await Task.Run(() =>
            {
                return dataBase.Users.Where(predicate);
            });
        }

        public async Task UpdateItemAsync(ApplicationUser item)
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
