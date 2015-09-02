using Common.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Api.Util
{
    /// <summary>
    /// 文件操作的工具类
    /// </summary>
    public class FileUtil
    {
        private static readonly ILog logger = LogManager.GetLogger("FileUtil");

        public static string GetFileType(string filename)
        {
            string[] name = filename.Split('.');
            if(name == null || name.Length <=0 )
            {
                return "";
            }
            string type = name[name.Length - 1];
            return type.ToLower();
        }

        public static bool CreateDirectory(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    FileInfo fi = new FileInfo(path);
                    fi.Delete();
                    logger.Debug("CreateDirectory - delete file - fileName==" + fi.FullName);
                }
                if (Directory.Exists(path))
                {
                    return true;
                }
                else
                {
                    DirectoryInfo di = new DirectoryInfo(path);
                    di.Create();
                    return true;
                }
            }
            catch (Exception e)
            {
                logger.Debug("CreateDirectory - path=" + path + ", e=" + e.ToString());
            }
            return false;
        }

        public static bool DeleteFile(string path)
        {
           
            if (File.Exists(path))
            {
                try
                {
                    FileInfo fi = new FileInfo(path);
                    fi.Delete();
                    logger.Info("DeleteFile - fileName=" + fi.FullName);
                }
                catch(Exception e)
                {
                    logger.Info("DeleteFile - path=" + path + ", e=" + e.ToString());
                }
                return true;
            }
            else if (Directory.Exists(path))
            {
                return DeleteDirectory(path);
            }
            logger.Info("DeleteFile - 文件不存在 - path=" + path);
            return false;
        }

        public static bool DeleteDirectory(string path)
        {
             if (Directory.Exists(path))
             {
                 DirectoryInfo di = new DirectoryInfo(path);
                 foreach (FileInfo f in di.GetFiles())
                 {
                     //logger.debug("DeleteDirectory - file=" + f.FullName);
                     try
                     {
                         f.Delete();
                     }
                     catch (Exception e)
                     {
                         logger.Debug("DeleteDirectory - file=" + f.FullName + ", e=" + e.ToString());
                     }
                 }
                 foreach(DirectoryInfo d in di.GetDirectories())
                 {
                     DeleteDirectory(d.FullName);
                 }
                 //logger.debug("DeleteDirectory - Directory=" + di.FullName);
                 try
                 {
                    di.Delete();
                 }
                 catch (Exception e)
                 {
                     logger.Debug("DeleteDirectory - Directory=" + di.FullName + ", e=" + e.ToString());
                 }
             }
             return false;
        }
    }
}
