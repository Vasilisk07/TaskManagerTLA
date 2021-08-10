using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagerTLA.DAL.Entities
{
    public class TaskModel
    { 
        public int TaskModelId { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public int TaskLeigth { get; set; }
        public DateTime TaskBegin { get; set; }
        public DateTime TaskEnd { get; set; }
    }
}
