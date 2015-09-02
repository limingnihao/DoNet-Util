using Org.Limingnihao.Api.Util;
using Org.Limingnihao.Api.Util.Util;
using Org.Limingnihao.Application.Data.Domain;
using Org.Limingnihao.Application.Data.Impl;
using Org.Limingnihao.Application.Help;
using Org.Limingnihao.Application.Service.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Application
{
    public class MainClass
    {
        static LogUtil logger = LogUtil.getInstance("main");

        public static void Main(string[] args)
        {
            LogUtil.LEVEL = LogUtil.L_DEBUG;
            new MainClass();
        }

        public MainClass()
        {
            logger.debug("-------------start-------------");

            logger.debug("-------------end-------------");
            System.Console.ReadLine();
        }


    }
}
