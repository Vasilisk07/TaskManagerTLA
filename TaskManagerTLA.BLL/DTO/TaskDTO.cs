using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagerTLA.BLL.DTO
{
        // TODO remove all `Task..` prefixes.
    public class TaskDTO
    {
        // TODO `Id` is enough
        public int TaskModelId { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }

        // TODO `TotalSpentHuors` means the estimated time the task should take
        public int TaskLength { get; set; }
        public DateTime TaskBegin { get; set; }
        public DateTime TaskEnd { get; set; }

    }
}
