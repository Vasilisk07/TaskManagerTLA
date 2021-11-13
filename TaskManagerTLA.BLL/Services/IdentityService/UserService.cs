using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using TaskManagerTLA.BLL.DTO;
using TaskManagerTLA.BLL.Exeption;
using TaskManagerTLA.BLL.Services.IdentityService.Interfaces;
using TaskManagerTLA.DAL.Identity.Entities;
using TaskManagerTLA.DAL.UnitOfWork.IdentityUnitOfWork.Interfaces;

namespace TaskManagerTLA.BLL.Services.IdentityService
{
    public class UserService : IUserService
    {
        private IUserUnit DataBase { get; }
        private readonly IMapper mapper;

        public UserService(IUserUnit usersRepositories, IMapper mapper)
        {
            DataBase = usersRepositories;
            this.mapper = mapper;
        }

        public void CreateUserAndRole(UserDTO newUser, RoleDTO newRole)
        {
            if (DataBase.Users.Find(p => p.UserName == newUser.UserName).Count() >= 1)
            {
                throw new LoginException("Користувач з таким іменем вже існує!");
            }
            //створення User
            PasswordHasher<ApplicationUser> hasher = new PasswordHasher<ApplicationUser>();
            ApplicationUser newUserDb = new ApplicationUser()
            {
                UserName = newUser.UserName,
                NormalizedUserName = newUser.UserName.ToUpper(),
                Email = newUser.Email,
                NormalizedEmail = newUser.Email.ToUpper(),
            };
            newUserDb.PasswordHash = hasher.HashPassword(newUserDb, newUser.Password);
            //присвоюєм юзеру роль "Developer"
            //але ж ми можемо вяти роль з newUser
            if (DataBase.Users.CreateItem(newUserDb) && newRole != null)
            {
                newUserDb.UserRoles.Add(new ApplicationUserRole { RoleId = newRole.Id, UserId = newUser.Id });
                DataBase.Users.Save();
            }
            else
            {
                throw new LoginException("Не можливо створити користувача, введено не коректні данні.");
            }
        }

        public void DeleteUser(string userId)
        {
            DataBase.Users.DeleteItemById(userId);
            DataBase.Users.Save();
        }

        public UserDTO GetUserById(string userId)
        {
            var user = mapper.Map<UserDTO>(DataBase.Users.GetItemById(userId));
            return user;
        }

        public UserDTO GetUserByName(string userName)
        {
            ApplicationUser returnedUser = DataBase.Users.Find(p => p.UserName == userName).FirstOrDefault();
            var user = mapper.Map<UserDTO>(returnedUser);
            return user;
        }

        public IEnumerable<UserDTO> GetUsersWhoAreNotAssignedTask(int? globalTaskId)
        {
            var allUsers = DataBase.Users.GetAllItems().ToList();
            var busyUsers = allUsers.Where(p => p.GlobalTasks.Any(c => c.Id == globalTaskId));
            var freeUsers = allUsers.Except(busyUsers);
            freeUsers = freeUsers.Where(p => p.Roles.Any(c => c.Name == "Developer"));
            return mapper.Map<IEnumerable<ApplicationUser>, List<UserDTO>>(freeUsers);
        }

        public IEnumerable<UserDTO> GetUsers()
        {
            var users = DataBase.Users.GetAllItems().ToList();
            return mapper.Map<IEnumerable<ApplicationUser>, List<UserDTO>>(users);
        }

    }
}
