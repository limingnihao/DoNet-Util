using Common.Logging;
using NHibernate;
using Org.Limingnihao.Api.Util;
using Org.Limingnihao.Application.Data;
using Org.Limingnihao.Application.Data.Domain;
using Org.Limingnihao.Application.Data.Impl;
using Org.Limingnihao.Application.Excep;
using Org.Limingnihao.Application.Help;
using Org.Limingnihao.Application.Service.Model;
using Spring.Transaction.Interceptor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Application.Service.Impl
{
    public class UserServiceImpl : IUserService
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(UserServiceImpl));

        #region 初始化方法

        private IUserDao userDao = null;

        public IUserDao UserDao
        {
            get { return userDao; }
            set { userDao = value; }
        }

        public UserServiceImpl()
        {
        }

        #endregion


        #region 接口实现方法

        [Transaction(ReadOnly = true)]
        public UserVO Login(string username, string password)
        {
            logger.Info("Login - username=" + username + ", password=" + password);
            UserEntity entity = this.userDao.GetEntityByUsername(username);
            if(entity == null)
            {
                throw new UserNullPointerException();
            }
            else if (!entity.Password.Equals(AESUtil.EncryptHex(password, ApplicationHelp.AES_PASSWORD)))
            {
                throw new UserPasswordErrorException();
            }
            else
            {
                UserVO vo = new UserVO();
                vo.Username = entity.Username;
                vo.Nickname = entity.Nickname;
                vo.UserId = entity.UserId;
                return vo;
            }
        }

        public void RePassword(int userId, string password)
        {
            UserEntity entity = this.userDao.GetEntity(userId);
            if (entity == null)
            {
                throw new UserNullPointerException();
            }
            String code = AESUtil.EncryptHex(password, ApplicationHelp.AES_PASSWORD);
            entity.Password = code;
            this.userDao.SaveEntity(entity);
        }

        #endregion

    }
}
