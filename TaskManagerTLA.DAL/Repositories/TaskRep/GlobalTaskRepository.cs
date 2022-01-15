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
    public class GlobalTasksRepository : IGlobalTaskRepo<GlobalTask, int>
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

        public async Task<GlobalTask> FindFirstItemAsync(Expression<Func<GlobalTask, bool>> predicate)
        {
            return await Task.Run(() =>
            {
                return dataBase.GlobalTask.FirstOrDefault(predicate);
            });
        }

        public async Task<bool> DeleteItemAsync(GlobalTask item)
        {
            dataBase.GlobalTask.Remove(item);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemByIdAsync(int itemId)
        {
            dataBase.GlobalTask.Remove(dataBase.GlobalTask.Find(itemId));
            return await Task.FromResult(true);
        }

        public async Task SaveAsync()
        {
            await dataBase.SaveChangesAsync();
        }


        public async Task<GlobalTask> GetItemByGlobalTaskIdAsync(int taskId)
        {
            return await Task.Run(() =>
            {
                return dataBase.GlobalTask.Where(p => p.Id == taskId)
                                          .Include(p => p.Users)
                                          .Include(p => p.AssignedTasks)
                                          .ThenInclude(c => c.Comments)
                                          .FirstOrDefault();
            });
        }
    }
}
