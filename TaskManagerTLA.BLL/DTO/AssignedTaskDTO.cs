
namespace TaskManagerTLA.BLL.DTO
{
    public class AssignedTaskDTO
    {
        //через DTO передаю в представлення Id і Name GlobalTask та User
        //оскільки вони виступають ключами AssignedTask
        //а також допускаю можливу потребу задач з однаковим іменем
        public int SpentHours { get; set; }
        public string Description { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public int? GlobalTaskId { get; set; }
        public string GlobalTaskName { get; set; }
    }
}
