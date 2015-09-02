using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Api.Util
{
    public class ByteUtil
    {
        private static String HEX = "0123456789ABCDEF";

        /// <summary>
        /// 高位在前
        /// </summary>
        public static byte[] ToBytesHigh(int value)
        {
            byte[] data = new byte[4];
            data[0] = (byte)(value >> 0 & 0xff);
            data[1] = (byte)(value >> 8 & 0xff);
            data[2] = (byte)(value >> 16 & 0xff);
            data[3] = (byte)(value >> 24 & 0xff);
            return data;
        }

        /// <summary>
        /// 低位在前
        /// </summary>
        public static byte[] ToBytesLow(int value)
        {
            byte[] data = new byte[4];
            data[0] = (byte)(value >> 24 & 0xff);
            data[1] = (byte)(value >> 16 & 0xff);
            data[2] = (byte)(value >> 8 & 0xff);
            data[3] = (byte)(value >> 0 & 0xff);
            return data;
        }

        /// <summary>
        /// 高位在前
        /// </summary>
        public static int GetIntHigh(byte[] data)
        {
            return (int)((
                ((data[3] & 0xff) << 24) | 
                ((data[2] & 0xff) << 16) | 
                ((data[1] & 0xff) << 8) | 
                ((data[0] & 0xff) << 0)));
        }

        /// <summary>
        /// 低位在前
        /// </summary>
        public static int GetIntLow(byte[] data)
        {
            return (int)((((data[0] & 0xff) << 24) | ((data[1] & 0xff) << 16) | ((data[2] & 0xff) << 8) | ((data[3] & 0xff) << 0)));
        }

        public static String BytesToHex(byte[] buf)
        {
            if (buf == null)
                return "";
            StringBuilder result = new StringBuilder(2 * buf.Length);
            for (int i = 0; i < buf.Length; i++)
            {
                result.Append(HEX[((buf[i] >> 4) & 0x0f)]);
                result.Append(HEX[(buf[i] & 0x0f)]);
            }
            return result.ToString();
        }

        public static byte[] HexToBytes(String hexString)
        {
            int len = hexString.Length / 2;
            byte[] result = new byte[len];
            for (int i = 0; i < len; i++)
            {
                int index = 2 * i;
                result[i] = BitConverter.GetBytes(Convert.ToInt32(hexString.Substring(index, 2), 16))[0];
            }
            return result;
        }

    }
}
