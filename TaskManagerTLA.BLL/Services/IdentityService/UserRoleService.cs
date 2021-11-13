using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using TaskManagerTLA.BLL.DTO;
using TaskManagerTLA.BLL.Services.IdentityService.Interfaces;
using TaskManagerTLA.DAL.Identity.Entities;
using TaskManagerTLA.DAL.UnitOfWork.IdentityUnitOfWork.Interfaces;

namespace TaskManagerTLA.BLL.Services.IdentityService
{
    public class UserRoleService : IUserRoleService
    {
        private IUserUnit DataBase { get; }
        private readonly IMapper mapper;

        public UserRoleService(IUserUnit rolesRepositories, IMapper mapper)
        {
            DataBase = rolesRepositories;
            this.mapper = mapper;
        }

        public IEnumerable<RoleDTO> GetUserRoleName(string userId)
        {
            var userRolesList = DataBase.Users.GetAllItems().Where(p => p.Id == userId).FirstOrDefault().UserRoles;
            return mapper.Map<IEnumerable<RoleDTO>>(userRolesList);
        }

        public void UpdateUserRole(string userId, string roleId)
        {
            var user = DataBase.Users.GetAllItems().Where(p => p.Id == userId).FirstOrDefault();
            var newRoles = new List<ApplicationUserRole>(); 
            newRoles.Add(new ApplicationUserRole { UserId = userId, RoleId = roleId });
            user.UserRoles = newRoles;
            DataBase.Users.Save();
        }
    }
}
