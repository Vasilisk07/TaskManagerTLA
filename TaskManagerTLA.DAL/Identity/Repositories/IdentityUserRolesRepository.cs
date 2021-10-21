using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManagerTLA.DAL.EF;
using TaskManagerTLA.DAL.Identity.Entities;
using TaskManagerTLA.DAL.Identity.Interfaces;

namespace TaskManagerTLA.DAL.Identity.Repositories
{
    public class IdentityUserRolesRepository : IIdentityRepository<IdentityUserRole<string>>
    {
        private readonly IdentityContext database;

        public IdentityUserRolesRepository(IdentityContext identityContext)
        {
            this.database = identityContext;
        }

        public bool CreateItem(Microsoft.AspNetCore.Identity.IdentityUserRole<string> newItem)
        {
            var res = database.UserRoles.Add(newItem);
            return res.State == EntityState.Added;
        }

        public bool DeleteItem(IdentityUserRole<string> Item)
        {
            var res = database.UserRoles.Remove(Item);
            return res.State == EntityState.Deleted;
        }

        public void DeleteRange(IEnumerable<IdentityUserRole<string>> deletedList)
        {
            database.UserRoles.RemoveRange(deletedList);
        }

        public IEnumerable<IdentityUserRole<string>> GetAllItems()
        {
            return database.UserRoles;
        }

        public IdentityUserRole<string> GetItem(string itemId)
        {
            return database.UserRoles.Find(itemId);
        }

        public IEnumerable<IdentityUserRole<string>> Find(Func<IdentityUserRole<string>, Boolean> predicate)
        {
            return database.UserRoles.Where(predicate);
        }
    }
}
