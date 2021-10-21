using System.ComponentModel.DataAnnotations;

namespace TaskManagerTLA.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Імя користувача")]
        public string UserName { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Роль")]
        public string Role { get; set; }
    }
}
