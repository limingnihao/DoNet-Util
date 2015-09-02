using Org.Limingnihao.Api.Util;
using NHibernate;
using Org.Limingnihao.Application.Data.Domain;
using Org.Limingnihao.Application.Help;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Data.NHibernate.Generic.Support;

namespace Org.Limingnihao.Application.Data.Impl
{
    public class RegionDaoImpl : HibernateDaoSupport, IRegionDao
    {

        #region 初始化方法

        private LogUtil logger = LogUtil.getInstance("RegionDaoImpl");

        public RegionDaoImpl()
        {
        }

        #endregion


        #region 接口实现方法

        public RegionEntity GetEntity(int id)
        {
            return this.HibernateTemplate.Get<RegionEntity>(id);
        }

        public List<RegionEntity> GetListAll()
        {
            IQuery query = this.HibernateTemplate.SessionFactory.GetCurrentSession().CreateQuery("select r from RegionEntity as r");
            List<RegionEntity> list = new List<RegionEntity>();
            foreach (RegionEntity entity in query.List())
            {
                list.Add(entity);
            }
            return list;
        }

        #endregion


    }
}
