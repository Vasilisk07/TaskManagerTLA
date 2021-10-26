using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagerTLA.BLL.DTO;
using TaskManagerTLA.BLL.Exeption;
using TaskManagerTLA.BLL.Interfaces;
using TaskManagerTLA.DAL.Identity.Entities;
using TaskManagerTLA.DAL.Identity.Interfaces;

namespace TaskManagerTLA.BLL.Services
{
    // цей сервіс робить забагато, треба розділити на AuthService(login, logout), UserService, UserRoleSerivce, RolesService
    // все що зв'язано з тасками - в TaskService
    // почитай про SOLID принципи 
    public class IdentityService : IIdentityService
    {
        private IUnitOfWorkIdentity Db { get; }
        private readonly IMapper mapper;
        private readonly SignInManager<ApplicationUser> signInManager;

        public IdentityService(IUnitOfWorkIdentity identityRepositories, IMapper mapper, SignInManager<ApplicationUser> signInManager)
        {
            Db = identityRepositories;
            this.mapper = mapper;
            this.signInManager = signInManager;
        }

        public async Task Login(UserDTO loginUser)
        {
            var result = await signInManager.PasswordSignInAsync(loginUser.UserName, loginUser.Password, loginUser.RememberMe, false);
            if (!result.Succeeded)
            {
                // якщо хочеш кинути виключення, можна зробити щось типу InvalidCredentialsException чи LoginException, але точно не MyException
                throw new MyException("Невірний логін або пароль.");
            }
        }

        public async Task Logout()
        {
            await signInManager.SignOutAsync();
        }

        //в данному методі не використовую автомапер, тому що в данній реалізації для більшості полів IdentityUser буде присвоєно значення за замовчуванням 
        public void CreateUserAndRole(UserDTO newUser)
        {
            if (GetUserByName(newUser.UserName) != null)
            {
                throw new MyException("Користувач з таким іменем вже існує!");
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
            if (Db.UsersRepository.CreateItem(newUserDb))
            {
                ApplicationRole roleDeveloper = Db.RolesRepository.Find(p => p.Name == "Developer").FirstOrDefault();
                newUserDb.Roles.Add(roleDeveloper);
                Db.Save();
            }
            else
            {
                throw new MyException("Не можливо створити користувача, введено не коректні данні.");
            }
        }

        public void DeleteUser(string userId)
        {
            Db.UsersRepository.DeleteItem(Db.UsersRepository.GetAllItems().Where(p => p.Id == userId).FirstOrDefault());
            Db.Save();
        }

        public void UpdateUserRole(string userId, string newRoleId)
        {
            ApplicationUser user = Db.UsersRepository.GetAllItems().Where(p => p.Id == userId).FirstOrDefault();
            // user.Roles = nw List().Append(Db.RolesRepositories.Find(p => p.Id == newRoleId).FirstOrDefault());
            user.Roles.RemoveRange(0, user.Roles.Count - 1);
            user.Roles.Add(Db.RolesRepository.Find(p => p.Id == newRoleId).FirstOrDefault());
            Db.Save();
        }


        public void DeleteRole(string roleId)
        {
            // добав в RolesRepositories метод DeleteById(..) 
            var DeletedItem = Db.RolesRepository.Find(p => p.Id == roleId).GetEnumerator().Current;
            if (DeletedItem != null)
            {
                Db.RolesRepository.DeleteItem(DeletedItem);
                Db.Save();
            }
        }

        public void CreateRole(string newRoleName)
        {
            Db.RolesRepository.CreateItem(new ApplicationRole(newRoleName));
            Db.Save();
        }

        public IEnumerable<RoleDTO> GetUserRoleName(string userId)
        {
            var userRolesList = Db.UsersRepository.GetAllItems().Where(p => p.Id == userId).FirstOrDefault().UserRoles;
            return mapper.Map<IEnumerable<RoleDTO>>(userRolesList);
        }

        public IEnumerable<RoleDTO> GetRoles()
        {
            IEnumerable<ApplicationRole> rolesDb = Db.RolesRepository.GetAllItems();
            return mapper.Map<IEnumerable<ApplicationRole>, List<RoleDTO>>(rolesDb);
        }

        public UserDTO GetUserById(string userId)
        {
            var user = mapper.Map<UserDTO>(Db.UsersRepository.GetItem(userId));
            return user;
        }

        public UserDTO GetUserByName(string userName)
        {
            ApplicationUser returnedUser = Db.UsersRepository.Find(p => p.UserName == userName).FirstOrDefault();
            var user = mapper.Map<UserDTO>(returnedUser);
            return user;
        }
        public IEnumerable<UserDTO> GetUsersWhoAreNotAssignedTask(int? globalTaskId)
        {
            var allUsers = Db.UsersRepository.GetAllItems().ToList();
            var busyUsers = allUsers.Where(p => p.GlobalTasks.Any(c => c.Id == globalTaskId));
            var freeUsers = allUsers.Except(busyUsers);
            freeUsers = freeUsers.Where(p => p.Roles.Any(c => c.Name == "Developer"));
            return mapper.Map<IEnumerable<ApplicationUser>, List<UserDTO>>(freeUsers);
        }

        public IEnumerable<UserDTO> GetUsers()
        {
            var users = Db.UsersRepository.GetAllItems().ToList();
            return mapper.Map<IEnumerable<ApplicationUser>, List<UserDTO>>(users);
        }
    }
}
