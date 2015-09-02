using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Api.Util
{
    /// <summary>
    /// 日志相关功能
    /// </summary>
    public class LogUtil
    {
        private static Object thisLock = new Object();
        private String tag = "";
        private static String applicationPath = null;
        private static String logFilePath = null;
        private static String logFileName = null;
        private static FileStream fs = null;
        private static StreamWriter writer = null;

        public static string FileTitle = "longshine";

        public static bool IS_FILE = false;
        public static int L_DEBUG = 1;
        public static int L_INFO = 2;

        public static int LEVEL = L_INFO;

        public static LogUtil getInstance(String tag)
        {
            LogUtil logger = new LogUtil(tag);
            return logger;
        }

        private LogUtil(String tag)
        {
            this.tag = tag;
            if (!IS_FILE)
            {
                return;
            }
            if (applicationPath != null && writer != null)
            {
                return;
            }
            Console.WriteLine("LogUtil - 初始化");
            lock (thisLock)
            {
                applicationPath = System.Environment.CurrentDirectory;
                logFilePath = applicationPath + "\\log";
                logFileName = DateUtil.Format(DateTime.Now, "yyyy-MM-dd-HH-mm");
                if (!Directory.Exists(logFilePath))
                {
                    Directory.CreateDirectory(logFilePath);
                }
                fs = new FileStream(logFilePath + "\\" + FileTitle + "_" + logFileName + ".log", FileMode.Append);
                writer = new StreamWriter(fs);
            }
        }

        public void Info(String v)
        {
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1, true);
            string message = "[" + DateUtil.GetNowDate() + "][" + this.tag + "][" + st.GetFrame(0).GetFileLineNumber() + "] - " + v;
            this.write(message);
        }

        public void Debug(String v)
        {
            if (LEVEL == L_DEBUG)
            {
                System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1, true);
                string message = "[" + DateUtil.GetNowDate() + "][" + this.tag + "][" + st.GetFrame(0).GetFileLineNumber() + "] - " + v;
                this.write(message);
            }
        }

        private void write(String message)
        {
            try
            {
                System.Console.WriteLine(message);
                if (IS_FILE && writer!=null)
                {
                    writer.WriteLine(message);
                    writer.Flush();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("LogUtil - 日志文件写入失败信息:" + e.ToString()); 
            }
        }
    }
}
