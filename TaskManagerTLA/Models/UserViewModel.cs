using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskManagerTLA.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Вкажіть ім'я користувача")]
        [Display(Name = "Ім'я користувача")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Вкажіть Email адресу")]
        [Display(Name = "Email адреса")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Display(Name = "Ролі")]
        public List<RoleViewModel> Roles { get; set; }

    }
}
