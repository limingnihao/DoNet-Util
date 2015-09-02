using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Application.Data.Domain
{
    [Serializable]
    public class RegionEntity
    {
        public virtual Int32 RegionId { get; set; }
        public virtual String RegionName { get; set; }
        public virtual Int32 ParentId { get; set; }
        public virtual RegionEntity ParentEntity { get; set; }

    }
}
