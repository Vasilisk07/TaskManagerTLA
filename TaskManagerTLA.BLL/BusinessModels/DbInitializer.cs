using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerTLA.BLL.BusinessModels
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
                IdentityUser admin = new IdentityUser { UserName = "Administrator" };
                IdentityResult res = await userManager.CreateAsync(admin, "123L_");
                if (res.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }





        }




    }
}
