using ReleaseSystem.bean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ReleaseSystem.pages
{
    /// <summary>
    /// PageDeviceManage.xaml 的交互逻辑
    /// </summary>
    public partial class PageDeviceManage : Page
    {
        public PageDeviceManage()
        {
            InitializeComponent();
        }


        private void pageDeviceManage_Loaded(object sender, RoutedEventArgs e)
        {
           /* List<DeviceBean> list = new List<DeviceBean>();

            DeviceBean db1 = new DeviceBean()
            {
                name = "门诊",
                ip = "123",
                IsGrouping = true
            };

            for (int i = 1; i < 3; i++)
            {
                DeviceBean db = new DeviceBean()
                {
                    name = "门诊科室" + i,
                    ip = "192.168.2.10" + i,
                    mac = "E4:FF:54:6A:BC",
                    type = "终端液晶"

                };

                db1.children.Add(db);
            }




            DeviceBean db2 = new DeviceBean();
            db2.name = "药房";
            db2.ip = "456";
            db2.IsGrouping = true;

            for (int i = 1; i < 3; i++)
            {
                DeviceBean db = new DeviceBean()
                {
                    name = "门诊科室" + i,
                    ip = "192.168.2.14" + i,
                    mac = "E4:FF:54:6A:BC",
                    type = "终端液晶"
                };

                db2.children.Add(db);
            }

            list.Add(db1);
            list.Add(db2);

            tree.ItemsSource = list;*/

        }

        private void PageDeviceManage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                WMain w = (WMain)Window.GetWindow(this);

                List<DeviceBean> list = new List<DeviceBean>();
                DeviceBean db2 = new DeviceBean();
                db2.name = "Android";
                db2.IsGrouping = true;

                foreach (Socket socket in w.ClientProxSocketList)
                {
                    // StopConnetct(socket);
                    Console.WriteLine(socket.RemoteEndPoint.ToString());
                    string soc = socket.RemoteEndPoint.ToString();
                    DeviceBean db = new DeviceBean()
                    {
                        name = "Android",
                        ip = soc,
                        mac = "E4:FF:54:6A:BC",
                        type = "终端液晶"
                    };

                    db2.children.Add(db);

                }

                list.Add(db2);
                tree.ItemsSource = null;
                tree.ItemsSource = list;
            }
        }
    }
}
