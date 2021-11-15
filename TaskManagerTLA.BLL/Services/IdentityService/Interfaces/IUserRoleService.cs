using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagerTLA.BLL.DTO;

namespace TaskManagerTLA.BLL.Services.IdentityService.Interfaces
{
    public interface IUserRoleService
    {
        Task<IEnumerable<RoleDTO>> GetUserRoleNameAsync(string userId);
        Task UpdateUserRoleAsync(string userId, string roleId);
    }
}
