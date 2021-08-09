using BaseCore.Utils;
using ReleaseSystem.bean;
using ReleaseSystem.ui;
using ReleaseSystem.utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ReleaseSystem
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WMain : Window
    {
        public WMain()
        {
            InitializeComponent();

            // 设置全屏

            //  this.WindowStyle = System.Windows.WindowStyle.None;
            //   this.ResizeMode = System.Windows.ResizeMode.NoResize;
            //this.Topmost = true;

            //   this.Left = 0.0;
            //   this.Top = 0.0;
            this.Width = SystemParameters.WorkArea.Size.Width;
            this.Height = SystemParameters.WorkArea.Size.Height;



        }

        public List<Socket> ClientProxSocketList = new List<Socket>();
        bool exit;

        private void main_Loaded(object sender, RoutedEventArgs e)
        {
            //frame.Navigate(new Uri("/pages/PageMaker.xaml", UriKind.Relative));
            exit = false;

            StartReceive();
        }


        /// <summary>
        /// 用于UDP发送的网络服务类
        /// </summary>
        static UdpClient udpcRecv = null;

        static IPEndPoint localIpep = null;

        /// <summary>
        /// 开关：在监听UDP报文阶段为true，否则为false
        /// </summary>
        static bool IsUdpcRecvStart = false;
        /// <summary>
        /// 线程：不断监听UDP报文
        /// </summary>
        static Thread thrRecv;

        public static void StartReceive()
        {
            if (!IsUdpcRecvStart) // 未监听的情况，开始监听
            {
                localIpep = new IPEndPoint(IPAddress.Parse(DeviceUtil.Computer.GetIPAddress()), 8282); // 本机IP和监听端口号
                udpcRecv = new UdpClient(localIpep);
                thrRecv = new Thread(ReceiveMessage);
                thrRecv.Start();
                IsUdpcRecvStart = true;
                Console.WriteLine("UDP监听器已成功启动");
            }
        }

        public static void StopReceive()
        {
            if (IsUdpcRecvStart)
            {
                thrRecv.Abort(); // 必须先关闭这个线程，否则会异常
                udpcRecv.Close();
                IsUdpcRecvStart = false;
                Console.WriteLine("UDP监听器已成功关闭");
            }
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="obj"></param>
        private static void ReceiveMessage(object obj)
        {
            while (IsUdpcRecvStart)
            {
                try
                {
                    byte[] bytRecv = udpcRecv.Receive(ref localIpep);
                    string message = Encoding.UTF8.GetString(bytRecv, 0, bytRecv.Length);
                    Console.WriteLine(string.Format("{0}[{1}]", localIpep, message));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    break;
                }
            }
        }


        private void Main_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            exit = true;

        }

        private void frame_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine("12312312312");
            if (tabMaker.IsSelected)
            {
                Frame fr = new Frame();
                fr.Source = new Uri("pages/PageMaker.xaml", UriKind.Relative);
                tabMaker.Content = fr;
            }
            
        }

        private void tabMaker_MouseDown(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void tabDevice_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/control.txt"))
            {
                ShowUtils.showTips(false, "当前页面未保存！是否丢弃？");
                tabDevice.IsSelected = false;
                tabMaker.IsSelected = true;
            }
            else
            {
                Frame fr = new Frame();
                fr.Source = new Uri("pages/PageDeviceManage.xaml", UriKind.Relative);
                tabDevice.Content = fr;
            }

        }
    }
}
