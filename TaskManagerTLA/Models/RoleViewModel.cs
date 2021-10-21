using System.ComponentModel.DataAnnotations;

namespace TaskManagerTLA.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Ролі")]
        public string Name { get; set; }
    }
}
