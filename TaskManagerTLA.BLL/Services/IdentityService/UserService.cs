using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TaskManagerTLA.BLL.DTO;
using TaskManagerTLA.BLL.Exeption;
using TaskManagerTLA.BLL.Services.IdentityService.Interfaces;
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

        public async Task CreateUserAndRoleAsync(UserDTO newUser)
        {
            //перевіряєм, якщо юзерів не існує надаєм першому роль адміна всім решта дев
            //RB тоді краще сопчатку перевірити, а потім рішати дев чи ні, щоб не ганяти лишній реквест
            // а якщо адмін хоче добавити ще однго адміна, хіба не простіше одарзу передати сюди яку роль присвоїти?
            //ролі бажано запхнути в enum, бо дуже лешко промахнутись буквою
            var role = await UserAndRoleUnit.Roles.FindItemAsync(p => p.Name == "Developer");

            // ця перевірка дістане всі мільон юзерів, тільки для того щоб перевірити чи є один, краще перевірити dbSet.Any()
            if (!(await UserAndRoleUnit.Users.GetAllItemsAsync()).Any())
            {
                role = await UserAndRoleUnit.Roles.FindItemAsync(p => p.Name == "Admin");
            }

            // в тебе не може бути більше однго юзера з таким userName, тому діставати рендж не варто
            // навіть якщо в бд один такий юзер - твій запит перелопатить усю таблицю
            // зроби метод Task<ApplicationUser> users.GetByNameAsync()
            if ((await UserAndRoleUnit.Users.FindRangeAsync(p => p.UserName == newUser.UserName)).Any())
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

            if (role != null && newUserDb != null) // role != null && newUserDb != null вони ніколи не можуть бути в тебе null в цій точці 
            {
                newUserDb.Roles.Add(role);
                await UserAndRoleUnit.Users.CreateItemAsync(newUserDb);
                // має бути await UserAndRoleUnit.SaveAsync(); в цьому і роль юніта
                await UserAndRoleUnit.Users.SaveAsync();
            }
            else
            {
                throw new LoginException("Не можливо створити користувача, введено не коректні данні.");
            }
        }

        public async Task DeleteUserAsync(string userId)
        {
            await UserAndRoleUnit.Users.DeleteItemByIdAsync(userId);
            // має бути await UserAndRoleUnit.SaveAsync(); в цьому і роль юніта
            await UserAndRoleUnit.Users.SaveAsync();
            await UserAndRoleUnit.Users.SaveAsync();
        }

        public async Task<UserDTO> GetUserByIdAsync(string userId)
        {
            var userDb = await UserAndRoleUnit.Users.GetItemByIdAsync(userId);
            var user = mapper.Map<UserDTO>(userDb);
            return user;
        }

        public async Task<UserDTO> GetUserByNameAsync(string userName)
        {
            // в попередньому методі ця змінна називалаьс userDb, притримуйся одної термінології це полегшить життя і тобі і тим хто буде бачити твій код
            var returnedUser = await UserAndRoleUnit.Users.FindItemAsync(p => p.UserName == userName);
            var user = mapper.Map<UserDTO>(returnedUser);
            return user;
        }

        public async Task<IEnumerable<UserDTO>> GetUsersWhoAreNotAssignedTaskAsync(int? globalTaskId)
        {
            // поламав всю красу LINQ
            //var allUsers = (await UserAndRoleUnit.Users.GetAllItemsAsync()).ToList(); // тут ти дістаєш ВСІ записи в пам'ять, а якщо їх мільйон?
            //var busyUsers = allUsers.Where(p => p.GlobalTasks.Any(c => c.Id == globalTaskId)); 
            //var freeUsers = allUsers.Except(busyUsers); // це зайве, просто інвертиш попередню умову
            //freeUsers = freeUsers.Where(p => p.Roles.Any(c => c.Name == "Developer"));

            var freeUsers = (await UserAndRoleUnit.Users.GetAllItemsAsync())
                .Where(p => p.GlobalTasks.Any(c => c.Id != globalTaskId))
                .Where(p => p.Roles.Any(c => c.Name == "Developer"));

            return mapper.Map<IEnumerable<ApplicationUser>, List<UserDTO>>(freeUsers);
        }

        public async Task<IEnumerable<UserDTO>> GetUsersAsync()
        {
            var users = (await UserAndRoleUnit.Users.GetAllItemsAsync()).ToList();
            return mapper.Map<IEnumerable<ApplicationUser>, List<UserDTO>>(users);
        }
    }
}
