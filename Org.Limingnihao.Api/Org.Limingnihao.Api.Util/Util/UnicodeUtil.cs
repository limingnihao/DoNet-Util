using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Api.Util
{
    public class UnicodeUtil
    {
            /// <summary>
            /// 编码
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public static string StringToUnicode(string value)
            {
                if(value == null || "".Equals(value))
                {
                    return "";
                }
                char[] charbuffers = value.ToCharArray();
                byte[] buffer;
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < charbuffers.Length; i++)
                {
                    buffer = Encoding.Unicode.GetBytes(charbuffers[i].ToString());
                    sb.Append(String.Format("\\u{0:X2}{1:X2}", buffer[1], buffer[0]));
                }
                return sb.ToString();
            }

            /// <summary>
            /// 解码
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public static string UnicodeToString(string value)
            {
                if (value == null || "".Equals(value))
                {
                    return "";
                }

                string dst = "";
                string src = value;
                int len = value.Length / 6;
                for (int i = 0; i <= len - 1; i++)
                {
                    string str = "";
                    str = src.Substring(0, 6).Substring(2);
                    src = src.Substring(6);
                    byte[] bytes = new byte[2];
                    bytes[1] = byte.Parse(int.Parse(str.Substring(0, 2), NumberStyles.HexNumber).ToString());
                    bytes[0] = byte.Parse(int.Parse(str.Substring(2, 2), NumberStyles.HexNumber).ToString());
                    dst += Encoding.Unicode.GetString(bytes);
                }
                return dst;
            }

    }
}
