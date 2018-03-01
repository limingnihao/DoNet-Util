using Common.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace Com.Shell.Import.View.Service.Impl
{
    public class DatabaseServiceImpl : IDatabaseService
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(DatabaseServiceImpl));

        private static IDatabaseService INSTANCE = null;

        public static IDatabaseService GetInstance()
        {
            if (INSTANCE == null)
            {
                INSTANCE = new DatabaseServiceImpl();
            }
            return INSTANCE;
        }

        private MySqlConnection mySqlConnection = null;
        private String mysqlString = "";
        public void ConnectMySql(string server, int port, string database, string user, string password)
        {
            this.mysqlString = "Server=" + server + ";Port=" + port + ";Database=" + database + ";User Id=" + user + ";Password=" + password + ";Charset=utf8;";
            //String mysqlString = "Data Source=127.0.0.1;port=3306;User Id=admin;Password=admin;Database=test;pooling=false;CharSet=utf8;";
            logger.Info("ConnectMySql - " + this.mysqlString);
            if(this.mySqlConnection != null)
            {
                this.mySqlConnection.Close();
            }
            this.mySqlConnection = new MySqlConnection(this.mysqlString);
            this.mySqlConnection.Open();
        }
        public bool IsConnect()
        {
            if(this.mySqlConnection != null)
            {
                return this.mySqlConnection.State.Equals(ConnectionState.Open);
            }
            return false;
        }

        public int ExecuteSql(string sql, ref string error)
        {
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(sql, this.mySqlConnection);
                return mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                error = "error - sql=[" + sql + "] - " +  e.Message;
                logger.Info("error - " + sql);
            }
            return 0;
        }



        public void ExecuteSql(string sql, ref int count, ref int total, ref List<string> errorList, ref string error)
        {
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(sql, this.mySqlConnection);
                count++;
                int t = mySqlCommand.ExecuteNonQuery();
                total += t;
                //logger.Info("count=" + count + ", execute=" + t + ", total=" + total);
            }
            catch(Exception e)
            {
                error = "error - sql=[" + sql + "] - " + e.Message;
                logger.Info("error - " + sql);
                errorList.Add(sql);
            }
        }

        public int ExecuteSqlCount(string sql)
        {
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(sql, this.mySqlConnection);
                MySqlDataReader reader = mySqlCommand.ExecuteReader();
                //int i = mySqlCommand.ExecuteNonQuery();
                object obj = reader.GetValue(0);
                return 1;
            }
            catch (Exception e)
            {
                logger.Info("error - " + sql);
            }
            return 0;
        }

        public void ExecuteFileSql(string path, ref int count, ref string error)
        {
            try
            {
                FileInfo file = new FileInfo(path);
                string script = file.OpenText().ReadToEnd();
                SqlConnection conn = new SqlConnection("Data Source=127.0.0.1;User Id=admin;Password=admin;Database=ps_gx");
                Microsoft.SqlServer.Management.Smo.Server server = new Microsoft.SqlServer.Management.Smo.Server(new Microsoft.SqlServer.Management.Common.ServerConnection(conn));
                count = server.ConnectionContext.ExecuteNonQuery(script);
                conn.Close();
            }
            catch(Exception e)
            {
                logger.Info("ExecuteFileSql - error=" + e.Message);
            }
        }

    }
}
