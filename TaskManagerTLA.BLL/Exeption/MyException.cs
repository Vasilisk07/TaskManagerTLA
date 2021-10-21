using System;

namespace TaskManagerTLA.BLL.Exeption
{
    public class MyException : Exception
    {
        public MyException(string message) : base(message)
        {

        }
    }
}
