using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagerTLA.Models
{
    public class EditATModel
    {
        public int ActualTaskId { get; set; }
        [Display(Name = "Затрачений час")]
        public int ActTaskLeigth { get; set; }
        [Display(Name = "Короткий звіт")]
        public string Description { get; set; }
    }
}
