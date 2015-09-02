using NHibernate;
using NHibernate.Cfg;
using Org.Limingnihao.Application.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Limingnihao.Application.Help
{
    public class NHibernateHelper
    {
        public static Configuration Configuration = null;
        public static ISessionFactory SessionFactory = null;
        public static ISession CurrentSession = null;

        public static Configuration InitConfiguration()
        {
            Configuration = new Configuration();
            Configuration.Configure(@"Config\NHibernate.cfg.xml");
            Configuration.AddAssembly("Org.Limingnihao.Application");
            return Configuration;
        }

        public static void InitFactory()
        {
            SessionFactory = Configuration.BuildSessionFactory();
            CurrentSession = SessionFactory.OpenSession();
        }

        //public static void CloseSessionFactory()
        //{
        //    if (sessionFactory != null)
        //    {
        //        sessionFactory.Close();
        //    }
        //}
    }
}
