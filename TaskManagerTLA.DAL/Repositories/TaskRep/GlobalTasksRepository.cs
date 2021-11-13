using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagerTLA.DAL.EF;
using TaskManagerTLA.DAL.Entities;
using TaskManagerTLA.DAL.Repositories.Interfaces;

namespace TaskManagerTLA.DAL.Repositories.TaskRep
{
    public class GlobalTasksRepository : IRepository<GlobalTask, int>
    {
        private readonly TaskContext dataBase;
        public GlobalTasksRepository(TaskContext context)
        {
            this.dataBase = context;
        }

        public IEnumerable<GlobalTask> GetAllItems()
        {
            return dataBase.GlobalTask.Include(c => c.Users).Include(p => p.AssignedTasks);
        }

        public GlobalTask GetItemById(int id)
        {
            var globalTask = dataBase.GlobalTask.Find(id);
            return globalTask;
        }

        public bool CreateItem(GlobalTask task)
        {
            var res = dataBase.GlobalTask.Add(task);
            return res.State == EntityState.Added;
        }

        public void UpdateItem(GlobalTask task)
        {
            dataBase.Entry(task).State = EntityState.Modified;
        }

        public IEnumerable<GlobalTask> Find(Func<GlobalTask, Boolean> predicate)
        {
            return dataBase.GlobalTask.Where(predicate);
        }

        public bool DeleteItem(GlobalTask item)
        {
            if (item != null)
            {
                var res = dataBase.GlobalTask.Remove(item);
                return res.State == EntityState.Deleted;
            }
            return false;
        }

        public bool DeleteItemById(int id)
        {
            var task = dataBase.GlobalTask.Find(id);
            if (task != null)
            {
                var res = dataBase.GlobalTask.Remove(task);
                return res.State == EntityState.Deleted;
            }
            return false;
        }
        public void DeleteRange(IEnumerable<GlobalTask> deletedList)
        {
            dataBase.GlobalTask.RemoveRange(deletedList);
        }

        public void Save()
        {
            dataBase.SaveChanges();
        }
    }
}
