using Common.Logging;
using Org.Limingnihao.Api.Util;
using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Application.Help
{
    public class SpringHelp
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(SpringHelp));

        public static IApplicationContext ApplicationContext = null;

        public static bool Init(String dir = "")
        {
            try
            {
                if (ApplicationContext == null)
                {
                    ApplicationContext = new XmlApplicationContext(dir + @"\Config\spring.net.xml");
                    logger.Info("ApplicationContext - init - new - " + dir + @"\Config\spring.net.xml");
                }
                else
                {
                    logger.Info("ApplicationContext - init - have");
                }
                return true;
            }
            catch(Exception e)
            {
                logger.Info("Init - " + e.Message);
            }
            return false;
        }

        public static T GetObject<T>(string name)
        {
            return (T)ApplicationContext.GetObject(name);
        }

    }
}
