using System.Collections.Generic;

namespace TaskManagerTLA.BLL.DTO
{
    public class AssignedTaskDTO
    {
        public int SpentHours { get; set; }
        public List<AssignedTCommentsDTO> Comments { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public int? GlobalTaskId { get; set; }
        public string GlobalTaskName { get; set; }
    }
}
