using AutoMapper;
using System.Linq;
using TaskManagerTLA.BLL.DTO;
using TaskManagerTLA.DAL.Entities;
using TaskManagerTLA.DAL.Identity.Entities;
using TaskManagerTLA.Models;

namespace TaskManagerTLA.BLL.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationRole, RoleDTO>();
            CreateMap<ApplicationUser, UserDTO>()
                .ForMember("Role", opt => opt.MapFrom(c => c.Roles.FirstOrDefault().Name));
            CreateMap<RegisterViewModel, UserDTO>();
            CreateMap<UserDTO, UserViewModel>();
            CreateMap<LoginViewModel, UserDTO>();
            CreateMap<RoleDTO, RoleViewModel>();
            CreateMap<GlobalTaskViewModel, GlobalTaskDTO>();
            CreateMap<GlobalTaskDTO, GlobalTaskViewModel>();
            CreateMap<AssignedTask, AssignedTaskDTO>()
                .ForMember("GlobalTaskName", opt => opt.MapFrom(c => c.GlobalTask.Name))
                .ForMember("UserName", opt => opt.MapFrom(c => c.User.UserName));
            CreateMap<GlobalTask, GlobalTaskDTO>();
            CreateMap<GlobalTaskDTO, GlobalTask>();
            CreateMap<AssignedTaskDTO, AssignedTask>();
            CreateMap<AssignedTaskDTO, AssignedTaskViewModel>();
            CreateMap<AssignedTaskViewModel, AssignedTaskDTO>();
        }
    }
}
