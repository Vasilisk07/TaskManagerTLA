using System;

namespace TaskManagerTLA.BLL.DTO
{
    public class AssignedTCommentsDTO
    {
        public int Id { get; set; }
        public DateTime DateModified { get; set; }
        public string Comments { get; set; }
        public AssignedTaskDTO AssignedTask { get; set; }
    }
}
