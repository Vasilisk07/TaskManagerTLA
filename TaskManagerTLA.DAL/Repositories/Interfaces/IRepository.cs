using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManagerTLA.DAL.Repositories.Interfaces
{

    public interface IRepository<T, I> where T : class
    {
        Task<IEnumerable<T>> GetAllItemsAsync();
        Task<T> GetItemByIdAsync(I id);
        Task<IEnumerable<T>> FindAsync(Func<T, Boolean> predicate);
        Task<bool> CreateItemAsync(T item);
        Task UpdateItemAsunc(T item);
        Task<bool> DeleteItemByIdAsync(I id);
        Task<bool> DeleteItemAsync(T item);
        Task DeleteRangeAsync(IEnumerable<T> deletedList);
        Task SaveAsync();
    }

}
