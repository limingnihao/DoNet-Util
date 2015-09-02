using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Org.Limingnihao.Api.Util
{
    /// <summary>
    /// MD5相关工具
    /// </summary>
    public class MD5Util
    {
        /// <summary>
        /// 获取文件MD5
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetFileMD5(string file)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read, 8192);
            md5.ComputeHash(stream);
            stream.Close();

            byte[] hash = md5.Hash;
            StringBuilder result = new StringBuilder();
            foreach (byte b in hash)
            {
                result.Append(string.Format("{0:X2}", b));
            }
            return result.ToString();
        }

        /// <summary>
        /// 和java的不一样 有问题
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string EncodingToMD5(string source)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] byteSource = Encoding.GetEncoding("UTF-8").GetBytes(source);
            byte[] byteHash = md5.ComputeHash(byteSource);
            string result = System.BitConverter.ToString(byteHash).Replace("-", "").ToUpper();
            return result;
        }

 
    }
}
