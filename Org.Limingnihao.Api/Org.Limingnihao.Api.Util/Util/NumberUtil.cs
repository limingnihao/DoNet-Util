using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Api.Util
{
    public class NumberUtil
    {
        public static bool TryParse(String value)
        {
            try
            {
                Int32.Parse(value);
                return true;
            }
            catch(Exception e)
            {
                System.Console.WriteLine("NumberUtil - TryParse - value=" + value + ", e=" + e.Message);
                return false;
            }
        }

        public static int Parse(String value, int def = 0)
        {
            try
            {
                return Int32.Parse(value);
            }
            catch (Exception e) 
            {
                System.Console.WriteLine("NumberUtil - Parse - value=" + value + ", e=" + e.Message);
                //throw new Exception(e.Message);
            }
            return def;
        }

        /// <summary>
        /// 得到min到max（包括min和max）之间的随机数
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int RandomInt(int min, int max)
        {
            if (max <= min)
            {
                return max;
            }
            Random radom = new Random();
            int result = radom.Next(max - min) + min;
            return result;
        }

        /// <summary>
        /// 文件大小字符串转换
        /// </summary>
        /// <param name="size"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static String ConversionUnitMemory(double size)
        {
            double value = (double)0;
            String unitString = " B";
            if (size == 0)
            {
                value = (double)0;
                unitString = " B";
            }
            else if (size < Math.Pow(1024, 1))
            {
                value = size;
                unitString = " KB";
            }
            else if (size >= Math.Pow(1024, 1) && size < Math.Pow(1024, 2))
            {
                value = size / Math.Pow(1024, 1);
                unitString = " KB";
            }
            else if (size >= Math.Pow(1024, 2) && size < Math.Pow(1024, 3))
            {
                value = size / Math.Pow(1024, 2);
                unitString = " MB";
            }
            else if (size >= Math.Pow(1024, 3) && size < Math.Pow(1024, 4))
            {
                value = size / Math.Pow(1024, 3);
                unitString = " GB";
            }
            else if (size >= Math.Pow(1024, 4) && size < Math.Pow(1024, 5))
            {
                value = size / Math.Pow(1024, 4);
                unitString = " TB";
            }
            else if (size >= Math.Pow(1024, 5) && size < Math.Pow(1024, 6))
            {
                value = size / Math.Pow(1024, 5);
                unitString = " PB";
            }
            else
            {
                value = size;
                unitString = " B";
            }
            return value.ToString("F2") + unitString;
        }

    }
}
