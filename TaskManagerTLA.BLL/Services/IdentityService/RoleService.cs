using AutoMapper;
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
        private IRoleRepository<ApplicationRole, string> RoleRepository { get; }
        private readonly IMapper mapper;
        public RoleService(IRoleRepository<ApplicationRole, string> roleRepository, IMapper mapper)
        {
            this.RoleRepository = roleRepository;
            this.mapper = mapper;
        }

        public async Task<RoleDTO> GetRoleByNameAsync(string roleName)
        {
            var role = await RoleRepository.GetItemByNameAsync(roleName);
            return mapper.Map<RoleDTO>(role);
        }

        public async Task<IEnumerable<RoleDTO>> GetRolesAsync()
        {
            var rolesDb = await RoleRepository.GetAllItemsAsync();
            return mapper.Map<IEnumerable<ApplicationRole>, List<RoleDTO>>(rolesDb);
        }

    }
}
