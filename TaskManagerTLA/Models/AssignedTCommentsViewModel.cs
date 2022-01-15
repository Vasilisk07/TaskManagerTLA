using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManagerTLA.Models
{
    public class AssignedTCommentsViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Дата внесення змін")]
        public DateTime DateModified { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Короткий опис змін")]
        public string Comments { get; set; }
        public AssignedTaskViewModel AssignedTask { get; set; }
    }
}
