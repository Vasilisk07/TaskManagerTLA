using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagerTLA.BLL.DTO;

namespace TaskManagerTLA.BLL.Services.IdentityService.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDTO>> GetRolesAsync();
        Task<RoleDTO> GetRoleByNameAsync(string roleName);
    }
}
