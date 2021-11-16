using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TaskManagerTLA.BLL.InitializeDb;
using TaskManagerTLA.DAL.Identity.Entities;

namespace TaskManagerTLA
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            // використав не зовсім вдале рішення, бо шось не дуже зрозумів як правельно ініціалізувати базу данних початковими ролями,
            // якщо ми використовуєм підхід base first тоді все ок все зрозуміло, але якщо ми використовуєм code first, тоді не зовсім
            // зрозуміло як правельно ініціалізувати базу данних тимиж ролями і хочаб одним адміном, створити метод з данними і кожного разу моніторити чи 
            // створено базу і чи є в ній записи чи брати початкові данні з файлу конфігураціїї, чи в SQL якись скріпт писати, 
            // і це все певно має бути якось захищено певно паролі не повинні лежати в явному вигляді,
            // чи взагалі ніякого адміна не потрібно створювати а просто першому користувачеві треба присвоювати роль адміна
            // коротше трохи заплутався тут, якшо можеш то хоча б в двух словах розясни як то його все правельно розложити ту ініціалізацію)
#if DEBUG
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    // ми точно не хочемо робити це для продакшина, так як при кожному запуску програми база буде очищатись 
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var rolesManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
                    await DbInitializer.InitializeAsync(userManager, rolesManager);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
#endif
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureLogging((context, builder) =>
            {
                builder.AddConfiguration(context.Configuration.GetSection("Logging"));
                builder.AddConsole();
                builder.AddFile();
            })

        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
    }
}
