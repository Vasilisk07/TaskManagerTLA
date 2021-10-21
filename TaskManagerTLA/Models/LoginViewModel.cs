using System.ComponentModel.DataAnnotations;

namespace TaskManagerTLA.Models
{
    public class LoginViewModel
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
    }
}
