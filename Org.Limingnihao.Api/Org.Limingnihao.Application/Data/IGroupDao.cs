using Org.Limingnihao.Application.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Application.Data
{
    public interface IGroupDao
    {
        GroupEntity GetEntity(Int32 id);

        List<GroupEntity> GetListAll();

        List<GroupEntity> GetListByParentId(Int32 parentId);
    }
}
