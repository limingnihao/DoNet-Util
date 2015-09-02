using Org.Limingnihao.Api.Util.Model;
using Org.Limingnihao.Update.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Update.Help
{
    public class ConfigHelp
    {
        #region 升级配置信息

        /// <summary>
        /// 服务器地址
        /// </summary>
        public static string ServerAddress = "";

        /// <summary>
        /// 设备类型
        /// </summary>
        public static string DeviceType = "1001";

        /// <summary>
        /// 更新程序名称
        /// </summary>
        public static String AppNameUpdate = @"Update.exe";

        /// <summary>
        /// 主应用程序名称
        /// </summary>
        public static String AppNameMain = @"T4.exe";

        /// <summary>
        /// 当前版本号
        /// </summary>
        public static Int32 VersionCode = 0;

        /// <summary>
        /// 当前版本号
        /// </summary>
        public static String VersionName = "";

        /// <summary>
        /// 应用程序路径
        /// </summary>
        public static String CurrentDirectory = "";

        /// <summary>
        /// 下载的文件绝对路径
        /// </summary>
        public static String FileDownPath = "";

        /// <summary>
        /// 更新文件解压文件夹
        /// </summary>
        public static String FileUpdataDir = "temp";

        /// <summary>
        /// 更新文件解压路径
        /// </summary>
        public static String FileUpdatePath = "";

        public static IVersionVO VersionVO = null;

        /// <summary>
        /// 连接数据库字符串
        /// </summary>
        public static String ConnectionString = "";

        #endregion


    }
}
