using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Org.Limingnihao.Api.Util
{
    /// <summary>
    /// 修改ini文件
    /// </summary>
    public class IniFileUtil
    {
        //  <summary>
        ///  ini文件名称（带路径)
        ///  </summary>
        public string filePath;

        //声明读写INI文件的API函数
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public static IniFileUtil GetInstance(string iniPath)
        {
            return new IniFileUtil(iniPath);
        }


        ///  <summary>
        ///  类的构造函数
        ///  </summary>
        ///  <param  name="INIPath">INI文件名</param>  
        public IniFileUtil(string iniPath)
        {
            filePath = iniPath;
        }

        ///  <summary>
        ///   写INI文件
        ///  </summary>
        ///  <param  name="Section">Section</param>
        ///  <param  name="Key">Key</param>
        ///  <param  name="value">value</param>
        public void WriteInivalue(string Section, string Key, string value)
        {
            WritePrivateProfileString(Section, Key, value, this.filePath);
        }

        ///  <summary>
        ///  读取INI文件指定部分
        ///  </summary>
        ///  <param  name="Section">Section</param>
        ///  <param  name="Key">Key</param>
        ///  <returns>String</returns>  
        public string ReadInivalue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(2550);
            int i = GetPrivateProfileString(Section, Key, "", temp, 2550, this.filePath);
            return temp.ToString();

        }
    }
}
