using System;
using System.Collections.Generic;

namespace TaskManagerTLA.DAL.Identity.Interfaces
{
    public interface IIdentityRepository<T> where T : class
    {
        bool CreateItem(T newItem);
        bool DeleteItem(T Item);
        void DeleteRange(IEnumerable<T> deletedList);
        IEnumerable<T> GetAllItems();
        T GetItem(string itemId);
        IEnumerable<T> Find(Func<T, Boolean> predicate);
    }
}
