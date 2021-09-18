using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagerTLA.BLL.DTO;
using TaskManagerTLA.Models;

namespace TaskManagerTLA.BLL.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<IdentityRole, RoleDTO>().ForMember("UserRole", opt => opt.MapFrom(c => c.Name));
            CreateMap<IdentityUser, UserDTO>();
            CreateMap<RegisterModel, UserDTO>();
            CreateMap<UserDTO, UserModel>();
            CreateMap<LoginModel, LoginDTO>();
            CreateMap<RoleDTO, RoleModel>();
            CreateMap<TModel, TaskDTO>();
            CreateMap<TaskDTO, TModel>();
            CreateMap<ActualTaskDTO, ActualTaskViewModel>();
            CreateMap<ActualTaskViewModel, ActualTaskDTO>();
        }
    }
}
