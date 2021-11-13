using System;
using System.Collections.Generic;
using System.Text;
using TaskManagerTLA.DAL.Identity.Entities;
using TaskManagerTLA.DAL.Repositories.Interfaces;

namespace TaskManagerTLA.DAL.UnitOfWork.IdentityUnitOfWork.Interfaces
{
    public interface IUserUnit
    {
        IRepository<ApplicationUser, string> Users { get; }
    }
}
