using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using TaskManagerTLA.BLL.DTO;
using TaskManagerTLA.BLL.Exeption;
using TaskManagerTLA.BLL.Services.IdentityService.Interfaces;
using TaskManagerTLA.DAL.Identity.Entities;

namespace TaskManagerTLA.BLL.Services.IdentityService
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<ApplicationUser> signInManager;

        public AuthService(SignInManager<ApplicationUser> signInManager)
        {
            this.signInManager = signInManager;
        }
        public async Task LoginAsync(UserDTO loginUser)
        {
            var result = await signInManager.PasswordSignInAsync(loginUser.UserName, loginUser.Password, loginUser.RememberMe, false);
            if (!result.Succeeded)
            {
                throw new LoginException("Невірний логін або пароль.");
            }
        }
        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();
        }


    }
}
