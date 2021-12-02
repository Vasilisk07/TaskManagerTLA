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
        // з маленької букви лише філди, а це пропертя бо в неї є {get;}
        private IRepository<ApplicationRole, string> RoleRepository { get; }
        private readonly IMapper mapper;
        public RoleService(IRepository<ApplicationRole, string> roleRepository, IMapper mapper)
        {
            this.RoleRepository = roleRepository;
            this.mapper = mapper;
        }

        public async Task<RoleDTO> GetRoleByNameAsync(string roleName)
        {
            var role = await RoleRepository.FindItemAsync(p => p.Name == roleName);
            return mapper.Map<RoleDTO>(role);
        }

        public async Task<IEnumerable<RoleDTO>> GetRolesAsync()
        {
            var rolesDb = await RoleRepository.GetAllItemsAsync();
            return mapper.Map<IEnumerable<ApplicationRole>, List<RoleDTO>>(rolesDb);
        }

        public async Task DeleteRoleAsync(string roleId)
        {
            if (roleId != null)
            {
                await RoleRepository.DeleteItemByIdAsync(roleId);
                await RoleRepository.SaveAsync();
            }
        }

        public async Task CreateRoleAsync(string newRoleName)
        {
            await RoleRepository.CreateItemAsync(new ApplicationRole(newRoleName));
            await RoleRepository.SaveAsync();
        }

    }
}
