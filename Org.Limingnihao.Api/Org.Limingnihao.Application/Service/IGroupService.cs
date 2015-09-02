using Org.Limingnihao.Application.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Application.Service
{
    public interface IGroupService
    {
        IList<GroupVO> GetListAll();
    }
}
