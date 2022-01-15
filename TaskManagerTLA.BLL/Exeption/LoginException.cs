using System;

namespace TaskManagerTLA.BLL.Exeption
{
    public class LoginException : Exception
    {
        public LoginException(string message) : base(message)
        {

        }
    }
}
