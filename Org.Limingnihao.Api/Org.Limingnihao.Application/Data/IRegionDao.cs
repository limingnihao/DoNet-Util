using Org.Limingnihao.Application.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Application.Data
{
    public interface IRegionDao
    {
        RegionEntity GetEntity(Int32 id);

        List<RegionEntity> GetListAll();

    }
}
