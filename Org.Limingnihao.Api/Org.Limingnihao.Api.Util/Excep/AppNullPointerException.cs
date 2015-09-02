using Org.Limingnihao.Api.Excep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Api.Excep
{
    public class AppNullPointerException : AppCustomException
    {
        public AppNullPointerException()
            : base("数据不存在。")
        {
        }

        public AppNullPointerException(string message) : base(message)
        {
        }


    }
}
