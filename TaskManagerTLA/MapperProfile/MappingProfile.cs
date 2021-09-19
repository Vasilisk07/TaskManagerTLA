using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagerTLA.BLL.DTO;
using TaskManagerTLA.DAL.Entities;
using TaskManagerTLA.Models;

namespace TaskManagerTLA.BLL.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Тут я нарешті розібрався з мапером) я так думаю.
            CreateMap<IdentityRole, RoleDTO>().ForMember("UserRole", opt => opt.MapFrom(c => c.Name));
            CreateMap<IdentityUser, UserDTO>();
            CreateMap<RegisterViewModel, UserDTO>();
            CreateMap<UserDTO, UserViewModel>();
            CreateMap<LoginViewModel, UserDTO>();
            CreateMap<RoleDTO, RoleViewModel>();
            CreateMap<TaskViewModel, TaskDTO>();
            CreateMap<TaskDTO, TaskViewModel>();
            CreateMap<ActualTask, ActualTaskDTO>();
            CreateMap<TaskModel, TaskDTO>();
            CreateMap<TaskDTO, TaskModel>();
            CreateMap<ActualTaskDTO, ActualTask>();
            CreateMap<ActualTaskDTO, ActualTaskViewModel>();
            CreateMap<ActualTaskViewModel, ActualTaskDTO>();
        }
    }
}
