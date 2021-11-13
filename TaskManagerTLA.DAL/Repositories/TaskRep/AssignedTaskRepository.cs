using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagerTLA.DAL.EF;
using TaskManagerTLA.DAL.Entities;
using TaskManagerTLA.DAL.Repositories.Interfaces;

namespace TaskManagerTLA.DAL.Repositories.TaskRep
{
    public class AssignedTaskRepository : IRepository<AssignedTask, int>
    {
        private readonly TaskContext dataBase;

        public AssignedTaskRepository(TaskContext context)
        {
            this.dataBase = context;
        }

        public IEnumerable<AssignedTask> GetAllItems()
        {
            return dataBase.AssignedTask.Include(c => c.GlobalTask).Include(c => c.User);
        }

        public AssignedTask GetItemById(int id)
        {
            return dataBase.AssignedTask.Find(id);
        }

        public bool CreateItem(AssignedTask task)
        {
            var res = dataBase.AssignedTask.Add(task);
            return res.State == EntityState.Added;
        }
        public void UpdateItem(AssignedTask task)
        {
            dataBase.Entry(task).State = EntityState.Modified;
        }

        public IEnumerable<AssignedTask> Find(Func<AssignedTask, Boolean> predicate)
        {
            return dataBase.AssignedTask.Where(predicate);
        }

        public bool DeleteItem(AssignedTask item)
        {
            if (item != null)
            {
                var res = dataBase.AssignedTask.Remove(item);
                return res.State == EntityState.Deleted;
            }
            return false;
        }

        public bool DeleteItemById(int id)
        {
            var item = dataBase.AssignedTask.Find(id);
            if (item != null)
            {
                var res = dataBase.AssignedTask.Remove(item);
                return res.State == EntityState.Deleted;
            }
            return false;
        }

        public void DeleteRange(IEnumerable<AssignedTask> deletedList)
        {
            dataBase.AssignedTask.RemoveRange(deletedList);
        }
        public void Save()
        {
            dataBase.SaveChanges();
        }
    }
}
