using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagerTLA.BLL.Infrastructure
{
    class ValidationException : Exception
    {
        public string Property { get; protected set; }
        public ValidationException(string message,string property):base(message)
        {
            this.Property = property;

        }



    }
}
