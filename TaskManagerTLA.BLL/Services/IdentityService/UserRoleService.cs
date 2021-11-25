using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagerTLA.BLL.DTO;
using TaskManagerTLA.BLL.Services.IdentityService.Interfaces;
using TaskManagerTLA.DAL.Identity.Entities;
using TaskManagerTLA.DAL.UnitOfWork.IdentityUnitOfWork.Interfaces;

namespace TaskManagerTLA.BLL.Services.IdentityService
{
    public class UserRoleService : IUserRoleService
    {
        private IUserAndRoleUnit userRoleUnit { get; }
        private readonly IMapper mapper;

        public UserRoleService(IUserAndRoleUnit userRoleUnit, IMapper mapper)
        {
            this.userRoleUnit = userRoleUnit;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<RoleDTO>> GetUserRoleNameAsync(string userId)
        {
            var userRolesList = (await userRoleUnit.Users.GetAllItemsAsync()).Where(p => p.Id == userId).FirstOrDefault().UserRoles;
            return mapper.Map<IEnumerable<RoleDTO>>(userRolesList);
        }

        public async Task UpdateUserRoleAsync(string userId, string roleId)
        {
            var user = (await userRoleUnit.Users.GetAllItemsAsync()).Where(p => p.Id == userId).FirstOrDefault();
            var newRoles = new List<ApplicationUserRole>();
            newRoles.Add(new ApplicationUserRole { UserId = userId, RoleId = roleId });
            user.UserRoles = newRoles;
            await userRoleUnit.Users.SaveAsync();
        }
    }
}
