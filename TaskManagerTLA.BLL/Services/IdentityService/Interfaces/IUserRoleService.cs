using System.Collections.Generic;
using TaskManagerTLA.BLL.DTO;

namespace TaskManagerTLA.BLL.Services.IdentityService.Interfaces
{
    public interface IUserRoleService
    {
        IEnumerable<RoleDTO> GetUserRoleName(string userId);
        void UpdateUserRole(string userId, string roleId);
    }
}
