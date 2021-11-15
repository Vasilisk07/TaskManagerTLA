using System;
using System.Collections.Generic;
using System.Text;
using TaskManagerTLA.DAL.Identity.Entities;
using TaskManagerTLA.DAL.Repositories.Interfaces;
using TaskManagerTLA.DAL.UnitOfWork.IdentityUnitOfWork.Interfaces;

namespace TaskManagerTLA.DAL.UnitOfWork.IdentityUnitOfWork
{
    public class UserUnit : IUserUnit
    {
        public IRepository<ApplicationUser, string> Users { get; }
        public UserUnit(IRepository<ApplicationUser, string> users)
        {
            Users = users;
        }
    }
}
