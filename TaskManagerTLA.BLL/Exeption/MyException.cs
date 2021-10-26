using System;

namespace TaskManagerTLA.BLL.Exeption
{
    // MyException - дуже невдале ім'я, воно зовсім не описує виключення
    public class MyException : Exception
    {
        public MyException(string message) : base(message)
        {

        }
    }
}
