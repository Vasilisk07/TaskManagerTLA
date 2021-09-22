using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagerTLA.DAL.Identity.Interfaces
{
    // TODO IIdentityRepository він один
    public interface IIdentityRepositories<T> where T : class
    {
        bool CreateItem(T newItem);
        bool DeleteItem(T Item);
        IEnumerable<T> GetAllItems();
        T GetItem(string itemId);
    }
}
