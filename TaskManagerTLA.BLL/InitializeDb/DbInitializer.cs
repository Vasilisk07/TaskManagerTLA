using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using TaskManagerTLA.DAL.Identity.Entities;


namespace TaskManagerTLA.BLL.InitializeDb
{
    public class DbInitializer
    {
        // це ніколи не повиноо запускатись, інакше можемо поламати данні в бд
        // хіба що лакально для development оточення
        public static async Task InitializeAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            if (await roleManager.FindByNameAsync("Admin") == null)
            {
                await roleManager.CreateAsync(new ApplicationRole("Admin"));
            }

            if (await roleManager.FindByNameAsync("Manager") == null)
            {
                await roleManager.CreateAsync(new ApplicationRole("Manager"));
            }

            if (await roleManager.FindByNameAsync("Developer") == null)
            {
                await roleManager.CreateAsync(new ApplicationRole("Developer"));
            }

            if (await userManager.FindByNameAsync("Administrator") == null)
            {
                ApplicationUser admin = new ApplicationUser { UserName = "Administrator", Email = "Admin@mail.com" };
                IdentityResult res = await userManager.CreateAsync(admin, "1234lL_");
                if (res.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }

            if (await userManager.FindByNameAsync("Bob") == null)
            {
                ApplicationUser manager = new ApplicationUser { UserName = "Bob", Email = "Bob@mail.com" };
                IdentityResult res = await userManager.CreateAsync(manager, "1234lL_");
                if (res.Succeeded)
                {
                    await userManager.AddToRoleAsync(manager, "Manager");
                }
            }

            if (await userManager.FindByNameAsync("Jack") == null)
            {
                ApplicationUser dev = new ApplicationUser { UserName = "Jack", Email = "Jack@mail.com" };
                IdentityResult res = await userManager.CreateAsync(dev, "1234lL_");
                if (res.Succeeded)
                {
                    await userManager.AddToRoleAsync(dev, "Developer");
                }
            }

            if (await userManager.FindByNameAsync("Nick") == null)
            {
                ApplicationUser dev = new ApplicationUser { UserName = "Nick", Email = "Nick@mail.com" };
                IdentityResult res = await userManager.CreateAsync(dev, "1234lL_");
                if (res.Succeeded)
                {
                    await userManager.AddToRoleAsync(dev, "Developer");
                }
            }

            if (await userManager.FindByNameAsync("Adam") == null)
            {
                ApplicationUser dev = new ApplicationUser { UserName = "Adam", Email = "Adam@mail.com" };
                IdentityResult res = await userManager.CreateAsync(dev, "1234lL_");
                if (res.Succeeded)
                {
                    await userManager.AddToRoleAsync(dev, "Developer");
                }
            }
        }
    }
}
