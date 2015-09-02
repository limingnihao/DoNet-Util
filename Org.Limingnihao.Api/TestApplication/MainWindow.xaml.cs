using Org.Limingnihao.Application.Data;
using Org.Limingnihao.Application.Service.Impl;
using Spring.Context;
using Spring.Context.Support;
using System.Windows;

namespace TestApplication
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //log4net.Config.XmlConfigurator.Configure();
            IApplicationContext context = new XmlApplicationContext("app_dao.xml");
            IApplicationDao service = (IApplicationDao)context.GetObject("ApplicationDaoImpl");
            System.Console.WriteLine("" + service);
            //IList userList = service.GetUserNames();
        }
    }
}
