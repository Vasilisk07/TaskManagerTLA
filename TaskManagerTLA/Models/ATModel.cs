using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagerTLA.Models
{
    public class ATModel
    {  
        public int ActualTaskId { get; set; }
        [Display(Name = "Затрачений час")]
        public int ActTaskLeigth { get; set; }
        [Display(Name = "Короткі звіти")]
        public string Description { get; set; }
        [Display(Name = "Виконавці")]
        public string UserName { get; set; }
        [Display(Name = "Назва задачі")]
        public string TaskName { get; set; }
        public int TaskId { get; set; }
    }
}
