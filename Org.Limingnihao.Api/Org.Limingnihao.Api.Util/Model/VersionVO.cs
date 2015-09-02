using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Api.Util.Model
{
    public class VersionVO
    {
        private bool _success;
        private int _versionId;
        private int _versionCode;
        private string _versionName;
        private string _downUrl;
        private string _fileMd5;
        private string _fileName;

        public bool Success
        {
            get { return _success; }
            set { _success = value; }
        }

        public int VersionId
        {
            get { return _versionId; }
            set { _versionId = value; }
        }

        public Int32 VersionCode
        {
            get { return _versionCode; }
            set { _versionCode = value; }
        }

        public string VersionName
        {
            get { return _versionName; }
            set { _versionName = value; }
        }

        public string DownUrl
        {
            get { return _downUrl; }
            set { _downUrl = value; }
        }

        public string FileMd5
        {
            get { return _fileMd5; }
            set { _fileMd5 = value; }
        }

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

    }
}
