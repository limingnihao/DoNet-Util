using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Org.Limingnihao.Api.Util.Util
{
    public class RegexUtil
    {

        #region 正则表达式 - 网络

        /// <summary>
        /// IP地址
        /// </summary>
        public static String Regex_IpAddress = "^([1-9]|[1-9]\\d|1\\d{2}|2[0-4]\\d|25[0-5])(\\.(\\d|[1-9]\\d|1\\d{2}|2[0-4]\\d|25[0-5])){3}$";

        /// <summary>
        /// 端口号
        /// </summary>
        public static String Regex_Port = "^\\d{2,5}$";

        #endregion


        #region 正则表达式 - 手机号

        /// <summary>
        /// 中国移动
        /// </summary>
        public static String Regex_Phone_Mobile = "^13[456789]\\d{8}$|^15[012789]\\d{8}$|^18[23478]\\d{8}$";

        /// <summary>
        /// 中国联通
        /// </summary>
        public static String Regex_Phone_Unicom = "^13[012]\\d{8}$|^15[56]\\d{8}$|^18[56]\\d{8}$";

        /// <summary>
        /// 中国电信
        /// </summary>
        public static String Regex_Phone_Telecom = "^13[3]\\d{8}$|^15[3]\\d{8}$|^18[019]\\d{8}$";

        #endregion


        #region 正则表达式 - 身份证号

        /// <summary>
        /// 身份证号
        /// </summary>
        public static String Regex_Identity_Card = "^\\d{15}$|^\\d{18}$|^\\d{17}(\\d|X|x)$";

        #endregion


        #region 正则表达式 - 日期

        /// <summary>
        /// 日期 - 年份
        /// </summary>
        public static String Regex_Date_Year = "^[12]\\d{3}$";

        #endregion


        #region 判断IP地址

        /// <summary>
        /// 是否是IP地址
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsIpAddress(String value)
        {
            Regex reg = new Regex(Regex_IpAddress, RegexOptions.IgnoreCase);
            return reg.IsMatch(value);
        }

        /// <summary>
        /// 是否是端口号
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsPort(String value)
        {
            Regex reg = new Regex(Regex_Port, RegexOptions.IgnoreCase);
            return reg.IsMatch(value);
        }

        #endregion


        #region 判断手机号

        /// <summary>
        /// 是否是手机号
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsPhone(String value)
        {
            return IsPhoneMobile(value) || IsPhoneUnicom(value) || IsPhoneTelecom(value);
        }

        /// <summary>
        /// 是否是手机号 - 移动
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsPhoneMobile(String value)
        {
            Regex reg = new Regex(Regex_Phone_Mobile, RegexOptions.IgnoreCase);
            return reg.IsMatch(value);
        }

        /// <summary>
        /// 是否是手机号 - 联通
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsPhoneUnicom(String value)
        {
            Regex reg = new Regex(Regex_Phone_Unicom, RegexOptions.IgnoreCase);
            return reg.IsMatch(value);
        }

        /// <summary>
        /// 是否是手机号 - 电信
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsPhoneTelecom(String value)
        {
            Regex reg = new Regex(Regex_Phone_Telecom, RegexOptions.IgnoreCase);
            return reg.IsMatch(value);
        }

        #endregion


        #region 判断身份证号

        /// <summary>
        /// 是否是身份证号
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsIdentityCard(String value)
        {
            Regex reg = new Regex(Regex_Identity_Card, RegexOptions.IgnoreCase);
            return reg.IsMatch(value);
        }

        #endregion


        #region 判断年份

        /// <summary>
        /// 是否是有效的年份
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsYearNumber(String value)
        {
            Regex reg = new Regex(Regex_Date_Year, RegexOptions.IgnoreCase);
            return reg.IsMatch(value);
        }

        #endregion


    }
}
