using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagerTLA.DAL.Interfaces
{
    // нічим не відрізняється від IIdentityRepository, може їх не треба два? Або наслдіувати IIdentityRepository від IRepository
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        IEnumerable<T> Find(Func<T, Boolean> predicate);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
        void DeleteRange(IEnumerable<T> deletedList);
    }
}
