using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagerTLA.BLL.DTO
{
    public class RoleDTO
    {
        public string Id { get; set; }
        //TODO `Name` so it's a name of the role, like 'developer', 'manager'...
        public string UserRole { get; set; }
    }
}
