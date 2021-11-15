using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task CreateUserAndRoleAsync(UserDTO newUser, RoleDTO newRole)
        {
            if ((await DataBase.Users.FindAsync(p => p.UserName == newUser.UserName)).Count() >= 1)
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
            if ((await DataBase.Users.CreateItemAsync(newUserDb)) && newRole != null)
            {
                newUserDb.UserRoles.Add(new ApplicationUserRole { RoleId = newRole.Id, UserId = newUser.Id });
                await DataBase.Users.SaveAsync();
            }
            else
            {
                throw new LoginException("Не можливо створити користувача, введено не коректні данні.");
            }
        }

        public async Task DeleteUserAsync(string userId)
        {
            await DataBase.Users.DeleteItemByIdAsync(userId);
            await DataBase.Users.SaveAsync();
        }

        public async Task<UserDTO> GetUserByIdAsync(string userId)
        {
            var userDb = await DataBase.Users.GetItemByIdAsync(userId);
            var user = mapper.Map<UserDTO>(userDb);
            return user;
        }

        public async Task<UserDTO> GetUserByNameAsync(string userName)
        {
            var returnedUser = (await DataBase.Users.FindAsync(p => p.UserName == userName)).FirstOrDefault();
            var user = mapper.Map<UserDTO>(returnedUser);
            return user;
        }

        //This method can be rewritten, and made optimized
        public async Task<IEnumerable<UserDTO>> GetUsersWhoAreNotAssignedTaskAsync(int? globalTaskId)
        {
            var allUsers = (await DataBase.Users.GetAllItemsAsync()).ToList();
            var busyUsers = allUsers.Where(p => p.GlobalTasks.Any(c => c.Id == globalTaskId));
            var freeUsers = allUsers.Except(busyUsers);
            freeUsers = freeUsers.Where(p => p.Roles.Any(c => c.Name == "Developer"));
            return mapper.Map<IEnumerable<ApplicationUser>, List<UserDTO>>(freeUsers);
        }

        public async Task<IEnumerable<UserDTO>> GetUsersAsync()
        {
            var users = (await DataBase.Users.GetAllItemsAsync()).ToList();
            return mapper.Map<IEnumerable<ApplicationUser>, List<UserDTO>>(users);
        }

    }
}
