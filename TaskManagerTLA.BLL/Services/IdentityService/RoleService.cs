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
        private IRoleUnit roleUnit { get; }
        private readonly IMapper mapper;
        public RoleService(IRoleUnit roleUnit, IMapper mapper)
        {
            this.roleUnit = roleUnit;
            this.mapper = mapper;
        }

        public async Task<RoleDTO> GetRoleByNameAsync(string roleName)
        {
            var roleDbList = await roleUnit.Roles.FindAsync(p => p.Name == roleName);
            var roleDb = roleDbList.FirstOrDefault();
            return mapper.Map<RoleDTO>(roleDb);
        }

        public async Task<IEnumerable<RoleDTO>> GetRolesAsync()
        {
            var rolesDb = await roleUnit.Roles.GetAllItemsAsync();
            return mapper.Map<IEnumerable<ApplicationRole>, List<RoleDTO>>(rolesDb);
        }

        public async Task DeleteRoleAsync(string roleId)
        {
            if (roleId != null)
            {
                await roleUnit.Roles.DeleteItemByIdAsync(roleId);
                await roleUnit.Roles.SaveAsync();
            }
        }

        public async Task CreateRoleAsync(string newRoleName)
        {
            await roleUnit.Roles.CreateItemAsync(new ApplicationRole(newRoleName));
            await roleUnit.Roles.SaveAsync();
        }

    }
}
