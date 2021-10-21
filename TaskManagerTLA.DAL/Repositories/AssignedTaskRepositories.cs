using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManagerTLA.DAL.EF;
using TaskManagerTLA.DAL.Entities;
using TaskManagerTLA.DAL.Interfaces;

namespace TaskManagerTLA.DAL.Repositories
{
    class AssignedTaskRepositories : IRepository<AssignedTask>
    {
        private readonly TaskContext db;

        public AssignedTaskRepositories(TaskContext context)
        {
            this.db = context;
        }

        public IEnumerable<AssignedTask> GetAll()
        {
            return db.AssignedTask.Include(c => c.GlobalTask).Include(c => c.User);
        }

        public AssignedTask Get(int id)
        {
            return db.AssignedTask.Find(id);
        }

        public void Create(AssignedTask task)
        {
            db.AssignedTask.Add(task);
        }
        public void Update(AssignedTask task)
        {
            db.Entry(task).State = EntityState.Modified;

        }

        public IEnumerable<AssignedTask> Find(Func<AssignedTask, Boolean> predicate)
        {
            return db.AssignedTask.Where(predicate);
        }

        public void Delete(int id)
        {
            AssignedTask task = db.AssignedTask.Find(id);
            if (task != null)
            {
                db.AssignedTask.Remove(task);
            }
        }
        public void DeleteRange(IEnumerable<AssignedTask> deletedList)
        {
            db.AssignedTask.RemoveRange(deletedList);
        }

    }
}
