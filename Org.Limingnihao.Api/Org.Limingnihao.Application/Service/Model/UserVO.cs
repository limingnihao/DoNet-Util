using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Application.Service.Model
{
    public class UserVO
    {
        public Int32 UserId { get; set; }
        public String Username { get; set; }
        public String Nickname { get; set; }

        public override string ToString()
        {
            return "[UserVO - UserId=" + UserId + ", Username=" + Username + ", Nickname=" + Nickname + "]";
        }
    }
}
