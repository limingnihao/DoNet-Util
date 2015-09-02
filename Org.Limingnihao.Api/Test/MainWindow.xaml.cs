using Org.Limingnihao.Application.Service;
using Org.Limingnihao.Application.Service.Model;
using Spring.Context;
using Spring.Context.Support;
using System.Collections.Generic;
using System.Windows;

namespace Test
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            IApplicationContext context = new XmlApplicationContext("Config/spring.net.xml");
            IUserService userService = (IUserService)context.GetObject("UserService");
            IGroupService groupService = (IGroupService)context.GetObject("GroupService");
            userService.Login("admin", "123456");
            IList<GroupVO> list = groupService.GetListAll();
            foreach (GroupVO vo in list)
            {
                this.textBox.Text += vo.GroupName + "---";
                System.Console.WriteLine("" + vo.GroupName);
            }

        }
    }
}
