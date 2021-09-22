using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagerTLA.DAL.EF;
using TaskManagerTLA.DAL.Identity.Interfaces;

namespace TaskManagerTLA.DAL.Identity
{
    class IdentityRoleRepositories : IIdentityRepositories<IdentityRole>
    {

        private readonly IdentityContext Db;
        public IdentityRoleRepositories(IdentityContext identityContext)
        {
            this.Db = identityContext;
        }

        public bool CreateItem(IdentityRole newItem)
        {
            var res = Db.Roles.Add(newItem);
            //TODO  return res.State == EntityState.Added;
            bool creationState = false;
            if (res.State == EntityState.Added)
            {
                creationState = true;
            }
            return creationState;
        }

        public bool DeleteItem(IdentityRole Item)
        {
            var res = Db.Roles.Remove(Item);
            //TODO  return res.State == EntityState.Deleted;
            bool deletedState = false;
            if (res.State == EntityState.Deleted)
            {
                deletedState = true;
            }
            return deletedState;
        }

        public IEnumerable<IdentityRole> GetAllItems()
        {
            return Db.Roles;
        }
        public IdentityRole GetItem(string itemId)
        {
            return Db.Roles.Find(itemId);
        }
    }
}
