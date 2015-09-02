using System;

namespace Org.Limingnihao.Application.Data.Domain
{
    [Serializable]
    public class GroupEntity
    {
        public virtual Int32 GroupId { get; set; }
        public virtual String GroupName { get; set; }
        public virtual Int32 ParentId { get; set; }
        public virtual Int32 RegionId { get; set; }
        public virtual Int32 Sequence { get; set; }
        public virtual String Description { get; set; }
        public virtual Int32 UseFlag { get; set; }

        public virtual GroupEntity ParentEntity { get; set; }
        //public virtual RegionEntity RegionEntity { get; set; }

        public virtual Iesi.Collections.Generic.ISet<GroupEntity> Children { get; set; }

    }
}
