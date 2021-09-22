using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManagerTLA.DAL.EF;
using TaskManagerTLA.DAL.Identity.Interfaces;

namespace TaskManagerTLA.DAL.Identity.Repositories
{
    public class IdentityUserRolesRepositories : IIdentityRepositories<IdentityUserRole<string>>
    {
        private readonly IdentityContext Db;
        public IdentityUserRolesRepositories(IdentityContext identityContext)
        {
            this.Db = identityContext;
        }

        public bool CreateItem(IdentityUserRole<string> newItem)
        {
            var res = Db.UserRoles.Add(newItem);
            bool creationState = false;
            if (res.State == EntityState.Added)
            {
                creationState = true;
            }
            return creationState;
        }

        public bool DeleteItem(IdentityUserRole<string> Item)
        {
            var res = Db.UserRoles.Remove(Item);
            bool deletedState = false;
            if (res.State == EntityState.Deleted)
            {
                deletedState = true;
            }
            return deletedState;
        }

        public IEnumerable<IdentityUserRole<string>> GetAllItems()
        {
            // TODO 
            // не треба робити ніяких видалень в Get методі, я точно не очікую що щось видалиться коли викликаю GetAllItems
            Db.UserRoles.RemoveRange(Db.UserRoles.Where(_ => _.UserId == ""));
            // підозрюю що це повертає пустоту в правильній базі, де немає "поганих" UserRole з UserId==""
            //             return Db.UserRoles; ?
            return Db.UserRoles.Where(_=>_.UserId == "");
        }
        public IdentityUserRole<string> GetItem(string itemId)
        {
            return Db.UserRoles.Find(itemId);
        }
    }
}
