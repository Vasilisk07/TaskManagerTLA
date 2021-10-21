using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagerTLA.DAL.EF;
using TaskManagerTLA.DAL.Identity.Entities;

namespace TaskManagerTLA.DAL.Identity.Interfaces
{
    public class IdentityUserRepository : IIdentityRepository<ApplicationUser>
    {
        private readonly IdentityContext database;

        public IdentityUserRepository(IdentityContext identityContext)
        {
            this.database = identityContext;
        }

        public bool CreateItem(ApplicationUser newItem)
        {
            var res = database.Users.Add(newItem);
            return res.State == EntityState.Added;
        }

        public bool DeleteItem(ApplicationUser Item)
        {
            var res = database.Users.Remove(Item);
            return res.State == EntityState.Deleted;
        }

        public void DeleteRange(IEnumerable<ApplicationUser> deletedList)
        {
            database.Users.RemoveRange(deletedList);
        }

        public IEnumerable<ApplicationUser> GetAllItems()
        {
            var users = database.Users.Include(c => c.GlobalTasks).Include(c => c.AssignedTasks);//.Include(c=>c.UserRoles);
            return users;
        }

        public ApplicationUser GetItem(string itemId)
        {
            return database.Users.Find(itemId);
        }

        public IEnumerable<ApplicationUser> Find(Func<ApplicationUser, Boolean> predicate)
        {
            return database.Users.Where(predicate);
        }
    }
}