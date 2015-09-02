using Org.Limingnihao.Api.Util;
using NHibernate;
using Org.Limingnihao.Application.Data.Domain;
using Org.Limingnihao.Application.Help;
using System;
using System.Collections.Generic;
using Spring.Data.NHibernate.Generic.Support;

namespace Org.Limingnihao.Application.Data.Impl
{
    public class GroupDaoImpl : HibernateDaoSupport, IGroupDao
    {


        #region 初始化方法

        private LogUtil logger = LogUtil.getInstance("GroupDaoImpl");

        public GroupDaoImpl()
        {
        }

        #endregion


        #region 接口实现方法

        public GroupEntity GetEntity(int id)
        {
            return this.HibernateTemplate.Get<GroupEntity>(id);
        }

        public List<GroupEntity> GetListAll()
        {
            IQuery query = this.HibernateTemplate.SessionFactory.GetCurrentSession().CreateQuery("select g from GroupEntity as g");
            List<GroupEntity> list = new List<GroupEntity>();
            foreach (GroupEntity entity in query.List())
            {
                list.Add(entity);
            }
            return list;
        }

        public List<GroupEntity> GetListByParentId(int parentId)
        {
            IQuery query = null;
            if( parentId > 0)
            {
                query = this.HibernateTemplate.SessionFactory.GetCurrentSession().CreateQuery("select g from GroupEntity as g where g.parentId=:parentId");
                query.SetParameter("parentId", parentId);
            }
            else
            {
                query = this.HibernateTemplate.SessionFactory.GetCurrentSession().CreateQuery("select g from GroupEntity as g where g.ParentEntity is null");
            }
            List<GroupEntity> list = new List<GroupEntity>();
            foreach (GroupEntity entity in query.List())
            {
                list.Add(entity);
            }
            return list;
        }

        #endregion


    }
}
