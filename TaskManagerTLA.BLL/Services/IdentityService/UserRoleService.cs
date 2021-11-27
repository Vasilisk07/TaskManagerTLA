using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagerTLA.BLL.DTO;
using TaskManagerTLA.BLL.Services.IdentityService.Interfaces;
using TaskManagerTLA.DAL.Identity.Entities;
using TaskManagerTLA.DAL.Repositories.Interfaces;
using TaskManagerTLA.DAL.UnitOfWork.IdentityUnitOfWork.Interfaces;

namespace TaskManagerTLA.BLL.Services.IdentityService
{
    public class UserRoleService : IUserRoleService
    {
        private IRepository<ApplicationUser, string> userRepository { get; }
        private readonly IMapper mapper;

        public UserRoleService(IRepository<ApplicationUser, string> userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<RoleDTO>> GetUserRoleNameAsync(string userId)
        {
            var userRolesList = (await userRepository.GetAllItemsAsync()).Where(p => p.Id == userId).FirstOrDefault().UserRoles;
            return mapper.Map<IEnumerable<RoleDTO>>(userRolesList);
        }

        public async Task UpdateUserRoleAsync(string userId, string roleId)
        {
            var user = (await userRepository.GetAllItemsAsync()).Where(p => p.Id == userId).FirstOrDefault();
            var newRoles = new List<ApplicationUserRole>();
            newRoles.Add(new ApplicationUserRole { UserId = userId, RoleId = roleId });
            user.UserRoles = newRoles;
            await userRepository.SaveAsync();
        }
    }
}
