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
        private IUserAndRoleUnit userAndRoleUnit { get; }
        private readonly IMapper mapper;

        public UserService(IUserAndRoleUnit userAndRoleUnit, IMapper mapper)
        {
            this.userAndRoleUnit = userAndRoleUnit;
            this.mapper = mapper;
        }

        public async Task CreateUserAndRoleAsync(UserDTO newUser)
        {
            //перевіряєм, якщо юзерів не існує надаєм першому роль адміна всім решта дев

            var role = (await userAndRoleUnit.Roles.FindAsync(p => p.Name == "Developer")).FirstOrDefault();
            if (!(await userAndRoleUnit.Users.GetAllItemsAsync()).Any())
            {
                role = (await userAndRoleUnit.Roles.FindAsync(p => p.Name == "Admin")).FirstOrDefault();
            }

            if ((await userAndRoleUnit.Users.FindAsync(p => p.UserName == newUser.UserName)).Any())
            {
                throw new LoginException("Користувач з таким іменем вже існує!");
            }

            PasswordHasher<ApplicationUser> hasher = new PasswordHasher<ApplicationUser>();
            ApplicationUser newUserDb = new ApplicationUser()
            {
                UserName = newUser.UserName,
                NormalizedUserName = newUser.UserName.ToUpper(),
                Email = newUser.Email,
                NormalizedEmail = newUser.Email.ToUpper(),
            };
            newUserDb.PasswordHash = hasher.HashPassword(newUserDb, newUser.Password);
            if (role != null && newUserDb != null)
            {
                newUserDb.Roles.Add(role);
                await userAndRoleUnit.Users.CreateItemAsync(newUserDb);
                await userAndRoleUnit.Users.SaveAsync();
            }
            else
            {
                throw new LoginException("Не можливо створити користувача, введено не коректні данні.");
            }
        }

        public async Task DeleteUserAsync(string userId)
        {
            await userAndRoleUnit.Users.DeleteItemByIdAsync(userId);
            await userAndRoleUnit.Users.SaveAsync();
        }

        public async Task<UserDTO> GetUserByIdAsync(string userId)
        {
            var userDb = await userAndRoleUnit.Users.GetItemByIdAsync(userId);
            var user = mapper.Map<UserDTO>(userDb);
            return user;
        }

        public async Task<UserDTO> GetUserByNameAsync(string userName)
        {
            var returnedUser = (await userAndRoleUnit.Users.FindAsync(p => p.UserName == userName)).FirstOrDefault();
            var user = mapper.Map<UserDTO>(returnedUser);
            return user;
        }

        public async Task<IEnumerable<UserDTO>> GetUsersWhoAreNotAssignedTaskAsync(int? globalTaskId)
        {
            var allUsers = (await userAndRoleUnit.Users.GetAllItemsAsync()).ToList();
            var busyUsers = allUsers.Where(p => p.GlobalTasks.Any(c => c.Id == globalTaskId));
            var freeUsers = allUsers.Except(busyUsers);
            freeUsers = freeUsers.Where(p => p.Roles.Any(c => c.Name == "Developer"));
            return mapper.Map<IEnumerable<ApplicationUser>, List<UserDTO>>(freeUsers);
        }

        // передаєм в метод імя залогіненого юзера щоб в нього не було можливості редагувати самого себе.
        public async Task<IEnumerable<UserDTO>> GetUsersAsync(string curentUserName)
        {
            var users = (await userAndRoleUnit.Users.GetAllItemsAsync()).Where(p => p.UserName != curentUserName).ToList();
            return mapper.Map<IEnumerable<ApplicationUser>, List<UserDTO>>(users);
        }

    }
}
