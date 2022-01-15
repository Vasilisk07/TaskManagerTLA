using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TaskManagerTLA.BLL.DTO;
using TaskManagerTLA.BLL.Exeption;
using TaskManagerTLA.BLL.Services.IdentityService.Interfaces;
using TaskManagerTLA.DAL.Entities;
using TaskManagerTLA.DAL.Identity.Entities;
using TaskManagerTLA.DAL.UnitOfWork.IdentityUnitOfWork.Interfaces;

namespace TaskManagerTLA.BLL.Services.IdentityService
{
    public class UserService : IUserService
    {
        private IUserAndRoleUnit UserAndRoleUnit { get; }
        private readonly IMapper mapper;

        public UserService(IUserAndRoleUnit userAndRoleUnit, IMapper mapper)
        {
            this.UserAndRoleUnit = userAndRoleUnit;
            this.mapper = mapper;
        }

        public async Task CreateUserAndRoleAsync(UserDTO newUser, string roleName)
        {

            if ((await UserAndRoleUnit.Users.FindFirstItemAsync(p => p.UserName == newUser.UserName)) != null)
            {
                throw new LoginException("Користувач з таким іменем вже існує!");
            }

            //Визначаємо яку роль присвоїти користувачу
            ApplicationRole newUserRole;
            if (await UserAndRoleUnit.Users.AnyThereUsersAsync())
            {
                newUserRole = await UserAndRoleUnit.Roles.GetItemByNameAsync(UserRoles.Admin.ToString());
            }
            else if (roleName != null)
            {
                newUserRole = await UserAndRoleUnit.Roles.GetItemByNameAsync(roleName);
            }
            else
            {
                newUserRole = await UserAndRoleUnit.Roles.GetItemByNameAsync(UserRoles.Developer.ToString());
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
            newUserDb.Roles.Add(newUserRole);
            await UserAndRoleUnit.Users.CreateItemAsync(newUserDb);
            await UserAndRoleUnit.SaveAsync();
        }

        public async Task DeleteUserAsync(string userId)
        {
            await UserAndRoleUnit.Users.DeleteItemByIdAsync(userId);
            await UserAndRoleUnit.SaveAsync();
        }

        public async Task<UserDTO> GetUserByIdAsync(string userId)
        {
            var userDb = await UserAndRoleUnit.Users.GetItemByIdAsync(userId);
            var user = mapper.Map<UserDTO>(userDb);
            return user;
        }

        public async Task<UserDTO> GetUserByNameAsync(string userName)
        {
            var userDb = await UserAndRoleUnit.Users.FindFirstItemAsync(p => p.UserName == userName);
            var user = mapper.Map<UserDTO>(userDb);
            return user;
        }

        public async Task<IEnumerable<UserDTO>> GetUsersWhoAreNotAssignedTaskAsync(int globalTaskId)
        {
            var freeUsers = (await UserAndRoleUnit.Users.GetAllItemsAsync())
                     .Where(p => p.Roles.Any(c => c.Name == UserRoles.Developer.ToString()))
                     .Where(p => p.GlobalTasks.Count == 0 || p.GlobalTasks.All(c => c.Id != globalTaskId));

            return mapper.Map<IEnumerable<ApplicationUser>, List<UserDTO>>(freeUsers);
        }

        public async Task<IEnumerable<UserDTO>> GetUsersAsync()
        {
            var usersDb = (await UserAndRoleUnit.Users.GetAllItemsAsync()).ToList();
            var userDTO = mapper.Map<IEnumerable<ApplicationUser>, List<UserDTO>>(usersDb);
            return userDTO;
        }

        public async Task<IEnumerable<RoleDTO>> GetUserRolesAsync(string userId)
        {
            var roles = (await UserAndRoleUnit.Users.GetItemByIdAsync(userId)).Roles;
            return mapper.Map<IEnumerable<ApplicationRole>, List<RoleDTO>>(roles);
        }

        public async Task ChangeUserRolesAsync(string userId, string[] roles)
        {

            if (roles != null && roles.Length != 0)
            {
                var editUser = await UserAndRoleUnit.Users.GetItemByIdAsync(userId);
                var newRoles = new List<ApplicationRole>();
                foreach (var item in roles)
                {
                    newRoles.Add(await UserAndRoleUnit.Roles.GetItemByNameAsync(item));
                }
                editUser.Roles = newRoles;
                await UserAndRoleUnit.SaveAsync();
            }
        }

        public async Task UserUpdateAsync(UserDTO user)
        {
            if (user.UserName == null || user.Email == null)
            {
                throw new ServiceException("Всі поля повинні бути заповнені");
            }
            else if ((await UserAndRoleUnit.Users.FindFirstItemAsync(p => p.UserName.ToUpper() == user.UserName.ToUpper() && p.Id != user.Id)) != null)
            {
                throw new ServiceException($"Користувач з таким іменем: {user.UserName} вже існує");
            }
            else if ((await UserAndRoleUnit.Users.FindFirstItemAsync(p => p.Email.ToUpper() == user.Email.ToUpper() && p.Id != user.Id)) != null)
            {
                throw new ServiceException($"Користувач з таким Email: {user.Email} вже існує");
            }
            else
            {
                var userDb = await UserAndRoleUnit.Users.GetItemByIdAsync(user.Id);
                userDb.UserName = user.UserName;
                userDb.NormalizedUserName = user.UserName.ToUpper();
                userDb.Email = user.Email;
                userDb.NormalizedEmail = user.Email.ToUpper();
                await UserAndRoleUnit.SaveAsync();
            }

        }
    }
}
