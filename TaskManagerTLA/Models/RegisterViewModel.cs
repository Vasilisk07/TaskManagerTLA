using System.ComponentModel.DataAnnotations;

namespace TaskManagerTLA.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Вкажіть ім'я користувача")]
        [Display(Name = "Ім'я користувача")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Вкажіть Email адресу")]
        [Display(Name = "Email адреса")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [MinLength(5)]
        [Required(ErrorMessage = "Вкажіть пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Повторіть введений пароль")]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        [DataType(DataType.Password)]
        [Display(Name = "Підтвердіть пароль")]
        public string PasswordConfirm { get; set; }

    }
}
