using Common.Logging;
using System;
using System.IO;
using System.Net;
using System.Windows.Threading;

namespace Org.Limingnihao.Api.Util
{
    /// <summary>
    /// 下载文件的回调代理
    /// </summary>
    /// <param name="percent"></param>
    public delegate void FileDownloadDelegate(double percent, double second, double speed, string info);

    /// <summary>
    /// 下载文件工具类
    /// </summary>
    public class FileDownUtil
    {

        private static readonly ILog logger = LogManager.GetLogger("FileDownUtil");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">下载的文件http地址</param>
        /// <param name="path">文件保存的绝对路径</param>
        /// <param name="d">下载过程回调</param>
        /// <returns></returns>
        public static bool DownFileRequest(string url, string path, FileDownloadDelegate d=null)
        {
            logger.Debug("DownFileRequest - url=" + url + ", path=" + path);
            if (!FileUtil.CreateDirectory(Path.GetDirectoryName(path)))
            {
                throw new DirectoryNotFoundException();
            }
            DateTime startTime = DateTime.Now;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            long contentLength = response.ContentLength;
            logger.Debug("DownFileRequest - Start - statusCode=" + response.StatusCode + ", contentLength=" + contentLength + ", date=" + DateUtil.GetNowDate());
            if (!HttpStatusCode.OK.Equals(response.StatusCode))
            {
                return false;
            }
            Stream stream = response.GetResponseStream();
            FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            long offset = 0;
            int size = -1;
            byte[] data = new byte[10240];
            TimeSpan span = DateTime.Now - startTime;
            double percent = 0;
            double second = span.TotalSeconds;
            double speed = 0;
            double interval = 0;
            String info = "";
            while ((size = stream.Read(data, 0, data.Length)) > 0)
            {
                fileStream.Write(data, 0, size);
                fileStream.Flush();
                offset += size;
                span = DateTime.Now - startTime;
                second = span.TotalSeconds;
                percent = (offset * 1.0 / contentLength);
                speed = (offset / second);
                info = second.ToString("F2") + "秒 " + NumberUtil.ConversionUnitMemory(speed) + "/秒 " + (percent * 100.0).ToString("F2") + "%";
                if (d != null && second - interval > 0.5)
                {
                    interval = second;
                    d.Invoke(percent, second, speed, info);
                }
            }
            span = DateTime.Now - startTime;
            second = span.TotalSeconds;
            percent = 1;
            speed = (contentLength / second);
            info = second.ToString("F2") + "秒 " + NumberUtil.ConversionUnitMemory(speed) + "/秒 " + (percent * 100.0).ToString("F2") + "%";
            if (d != null)
            {
                d.Invoke(percent, second, speed, info);
            }
            fileStream.Close();
            stream.Close();
            response.Close();
            logger.Debug("DownFileRequest - Over - length=" + offset + ", date=" + DateUtil.GetNowDate());
            return true;
        }

    }
}
