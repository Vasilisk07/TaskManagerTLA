using System;

namespace TaskManagerTLA.BLL.DTO
{
    public class GlobalTaskDTO
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TotalSpentHours { get; set; }
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
    }
}
