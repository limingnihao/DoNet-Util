using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Api.Util
{
    public class EnumUtil
    {
        public static T Parse<T>(object value)
        {
            if (value != null && Enum.IsDefined(typeof(T), value))
            {
                return (T)Enum.Parse(typeof(T), value.ToString());
            }
            return default(T);
        }

        public static String GetName<T>(object value)
        {
            if (value != null && Enum.IsDefined(typeof(T), value))
            {
                return Enum.GetName(typeof(T), (T)Enum.Parse(typeof(T), value.ToString()));
            }
            return null;
        }

    }
}
