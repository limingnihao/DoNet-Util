using Org.Limingnihao.Api.Excep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Application.Excep
{
    public class UserNullPointerException : AppCustomException
    {
        public UserNullPointerException()  : base("用户不存在。")
        {
        }
    }
}
