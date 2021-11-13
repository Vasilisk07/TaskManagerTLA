using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManagerTLA.DAL.EF;
using TaskManagerTLA.DAL.Identity.Entities;
using TaskManagerTLA.DAL.Repositories.Interfaces;

namespace TaskManagerTLA.DAL.Repositories.IdentityRep
{
    public class RolesRepository : IRepository<ApplicationRole, string>
    {

        private readonly IdentityContext dataBase;

        public RolesRepository(IdentityContext identityContext)
        {
            this.dataBase = identityContext;
        }

        public bool CreateItem(ApplicationRole newItem)
        {
            var res = dataBase.Roles.Add(newItem);
            return res.State == EntityState.Added;
        }

        public bool DeleteItem(ApplicationRole item)
        {
            if (item != null)
            {
                var res = dataBase.Roles.Remove(item);
                return res.State == EntityState.Deleted;
            }
            return false;
        }

        public bool DeleteItemById(string ItemId)
        {                     
            var item = dataBase.Roles.Find(ItemId);
            if (item != null)
            {
                var res = dataBase.Roles.Remove(item);
                return res.State == EntityState.Deleted;
            }
            return false;
        }
        public void DeleteRange(IEnumerable<ApplicationRole> deletedList)
        {
            dataBase.Roles.RemoveRange(deletedList);
        }

        public IEnumerable<ApplicationRole> GetAllItems()
        {
            return dataBase.Roles.Include(p => p.Users);
        }

        public ApplicationRole GetItemById(string itemId)
        {
            return dataBase.Roles.Find(itemId);
        }

        public IEnumerable<ApplicationRole> Find(Func<ApplicationRole, Boolean> predicate)
        {
            return dataBase.Roles.Where(predicate);
        }

        public void UpdateItem(ApplicationRole item)
        {
            dataBase.Roles.Update(item);
            
        }

        public void Save()
        {
            dataBase.SaveChanges();
        }
    }
}
