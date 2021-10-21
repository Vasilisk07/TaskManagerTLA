using System.ComponentModel.DataAnnotations;

namespace TaskManagerTLA.Models
{
    public class EditAssignedTaskViewModel
    {
        public int GlobalTaskId { get; set; }
        public string UserId { get; set; }
        [Display(Name = "Затрачений час")]
        public int SpentHours { get; set; }
        [Display(Name = "Короткий звіт")]
        public string Description { get; set; }
    }
}
