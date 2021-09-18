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
    class TaskRepositories : IRepository<TaskModel>
    {
        private readonly TaskContext db;

        public TaskRepositories(TaskContext context)
        {
            this.db = context;
        }

        public IEnumerable<TaskModel>GetAll()
        {
            return db.Tasks;
        }

        public TaskModel Get(int id)
        {
            return db.Tasks.Find(id);
        }

        public void Create(TaskModel task)
        {
            db.Tasks.Add(task);
        }
        public void Update(TaskModel task)
        {
            db.Entry(task).State = EntityState.Modified;

        }

        public IEnumerable<TaskModel> Find(Func<TaskModel,Boolean> predicate)
        {
            return db.Tasks.Where(predicate).ToList();

        }
        public void Delete(int id)
        {
            TaskModel task = db.Tasks.Find(id);
            if (task!=null)
            {
                db.Tasks.Remove(task);
            }
        }



    }
}
