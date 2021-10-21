using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TaskManagerTLA.BLL.Interfaces;
using TaskManagerTLA.BLL.Mapper;
using TaskManagerTLA.BLL.Services;
using TaskManagerTLA.DAL.EF;
using TaskManagerTLA.DAL.Identity.Entities;
using TaskManagerTLA.DAL.Identity.Interfaces;
using TaskManagerTLA.DAL.Identity.Repositories;
using TaskManagerTLA.DAL.Interfaces;
using TaskManagerTLA.DAL.Repositories;


namespace TaskManagerTLA
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddTransient<IUnitOfWork>(x => new EFUnitOfWork(connection));
            services.AddTransient<ITaskService, TaskService>();
            services.AddDbContext<IdentityContext>(options => options.UseSqlServer(connection));
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<IdentityContext>();
            services.AddTransient<IUnitOfWorkIdentity>(x => new UnitOfWorkIdentity(connection));
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddAutoMapper(typeof(MappingProfile));


            services.AddControllersWithViews();
        } 

        public void Configure(IApplicationBuilder app, ILogger<Startup> logger, IWebHostEnvironment env )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
          
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseResponseCaching(); 
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
