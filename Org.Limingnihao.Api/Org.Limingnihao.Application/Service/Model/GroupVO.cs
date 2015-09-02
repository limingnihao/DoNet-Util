using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Application.Service.Model
{
    public class GroupVO
    {
        public Int32 GroupId { get; set; }
        public String GroupName { get; set; }
        public Int32 ParentId { get; set; }
        public Int32 RegionId { get; set; }
        public Int32 Sequence { get; set; }
        public String Description { get; set; }
        public Int32 UseFlag { get; set; }
    }
}
