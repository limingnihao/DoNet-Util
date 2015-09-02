using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Update.Service.Model
{
    public class IVersionVO
    {
        public bool success { get; set; }
        public int versionId { get; set; }
        public int versionCode { get; set; }
        public string versionName { get; set; }
        public string downUrl { get; set; }
        public string fileMd5 { get; set; }
        public string fileName { get; set; }
    }
}
