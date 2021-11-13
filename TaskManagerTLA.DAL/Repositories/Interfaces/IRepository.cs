using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagerTLA.DAL.Repositories.Interfaces
{

    public interface IRepository<T, I> where T : class 
    {
        IEnumerable<T> GetAllItems();
        T GetItemById(I id);
        IEnumerable<T> Find(Func<T, Boolean> predicate);
        bool CreateItem(T item);
        void UpdateItem(T item);
        bool DeleteItemById(I id);
        bool DeleteItem(T item);
        void DeleteRange(IEnumerable<T> deletedList);
        void Save();
    }

}
