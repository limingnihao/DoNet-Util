using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Api.Util.Model
{
    public class ThreadParameterVO
    {
        public int Type { get; set; }
        public int Start { get; set; }
        public int Over { get; set; }
        public Object DataObj { get; set; } 
        public IList DataList{ get; set; }

    }
}
