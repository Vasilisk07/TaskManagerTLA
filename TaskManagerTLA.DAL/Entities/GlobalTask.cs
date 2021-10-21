using System;
using System.Collections.Generic;
using TaskManagerTLA.DAL.Identity.Entities;

namespace TaskManagerTLA.DAL.Entities
{
    public class GlobalTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TotalSpentHours { get; set; }
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
        public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        public List<AssignedTask> AssignedTasks { get; set; } = new List<AssignedTask>();
    }
}
