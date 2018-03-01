using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Shell.Import.View.Service
{
    public interface IDatabaseService
    {

        void ConnectMySql(string server, int port, string database, string user, string password);

        bool IsConnect();

        int ExecuteSql(string sql, ref string error);

        void ExecuteSql(string sql, ref int count, ref int total, ref List<string> errorList, ref string error);

        int ExecuteSqlCount(string sql);

        void ExecuteFileSql(string path, ref int count, ref string error);

    }
}
