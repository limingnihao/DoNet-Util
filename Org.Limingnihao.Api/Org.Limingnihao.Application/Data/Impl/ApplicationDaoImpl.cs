using NHibernate;
using Org.Limingnihao.Application.Data.Domain;
using Spring.Data.Core;
using Spring.Data.NHibernate;
using Spring.Data.NHibernate.Generic.Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Application.Data.Impl
{
    public class ApplicationDaoImpl : HibernateDaoSupport, IApplicationDao
    {

        public string Test()
        {
            throw new NotImplementedException();
        }

    }
}
