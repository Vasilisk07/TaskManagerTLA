using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskManagerTLA.BLL.DTO;


namespace TaskManagerTLA.BLL.Interfaces
{
    //TODO `IIdentityService` it's a single service
    public interface IIdentityServices
    {
        Task Login(UserDTO loginUser, SignInManager<IdentityUser> signInManager);
        Task Logout(SignInManager<IdentityUser> signInManager);
        // TODO `AddUser` чи `CreateUser` ти всюди кажеш Create тільки цей метод чимось не вгодив
        void RegisterNewUser(UserDTO newUser);
        // TODO метод вроді як не потрібен, бо є RegisterNewUser і CreateRole
        // більше того ти створюєш UserRole а не Role, метод мав би назватись CreateUserAndUserRole
        // RegisterNewUser не робить нічого важливого, що CreateUserAndRole не міг би зробити
        // хтось може викликати CreateUserAndRole і обійти всю валідацію з RegisterNewUser
        bool CreateUserAndRole(UserDTO newUser);
        bool CreateUserRole(string userId, string roleId);
        // TODO public не потрібен
        public void DeleteUser(string userId, ITaskService taskService);
        void ChangeUserRole(string userId, string newRoleId);
        void DeleteRole(string roleId);
        void CreateRole(string newRoleName);
        //TODO це скоріш GetUserRoleName
        string GetUserRole(string userId);
        IEnumerable<RoleDTO> GetRoles();
        UserDTO GetUserById(string userId);
        UserDTO GetUserByName(string userName);
        IEnumerable<UserDTO> GetUsers();
    }
}
