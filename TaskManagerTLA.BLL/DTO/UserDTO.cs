using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagerTLA.BLL.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        // TODO UserName багато де використовується як ідентифікатор, хоча у нас уже є Id
        public string UserName { get; set; }
        public string Email { get; set; }
        // TODO should be role id?
        public string UserRole { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }

    }
}
