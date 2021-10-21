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
                throw new MyException("Невірний логін або пароль.");
            }
        }

        public async Task Logout()
        {
            await signInManager.SignOutAsync();
        }

        //в данному методі не використовую автомапер, тому що в данній реалізації для більшості полів IdentityUser буде присвоєно значення за замовчуванням 
        public bool CreateUserAndRole(UserDTO newUser)
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
            if (Db.UsersRepositories.CreateItem(newUserDb))
            {
                string developerRoleId = "";
                foreach (var item in Db.RolesRepositories.GetAllItems())
                {
                    if (item.Name == "Developer")
                    {
                        developerRoleId = item.Id;
                    }
                }
                //зміни в Базі Данних зберігаются лише після успішного додавання юзера і ролі в БД, таким чином ми уникаєм можливості створення юзера без ролі 
                return CreateUserRole(newUserDb.Id, developerRoleId);
            }
            else
            {
                throw new MyException("Не можливо створити користувача, введено не коректні данні.");
            }
        }

        public bool CreateUserRole(string userId, string roleId)
        {
            if (Db.UserRolesRepositories.CreateItem(new IdentityUserRole<string>() { RoleId = roleId, UserId = userId }))
            {
                Db.Save();
                return true;
            }
            return false;
        }

        public void DeleteUser(string userId)
        {
            //видаляєм користувача і всі звязані з ним ролі
            if (Db.UsersRepositories.DeleteItem(Db.UsersRepositories.GetItem(userId)))
            {
                DeleteAllUserRoles(userId);
                Db.Save();
            }
        }

        public void UpdateUserRole(string userId, string newRoleId)
        {
            DeleteAllUserRoles(userId);
            CreateUserRole(userId, newRoleId);
        }

        //видаляєм всі звязані з користувачем ролі
        public void DeleteAllUserRoles(string userId)
        {
            var userRoleList = Db.UserRolesRepositories.Find(opt => opt.UserId == userId);
            Db.UserRolesRepositories.DeleteRange(userRoleList);

        }

        public void DeleteRole(string roleId)
        {
            var DeletedItem = Db.RolesRepositories.Find(p => p.Id == roleId).GetEnumerator().Current;
            if (DeletedItem != null)
            {
                Db.RolesRepositories.DeleteItem(DeletedItem);
                Db.Save();
            }
        }

        public void CreateRole(string newRoleName)
        {
            Db.RolesRepositories.CreateItem(new IdentityRole(newRoleName));
            Db.Save();
        }

        public IEnumerable<RoleDTO> GetUserRoleName(string userId)
        {
            var userRolesList = Db.UserRolesRepositories.Find(p => p.UserId == userId);
            return mapper.Map<IEnumerable<RoleDTO>>(userRolesList);
        }

        public IEnumerable<RoleDTO> GetRoles()
        {
            IEnumerable<IdentityRole> rolesDb = Db.RolesRepositories.GetAllItems();
            return mapper.Map<IEnumerable<IdentityRole>, List<RoleDTO>>(rolesDb);
        }

        public UserDTO GetUserById(string userId)
        {
            var user = mapper.Map<UserDTO>(Db.UsersRepositories.GetItem(userId));
            return user;
        }

        public UserDTO GetUserByName(string userName)
        {
            ApplicationUser returnedUser = Db.UsersRepositories.Find(p => p.UserName == userName).FirstOrDefault();
            var user = mapper.Map<UserDTO>(returnedUser);
            return user;
        }
        public IEnumerable<UserDTO> GetUsersWhoAreNotAssignedTask(int? globalTaskId)
        {
            var allUsers = Db.UsersRepositories.GetAllItems().ToList();
            //Rewrite
            allUsers = AssociateWithRoles(allUsers);
            //
            var busyUsers = allUsers.Where(p => p.GlobalTasks.Any(c => c.Id == globalTaskId));
            var freeUsers = allUsers.Except(busyUsers);
            return mapper.Map<IEnumerable<ApplicationUser>, List<UserDTO>>(freeUsers);
        }

        public IEnumerable<UserDTO> GetUsers()
        {
            // TODO так як у нас є EntityFramework він сам може підтягнути UserRole, якщо правильно налаштувати зв'язки

            var users = Db.UsersRepositories.GetAllItems().ToList();
            //Rewrite
            users = AssociateWithRoles(users);
            //
            return mapper.Map<IEnumerable<ApplicationUser>, List<UserDTO>>(users);
        }

        //Ніяк не зміг розыбратись як сформувати звязки в базі данних
        //так щоб звязати юзера з ролями через вже існуючу таблицю AspNetUserRoles
        //тому тут тимчасово використовую таку "заплптку"
        private List<ApplicationUser> AssociateWithRoles(List<ApplicationUser> users)
        {
            var roles = Db.RolesRepositories.GetAllItems().ToList();
            var userroles = Db.UserRolesRepositories.GetAllItems().ToList();
            foreach (var user in users)
            {
                foreach (var userrole in userroles)
                {
                    if (user.Id == userrole.UserId)
                    {
                        foreach (var role in roles)
                        {
                            if (role.Id == userrole.RoleId)
                            {
                                user.Roles.Add(role);

                            }
                        }
                    }
                }
            }
            return users;
        }
    }
}
