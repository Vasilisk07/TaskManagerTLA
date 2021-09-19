using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagerTLA.DAL.Entities
{
    public class ActualTask
    {
        public int ActualTaskId { get; set; }
        public int ActTaskLeigth { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public string TaskName { get; set; }
        public int TaskId { get; set; }
    }
}
