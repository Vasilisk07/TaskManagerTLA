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
    public class AssignedTaskRepository : IAssignedTaskRepo<AssignedTask, int>
    {
        private readonly ApplicationContext dataBase;

        public AssignedTaskRepository(ApplicationContext context)
        {
            this.dataBase = context;
        }

        public async Task<IEnumerable<AssignedTask>> GetAllItemsAsync()
        {
            return await Task.Run(() =>
            {
                return dataBase.AssignedTask.Include(c => c.GlobalTask).Include(c => c.User);
            });
        }

        public async Task<AssignedTask> GetItemByIdAsync(int id)
        {
            return await Task.Run(() =>
            {
                return dataBase.AssignedTask.Find(id);
            });
        }

        public async Task<bool> CreateItemAsync(AssignedTask newItem)
        {
            return await Task.Run(() =>
            {
                var res = dataBase.AssignedTask.Add(newItem);
                return res.State == EntityState.Added;
            });
        }

        public async Task<AssignedTask> FindFirstItemAsync(Expression<Func<AssignedTask, bool>> predicate)
        {
            return await Task.Run(() =>
            {
                return dataBase.AssignedTask.Where(predicate)
                                            .Include(p => p.Comments)
                                            .Include(p => p.User)
                                            .Include(p => p.GlobalTask)
                                            .FirstOrDefault();
            });
        }

        public async Task<bool> DeleteItemAsync(AssignedTask item)
        {
            dataBase.AssignedTask.Remove(item);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemByIdAsync(int itemId)
        {
            dataBase.AssignedTask.Remove(dataBase.AssignedTask.Find(itemId));
            return await Task.FromResult(true);
        }

        public async Task SaveAsync()
        {
            await dataBase.SaveChangesAsync();
        }

        public async Task<IEnumerable<AssignedTask>> GetItemsByUserNameAsync(string username)
        {
            return await Task.Run(() =>
            {
                return dataBase.AssignedTask.Where(p => p.User.UserName == username)
                                            .Include(p => p.GlobalTask)
                                            .Include(p => p.Comments);
            });
        }

        public async Task<AssignedTask> GetTaskForUserIdAsync(string userId, int taskId)
        {
            return await Task.Run(() =>
            {
                return dataBase.AssignedTask.Where(p => p.User.Id == userId && p.GlobalTask.Id == taskId)
                                            .Include(p => p.GlobalTask)
                                            .Include(p => p.Comments)
                                            .FirstOrDefault();
            });
        }
    }
}
