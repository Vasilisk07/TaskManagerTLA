using AutoMapper;
using System.Linq;
using System.Collections.Generic;
using TaskManagerTLA.BLL.DTO;
using TaskManagerTLA.BLL.Services.IdentityService.Interfaces;
using TaskManagerTLA.DAL.Identity.Entities;
using TaskManagerTLA.DAL.UnitOfWork.IdentityUnitOfWork;
using TaskManagerTLA.DAL.UnitOfWork.IdentityUnitOfWork.Interfaces;

namespace TaskManagerTLA.BLL.Services.IdentityService
{
    public class RoleService : IRoleService
    {
        private IRoleUnit DataBase { get; }
        private readonly IMapper Mapper;
        public RoleService( IRoleUnit rolesRepositories, IMapper mapper)
        {
            DataBase = rolesRepositories;
            this.Mapper = mapper;
        }
        public RoleDTO GetRoleByName(string roleName)
        {
            var roleDb = DataBase.Roles.Find(p => p.Name == roleName).FirstOrDefault();
            return Mapper.Map<RoleDTO> (roleDb);
        }

        public IEnumerable<RoleDTO> GetRoles()
        {
            IEnumerable<ApplicationRole> rolesDb = DataBase.Roles.GetAllItems();
            return Mapper.Map<IEnumerable<ApplicationRole>, List<RoleDTO>>(rolesDb);
        }

        public void DeleteRole(string roleId)
        {
            if (roleId != null)
            {
                DataBase.Roles.DeleteItemById(roleId);
                DataBase.Roles.Save();
            }
        }

        public void CreateRole(string newRoleName)
        {
            DataBase.Roles.CreateItem(new ApplicationRole(newRoleName));
            DataBase.Roles.Save();
        }

    }
}
