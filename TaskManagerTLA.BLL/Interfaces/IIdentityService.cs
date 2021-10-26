﻿using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagerTLA.BLL.DTO;

namespace TaskManagerTLA.BLL.Interfaces
{
    public interface IIdentityService
    {
        // ці всі методи можуть бути async, а async це дуже круто, особливо для нас, коли треба чекати на відповідь з бд
        Task Login(UserDTO loginUser);
        Task Logout();
        void CreateUserAndRole(UserDTO newUser);
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
