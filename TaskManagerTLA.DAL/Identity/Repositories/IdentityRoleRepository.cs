using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagerTLA.DAL.EF;
using TaskManagerTLA.DAL.Identity.Interfaces;

namespace TaskManagerTLA.DAL.Identity
{
    class IdentityRoleRepository : IIdentityRepository<IdentityRole>
    {

        private readonly IdentityContext database;

        public IdentityRoleRepository(IdentityContext identityContext)
        {
            this.database = identityContext;
        }

        public bool CreateItem(IdentityRole newItem)
        {
            var res = database.Roles.Add(newItem);
            return res.State == EntityState.Added;
        }

        public bool DeleteItem(IdentityRole Item)
        {
            var res = database.Roles.Remove(Item);
            return res.State == EntityState.Deleted;
        }
        public void DeleteRange(IEnumerable<IdentityRole> deletedList)
        {
            database.Roles.RemoveRange(deletedList);
        }

        public IEnumerable<IdentityRole> GetAllItems()
        {
            return database.Roles;
        }
        public IdentityRole GetItem(string itemId)
        {
            return database.Roles.Find(itemId);
        }

        public IEnumerable<IdentityRole> Find(Func<IdentityRole, Boolean> predicate)
        {
            return database.Roles.Where(predicate);
        }
    }
}
