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
    class ActualTaskRepositories : IRepository<ActualTask>
    {
        private readonly TaskContext db;

        public ActualTaskRepositories(TaskContext context)
        {
            this.db = context;
        }

        public IEnumerable<ActualTask> GetAll()
        {
            return db.ActualTasks;
        }

        public ActualTask Get(int id)
        {
            return db.ActualTasks.Find(id);
        }

        public void Create(ActualTask task)
        {
            db.ActualTasks.Add(task);
        }
        public void Update(ActualTask task)
        {
            db.Entry(task).State = EntityState.Modified;

        }

        public IEnumerable<ActualTask> Find(Func<ActualTask, Boolean> predicate)
        {
            return db.ActualTasks.Where(predicate).ToList();

        }

        public void Delete(int id)
        {
            ActualTask task = db.ActualTasks.Find(id);
            if (task != null)
            {
                db.ActualTasks.Remove(task);
            }
        }


    }
}
