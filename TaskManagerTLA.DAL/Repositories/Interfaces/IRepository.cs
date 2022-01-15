using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TaskManagerTLA.DAL.Repositories.Interfaces
{
    public interface IRepository<T, I>
    {
        Task<IEnumerable<T>> GetAllItemsAsync();
        Task<T> GetItemByIdAsync(I id);
        Task<T> FindFirstItemAsync(Expression<Func<T, bool>> predicate);
        Task<bool> CreateItemAsync(T item);
        Task<bool> DeleteItemAsync(T item);
        Task<bool> DeleteItemByIdAsync(I itemId);
        Task SaveAsync();
    }
}
