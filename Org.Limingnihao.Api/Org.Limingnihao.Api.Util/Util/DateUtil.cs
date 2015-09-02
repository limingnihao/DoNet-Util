using Org.Limingnihao.Api.Util.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Api.Util
{
    /// <summary>
    /// 时间操作
    /// </summary>
    public class DateUtil
    {
        public const String DefaultDateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        public const String DefaultDateFormat = "yyyy-MM-dd";
        public const String DefaultTimeFormat = "HH:mm:ss";

        public static DateTime GetDefaultStartDate() {
            return DateTime.Parse("1900-01-01");
	    }

	    public static DateTime GetDefaultEndDate() {
		    return DateTime.Now;
	    }


        #region 增加年、月、日、时、分

        /// <summary>
        /// 当前时间增加分钟
        /// </summary>
        public static String AddMinutes(double value, string format = DefaultDateTimeFormat)
        {
            return DateTime.Now.AddMinutes(value).ToString(format);
        }

        /// <summary>
        /// 当前时间增加小时
        /// </summary>
        public static String AddHours(double value, string format = DefaultDateTimeFormat)
        {
            return DateTime.Now.AddHours(value).ToString(format);
        }

        /// <summary>
        /// 当前时间增加天
        /// </summary>
        public static String AddDays(double value, string format = DefaultDateTimeFormat)
        {
            return DateTime.Now.AddDays(value).ToString(format);
        }

        /// <summary>
        /// 当前时间增加月
        /// </summary>
        public static String AddMonths(int value, string format = DefaultDateTimeFormat)
        {
            return DateTime.Now.AddMonths(value).ToString(format);
        }

        #endregion


        #region 格式化操作相关

        /// <summary>
        /// 当前时间字符串
        /// </summary>
        /// <returns></returns>
        public static String GetNowDate(string format = DefaultDateTimeFormat)
        {
            return DateTime.Now.ToString(format);
        }

        /// <summary>
        /// 将时间转换为格式为 yyyy-MM-dd HH:mm:ss 的字符串
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static String Format(DateTime date)
        {
            if (date != null)
            {
                return date.ToString("yyyy-MM-dd HH:mm:ss");
            }
            return "";
        }

        /// <summary>
        /// 将时间转换为字符串，按照指定的格式
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static String Format(DateTime date, String format)
        {
            if (date != null)
            {
                return date.ToString(format);
            }
            return "";
        }

        /// <summary>
        /// 将字符串转为时间对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime Parse(string value)
        {
            try
            {
                System.IFormatProvider format = new System.Globalization.CultureInfo("zh-cn", true);
                return DateTime.Parse(value, format);
            }
            catch(Exception e)
            {
                System.Console.WriteLine("DateUtil - Parse - value=" + value + ", e=" + e.Message);
                //throw new Exception(e.Message);
            }
            return DateTime.MinValue;
        }

        /// <summary>
        /// 将字符串转为时间对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Parse(string value, ref DateTime time)
        {
            try
            {
                System.IFormatProvider format = new System.Globalization.CultureInfo("zh-cn", true);
                time = DateTime.Parse(value, format);
                return true;
            }
            catch (Exception) 
            {
                return false;
            }
        }

        /// <summary>
        /// 获取周的汉字名称
        /// </summary>
        /// <param name="date"当前日期对象></param>
        /// <returns></returns>
        public static String GetWeekName(DateTime date)
        {
            DayOfWeek week = date.DayOfWeek;
            String weekName = "";
            switch(week)
            {
                case DayOfWeek.Monday:      weekName = "星期一"; break;
                case DayOfWeek.Tuesday:     weekName = "星期二"; break;
                case DayOfWeek.Wednesday:   weekName = "星期三"; break;
                case DayOfWeek.Thursday:    weekName = "星期四"; break;
                case DayOfWeek.Friday:      weekName = "星期五"; break;
                case DayOfWeek.Saturday:    weekName = "星期六"; break;
                case DayOfWeek.Sunday:      weekName = "星期日"; break;
            }
            return weekName;
        }

        #endregion


        #region 年、月、日的Map列表，可下拉选项三级联动使用

        /// <summary>
        /// 获取年份列表
        /// </summary>
        /// <param name="startYear">其实年份</param>
        /// <returns></returns>
        public static ObservableCollection<HashmapVO> GetYearList(int startYear=1900)
        {
            ObservableCollection<HashmapVO> list = new ObservableCollection<HashmapVO>();
            for (int i = startYear; i <= DateTime.Now.Year; i++)
            {
                list.Add(new HashmapVO(i.ToString(), i + "年"));
            }
            return list;
        }
        /// <summary>
        /// 获取年份列表
        /// </summary>
        /// <param name="startYear">其实年份</param>
        /// <returns></returns>
        public static ObservableCollection<HashmapVO> GetYearListDesc(int startYear = 1900)
        {
            ObservableCollection<HashmapVO> list = new ObservableCollection<HashmapVO>();
            for (int i = DateTime.Now.Year; i >= startYear; i--)
            {
                list.Add(new HashmapVO(i.ToString(), i + "年"));
            }
            return list;
        }


        /// <summary>
        /// 获取月份列表
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<HashmapVO> GetMonthList()
        {
            ObservableCollection<HashmapVO> list = new ObservableCollection<HashmapVO>();
            for (int i = 1; i <= 12; i++)
            {
                list.Add(new HashmapVO(i.ToString(), i + "月"));
            }
            return list;
        }

        /// <summary>
        /// 获取日列表
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static ObservableCollection<HashmapVO> GetDayList(int year, int month)
        {
            ObservableCollection<HashmapVO> list = new ObservableCollection<HashmapVO>();
            int max = 1;
            if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
            {
                max = 31;
            }
            else if (month == 4 || month == 6 || month == 9 || month ==11)
            {
                max = 30;
            }
            else if (month == 2)
            {
                if ((year % 4 == 0 && year % 100 != 0) || year % 400 == 0)
                {
                    max = 29;
                }
                else
                {
                    max = 28;
                }
            }
            for (int i = 1; i <= max; i++)
            {
                list.Add(new HashmapVO(i.ToString(), i + "日"));
            }
            return list;
        }

        #endregion
    
    
    }
}
