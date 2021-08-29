using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagerTLA.Models
{
    public class LoginModel
    {
       
        [Required]
        [Display(Name = "Імя користувача")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        [Display(Name = "Запамятати?")]
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }

    }
}
