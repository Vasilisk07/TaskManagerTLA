using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagerTLA.DAL.EF;
using TaskManagerTLA.DAL.Identity.Entities;
using TaskManagerTLA.DAL.Repositories.Interfaces;

namespace TaskManagerTLA.DAL.Repositories.IdentityRep
{
    public class UsersRepository : IRepository<ApplicationUser, string>
    {
        private readonly IdentityContext dataBase;

        public UsersRepository(IdentityContext identityContext)
        {
            this.dataBase = identityContext;
        }

        public bool CreateItem(ApplicationUser newItem)
        {
            var res = dataBase.Users.Add(newItem);
            return res.State == EntityState.Added;
        }

        public bool DeleteItem(ApplicationUser item)
        {
            if (item != null)
            {
                var res = dataBase.Users.Remove(item);
                return res.State == EntityState.Deleted;
            }
            return false;
        }

        public bool DeleteItemById(string itemId)
        {                     
            var item = dataBase.Users.Find(itemId);
            if (item != null)
            {
                var res = dataBase.Users.Remove(item);
                return res.State == EntityState.Deleted;
            }
            return false;
        }

        public void DeleteRange(IEnumerable<ApplicationUser> deletedList)
        {
            dataBase.Users.RemoveRange(deletedList);
        }

        public IEnumerable<ApplicationUser> GetAllItems()
        {
            var users = dataBase.Users.Include(c => c.GlobalTasks).Include(c => c.AssignedTasks).Include(c => c.Roles);
            return users;
        }

        public ApplicationUser GetItemById(string itemId)
        {
            return dataBase.Users.Find(itemId);
        }

        public IEnumerable<ApplicationUser> Find(Func<ApplicationUser, Boolean> predicate)
        {
            return dataBase.Users.Where(predicate);
        }

        public void UpdateItem(ApplicationUser item)
        {
            dataBase.Users.Update(item);
        }
        public void Save()
        {
            dataBase.SaveChanges();
        }
    }
}
