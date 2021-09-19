using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagerTLA.Models
{
    public class TaskViewModel
    {
        [Display(Name = "Id Задачі")]
        public int TaskModelId { get; set; }
        [Display(Name = "Назва Задачі")]
        public string TaskName { get; set; }
        [Display(Name = "Короткий опис")]
        public string TaskDescription { get; set; }
        [Display(Name = "Загальний затрачений час")]
        public int TaskLeigth { get; set; }
        [Display(Name = "Дата початку")]
        public DateTime TaskBegin { get; set; }
        [Display(Name = "Дата закінчення")]
        public DateTime TaskEnd { get; set; }
    }
}
