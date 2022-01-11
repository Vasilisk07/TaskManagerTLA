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
    public class AssignedTaskRepository : IRepository<AssignedTask, int>
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

        public async Task UpdateItemAsync(AssignedTask item)
        {
            await Task.Run(() =>
            {
                dataBase.Entry(item).State = EntityState.Modified;
            });
        }

        public async Task<AssignedTask> FindItemAsync(Expression<Func<AssignedTask, bool>> predicate)
        {
            return await Task.Run(() =>
            {
                return dataBase.AssignedTask.FirstOrDefault(predicate);
            });
        }

        public async Task<IEnumerable<AssignedTask>> FindRangeAsync(Expression<Func<AssignedTask, bool>> predicate)
        {
            return await Task.Run(() =>
            {
                return dataBase.AssignedTask.Where(predicate);
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


        public async Task DeleteRangeAsync(IEnumerable<AssignedTask> deletedList)
        {
            await Task.Run(() =>
            {
                dataBase.AssignedTask.RemoveRange(deletedList);
            });
        }

        public async Task SaveAsync()
        {
            await dataBase.SaveChangesAsync();
        }

    }
}
