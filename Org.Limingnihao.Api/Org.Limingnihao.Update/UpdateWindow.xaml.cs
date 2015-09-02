using Common.Logging;
using MySql.Data.MySqlClient;
using Org.Limingnihao.Api.Util;
using Org.Limingnihao.Update.Help;
using Org.Limingnihao.Update.Service.Impl;
using Schematrix;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Org.Limingnihao.Update
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UpdateWindow : Window
    {
        private static readonly ILog logger = LogManager.GetLogger("UpdateWindow");
        private ConfigServiceImpl configService = ConfigServiceImpl.GetInstance();
        private FileDownloadDelegate downDelegate = null;
        private RoutedEventHandler buttonEventHandler = null;

        private int fileCount = 0;
        private int fileIndex = 0;
        private bool isDeleteFile = true;

        public UpdateWindow()
        {
            InitializeComponent();
            LogUtil.FileTitle = "update";
            LogUtil.IS_FILE = true;
            LogUtil.LEVEL = LogUtil.L_INFO;
            logger.Info("----------------------start---------------------------");

            this.downDelegate = new FileDownloadDelegate(this.downloadingHandler);
            this.configService.InitUpdateSetting();

            //ConfigHelp.FileDownPath = "d://a.rar";
            //ConfigHelp.FileUpdatePath = "d://update/";
            //this.extractFile();
        }

        private void window_loadedHandler(object sender, RoutedEventArgs e)
        {
            logger.Info("window_loadedHandler");
            //if (!this.checkRockey())
            //{
            //    this.Close();
            //    return;
            //}
            this.startDownThread();
        }


        #region 加密狗检测

        public bool checkRockey()
        {
            String path = ConfigHelp.CurrentDirectory + "\\Com.Analysis.T4.Rockey.exe";
            logger.Info("checkRockey - " + path);
            Process p = new Process();
            p.StartInfo.FileName = path;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine(@"" + path);
            String result = p.StandardOutput.ReadToEnd();
            logger.Info(result);
            p.Close();
            String[] results = result.Split(',');
            if (results == null || results.Length != 2)
            {
                MessageBox.Show(this, "没有检测到任何加密狗！");
                return false;
            }

            if("1".Equals(results[0])){
                return true;
            }
            else
            {
                MessageBox.Show(this, results[1]);
                return false;
            }
        }

        #endregion

        #region 下载文件

        private void startDownThread()
        {
            logger.Info("startDownThread------------start");
            Thread thread = new Thread(new ThreadStart(this.downHandler));
            thread.Start();
        }

        /// <summary>
        /// 下载线程
        /// </summary>
        private void downHandler()
        {
            try
            {
                logger.Info("downHandler - start");
                this.configService.StopApp();
                ConfigHelp.VersionVO = this.configService.GetNewVersion();
                if (ConfigHelp.VersionVO == null || !ConfigHelp.VersionVO.success || ConfigHelp.VersionVO.versionCode <= ConfigHelp.VersionCode)
                {
                    logger.Info("downHandler - version=" + ConfigHelp.VersionVO);
                    this.setMessage("当前没有可更新的版本！");
                    this.updateLocal();
                    return;
                }
                ConfigHelp.FileDownPath = ConfigHelp.CurrentDirectory + "\\" + ConfigHelp.VersionVO.fileName;
                ConfigHelp.FileUpdatePath = ConfigHelp.CurrentDirectory + "\\" + ConfigHelp.FileUpdataDir;
                this.setVersion("准备升级版本：" + ConfigHelp.VersionVO.versionName);
                FileUtil.DeleteFile(ConfigHelp.FileDownPath);
                FileUtil.DeleteFile(ConfigHelp.FileUpdatePath);
                logger.Info("downHandler - versionName=" + ConfigHelp.VersionVO.versionName + ", versionCode=" + ConfigHelp.VersionVO.versionCode + ", fileName=" + ConfigHelp.VersionVO.fileName + ", fileMd5=" + ConfigHelp.VersionVO.fileMd5 + ", downUrl=" + ConfigHelp.VersionVO.downUrl + ", FileDownPath=" + ConfigHelp.FileDownPath);
                FileDownUtil.DownFileRequest(ConfigHelp.VersionVO.downUrl, ConfigHelp.FileDownPath, this.downDelegate);
                logger.Info("downHandler - over");
                this.isDeleteFile = true;
                this.startCheckThread();
            }
            catch (Exception e)
            {
                this.setMessage("升级包下载失败！");
                this.updateLocal();
                logger.Info("downHandler - e=" + e);
                return;
            }
        }

        /// <summary>
        /// 下载过程回调
        /// </summary>
        private void downloadingHandler(double percent, double second, double speed, string info)
        {
            int value = (int)(percent * 100);
            this.setProcess(value);
            this.setMessage("正在下载更新包，请稍等。" + value + "%");
        }

        #endregion

        #region 校验文件
        /// <summary>
        /// 安装过程。
        /// 1.校验文件的MD5。
        /// 2.解压文件。
        /// 3.覆盖文件。
        /// 4.升级数据库
        /// </summary>
        private void startCheckThread()
        {
            Thread thread = new Thread(new ThreadStart(this.checkHandler));
            thread.Start();
        }
        /// <summary>
        /// 安装线程
        /// </summary>
        private void checkHandler()
        {
            logger.Info("checkHandler - start");
            try
            {
                this.setMessage("正在验证安装包...");
                string md5 = MD5Util.GetFileMD5(ConfigHelp.FileDownPath).ToUpper();
                logger.Info("checkHandler - 下载包md5=" + md5 + ", 服务器MD5=" + ConfigHelp.VersionVO.fileMd5.ToUpper() + " - FileType=" + FileUtil.GetFileType(ConfigHelp.FileDownPath));
                if (ConfigHelp.VersionVO.fileMd5.Equals("123456") || md5.Equals(ConfigHelp.VersionVO.fileMd5.ToUpper()))
                {
                    logger.Info("checkHandler - file=" + ConfigHelp.FileDownPath + ", md5=" + md5);
                    Thread.Sleep(2000);
                    if ("rar".Equals(FileUtil.GetFileType(ConfigHelp.FileDownPath)) || "zip".Equals(FileUtil.GetFileType(ConfigHelp.FileDownPath)))
                    {
                        bool Is64Bit = Environment.Is64BitOperatingSystem;
                        logger.Info("---------------------Is64Bit=" + Is64Bit);
                        this.setMessage("开始解压更新包...");
                        if (Is64Bit)
                        {
                            this.extractFile64();
                        }
                        else
                        {
                            this.extractFile32();
                        }
                        this.setMessage("开始安装更新包...");
                        this.copyUpdate();
                        this.startUpdateMySqlThread();
                        this.setMessage("已成功完成更新，请重新启动程序。");
                        this.updateComplate();
                    }
                    else if ("msi".Equals(FileUtil.GetFileType(ConfigHelp.FileDownPath)))
                    {
                        this.setMessage("开始全新安装...");
                        this.installFile();
                    }
                    else
                    {
                        this.setMessage("当前更新文件不支持，请联系管理员。");
                        this.updateFail();
                    }
                }
                else
                {
                    this.setMessage("更新包校验失败。");
                    this.updateFail();
                }
            }
            catch (Exception e)
            {
                logger.Info("" + e.ToString());
                this.setMessage("更新包校验失败，请尝试重新启动程序。");
                this.updateFail();
            }
            logger.Info("InstallThread - over");
        }

        #endregion

        #region 解压文件

        /// <summary>
        /// 解压文件64
        /// </summary>
        private void extractFile64()
        {
            Unrar64 unrar = new Unrar64();
            unrar.Open(ConfigHelp.FileDownPath, Unrar64.OpenMode.List);
            while (unrar.ReadHeader())
            {
                if (!unrar.CurrentFile.IsDirectory && unrar.CurrentFile.PackedSize != 0 && unrar.CurrentFile.UnpackedSize != 0)
                {
                    fileCount++;
                }
                unrar.Skip();
            }
            unrar.Close();
            logger.Info("extractFile64 - start - fileCount=" + this.fileCount + ", FileDownPath=" + ConfigHelp.FileDownPath);

            unrar.Open(ConfigHelp.FileDownPath, Unrar64.OpenMode.Extract);
            unrar.DestinationPath = ConfigHelp.FileUpdatePath;
            unrar.ExtractionProgress +=(new ExtractionProgressHandler(unrar_ExtractionProgress));
            unrar.MissingVolume += (new MissingVolumeHandler(unrar_MissingVolume));
            unrar.PasswordRequired += (new PasswordRequiredHandler(unrar_PasswordRequired));
            while (unrar.ReadHeader())
            {
                unrar.Extract();
            }
            unrar.Close();
        }

        /// <summary>
        /// 解压文件32
        /// </summary>
        private void extractFile32()
        {
            Unrar32 unrar = new Unrar32();
            unrar.Open(ConfigHelp.FileDownPath, Unrar32.OpenMode.List);
            while (unrar.ReadHeader())
            {
                if (!unrar.CurrentFile.IsDirectory && unrar.CurrentFile.PackedSize != 0 && unrar.CurrentFile.UnpackedSize != 0)
                {
                    fileCount++;
                }
                unrar.Skip();
            }
            unrar.Close();
            logger.Info("extractFile32 - start - fileCount=" + this.fileCount + ", FileDownPath=" + ConfigHelp.FileDownPath);

            unrar.Open(ConfigHelp.FileDownPath, Unrar32.OpenMode.Extract);
            unrar.DestinationPath = ConfigHelp.FileUpdatePath;
            unrar.ExtractionProgress += (new ExtractionProgressHandler(unrar_ExtractionProgress));
            unrar.MissingVolume += (new MissingVolumeHandler(unrar_MissingVolume));
            unrar.PasswordRequired += (new PasswordRequiredHandler(unrar_PasswordRequired));
            while (unrar.ReadHeader())
            {
                unrar.Extract();
            }
            unrar.Close();
        }
        private void unrar_ExtractionProgress(object sender, ExtractionProgressEventArgs e)
        {
            if (e.PercentComplete == 100)
            {
                this.fileIndex++;
                int value = (int)(this.fileIndex * 100 / (this.fileCount * 1.0));
                this.setProcess(value);
                this.setMessage("正在解压文件。（第" + this.fileIndex + "个,共" + this.fileCount + "个)。" + value + "%");
                logger.Info("ExtractionProgress - fileName=" + e.FileName + ", PercentComplete=" + e.PercentComplete + "。（第" + this.fileIndex + "个,共" + this.fileCount + "个), value=" + value);
            }
        }

        private void unrar_MissingVolume(object sender, MissingVolumeEventArgs e)
        {
            logger.Info("MissingVolume - " + e.VolumeName + " - Volume is missing.  Correct or cancel");
        }

        private void unrar_PasswordRequired(object sender, PasswordRequiredEventArgs e)
        {
            logger.Info("PasswordRequired - ");
        }

        #endregion

        #region 复制文件

        /// <summary>
        /// 复制替换文件
        /// </summary>
        private void copyUpdate()
        {
            logger.Info("setupUpdate - start - path=" + ConfigHelp.FileUpdatePath);
            this.fileIndex = 0;
            this.copyDirectory(ConfigHelp.FileUpdatePath);
            logger.Info("setupUpdate - end");
        }

        private void copyDirectory(string path)
        {
            logger.Info("copyDirectory - path=" + path);
            if (!Directory.Exists(path))
            {
                logger.Info("copyDirectory - 目录不存在 - path=" + path);
                this.setMessage("更新新文件出现错误，请尝试重新启动程序！");
                return;
            }
            DirectoryInfo dir = new DirectoryInfo(path);
            foreach (FileInfo f in dir.GetFiles())
            {
                string newFile = f.FullName.Replace(ConfigHelp.FileUpdatePath, "");
                string oldFile = ConfigHelp.CurrentDirectory + newFile;
                if (File.Exists(oldFile))
                {
                    FileUtil.DeleteFile(oldFile);
                }
                FileInfo newFileInfo = new FileInfo(ConfigHelp.CurrentDirectory + newFile);
                if (!newFileInfo.Directory.Exists)
                {
                    newFileInfo.Directory.Create();
                    newFileInfo = null;
                }
                f.CopyTo(ConfigHelp.CurrentDirectory + newFile);

                this.fileIndex++;
                int value = (int)(this.fileIndex * 100 / (this.fileCount * 1.0));
                this.setProcess(value);
                this.setMessage("正在安装文件。（第" + this.fileIndex + "个,共" + this.fileCount + "个)。" + value + "%");
                logger.Info("copyDirectory - file=" + newFile + "。（第" + this.fileIndex + "个,共" + this.fileCount + "个), value=" + value);
            }
            foreach (DirectoryInfo d in dir.GetDirectories())
            {
                this.copyDirectory(d.FullName);
            }
        }

        #endregion

        #region 安装MSI
        /// <summary>
        /// 安装msi文件
        /// </summary>
        private void installFile()
        {
            string filepath = ConfigHelp.FileDownPath;
            logger.Info("installFile - filepath=" + filepath);
            try
            {
                this.setMessage("已成功完成更新，请关闭程序进行全新安装。");
                this.installMsi();
                logger.Info("installFile - end");
            }
            catch (Exception e)
            {
                logger.Info("installFile - e=" + e.ToString());
            }
        }

        #endregion

        #region 更新数据库

        private void startUpdateMySqlThread()
        {
            Thread thread = new Thread(new ThreadStart(this.updateMySql));
            thread.Start();
        }

        private void updateMySql()
        {
            string path = ConfigHelp.CurrentDirectory + "\\sql";
            logger.Info("updateMySql - path=" + path + " - " + Directory.Exists(path) + ", ConnectionString=" + ConfigHelp.ConnectionString);
            if (!Directory.Exists(path))
            {
                logger.Info("updateMySql - path is not Directory - path=" + path);
                return;
            }
            DirectoryInfo dir = new DirectoryInfo(path);
            if (dir.GetFiles().Length <= 0)
            {
                logger.Info("updateMySql - Directory not have file - path=" + path);
                return;
            }
            MySqlConnection conn = null;
            try
            {
                conn = this.configService.ConnectMySql(ConfigHelp.ConnectionString);
            }
            catch(Exception e)
            {
                logger.Info("ConnectMySql error - " + e.Message);
            }
            if (conn == null)
            {
                logger.Info("updateMySql - MySqlConnection is null - ConnectionString=" + ConfigHelp.ConnectionString);
                this.setMessage("更新数据库出错，请检查数据库是否已经启动！");
                return;
            }
            int count = 0;
            int error  = 0;
            foreach (FileInfo f in dir.GetFiles())
            {
                logger.Info("updateMySql - 1. - fileName=" + f.FullName + ", " + f.Extension + ", " + f.Length);
                if (f.Extension.Equals(".sql"))
                {
                    StreamReader fs = new StreamReader(f.FullName, System.Text.Encoding.UTF8);
                    while (!fs.EndOfStream)
                    {
                        try
                        {
                            string sql = fs.ReadLine();
                            this.configService.UpdateSql(conn, sql);
                            count++;
                        }
                        catch (Exception e)
                        {
                            error++;
                            logger.Info("updateMySql - 3. running error - count=" + count + ", " + e.Message);
                        }
                        int value = (int)((count + error) % 100);
                        this.setProcess(value);
                        this.setMessage("正在更新数据库，请稍等。第" + (count + error) + "条。");
                    }
                    fs.Close();
                    fs.Dispose();
                    File.Delete(f.FullName);
                }
            }
            logger.Info("updateMySql - 4. over - count=" + count + ", error=" + error);
            this.setProcess(100);
            this.setMessage("数据库更新完成, 共" + count + "条。");
        }

        #endregion

        #region 界面更新

        private void setMessage(string message)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                this.label_message.Text = message;
            }));
        }

        private void setVersion(string message)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                this.label_version.Content = message;
            }));
        }

        private void setProcess(int percent)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                this.progressBar.Value = percent;
            }));
        }

        #endregion

        #region 结果处理

        /// <summary>
        /// 成功完成后
        /// </summary>
        private void updateComplate()
        {
            try
            {
                logger.Info("updateComplate - start - isDeleteFile=" + this.isDeleteFile);
                if (this.isDeleteFile)
                {
                    logger.Info("updateComplate - delete down path - " + FileUtil.DeleteFile(ConfigHelp.FileDownPath) + ", " + ConfigHelp.FileDownPath);
                    logger.Info("updateComplate - delete update path - " + FileUtil.DeleteFile(ConfigHelp.FileUpdatePath) + ", " + ConfigHelp.FileUpdatePath);
                }
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    this.button_confirm.Content = "启动程序";
                    if (this.buttonEventHandler != null)
                    {
                        this.button_confirm.RemoveHandler(Button.ClickEvent, this.buttonEventHandler);
                    }
                    this.buttonEventHandler = new RoutedEventHandler(this.button_startApp_ClickHandler);
                    this.button_confirm.AddHandler(Button.ClickEvent, this.buttonEventHandler);
                    if (ConfigHelp.VersionVO != null)
                    {
                        this.configService.SetVersionCodeName(ConfigHelp.VersionVO.versionCode.ToString(), ConfigHelp.VersionVO.versionName);
                    }
                    logger.Info("updateComplate - end");
                }));
            }
            catch(Exception ex)
            {
                logger.Info("updateComplate - ex=" + ex.ToString());
            }
        }

        /// <summary>
        /// 升级失败
        /// </summary>
        private void updateFail()
        {
            logger.Info("updateFail - start - isDeleteFile=" + this.isDeleteFile);
            if (this.isDeleteFile)
            {
                logger.Info("updateFail - delete down path - " + FileUtil.DeleteFile(ConfigHelp.FileDownPath) + ", " + ConfigHelp.FileDownPath);
                logger.Info("updateFail - delete update path - " + FileUtil.DeleteFile(ConfigHelp.FileUpdatePath) + ", " + ConfigHelp.FileUpdatePath);
            }
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                this.button_confirm.Content = "关闭";
                if (this.buttonEventHandler != null)
                {
                    this.button_confirm.RemoveHandler(Button.ClickEvent, this.buttonEventHandler);
                }
                this.buttonEventHandler = new RoutedEventHandler(this.button_startApp_ClickHandler);
                this.button_confirm.AddHandler(Button.ClickEvent, this.buttonEventHandler);
                logger.Info("updateFail - end");
            }));
        }

        /// <summary>
        /// 本地升级
        /// </summary>
        private void updateLocal()
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                this.button_confirm.Content = "本地升级";
                if (this.buttonEventHandler != null)
                {
                    this.button_confirm.RemoveHandler(Button.ClickEvent, this.buttonEventHandler);
                }
                this.buttonEventHandler = new RoutedEventHandler(this.button_openFile_clickHandler);
                this.button_confirm.AddHandler(Button.ClickEvent, this.buttonEventHandler);
                logger.Info("updateLocal - end");
            }));
        }

        /// <summary>
        /// 安装包msi
        /// </summary>
        private void installMsi()
        {
            logger.Info("installMsi - start");
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                this.button_confirm.Content = "安装";
                if (this.buttonEventHandler != null)
                {
                    this.button_confirm.RemoveHandler(Button.ClickEvent, this.buttonEventHandler);
                }
                this.buttonEventHandler = new RoutedEventHandler(this.button_msiFile_clickHandler);
                this.button_confirm.AddHandler(Button.ClickEvent, this.buttonEventHandler);
                this.configService.SetVersionCodeName(ConfigHelp.VersionVO.versionCode.ToString(), ConfigHelp.VersionVO.versionName);
                logger.Info("installMsi - end");
            }));
        }

        private void button_openFile_clickHandler(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.Filter = "升级压缩文件(*.rar,*.zip)|*.rar;*.zip;*.RAR;*.ZIP";
            fileDialog.CheckFileExists = false;
            fileDialog.CheckPathExists = true;
            fileDialog.ValidateNames = false;
            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                
                ConfigHelp.FileDownPath = fileDialog.FileName;
                logger.Info("FileDownPath=" + ConfigHelp.FileDownPath);
                ConfigHelp.VersionVO = new Service.Model.IVersionVO();
                ConfigHelp.VersionVO.fileMd5 = "123456";
                this.isDeleteFile = false;
                this.startCheckThread();
            }
        }

        private void button_msiFile_clickHandler(object sender, RoutedEventArgs e)
        {
            Thread.Sleep(1000);
            ProcessUtil.StartProcess(ConfigHelp.FileDownPath);
            Application.Current.Shutdown();
            Environment.Exit(0);
        }

        private void button_startApp_ClickHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                Thread.Sleep(1000);
                this.configService.StartApp();
                Application.Current.Shutdown();
                Environment.Exit(0);
            }
            catch(Exception exc)
            {
                logger.Info("button_startApp_ClickHandler - " + exc.ToString());
            }
        }

        private void button_close_ClickHandler(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("是否确认关闭升级程序?", "退出更新", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                Application.Current.Shutdown();
                Environment.Exit(0);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            logger.Info("OnClosed");
            Application.Current.Shutdown();
            Environment.Exit(0);
        }

        #endregion

    }
}
