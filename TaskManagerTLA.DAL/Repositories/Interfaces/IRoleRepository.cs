using System.Threading.Tasks;
using TaskManagerTLA.DAL.Identity.Entities;

namespace TaskManagerTLA.DAL.Repositories.Interfaces
{
    public interface IRoleRepository<T, I> : IRepository<T, I>
    {
        Task<ApplicationRole> GetItemByNameAsync(string itemName);
    }
}
