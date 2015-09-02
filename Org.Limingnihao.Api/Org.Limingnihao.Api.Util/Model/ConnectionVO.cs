using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Api.Util.Model
{
    public class ConnectionVO
    {
        public String Server { get; set; }
        public String Port { get; set; }
        public String Database { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }

        public ConnectionVO()
        {
        }

        public ConnectionVO(String s, String p, String d, String u, String pw)
        {
            this.Server = s;
            this.Port = p;
            this.Database = d;
            this.Username = u;
            this.Password = pw;
        }

        public override string ToString()
        {
            return "[ConnectionVO - Server=" + Server + ", Port=" + Port + ", Database=" + Database + ", Username=" + Username + ", Password=" + Password + "]";
        }

    }
}
