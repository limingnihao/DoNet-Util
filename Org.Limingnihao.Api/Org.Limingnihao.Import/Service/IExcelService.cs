using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Shell.Import.View.Service
{
    public interface IExcelService
    {
        /// <summary>
        /// 获取所有的表名称
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        List<string> GetTableList(string connectionString);

        /// <summary>
        /// 读取表头
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        List<string> GetFieldList(string connectionString, string tableName);

        /// <summary>
        /// 读取表中数据
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        List<List<string>> GetValueList(string connectionString, string tableName);

        /// <summary>
        /// 拼接sql
        /// </summary>
        /// <param name="tables"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        List<string> GetSqlList(string tableName, List<string> fields, List<List<string>> values);

    }
}
