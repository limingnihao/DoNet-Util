using NHibernate;
using Org.Limingnihao.Api.Util;
using Org.Limingnihao.Application.Data.Domain;
using Org.Limingnihao.Application.Help;
using Spring.Data.NHibernate.Generic.Support;
using Spring.Stereotype;
using Spring.Transaction.Interceptor;
using System;
using System.Collections.Generic;

namespace Org.Limingnihao.Application.Data.Impl
{
    [Repository]
    public class UserDaoImpl : HibernateDaoSupport, IUserDao
    {

        #region 初始化方法

        private LogUtil logger = LogUtil.getInstance("UserDaoImpl");

        public UserDaoImpl()
        {
        }

        #endregion


        #region 接口实现方法

        public void SaveEntity(UserEntity entity)
        {
            this.HibernateTemplate.SaveOrUpdate(entity);
        }

        public UserEntity GetEntity(Int32 id)
        {
            return this.HibernateTemplate.Get<UserEntity>(id);
        }

        public UserEntity GetEntityByUsername(string username)
        {
            IQuery query = this.HibernateTemplate.SessionFactory.GetCurrentSession().CreateQuery("select user from UserEntity as user where user.Username=:username");
            query.SetParameter("username", username);
            List<UserEntity> list = new List<UserEntity>();
            foreach (UserEntity entity in query.List())
            {
                list.Add(entity);
            }
            if (list != null && list.Count > 0)
            {
                return list[0];
            }
            return null;
        }

        public List<UserEntity> GetListAll()
        {
            IQuery query = this.HibernateTemplate.SessionFactory.GetCurrentSession().CreateQuery("select user from UserEntity as user");
            List<UserEntity> list = new List<UserEntity>();
            foreach (UserEntity entity in query.List())
            {
                list.Add(entity);
            }
            return list;
        }

        #endregion

    }
}
