using System.Threading.Tasks;

namespace TaskManagerTLA.DAL.Repositories.Interfaces
{
    public interface IUserRepository<T, I> : IRepository<T, I>
    {
        Task<bool> AnyThereUsersAsync();
    }
}
