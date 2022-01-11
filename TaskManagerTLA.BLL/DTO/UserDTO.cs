using System.Collections.Generic;

namespace TaskManagerTLA.BLL.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<RoleDTO> Roles { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
