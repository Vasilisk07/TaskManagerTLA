using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagerTLA.DAL.EF;

namespace TaskManagerTLA.DAL.Identity.Interfaces
{
    public class IdentityUserRepositories : IIdentityRepositories<IdentityUser>
    {
        private readonly IdentityContext Db;
        public IdentityUserRepositories(IdentityContext identityContext)
        {
            this.Db = identityContext;
        }
        public bool CreateItem(IdentityUser newItem)
        {
            var res = Db.Users.Add(newItem);
            //TODO  return res.State == EntityState.Added;
            bool creationState = false;
            if (res.State == EntityState.Added)
            {
                creationState = true;
            }
            return creationState;
        }
        public bool DeleteItem(IdentityUser Item)
        {
            var res = Db.Users.Remove(Item);
            //TODO  return res.State == EntityState.Deleted;
            bool deletedState = false;
            if (res.State == EntityState.Deleted)
            {
                deletedState = true;
            }
            return deletedState;
        }
        public IEnumerable<IdentityUser> GetAllItems()
        {
            return Db.Users;
        }

        public IdentityUser GetItem(string itemId)
        {
            return Db.Users.Find(itemId);
        }
    }
}