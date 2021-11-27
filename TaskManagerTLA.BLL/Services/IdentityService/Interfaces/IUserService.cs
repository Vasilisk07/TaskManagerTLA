using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagerTLA.BLL.DTO;

namespace TaskManagerTLA.BLL.Services.IdentityService.Interfaces
{
    public interface IUserService
    {
        Task CreateUserAndRoleAsync(UserDTO newUser);
        Task DeleteUserAsync(string userId);
        Task<IEnumerable<UserDTO>> GetUsersWhoAreNotAssignedTaskAsync(int? globalTaskId);
        Task<UserDTO> GetUserByIdAsync(string userId);
        Task<UserDTO> GetUserByNameAsync(string userName);
        Task<IEnumerable<UserDTO>> GetUsersAsync();
    }
}
