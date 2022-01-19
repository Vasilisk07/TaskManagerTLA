using System.Collections.Generic;

namespace TaskManagerTLA.Models
{
    public class UserListViewModel
    {
        public IEnumerable<UserViewModel> Users { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
