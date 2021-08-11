using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerTLA.BLL.InitializeDb
{
   public class DbInitializer
    {
        public static async Task InitializeAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (await roleManager.FindByNameAsync("Admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (await roleManager.FindByNameAsync("Manager") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Manager"));
            }
            if (await roleManager.FindByNameAsync("Developer") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Developer"));
            }

            if (await userManager.FindByNameAsync("Administrator") == null)
            {
                IdentityUser admin = new IdentityUser { UserName = "Administrator", Email = "Admin@mail.com" };
                IdentityResult res = await userManager.CreateAsync(admin, "1234lL_");
                if (res.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
            if (await userManager.FindByNameAsync("Bob") == null)
            {
                IdentityUser manager = new IdentityUser { UserName = "Bob", Email = "Bob@mail.com" };
                IdentityResult res = await userManager.CreateAsync(manager, "1234lL_");
                if (res.Succeeded)
                {
                    await userManager.AddToRoleAsync(manager, "Manager");
                }
            }

            if (await userManager.FindByNameAsync("Jack") == null)
            {
                IdentityUser dev = new IdentityUser { UserName = "Jack", Email = "Jack@mail.com" };
                IdentityResult res = await userManager.CreateAsync(dev, "1234lL_");
                if (res.Succeeded)
                {
                    await userManager.AddToRoleAsync(dev, "Developer");
                }
            }
            if (await userManager.FindByNameAsync("Nick") == null)
            {
                IdentityUser dev = new IdentityUser { UserName = "Nick", Email = "Nick@mail.com" };
                IdentityResult res = await userManager.CreateAsync(dev, "1234lL_");
                if (res.Succeeded)
                {
                    await userManager.AddToRoleAsync(dev, "Developer");
                }
            }
            if (await userManager.FindByNameAsync("Adam") == null)
            {
                IdentityUser dev = new IdentityUser { UserName = "Adam", Email = "Adam@mail.com" };
                IdentityResult res = await userManager.CreateAsync(dev, "1234lL_");
                if (res.Succeeded)
                {
                    await userManager.AddToRoleAsync(dev, "Developer");
                }
            }


        }


    }
}
