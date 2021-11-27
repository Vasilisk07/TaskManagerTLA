using AutoMapper;
using System.Linq;
using System.Collections.Generic;
using TaskManagerTLA.BLL.DTO;
using TaskManagerTLA.BLL.Services.IdentityService.Interfaces;
using TaskManagerTLA.DAL.Identity.Entities;
using System.Threading.Tasks;
using TaskManagerTLA.DAL.Repositories.Interfaces;

namespace TaskManagerTLA.BLL.Services.IdentityService
{
    public class RoleService : IRoleService
    {
        private IRepository<ApplicationRole, string> roleRepository { get; }
        private readonly IMapper mapper;
        public RoleService(IRepository<ApplicationRole, string> roleRepository, IMapper mapper)
        {
            this.roleRepository = roleRepository;
            this.mapper = mapper;
        }

        public async Task<RoleDTO> GetRoleByNameAsync(string roleName)
        {
            var roleDbList = await roleRepository.FindItemAsync(p => p.Name == roleName);
            return mapper.Map<RoleDTO>(roleDbList);
        }

        public async Task<IEnumerable<RoleDTO>> GetRolesAsync()
        {
            var rolesDb = await roleRepository.GetAllItemsAsync();
            return mapper.Map<IEnumerable<ApplicationRole>, List<RoleDTO>>(rolesDb);
        }

        public async Task DeleteRoleAsync(string roleId)
        {
            if (roleId != null)
            {
                await roleRepository.DeleteItemByIdAsync(roleId);
                await roleRepository.SaveAsync();
            }
        }

        public async Task CreateRoleAsync(string newRoleName)
        {
            await roleRepository.CreateItemAsync(new ApplicationRole(newRoleName));
            await roleRepository.SaveAsync();
        }

    }
}
