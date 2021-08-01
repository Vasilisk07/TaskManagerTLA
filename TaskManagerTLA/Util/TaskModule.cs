using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagerTLA.BLL.Interfaces;
using TaskManagerTLA.BLL.Services;

namespace TaskManagerTLA.Util
{
    public class TaskModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ITaskService>().To<TaskService>();
        }
    }
}
