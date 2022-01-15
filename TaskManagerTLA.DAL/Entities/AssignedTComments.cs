using System;

namespace TaskManagerTLA.DAL.Entities
{
    public class AssignedTComments
    {
        public int Id { get; set; }
        public DateTime DateModified { get; set; }
        public string Comments { get; set; }
        public AssignedTask AssignedTask { get; set; }
    }
}
