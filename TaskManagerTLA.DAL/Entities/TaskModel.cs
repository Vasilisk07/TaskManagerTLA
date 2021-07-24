﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagerTLA.DAL.Entities
{
    class TaskModel
    { 
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public int TaskLeigth { get; set; }
        public DateTime TaskBegin { get; set; }
        public DateTime TaskEnd { get; set; }
    }
}
