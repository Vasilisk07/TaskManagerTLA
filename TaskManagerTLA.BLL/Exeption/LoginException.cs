using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagerTLA.BLL.Exeption
{
    public class LoginException : Exception
    {
        public LoginException(string message) : base(message)
        {

        }
    }
}
