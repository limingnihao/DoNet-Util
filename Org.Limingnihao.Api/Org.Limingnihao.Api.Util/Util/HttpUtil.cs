using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using Common.Logging;

namespace Org.Limingnihao.Api.Util
{
    /// <summary>
    /// http请求工具类
    /// </summary>
    public class HttpUtil
    {
        private static readonly ILog logger = LogManager.GetLogger("HttpUtil");

        public const String CONTENT_TYPE_PLAIN = "text/plain";
	    public const String CONTENT_TYPE_XML = "text/xml";
	    public const String CONTENT_TYPE_HTML = "text/html";
	    public const String CONTENT_TYPE_JS = "text/javascript";
	    public const String CONTENT_TYPE_APP_JSON = "application/json";
	    public const String CONTENT_TYPE_APP_EXCEL = "application/vnd.ms-excel";
	    public const String CONTENT_TYPE_APP_FORM = "application/x-www-form-urlencoded";
        public const String DEFAULT_ENCODING = "UTF-8";
        public const int TIME_OUT_HTTP = 5000;

        /// <summary>
        /// 进行http的post访问.
        /// 简单的使用方式如下：
        /// Dictionary<string, string> parameter = new Dictionary<string, string>();
        /// parameter.Add("key", "value");
        /// string result = HttpUtil.Post("http://127.0.0.1", parameter);
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="parameter">参数</param>
        /// <returns>结果</returns>
        public static string Post(string url, Dictionary<string, string> parameter)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                String paramString = "";
                if (parameter != null)
                {
                    foreach (KeyValuePair<string, string> author in parameter)
                    {
                        paramString += author.Key + "=" + author.Value + "&";
                    }
                    paramString = paramString.Substring(0, paramString.Length - 1);
                }
                return Post(url, paramString);
            }
            catch(Exception e)
            {
                logger.Info(url + " - " + e.ToString());
            }
            return null;
        }

        public static string Post(string url, NameValueCollection parameter = null)
        {
            try
            {
                String paramString = "";
                if (parameter != null && parameter.Count > 0)
                {
                    foreach (string key in parameter.Keys)
                    {
                        paramString += key + "=" + parameter[key] + "&";
                    }
                    paramString = paramString.Substring(0, paramString.Length - 1);
                }
                return Post(url, paramString);
            }
            catch(Exception e)
            {
                logger.Info(url + " - " + e.ToString());
            }
            return null;
        }


        public static string Post(string url, string parameter)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            logger.Debug("Post - url=" + url + ", param=" + parameter);
            byte[] byteArray = Encoding.UTF8.GetBytes(parameter);
            request.Method = "POST";
            request.Timeout = TIME_OUT_HTTP;
            request.ContentType = CONTENT_TYPE_APP_FORM;
            request.ContentLength = byteArray.Length;
            request.GetRequestStream().Write(byteArray, 0, byteArray.Length);
            request.GetRequestStream().Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (HttpStatusCode.OK.Equals(response.StatusCode))
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string responseFromServer = reader.ReadToEnd();
                reader.Close();
                response.GetResponseStream().Close();
                response.Close();
                return responseFromServer;
            }
            else
            {
                return null;
            }
        }

        public static string Get(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            logger.Debug("Get - url=" + url);
            request.Method = "GET";
            request.Timeout = TIME_OUT_HTTP;
            request.ContentType = CONTENT_TYPE_HTML;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (HttpStatusCode.OK.Equals(response.StatusCode))
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string responseFromServer = reader.ReadToEnd();
                reader.Close();
                response.GetResponseStream().Close();
                response.Close();
                return responseFromServer;
            }
            else
            {
                return null;
            }
        }
    }
}
