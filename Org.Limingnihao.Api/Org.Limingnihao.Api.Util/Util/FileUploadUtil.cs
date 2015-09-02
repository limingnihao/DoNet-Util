using Common.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Org.Limingnihao.Api.Util
{
    /// <summary>
    /// 上传文件的回调代理
    /// </summary>
    /// <param name="percent"></param>
    public delegate void FileUploadDelegate(double percent, double second, double speed, string info);

    /// <summary>
    /// 上传文件工具类
    /// </summary>
    public class FileUploadUtil
    {
        private static readonly ILog logger = LogManager.GetLogger("FileUploadUtil");

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="url">http的访问地址</param>
        /// <param name="filePath">文件绝对路径</param>
        /// <param name="filePath">文件到服务器后的名字</param>
        /// <param name="filedName">上传时的文件参数名称</param>
        /// <param name="parameter">其他参数信息</param>
        /// <param name="d">上传回调代理</param>
        /// <returns></returns>
        public static string UploadFileRequest(string url, string filePath, string fileName, string filedName, NameValueCollection parameter = null, FileUploadDelegate d = null)
        {
            String paramString = "";
            if (parameter != null && parameter.Count > 0)
            {
                foreach (string key in parameter.Keys)
                {
                    paramString += key + "=" + parameter[key] + "&";
                }
                paramString = paramString.Substring(0, paramString.Length - 1);
                url += "?" + paramString;
            }
            logger.Debug("url=" + url + ", param=" + paramString);

            DateTime startTime = DateTime.Now;
            string boundary = "----------" + DateTime.Now.Ticks.ToString("x");
            byte[] endBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long fileLength = fs.Length;

            //请求头部信息
            StringBuilder headerBuilder = new StringBuilder();
            headerBuilder.Append("--" + boundary + "\r\n");
            headerBuilder.Append("Content-Disposition: form-data; name=\"" + filedName + "\"; filename=\"" + fileName + "\"\r\n");
            headerBuilder.Append("Content-Type: application/octet-stream");
            headerBuilder.Append("\r\n");
            headerBuilder.Append("\r\n");
            string headerString = headerBuilder.ToString();
            byte[] headerBytes = Encoding.UTF8.GetBytes(headerString);
            long length = fs.Length + headerBytes.Length + endBytes.Length;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727)";
            request.Method = "POST";
            request.AllowWriteStreamBuffering = false;
            request.Timeout = 300000;
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.ContentLength = length;

            Stream postStream = request.GetRequestStream();
            //写入http头
            postStream.Write(headerBytes, 0, headerBytes.Length);
            //写入文件
            int bufferLength = 4096;
            byte[] buffer = new byte[bufferLength];
            long offset = 0;
            int size = 0;

            TimeSpan span = DateTime.Now - startTime;
            double percent = 0;
            double second = span.TotalSeconds;
            double speed = 0;
            double interval = 0;
            String info = "";
            while ((size = br.Read(buffer, 0, bufferLength)) > 0)
            {
                postStream.Write(buffer, 0, size);
                offset += size;
                span = DateTime.Now - startTime;
                percent = (offset * 1.0 / fileLength);
                second = span.TotalSeconds;
                speed = (offset / second);
                info = second.ToString("F2") + "秒 " + NumberUtil.ConversionUnitMemory(speed) + "/秒 " + (percent * 100.0).ToString("F2") + "%";
                if (d != null && second - interval > 0.5)
                {
                    interval = second;
                    d.Invoke(percent, second, speed, info);
                }
            }
            //添加尾部
            postStream.Write(endBytes, 0, endBytes.Length);
            span = DateTime.Now - startTime;
            second = span.TotalSeconds;
            percent = 1;
            speed = (fileLength / second);
            info = second.ToString("F2") + "秒 " + NumberUtil.ConversionUnitMemory(speed) + "/秒 " + (percent * 100.0).ToString("F2") + "%";
            if (d != null)
            {
                d.Invoke(percent, second, speed, info);
            }

            //读取返回
            StreamReader sr = new StreamReader(request.GetResponse().GetResponseStream());
            string returnValue = sr.ReadLine();
            sr.Close();
            br.Close();
            fs.Close();
            postStream.Close();
            return returnValue;
        }

        // 获取普通表单区域二进制数组
        public static string createValueData(string boundary, string name, string value)
        {
            string textTemplate = boundary + "\r\n Content-Disposition: form-data; name=\"{0}\" {1} ";
            return String.Format(textTemplate, name, value);;
        }
    }
}
