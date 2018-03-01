using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Shell.Import.View.Service.Impl
{
    public class CheckServiceImpl : ICheckService
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(DatabaseServiceImpl));

        private static ICheckService INSTANCE = null;

        private IDatabaseService databaseService;

        public static ICheckService GetInstance()
        {
            if (INSTANCE == null)
            {
                INSTANCE = new CheckServiceImpl();
            }
            return INSTANCE;
        }

        public CheckServiceImpl()
        {
            this.databaseService = DatabaseServiceImpl.GetInstance();
        }

        public void CheckGoods()
        {
            //商品类别，判断父节点
            string sql_1_1 = "SELECT count(*) FROM `商品和服务类别` s1 WHERE s1.`上级编码` != 0 AND NOT EXISTS ( SELECT 1 FROM `商品和服务类别` s2 WHERE s1.`上级编码` = s2.`编码`);";
            //int c1 = this.databaseService.ExecuteSqlCount(sql_1_1);

            string sql_1_2 = "select count(*) from `商品资料` s1 where NOT EXISTS(SELECT 1 FROM `商品品牌` s2 WHERE s1.`品牌` = s2.`编码`);";
            int c2 = this.databaseService.ExecuteSqlCount(sql_1_2);
        }


    }
}
