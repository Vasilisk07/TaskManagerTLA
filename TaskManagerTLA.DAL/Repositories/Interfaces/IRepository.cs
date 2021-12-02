using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TaskManagerTLA.DAL.Repositories.Interfaces
{

    public interface IRepository<T, I> where T : class
    {
        Task<IEnumerable<T>> GetAllItemsAsync();
        Task<T> GetItemByIdAsync(I id);
        Task<T> FindItemAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindRangeAsync(Expression<Func<T, bool>> predicate);
        Task<bool> CreateItemAsync(T item);
        Task UpdateItemAsync(T item);
        Task<bool> DeleteItemByIdAsync(I id);
        Task<bool> DeleteItemAsync(T item);
        Task DeleteRangeAsync(IEnumerable<T> deletedList);
        Task SaveAsync();
    }

}
