using Org.Limingnihao.Api.Excep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Application.Excep
{
    public class UserPasswordErrorException : AppCustomException
    {
        public UserPasswordErrorException() : base("密码输入有误。")
        {
        }
    }
}
