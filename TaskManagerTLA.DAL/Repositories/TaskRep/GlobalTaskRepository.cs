using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManagerTLA.DAL.EF;
using TaskManagerTLA.DAL.Entities;
using TaskManagerTLA.DAL.Repositories.Interfaces;

namespace TaskManagerTLA.DAL.Repositories.TaskRep
{
    public class GlobalTasksRepository : IRepository<GlobalTask, int>
    {
        private readonly ApplicationContext dataBase;
        public GlobalTasksRepository(ApplicationContext context)
        {
            this.dataBase = context;
        }

        public async Task<IEnumerable<GlobalTask>> GetAllItemsAsync()
        {
            return await Task.Run(() =>
            {
                return dataBase.GlobalTask.Include(c => c.Users).Include(p => p.AssignedTasks);
            });
        }

        public async Task<GlobalTask> GetItemByIdAsync(int id)
        {
            return await Task.Run(() =>
            {
                var globalTask = dataBase.GlobalTask.Find(id);
                return globalTask;
            });
        }

        public async Task<bool> CreateItemAsync(GlobalTask newItem)
        {
            return await Task.Run(() =>
            {
                var res = dataBase.GlobalTask.Add(newItem);
                return res.State == EntityState.Added;
            });
        }

        public async Task UpdateItemAsync(GlobalTask item)
        {
            await Task.Run(() =>
            {
                dataBase.Entry(item).State = EntityState.Modified;
            });
        }

        public async Task<GlobalTask> FindItemAsync(Expression<Func<GlobalTask, bool>> predicate)
        {
            return await Task.Run(() =>
            {
                return dataBase.GlobalTask.FirstOrDefault(predicate);
            });
        }

        public async Task<IEnumerable<GlobalTask>> FindRangeAsync(Expression<Func<GlobalTask, bool>> predicate)
        {
            return await Task.Run(() =>
            {
                return dataBase.GlobalTask.Where(predicate);
            });
        }

        public async Task<bool> DeleteItemAsync(GlobalTask item)
        {
            return await Task.Run(() =>
            {
                if (item != null)
                {
                    var res = dataBase.GlobalTask.Remove(item);
                    return res.State == EntityState.Deleted;
                }
                return false;
            });
        }

        public async Task<bool> DeleteItemByIdAsync(int id)
        {
            return await Task.Run(() =>
            {
                var task = dataBase.GlobalTask.Find(id);
                if (task != null)
                {
                    var res = dataBase.GlobalTask.Remove(task);
                    return res.State == EntityState.Deleted;
                }
                return false;
            });
        }

        public async Task DeleteRangeAsync(IEnumerable<GlobalTask> deletedList)
        {
            await Task.Run(() =>
            {
                dataBase.GlobalTask.RemoveRange(deletedList);
            });
        }

        public async Task SaveAsync()
        {
            await dataBase.SaveChangesAsync();
        }
    }
}
