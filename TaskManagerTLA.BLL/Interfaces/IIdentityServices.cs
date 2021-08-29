using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskManagerTLA.BLL.DTO;

namespace TaskManagerTLA.BLL.Interfaces
{
    public interface IIdentityServices
    {
        Task MakeUserAsync(UserDTO users);
        Task MakeRoleAsync(RoleDTO taskDTO);
        Task <UserDTO> GetUserByIdAsync(string id);
        Task DeleteUserAsync(string id);
        Task DeleteRoleAsync(string id);
        Task <IEnumerable<UserDTO>> GetUsersAsync();
        Task<UserDTO> GetUserByNameAsync(string name);
        Task <string> GetRoleAsync(string userId);
        IEnumerable <RoleDTO> GetRoles();
        Task EditUserRoleAsync(string userId, string role);
        Task LoginAsync(LoginDTO loginDTO);
        Task Logout();
        

    }
}
