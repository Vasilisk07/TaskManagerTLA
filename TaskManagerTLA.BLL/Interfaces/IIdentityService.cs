using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagerTLA.BLL.DTO;

namespace TaskManagerTLA.BLL.Interfaces
{
    public interface IIdentityService
    {
        Task Login(UserDTO loginUser);
        Task Logout();
        bool CreateUserAndRole(UserDTO newUser);
        bool CreateUserRole(string userId, string roleId);
        void DeleteUser(string userId);
        void UpdateUserRole(string userId, string newRoleId);
        void DeleteRole(string roleId);
        void CreateRole(string newRoleName);
        IEnumerable<RoleDTO> GetUserRoleName(string userId);
        IEnumerable<UserDTO> GetUsersWhoAreNotAssignedTask(int? globalTaskId);
        IEnumerable<RoleDTO> GetRoles();
        UserDTO GetUserById(string userId);
        UserDTO GetUserByName(string userName);
        IEnumerable<UserDTO> GetUsers();
    }
}
