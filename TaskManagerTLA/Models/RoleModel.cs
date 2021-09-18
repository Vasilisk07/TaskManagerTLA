using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagerTLA.Models
{
    public class RoleModel
    {
        public string Id { get; set; }
        [Display(Name = "Ролі")]
        public string UserRole { get; set; }
        
    }
}
