using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagerTLA.DAL.EF;
using TaskManagerTLA.DAL.Identity.Entities;
using TaskManagerTLA.DAL.Identity.Interfaces;

namespace TaskManagerTLA.DAL.Identity
{
    class IdentityRoleRepository : IIdentityRepository<ApplicationRole>
    {
        private readonly IdentityContext database;

        public IdentityRoleRepository(IdentityContext identityContext)
        {
            this.database = identityContext;
        }

        public bool CreateItem(ApplicationRole newItem)
        {
            var res = database.Roles.Add(newItem);
            return res.State == EntityState.Added;
        }

        public bool DeleteItem(ApplicationRole Item)
        {
            var res = database.Roles.Remove(Item);
            return res.State == EntityState.Deleted;
        }
        public void DeleteRange(IEnumerable<ApplicationRole> deletedList)
        {
            database.Roles.RemoveRange(deletedList);
        }

        public IEnumerable<ApplicationRole> GetAllItems()
        {
            return database.Roles.Include(p => p.Users);
        }
        public ApplicationRole GetItem(string itemId)
        {
            return database.Roles.Find(itemId);
        }

        public IEnumerable<ApplicationRole> Find(Func<ApplicationRole, Boolean> predicate)
        {
            return database.Roles.Where(predicate);
        }
    }
}
