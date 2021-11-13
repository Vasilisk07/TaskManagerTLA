using System.Collections.Generic;
using TaskManagerTLA.BLL.DTO;

namespace TaskManagerTLA.BLL.Services.IdentityService.Interfaces
{
    public interface IUserService
    {
        void CreateUserAndRole(UserDTO newUser, RoleDTO newRole);
        void DeleteUser(string userId);
        IEnumerable<UserDTO> GetUsersWhoAreNotAssignedTask(int? globalTaskId);
        UserDTO GetUserById(string userId);
        UserDTO GetUserByName(string userName);
        IEnumerable<UserDTO> GetUsers();
    }
}
