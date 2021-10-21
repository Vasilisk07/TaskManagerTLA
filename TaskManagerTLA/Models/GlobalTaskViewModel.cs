using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManagerTLA.Models
{
    public class GlobalTaskViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Назва Задачі")]
        public string Name { get; set; }
        [Display(Name = "Короткий опис")]
        public string Description { get; set; }
        [Display(Name = "Загальний затрачений час")]
        public int TotalSpentHours { get; set; }
        [Display(Name = "Дата початку")]
        public DateTime Begin { get; set; }
        [Display(Name = "Дата закінчення")]
        public DateTime End { get; set; }
    }
}
