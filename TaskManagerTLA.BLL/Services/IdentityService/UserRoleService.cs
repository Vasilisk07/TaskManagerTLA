using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TaskManagerTLA.BLL.DTO;
using TaskManagerTLA.BLL.Services.IdentityService.Interfaces;
using TaskManagerTLA.DAL.Identity.Entities;
using TaskManagerTLA.DAL.Repositories.Interfaces;

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
            // не ефективно
            // зроби метод await userRepository.GetByIdAsync(userId)
            var userRolesList = (await userRepository.GetAllItemsAsync()).Where(p => p.Id == userId).FirstOrDefault().UserRoles;
            return mapper.Map<IEnumerable<RoleDTO>>(userRolesList);
        }

        public async Task UpdateUserRoleAsync(string userId, string roleId)
        {
            // не ефективно
            // зроби метод await userRepository.GetByIdAsync(userId)
            var user = (await userRepository.GetAllItemsAsync()).Where(p => p.Id == userId).FirstOrDefault();
            // я так розумію старі ролі видаляються тільки тому що ти ще не знаєш як підтримувати декілька ролей?
            user.UserRoles = new List<ApplicationUserRole>
            {
                new ApplicationUserRole { UserId = userId, RoleId = roleId }
            };
            await userRepository.SaveAsync();
        }
    }
}
