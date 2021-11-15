using AutoMapper;
using System.Linq;
using System.Collections.Generic;
using TaskManagerTLA.BLL.DTO;
using TaskManagerTLA.BLL.Services.IdentityService.Interfaces;
using TaskManagerTLA.DAL.Identity.Entities;
using TaskManagerTLA.DAL.UnitOfWork.IdentityUnitOfWork.Interfaces;
using System.Threading.Tasks;

namespace TaskManagerTLA.BLL.Services.IdentityService
{
    public class RoleService : IRoleService
    {
        private IRoleUnit DataBase { get; }
        private readonly IMapper Mapper;
        public RoleService(IRoleUnit rolesRepositories, IMapper mapper)
        {
            DataBase = rolesRepositories;
            this.Mapper = mapper;
        }

        public async Task<RoleDTO> GetRoleByNameAsync(string roleName)
        {
            return await Task.Run(async() =>
            {
                var roleDbList = await DataBase.Roles.FindAsync(p => p.Name == roleName);
                var roleDb = roleDbList.FirstOrDefault();
                return Mapper.Map<RoleDTO>(roleDb);
            });

        }

        public async Task<IEnumerable<RoleDTO>> GetRolesAsync()
        {
            
            return await Task.Run(async() =>
            {
                var rolesDb = await DataBase.Roles.GetAllItemsAsync();
                return Mapper.Map<IEnumerable<ApplicationRole>, List<RoleDTO>>(rolesDb);
            });
        }

        public async Task DeleteRoleAsync(string roleId)
        {
            if (roleId != null)
            {
               await DataBase.Roles.DeleteItemByIdAsync(roleId);
               await  DataBase.Roles.SaveAsync();
            }
        }

        public async Task CreateRoleAsync(string newRoleName)
        {
            await DataBase.Roles.CreateItemAsync(new ApplicationRole(newRoleName));
            await DataBase .Roles.SaveAsync();
        }

    }
}
