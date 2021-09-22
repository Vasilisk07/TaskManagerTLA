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
        Task Login(UserDTO loginUser, SignInManager<IdentityUser> signInManager);
        Task Logout(SignInManager<IdentityUser> signInManager);
        void RegisterNewUser(UserDTO newUser);
        bool CreateUserAndRole(UserDTO newUser);
        bool CreateUserRole(string userId, string roleId);
        public void DeleteUser(string userId, ITaskService taskService);
        void ChangeUserRole(string userId, string newRoleId);
        void DeleteRole(string roleId);
        void CreateRole(string newRoleName);
        string GetUserRole(string userId);
        IEnumerable<RoleDTO> GetRoles();
        UserDTO GetUserById(string userId);
        UserDTO GetUserByName(string userName);
        IEnumerable<UserDTO> GetUsers();



    }
}
