using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Api.Util
{
    public class StringUtil
    {
        public static bool IsBlank(String value)
        {
            if(value == null || value.Length == 0 || value.Trim().Length == 0){
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
