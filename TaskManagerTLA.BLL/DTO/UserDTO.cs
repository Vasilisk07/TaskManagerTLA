using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagerTLA.BLL.DTO
{
    public class UserDTO
    {
        public string id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string UserRole { get; set; }
        public string Password { get; set; }

    }
}
