using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagerTLA.Models
{
    public class TModel
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public int TaskLeigth { get; set; }
        public DateTime TaskBegin { get; set; }
        public DateTime TaskEnd { get; set; }
    }
}
