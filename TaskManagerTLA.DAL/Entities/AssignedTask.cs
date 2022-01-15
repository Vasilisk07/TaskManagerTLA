using System.Collections.Generic;
using TaskManagerTLA.DAL.Identity.Entities;

namespace TaskManagerTLA.DAL.Entities
{
    public class AssignedTask
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int GlobalTaskId { get; set; }
        public GlobalTask GlobalTask { get; set; }
        public int SpentHours { get; set; }
        public List<AssignedTComments> Comments { get; set; }
    }
}
