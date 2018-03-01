using Common.Logging;
using Org.Limingnihao.Api.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace Com.Shell.Import.View.Service.Impl
{
    public class ExcelServiceImpl : IExcelService
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(ExcelServiceImpl));

        private static IExcelService INSTANCE = null;

        public static IExcelService GetInstance()
        {
            if(INSTANCE == null){
                INSTANCE = new ExcelServiceImpl();
            }
            return INSTANCE;
        }

        /// <summary>
        /// 构建连接字符串
        /// 1、.xls - Excel 8.0, .xlsx - Excel 12.0
        /// 2、HDR表示要把第一行作为数据还是作为列名，作为数据用HDR=NO，作为列名用HDR=YES；  
        /// 3、通过IMEX=1来把混合型作为文本型读取，避免null值。
        /// 4、把一个 Excel 文件看做一个数据库，一个sheet看做一张表。语法 "SELECT * FROM [sheet1$]"，表单要使用"[]"和"$"
        /// string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\";";
        /// </summary>
        public List<string> GetTableList(string connectionString)
        {
            List<string> tableList = new List<string>();
            OleDbConnection conn = new OleDbConnection(connectionString);
            conn.Open();
            DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    tableList.Add(row["TABLE_NAME"].ToString().Replace("$", ""));
                }
                return tableList;
            }
            else
            {
                return null;
            }
        }

        public List<string> GetFieldList(string connectionString, string tableName)
        {
            List<string> fields = new List<string>();
            string sql = string.Format("select * from [{0}$]", tableName);
            OleDbDataAdapter da = new OleDbDataAdapter(sql, connectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            foreach (object obj in dt.Rows[0].ItemArray)
            {
                String val = obj.ToString().Trim();
                if (!"".Equals(val))
                {
                    fields.Add(val);
                }
            }
            return fields;
        }

        public List<List<string>> GetValueList(string connectionString, string tableName)
        {
            List<List<string>> values = new List<List<string>>();
            //填充数据
            string sql = string.Format("select * from [{0}$]", tableName);
            OleDbDataAdapter da = new OleDbDataAdapter(sql, connectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 1; i < dt.Rows.Count; i++)
            {
                List<string> valueObj = new List<string>();
                int count = 0;
                foreach (object obj in dt.Rows[i].ItemArray)
                {
                    valueObj.Add(obj.ToString().Trim());
                    if (!obj.ToString().Trim().Equals(""))
                    {
                        count++;
                    }
                }
                if (count != 0)
                {
                    values.Add(valueObj);
                }
            }
            return values;
        }

        public List<string> GetSqlList(String tableName, List<string> fields, List<List<string>> values)
        {
            //INSERT INTO `商品品牌` (`编码`, `名称`) VALUES ('1', '1')
            List<string> sqls = new List<string>();
            string sqlStart = "insert into `" + tableName + "` (";
            foreach (string field in fields)
            {
                string ff = field;
                if (tableName.Equals("工单单据头") && field.Equals("录入时间"))
                {
                    ff = "录入日期";
                }
                else if ((tableName.Equals("工单单据明细") || tableName.Equals("工单结算信息")) && field.Equals("商品/服务编码"))
                {
                    ff = "商品编码";
                }
                else if ((tableName.Equals("工单单据明细") || tableName.Equals("工单结算信息")) && field.Equals("商品/服务类别"))
                {
                    ff = "商品类别";
                }
                else if (tableName.Equals("工单结算信息") && field.Equals("折扣字典编码"))
                {
                    ff = "折扣券折扣字典编码";
                }
                sqlStart += "`" + ff + "`,";
            }
            sqlStart = sqlStart.Substring(0, sqlStart.Length - 1) + ")";

            foreach (List<string> objs in values)
            {
                string sqlEnd = " values(";
                for (int i = 0; i < fields.Count; i++)
                {
                    objs[i] = objs[i].Replace("\\", "").Replace("\t", "").Replace("\r", "");
                    objs[i] = this.dataCheck_jichushuju(tableName, fields[i], objs[i]);
                    objs[i] = this.dataCheck_kucunxinxi(tableName, fields[i], objs[i]);
                    objs[i] = this.dataCheck_gongdanmokuai(tableName, fields[i], objs[i]);
                    objs[i] = this.dataCheck_yewushuju(tableName, fields[i], objs[i]);
                    objs[i] = this.dataCheck_zhekoushuju(tableName, fields[i], objs[i]);
                    if (objs[i].Equals("null"))
                    {
                        sqlEnd += "null,";
                    }
                    else
                    {
                        sqlEnd += "'" + objs[i] + "',";
                    }
                }
                sqlEnd = sqlEnd.Substring(0, sqlEnd.Length - 1) + ");";
                string sqlString = sqlStart + sqlEnd;
                sqls.Add(sqlString);
            }
            return sqls;
        }

        #region 解析检查

        /// <summary>
        /// 基础数据 - 解析检查
        /// </summary>
        private string dataCheck_jichushuju(string tableName, string field, string value)
        {
            if (tableName.Equals("商品资料") && field.Equals("标准售价") && value.Equals(""))
            {
                return "0";
            }
            else if (tableName.Equals("商品资料") && field.Equals("参考进价") && value.Equals(""))
            {
                return "0";
            }
            else if (tableName.Equals("车辆信息表") && field.Equals("年份"))
            {
                if (value != null && !value.Equals(""))
                {
                    try
                    {
                        int year = NumberUtil.Parse(value.Substring(0, 4));
                        if (year != 0)
                        {
                            return DateTime.Parse(year + "-01-01").ToString();
                        }
                    }
                    catch(Exception ey)
                    {
                        logger.Info("" + ey.Message);
                    }
                }
                return "null";
            }
            else if (tableName.Equals("车辆信息表") && field.Equals("行驶里程") && value.Equals(""))
            {
                return "0";
            }
            else if (tableName.Equals("车辆信息表") && field.Equals("车牌号"))
            {
                return value.Replace("\\", "").Replace(".", "");
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// 库存信息
        /// </summary>
        private string dataCheck_kucunxinxi(string tableName, string field, string value)
        {
            if (tableName.Equals("入库明细") && field.Equals("入库数量") && value.Equals(""))
            {
                return "0";
            }
            else if (tableName.Equals("成本调整明细") && field.Equals("入库金额") && value.Equals(""))
            {
                return "0";
            }
            else if (tableName.Equals("成本调整明细") && field.Equals("入库单价") && value.Equals(""))
            {
                return "0";
            }
            else if (tableName.Equals("成本调整明细") && field.Equals("调整后单价") && value.Equals(""))
            {
                return "0";
            }
            else if (tableName.Equals("成本调整明细") && field.Equals("调整金额") && value.Equals(""))
            {
                return "0";
            }
            else if (tableName.Equals("当前库存明细") && field.Equals("库存数量") )
            {
                int v = 0;
                try
                {
                    v = Int32.Parse(value);
                }
                catch (OverflowException)
                {
                    v = Int32.MaxValue;
                }
                catch (Exception e)
                {
                    logger.Info("当前库存明细 - 库存数量 - value=" + value + ", " + e.Message);
                }
                return v + "";
            }
            else if (tableName.Equals("当前库存明细") && field.Equals("库存总金额") && value.Equals(""))
            {
                int v = 0;
                try
                {
                    v = Int32.Parse(value);
                }
                catch (OverflowException)
                {
                    v = Int32.MaxValue;
                }
                catch (Exception e)
                {
                    logger.Info("当前库存明细 - 库存总金额 - value=" + value + ", " + e.Message);
                }
                return v + "";
            }

            else
            {
                return value;
            }
        }

        /// <summary>
        /// 工单模块 - 解析检查
        /// </summary>
        private string dataCheck_gongdanmokuai(string tableName, string field, string value)
        {
            if (tableName.Equals("套餐数据") && field.Equals("套餐对应商品的标准数量") && value.Equals(""))
            {
                return "0";
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// 业务数据 - 解析检查
        /// </summary>
        private string dataCheck_yewushuju(string tableName, string field, string value)
        {
            if (tableName.Equals("工单单据头") && field.Equals("车牌号"))
            {
                return value.Replace("\\", "").Replace(".", "");
            }
            else if (tableName.Equals("工单单据头") && field.Equals("工单状态"))
            {
                value = value.ToLower();
                if (value.Equals("000000"))
                {
                    return "0";
                }
                else if (value.Equals("000009"))
                {
                    return "9";
                }
                else
                {
                    return value;
                }
            }
            else if (tableName.Equals("工单单据头") && (field.Equals("录入日期") || field.Equals("录入时间")))
            {
              if (value != null && !value.Equals(""))
                {
                    string result =  DateUtil.Parse(value).ToString();
                    return result;
                }
                return "null";
            }
            else if (tableName.Equals("工单单据头") && field.Equals("审核日期"))
            {
                if (value != null && !value.Equals(""))
                {
                    string result = DateUtil.Parse(value).ToString();
                    return result;
                }
                return "null";
            }
            else if (tableName.Equals("工单单据明细") && field.Equals("零售金额") && value.Equals(""))
            {
                return "0";
            }
            else if (tableName.Equals("工单单据明细") && field.Equals("成交金额") && value.Equals(""))
            {
                return "0";
            }
            else if (tableName.Equals("工单结算信息") && field.Equals("单价") && value.Equals(""))
            {
                return "0";
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// 折扣数据 - 解析检查
        /// </summary>
        private string dataCheck_zhekoushuju(string tableName, string field, string value)
        {
            if (tableName.Equals("折扣字典") && field.Equals("折扣值"))
            {
                return value.Replace("%", "");
            }
            else if (tableName.Equals("卡数据（主）") && field.Equals("车牌号"))
            {
                return value.Replace("%", "").Replace(".", "");
            }
            else
            {
                return value;
            }
        }

        #endregion


    }
}
