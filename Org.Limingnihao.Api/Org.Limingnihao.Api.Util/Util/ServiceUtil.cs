using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace Org.Limingnihao.Api.Util.Util
{
    public class ServiceUtil
    {

        #region 安装服务

        /// <summary>
        /// 安装windows服务
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <param name="savedState">它用于保存执行提交、回滚或卸载操作所需的信息。</param>
        /// <param name="filepath">获取或设置要安装的程序集的路径。</param>
        public static void InstallService(String serviceName, IDictionary savedState, string filepath)
        {
            ServiceController service = new ServiceController(serviceName);
            if (!ServiceIsExisted(serviceName))
            {
                AssemblyInstaller myAssemblyInstaller = new AssemblyInstaller();
                myAssemblyInstaller.UseNewContext = true;
                myAssemblyInstaller.Path = filepath;
                myAssemblyInstaller.Install(savedState);
                myAssemblyInstaller.Commit(savedState);
                myAssemblyInstaller.Dispose();
                service.Start();
            }
            else
            {
                if (service.Status != ServiceControllerStatus.Running && service.Status != ServiceControllerStatus.StartPending)
                {
                    service.Start();
                }
            }
        }

        #endregion


        #region 卸载服务

        /// <summary>
        /// 卸载windows服务
        /// </summary>
        /// <param name="filepath">获取或设置要安装的程序集的路径。</param>
        public static void UnInstallService(String serviceName, string filepath)
        {
            if (ServiceIsExisted(serviceName))
            {
                AssemblyInstaller myAssemblyInstaller = new AssemblyInstaller();
                myAssemblyInstaller.UseNewContext = true;
                myAssemblyInstaller.Path = filepath;
                myAssemblyInstaller.Uninstall(null);
                myAssemblyInstaller.Dispose();
            }
        }

        #endregion


        #region 判断服务是否存在

        public static bool ServiceIsExisted(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController s in services)
            {
                //System.Console.WriteLine("ServiceName=" + s.ServiceName + ", Status=" + s.Status);
                if (s.ServiceName.ToLowerInvariant().Equals(serviceName.ToLowerInvariant()))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion


        #region 判断服务是否启动

        /// <summary>
        /// 判断某个Windows服务是否启动
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static bool IsServiceStart(string serviceName)
        {
            ServiceController psc = new ServiceController(serviceName);
            bool bStartStatus = false;
            try
            {
                if (!psc.Status.Equals(ServiceControllerStatus.Stopped))
                {
                    bStartStatus = true;
                }
                return bStartStatus;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion


        #region 启动服务

        /// <summary>
        /// 启动现有服务
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static bool StartService(string serviceName)
        {
            if (ServiceIsExisted(serviceName))
            {
                ServiceController service = new ServiceController(serviceName);
                if (service.Status != ServiceControllerStatus.Running && service.Status != ServiceControllerStatus.StartPending)
                {
                    service.Start();
                    for (int i = 0; i < 60; i++)
                    {
                        System.Threading.Thread.Sleep(1000);
                        service.Refresh();
                        System.Console.WriteLine("StartService - i=" + i + ", Status=" + service.Status);
                        if (service.Status == System.ServiceProcess.ServiceControllerStatus.Running)
                        {
                            return true;
                        }
                        if (i == 59)
                        {
                            return false;
                        }
                    }
                }
            }
            return false;
        }

        #endregion


        #region 停止服务

        /// <summary>
        /// 停止现有服务器
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static bool StopService(string serviceName)
        {
            bool flag = true;
            if (ServiceIsExisted(serviceName))
            {
                ServiceController service = new ServiceController(serviceName);
                if (service.Status == ServiceControllerStatus.Running)
                {
                    service.Stop();
                    for (int i = 0; i < 60; i++)
                    {
                        service.Refresh();
                        Thread.Sleep(1000);
                        if (service.Status == ServiceControllerStatus.Stopped)
                        {
                            break;
                        }
                        if (i == 59)
                        {
                            flag = false;
                        }
                    }
                }
            }
            return flag;
        }
        
        #endregion

    }
}
