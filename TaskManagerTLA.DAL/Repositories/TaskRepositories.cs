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
    class TaskRepositories : IRepository<GlobalTask>
    {
        private readonly TaskContext db;

        public TaskRepositories(TaskContext context)
        {
            this.db = context;
        }

        public IEnumerable<GlobalTask> GetAll()
        {
            return db.GlobalTask.Include(c=>c.Users).Include(p=>p.AssignedTasks);
        }

        public GlobalTask Get(int id)
        {
            var globalTask = db.GlobalTask.Find(id);
            return globalTask;
        }

        public void Create(GlobalTask task)
        {
            db.GlobalTask.Add(task);
        }

        public void Update(GlobalTask task)
        {
            db.Entry(task).State = EntityState.Modified;

        }

        public IEnumerable<GlobalTask> Find(Func<GlobalTask, Boolean> predicate)
        {
            //! TODO ToList() почне збирати усі записи 
            //! можливо тому хто викликає метод не потрібні усі записи одразу, і він хоче тягнути їх по одному з енумератора
            return db.GlobalTask.Where(predicate);
            

        }




        public void Delete(int id)
        {
            GlobalTask task = db.GlobalTask.Find(id);
            if (task != null)
            {
                db.GlobalTask.Remove(task);
            }
        }
        public void DeleteRange(IEnumerable<GlobalTask> deletedList)
        {
            db.GlobalTask.RemoveRange(deletedList);
        }


    }
}
