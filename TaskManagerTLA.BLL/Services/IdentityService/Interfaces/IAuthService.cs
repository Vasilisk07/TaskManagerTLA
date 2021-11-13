using System.Threading.Tasks;
using TaskManagerTLA.BLL.DTO;

namespace TaskManagerTLA.BLL.Services.IdentityService.Interfaces
{
    public interface IAuthService
    {
        Task LoginAsync(UserDTO loginUser);
        Task LogoutAsync();
    }
}
