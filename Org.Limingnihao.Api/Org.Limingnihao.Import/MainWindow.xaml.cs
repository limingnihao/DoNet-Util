
using Com.Shell.Import.View.Service;
using Com.Shell.Import.View.Service.Impl;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace Com.Shell.Import.View
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(MainWindow));

        private IExcelService excelService = null;
        private IDatabaseService databaseService = null;
        private ICheckService checkService = null;

        private IList<string> logList = new ObservableCollection<string>();

        private String filePath = "";
        private List<string> sqlList = new List<string>();
        private List<string> delSqlList = new List<string>();
        private Boolean isDelete = false;

        public MainWindow()
        {
            InitializeComponent();

            this.excelService = ExcelServiceImpl.GetInstance();
            this.databaseService = DatabaseServiceImpl.GetInstance();
            this.checkService = CheckServiceImpl.GetInstance();

            this.listBox_log.ItemsSource = this.logList;
        }

        #region 过程按钮处理

        private void connectMysql_clickHandler(object sender, RoutedEventArgs e)
        {
            string address = this.text_address.Text;
            string port = this.text_port.Text;
            string database = this.text_database.Text;
            string username = this.text_username.Text;
            string password = this.text_password.Text;
            logger.Info("connectMysql - ip=" + address + ":" + port + ", database=" + database + ", user=" + username + ", password=" + password);
            int p = 0;
            if ("".Equals(address) || "".Equals(port) || "".Equals(database) || "".Equals(username) || "".Equals(password))
            {
                MessageBox.Show(this, "不能有空内容!");
            }
            else if (!Int32.TryParse(port, out p))
            {
                MessageBox.Show(this, "端口号格式不正确!");
            }
            try
            {
                this.databaseService.ConnectMySql(address, Int32.Parse(port), database, username, password);
            }
            catch (Exception e1)
            {
                this.label_mysql.Content = "连接失败";
                this.addLog("连接失败" + e1.Message);
            }
            if(this.databaseService.IsConnect())
            {
                this.label_mysql.Content = "连接成功 - ip=" + address + ":" + port + ", database=" + database + ", user=" + username + ", password=" + password;
                this.addLog("连接成功 - ip=" + address + ":" + port + ", database=" + database + ", user=" + username + ", password=" + password);
            }
            else
            {
                this.label_mysql.Content = "连接失败";
                this.addLog("连接失败");
            }
        }

        private void openExcel_clickHandler(object sender, RoutedEventArgs e)
        {
            this.logList.Clear();
            this.sqlList.Clear();
            this.delSqlList.Clear();
            System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.Filter = "Excel文件(*.xls,*.xlsx)|*.xls;*.xlsx;";
            fileDialog.CheckFileExists = false;
            fileDialog.CheckPathExists = true;
            fileDialog.ValidateNames = false;
            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.filePath = fileDialog.FileName;
                this.label_filePath.Content = this.filePath;
                this.addLog("###打开文件### - " + this.filePath);
            }
        }

        private void readExcel_clickHandler(object sender, RoutedEventArgs e)
        {
            this.logList.Clear();
            this.sqlList.Clear();
            this.delSqlList.Clear();
            Thread thread = new Thread(new ThreadStart(this.readFile));
            thread.Start();
        }

        private void saveData_clickHandler(object sender, RoutedEventArgs e)
        {
            if (this.databaseService.IsConnect())
            {
                this.isDelete = this.check_isdelete.IsChecked == true;
                this.addLog("###开始保存数据### - 到数据库, 请稍后... - 是否删除已有数据：" + this.isDelete);
                Thread thread = new Thread(new ThreadStart(this.saveData));
                thread.Start();
            }
            else
            {
                MessageBox.Show(this, "没有连接数据库。");
            }
        }

        #endregion


        #region 数据操作

        public void readFile()
        {
            this.addLog("###开始解析文件###");
            string connectionString = "Provider=Microsoft.Ace.Oledb.12.0;Data Source=" + this.filePath + ";Extended Properties=\"Excel 12.0;HDR=No;IMEX=1;\"";
            List<string> tableList = this.excelService.GetTableList(connectionString);
            foreach (String tableName in tableList)
            {
                //this.addLog("-----------------表名：" + tableName);
                try
                {
                    string delSql = "delete from `" + tableName + "`;";
                    this.delSqlList.Add(delSql);
                    List<string> fields = this.excelService.GetFieldList(connectionString, tableName);
                    List<List<string>> values = this.excelService.GetValueList(connectionString, tableName);
                    List<string> sqls = this.excelService.GetSqlList(tableName, fields, values);
                    this.addLog("$表 - tableName=" + tableName + ", fields=" + fields.Count + ", values=" + values.Count + ", sqls=" + sqls.Count);
                    this.sqlList.AddRange(sqls);
                }
                catch(Exception e){
                    this.addLog("--解析文件 - error - 表 - tableName=" + tableName + "----------------" + e.Message);
                }
            }
            this.addLog("--解析完毕，共" + this.sqlList.Count + "条数据.");
        }

        public void saveData()
        {
            int count = 0;//sql执行成功的条数
            int total = 0;//执行一共处理总数
            List<string> errorList = new List<string>();

            // 删除已有数据
            if (this.isDelete)
            {
                foreach (string sql in this.delSqlList)
                {
                    string error = "";
                    this.databaseService.ExecuteSql(sql, ref count, ref total, ref errorList, ref error);
                    this.addLog("///" + error);
                }
                this.addLog("###清空完成### - 成功：" + total + ", 错误：" + errorList.Count + ", sql=" + count + ".");
                foreach (string sql in errorList)
                {
                    this.addLog("--出错的sql - [" + sql + "]");
                }
            }
            count = 0;
            errorList.Clear();
            this.addLog("--开始保存数据，共" + this.sqlList.Count + "条.");

            foreach (string sql in this.sqlList)
            {
                string error = "";
                this.databaseService.ExecuteSql(sql, ref count, ref total, ref errorList, ref error);
                if(error != null && !"".Equals(error))
                {
                    this.addLog("///" + error);
                }
                else
                {
                    this.updateLog("------------" + count);
                }
            }
            this.addLog("###保存数据完成### - 成功：" + count + ", 错误：" + errorList.Count + ".");
            foreach (string sql in errorList)
            {
                this.addLog("--出错的sql - [" + sql + "]");
            }
        }

        #endregion


        #region 数据检测

        private void button_1_1_clickHandler(object sender, RoutedEventArgs e)
        {
            int count = 0;
            string error = "";
            this.databaseService.ExecuteFileSql(@"Y:\workspace_private_shell\数据转换\导入使用sql\1.1-shell_table_excel.sql", ref count, ref error);
        }

        private void checkGoods_clickHandler(object sender, RoutedEventArgs e){
            //this.checkService.CheckGoods();
        }

        #endregion


        #region 日志操作

        public void addLog(string value)
        {
            logger.Info(value);
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                this.logList.Add(value);
                this.scroll_log.ScrollToEnd();
            }));
        }

        public void updateLog(string value)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                this.logList.RemoveAt(this.logList.Count - 1);
                this.logList.Add(value);
            }));
        }

        #endregion


        #region 关闭程序

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
            Environment.Exit(0);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
            Environment.Exit(0);
        }

        #endregion
   
    }
}
