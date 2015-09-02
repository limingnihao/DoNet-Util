using System;

namespace Org.Limingnihao.Api.Excep
{
    public class AppCustomException : ApplicationException
    {

        public AppCustomException()
        {
        }

        public AppCustomException(string message) : base(message)
        {
        }

        public AppCustomException(string message, Exception inner) : base(message, inner)
        {
              
        }

    }
}
