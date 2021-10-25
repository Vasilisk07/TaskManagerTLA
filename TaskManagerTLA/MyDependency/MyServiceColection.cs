using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManagerTLA.BLL.Interfaces;
using TaskManagerTLA.BLL.Mapper;
using TaskManagerTLA.BLL.Services;
using TaskManagerTLA.DAL.EF;
using TaskManagerTLA.DAL.Identity.Entities;
using TaskManagerTLA.DAL.Identity.Interfaces;
using TaskManagerTLA.DAL.Identity.Repositories;
using TaskManagerTLA.DAL.Interfaces;
using TaskManagerTLA.DAL.Repositories;

namespace TaskManagerTLA.MyDependency
{
    public static class MyServiceColection
    {
        public static IServiceCollection AddMyService(this IServiceCollection services, IConfiguration configuration)
        {
            string connection = configuration.GetConnectionString("DefaultConnection");
            services.AddTransient<IUnitOfWork, EFUnitOfWork>();
            services.AddTransient<ITaskService, TaskService>();
            services.AddDbContext<IdentityContext>(options => options.UseSqlServer(connection));
            services.AddDbContext<TaskContext>(options => options.UseSqlServer(connection));
            services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<IdentityContext>();
            services.AddTransient<IUnitOfWorkIdentity, UnitOfWorkIdentity>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddAutoMapper(typeof(MappingProfile));
            return services;
        }
    }
}
