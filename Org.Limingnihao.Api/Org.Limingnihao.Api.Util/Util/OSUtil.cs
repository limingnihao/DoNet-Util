using Common.Logging;
using Org.Limingnihao.Api.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Api.Util
{
    public class OSUtil
    {
        private static readonly ILog logger = LogManager.GetLogger("OSUtil");

        public static OSVersion GetOSVersionType()
        {
            //获取系统信息
            OperatingSystem osInfo = System.Environment.OSVersion;
            //获取操作系统ID
            PlatformID platformID = osInfo.Platform;
            //获取主版本号
            int versionMajor = osInfo.Version.Major;
            //获取副版本号
            int versionMinor = osInfo.Version.Minor;
            string osInfor = platformID.GetHashCode().ToString() + versionMajor.ToString() + versionMinor.ToString();
            logger.Info("主版本号=" + versionMajor + "副版本号=" + versionMinor + ", osInfor=" + osInfor);
            if (osInfor == OSVersion.WindowsXP.GetHashCode().ToString())
            {
                return OSVersion.WindowsXP;
            }
            else if (osInfor == OSVersion.Windows7.GetHashCode().ToString())
            {
                return OSVersion.Windows7;
            }
            else if (osInfor == OSVersion.Windows8.GetHashCode().ToString())
            {
                return OSVersion.Windows8;
            }
            return OSVersion.Other;
        }

    }
}
