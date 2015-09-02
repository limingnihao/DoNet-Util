using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Api.Util
{
    public class PathUtil
    {
        public static String MergeHttp(String server, String path)
        {
            if (server.StartsWith("http:"))
            {
                server = server.Substring(5);
            }
            while (server.StartsWith("/"))
            {
                server = server.Substring(1);
            }
            while (server.EndsWith("/"))
            {
                server = server.Substring(0, server.Length - 1);
            }
            while (path.StartsWith("/"))
            {
                path = path.Substring(1);
            }
            if (path.StartsWith(":"))
            {
                return "http://" + server + "" + path;
            }
            else
            {
                return "http://" + server + "/" + path;
            }
        }

        public static String MergeUrl(String server, String path)
        {
            if (server.StartsWith("http:"))
            {
                server = server.Substring(5, server.Length);
            }
            while (server.StartsWith("/"))
            {
                server = server.Substring(1);
            }
            while (server.EndsWith("/"))
            {
                server = server.Substring(0, server.Length - 1);
            }

            while (path.StartsWith("/"))
            {
                path = path.Substring(1);
            }
            if (path.StartsWith(":"))
            {
                return server + "" + path;
            }
            else
            {
                return server + "/" + path;
            }
        }

        public static String MergePath(String path1, String path2)
        {
            //while (path1.StartsWith("/") || path1.StartsWith("\\"))
            //{
            //    path1 = path1.Substring(1);
            //}
            while (path1.EndsWith("/") || path1.EndsWith("\\"))
            {
                path1 = path1.Substring(0, path1.Length - 1);
            }
            while (path2.StartsWith("/") || path2.StartsWith("\\"))
            {
                path2 = path2.Substring(1);
            }
            //while (path2.EndsWith("/") || path2.EndsWith("\\"))
            //{
            //    path2 = path2.Substring(0, path2.Length - 1);
            //}
            return path1 + "/" + path2;
        }

    }
}
