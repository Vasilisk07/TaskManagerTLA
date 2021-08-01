using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagerTLA.DAL.Interfaces;
using TaskManagerTLA.DAL.Repositories;

namespace TaskManagerTLA.BLL.Infrastructure
{
    public class ServiceModule : NinjectModule
    {
        private string ConnectionString;
        public ServiceModule(string connection)
        {
            ConnectionString = connection;
        }

        public override void Load()
        {
            Bind<IUnitOfWork>().To<EFUnitOfWork>().WithConstructorArgument(ConnectionString);
        }

    }
}

