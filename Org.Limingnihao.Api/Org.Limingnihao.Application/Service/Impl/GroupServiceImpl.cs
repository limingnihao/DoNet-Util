using Org.Limingnihao.Api.Util;
using Org.Limingnihao.Application.Data;
using Org.Limingnihao.Application.Data.Domain;
using Org.Limingnihao.Application.Service.Model;
using Spring.Transaction.Interceptor;
using System.Collections.Generic;

namespace Org.Limingnihao.Application.Service.Impl
{
    public class GroupServiceImpl :　IGroupService
    {

        #region 初始化方法

        private LogUtil logger = LogUtil.getInstance("GroupServiceImpl");

        private IUserDao userDao = null;
        private IGroupDao groupDao = null;

        public IUserDao UserDao
        {
            get { return userDao; }
            set { userDao = value; }
        }

        public IGroupDao GroupDao
        {
            get { return groupDao; }
            set { groupDao = value; }
        }

        public GroupServiceImpl()
        {
        }

        #endregion


        #region 接口实现方法

        [Transaction(ReadOnly = true)]
        public IList<GroupVO> GetListAll()
        {
            IList<GroupVO> list = new List<GroupVO>();
            foreach(GroupEntity e in this.groupDao.GetListAll())
            {
                GroupVO vo = new GroupVO();
                vo.GroupId = e.GroupId;
                vo.GroupName = e.GroupName;
                vo.ParentId = e.ParentId;
                vo.RegionId = e.RegionId;
                vo.Sequence = e.Sequence;
                vo.UseFlag = e.UseFlag;
                vo.Description = e.Description;
                list.Add(vo);
            }
            return list;
        }

        #endregion

    }
}
