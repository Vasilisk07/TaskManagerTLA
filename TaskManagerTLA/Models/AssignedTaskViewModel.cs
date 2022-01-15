using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskManagerTLA.Models
{
    public class AssignedTaskViewModel
    {
        [Display(Name = "Затрачений час")]
        public int SpentHours { get; set; }

        [Display(Name = "Короткі звіти")]
        public List<AssignedTCommentsViewModel> Comments { get; set; }

        [Display(Name = "Виконавці")]
        public string UserName { get; set; }

        public string UserId { get; set; }

        public int? GlobalTaskId { get; set; }

        [Display(Name = "Назва задачі")]
        public string GlobalTaskName { get; set; }
    }
}
