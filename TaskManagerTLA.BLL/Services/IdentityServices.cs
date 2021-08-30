using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using TaskManagerTLA.BLL.DTO;
using TaskManagerTLA.BLL.Interfaces;
using TaskManagerTLA.DAL.Identity;
using TaskManagerTLA.DAL.Identity.Interfaces;

namespace TaskManagerTLA.BLL.Services
{
    public class IdentityServices : IIdentityServices
    {
        IUnitOfWorkIdentity Manager { get;  }
     
        IMapper mapper;
        public IdentityServices(IUnitOfWorkIdentity manager)
        {
            Manager = manager;
        }

        public async Task DeleteRoleAsync(string id)
        {
            await Manager.RoleManager.DeleteAsync(await Manager.RoleManager.FindByIdAsync(id));
        }

        public async Task DeleteUserAsync(string id)
        {
            await Manager.UserManager.DeleteAsync(await Manager.UserManager.FindByIdAsync(id));
        }

        public async Task EditUserRoleAsync(string id, string role)
        {
            IdentityUser User = await Manager.UserManager.FindByIdAsync(id);
            var roles = await Manager.UserManager.GetRolesAsync(User);
            await Manager.UserManager.RemoveFromRolesAsync(User, roles.ToArray());
            await Manager.UserManager.AddToRoleAsync(User, role);
        }

        public async Task<string> GetRoleAsync(string UserId)
        {
            string role = (await Manager.UserManager.GetRolesAsync(await Manager.UserManager.FindByIdAsync(UserId))).First();
            return role;
        }

        public IEnumerable<RoleDTO> GetRoles()
        {
            IEnumerable<IdentityRole> rolesDb = Manager.RoleManager.Roles;
            mapper = new MapperConfiguration(cfg => cfg.CreateMap<IdentityRole, RoleDTO>().
                         ForMember("UserRole", opt=>opt.MapFrom(c=>c.Name))).CreateMapper();
            var roles = mapper.Map<IEnumerable<IdentityRole>, List<RoleDTO>>(rolesDb);
            return roles;
        }

        public async Task<UserDTO> GetUserByIdAsync(string id)
        {
            mapper = new MapperConfiguration(cfg => cfg.CreateMap<IdentityUser, UserDTO>()).CreateMapper();
            var user = mapper.Map<UserDTO>(await Manager.UserManager.FindByIdAsync(id));
            return user;

        }
        public async Task<UserDTO> GetUserByNameAsync(string name)
        {
            mapper = new MapperConfiguration(cfg => cfg.CreateMap<IdentityUser, UserDTO>()).CreateMapper();
            var user = mapper.Map<UserDTO>(await Manager.UserManager.FindByNameAsync(name));
            return user;

        }

        public async Task<IEnumerable<UserDTO>> GetUsersAsync()
        {
            mapper = new MapperConfiguration(cfg => cfg.CreateMap<IdentityUser, UserDTO>()).CreateMapper();
            var users = mapper.Map<IEnumerable<IdentityUser>, List<UserDTO>>(Manager.UserManager.Users);
            foreach (var item in users)
            {
                item.UserRole = await GetRoleAsync(item.id);
            }
            return users;
        }

        public async Task MakeRoleAsync(RoleDTO role)
        {
            mapper = new MapperConfiguration(cfg => cfg.CreateMap<RoleDTO, IdentityRole>()).CreateMapper();
            var roleDb = mapper.Map<IdentityRole>(role);
            await Manager.RoleManager.CreateAsync(roleDb);
        }

        public async Task MakeUserAsync(UserDTO user)
        {
            IdentityUser userDb = new IdentityUser() { UserName = user.UserName, Email = user.Email };
            var result = await Manager.UserManager.CreateAsync(userDb, user.Password);
            if (result.Succeeded)
            {
                await Manager.UserManager.AddToRoleAsync(userDb, "Developer");
                await Manager.SignInManager.SignInAsync(userDb, false);
            }
        }
        
        public async Task LoginAsync(LoginDTO loginDTO)
        {
           await  Manager.SignInManager.PasswordSignInAsync(loginDTO.UserName, loginDTO.Password, loginDTO.RememberMe, false);

        }

        public async Task Logout()
        {
           await Manager.SignInManager.SignOutAsync();
        }

    }
}
