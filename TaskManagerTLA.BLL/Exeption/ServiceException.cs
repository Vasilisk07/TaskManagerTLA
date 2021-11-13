using System;

namespace TaskManagerTLA.BLL.Exeption
{
    public class ServiceException : Exception
    {
        public ServiceException(string message) : base(message)
        {

        }
    }
}
