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
        public int ActTaskLeigth { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public string TaskName { get; set; }
        public int TaskId { get; set; }
    }
}
