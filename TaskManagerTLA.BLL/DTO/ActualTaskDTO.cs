using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagerTLA.BLL.DTO
{
    // TODO 'ActualTaskDTO' не дуже вдале ім'я, як задача і конкретна задача, чим ця задача конкретніша за іншу не зроузміло
    // давай краще `ReportedTask`, `AssignedTask` щось таке
    public class ActualTaskDTO
    {
        // TODO `Id`
        public int ActualTaskId { get; set; }

        // TODO `SpentHours` how many hours user spent on the task
        public int ActTaskLeigth { get; set; }
        public string Description { get; set; }
        // TODO use user Id instead of name
        public string UserName { get; set; }
        // TODO we have task name in the Task class, which is referenced by TaskId
        public string TaskName { get; set; }
        public int TaskId { get; set; }
    }
}
