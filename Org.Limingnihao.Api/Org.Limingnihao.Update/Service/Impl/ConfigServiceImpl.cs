using Common.Logging;
using MySql.Data.MySqlClient;
using Org.Limingnihao.Api.Util;
using Org.Limingnihao.Api.Util.Model;
using Org.Limingnihao.Update.Help;
using Org.Limingnihao.Update.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Update.Service.Impl
{
    public class ConfigServiceImpl
    {

        #region 初始化方法

        private static readonly ILog logger = LogManager.GetLogger("UpdateServiceImpl");
        private static ConfigServiceImpl INSTANCE = null;
        private IniFileUtil iniUpdate = null;

        public static ConfigServiceImpl GetInstance()
        {
            if (INSTANCE == null )
            {
                INSTANCE = new ConfigServiceImpl();
            }
            return INSTANCE;
        }

        #endregion


        #region 操作INI文件

        public void InitUpdateSetting()
        {
            ConfigHelp.CurrentDirectory = System.Environment.CurrentDirectory;
            logger.Info("getUpdateSetting - currentDirectory=" + ConfigHelp.CurrentDirectory);
            this.iniUpdate = IniFileUtil.GetInstance(ConfigHelp.CurrentDirectory + "\\Config\\update.ini");
            ConfigHelp.ServerAddress = this.iniUpdate.ReadInivalue("Version", "ServerAddress");
            ConfigHelp.AppNameUpdate = this.iniUpdate.ReadInivalue("Version", "AppNameUpdate");
            ConfigHelp.AppNameMain = this.iniUpdate.ReadInivalue("Version", "AppNameMain");
            ConfigHelp.DeviceType = this.iniUpdate.ReadInivalue("Version", "DeviceType");
            ConfigHelp.VersionCode = NumberUtil.Parse(this.iniUpdate.ReadInivalue("Version", "VersionCode"), 0);
            ConfigHelp.VersionName = this.iniUpdate.ReadInivalue("Version", "VersionName");
            ConfigHelp.ConnectionString = this.iniUpdate.ReadInivalue("Setting", "ConnectionString");
            logger.Info("getUpdateSetting - ServerAddress=" + ConfigHelp.ServerAddress + ", AppNameUpdate=" + ConfigHelp.AppNameUpdate + ", AppNameMain=" + ConfigHelp.AppNameMain +
                ", DeviceType=" + ConfigHelp.DeviceType + ", VersionCode=" + ConfigHelp.VersionCode + ", VersionName=" + ConfigHelp.VersionName + ", ConnectionString=" + ConfigHelp.ConnectionString);
        }

        public void SetVersionCodeName(string versionCode, string versionName)
        {
            logger.Info("SetVersionCodeName - versionCode=" + versionCode + ", versionName=" + versionName);
            this.iniUpdate.WriteInivalue("Version", "VersionCode", versionCode);
            this.iniUpdate.WriteInivalue("Version", "VersionName", versionName);
        }

        #endregion


        #region 控制应用程序

        public bool StartApp()
        {
            string clientPath = ConfigHelp.CurrentDirectory + "\\" + ConfigHelp.AppNameMain;
            logger.Info("StartApp - clientPath=" + clientPath);
            try
            {
                ProcessUtil.StartProcess(clientPath, ConfigHelp.AppNameMain);
            }
            catch (Exception e)
            {
                logger.Info("StartApp - " + e);
                return false;
            }
            return true;
        }

        public bool StopApp()
        {
            logger.Info("StopApp");
            try
            {
                ProcessUtil.KillProcess(ConfigHelp.AppNameMain);
            }
            catch (Exception e)
            {
                logger.Info("StopApp - " + e);
                return false;
            }
            return true;
        }

        #endregion


        #region 获取版本号

        public IVersionVO GetNewVersion()
        {
            try
            {
                string url = ConfigHelp.ServerAddress + "/interface/getNewVersion.do";
                Dictionary<string, string> parameter = new Dictionary<string, string>();
                parameter.Add("deviceType", ConfigHelp.DeviceType);
                parameter.Add("versionCode", ConfigHelp.VersionCode.ToString());
                string result = HttpUtil.Post(url, parameter);
                logger.Info("GetNewVersion - url=" + url + ", DeviceType=" + ConfigHelp.DeviceType + ", VersionCode=" + ConfigHelp.VersionCode + ", result=" + result);
                if (result != null && !"".Equals(result))
                {
                    IVersionVO vo = JsonUtil.FromJson<IVersionVO>(result);
                    return vo;
                }
            }
            catch (Exception e)
            {
                logger.Info("" + e.ToString());
            }
            return null;
        }

        #endregion


        #region 更新SQL

        public MySqlConnection ConnectMySql(String connnectString)
        {
            try
            {
                logger.Info("ConnectMySql - connnectString=" + connnectString);
                MySqlConnection conn = new MySqlConnection(connnectString);
                conn.Open();
                return conn;
            }
            catch(Exception e)
            {
                logger.Info("ConnectMySql - " + e.Message);
            }
            return null;
        }

        public int UpdateSql(MySqlConnection conn, string sql)
        {
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            return cmd.ExecuteNonQuery();
        }

        #endregion

    }
}
