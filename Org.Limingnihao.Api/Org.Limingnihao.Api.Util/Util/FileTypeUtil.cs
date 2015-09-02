using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Api.Util
{
    public class FileTypeUtil
    {
        public static bool IsOfficeFile(string value)
        {
            if (value == null || "".Equals(value))
            {
                return false;
            }
            string type = GetFileType(value);
            if (IsWordFile(type) || IsPowerPointFile(type) || IsExcelFile(type) || IsVisioFile(type) || IsOneNoteFile(type))
            {
                return true;
            }
            return false;
        }

        public static bool IsWordFile(string value)
        {
            if (value == null || "".Equals(value))
            {
                return false;
            }
            string type = GetFileType(value);
            if ("doc".Equals(type) || "docx".Equals(type))
            {
                return true;
            }
            return false;
        }

        public static bool IsPowerPointFile(string value)
        {
            if (value == null || "".Equals(value))
            {
                return false;
            }
            string type = GetFileType(value);
            if ("ppt".Equals(type) || "pptx".Equals(type))
            {
                return true;
            }
            return false;
        }

        public static bool IsExcelFile(string value)
        {
            if (value == null || "".Equals(value))
            {
                return false;
            }
            string type = GetFileType(value);
            if ("xls".Equals(type) || "xlsx".Equals(type))
            {
                return true;
            }
            return false;
        }

        public static bool IsVisioFile(string value)
        {
            if (value == null || "".Equals(value))
            {
                return false;
            }
            string type = GetFileType(value);
            if ("vsd".Equals(type) || "vsdx".Equals(type))
            {
                return true;
            }
            return false;
        }

        public static bool IsOneNoteFile(string value)
        {
            if (value == null || "".Equals(value))
            {
                return false;
            }
            string type = GetFileType(value);
            if ("one".Equals(type) || "onex".Equals(type))
            {
                return true;
            }
            return false;
        }

        public static bool IsPdfFile(string value)
        {
            if (value == null || "".Equals(value))
            {
                return false;
            }
            string type = GetFileType(value);
            if ("pdf".Equals(type) || "pdf".Equals(type))
            {
                return true;
            }
            return false;
        }

        public static string GetFileType(string value)
        {
            if (value == null || "".Equals(value))
            {
                return "";
            }
            string[] v = value.Split('.');
            if (v.Length == 1)
            {
                return v[0].ToLower();
            }
            else
            {
                return v[v.Length - 1].ToLower();
            }
        }

    }
}
