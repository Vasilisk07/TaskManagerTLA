using System.Collections.Generic;
using TaskManagerTLA.BLL.DTO;

namespace TaskManagerTLA.BLL.Services.IdentityService.Interfaces
{
    public interface IRoleService
    {
        IEnumerable<RoleDTO> GetRoles();
        RoleDTO GetRoleByName(string roleName);
        void DeleteRole(string roleId);
        void CreateRole(string newRoleName);
    }
}
