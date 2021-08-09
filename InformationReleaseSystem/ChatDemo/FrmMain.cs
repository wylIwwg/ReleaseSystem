using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatDemo
{
    public partial class FrmMain : Form
    {

        Dictionary<string, Socket> ClientProxSocketList = new Dictionary<string, Socket>();
        Socket socket;

        public FrmMain()
        {
            InitializeComponent();
            this.FormClosing += MainFrm_FormClosing;

            cbClient.SelectedValueChanged += CbClient_SelectedValueChanged;
        }

        private void CbClient_SelectedValueChanged(object sender, EventArgs e)
        {
        }

        #region 窗体关闭的时候通知客户端退出
        void MainFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var socket in ClientProxSocketList.Values)
            {
                StopConnetct(socket);
            }
        }
        #endregion

        #region 启动服务

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (serverSocket != null && serverSocket.IsBound)
            {
                servered = false;
                foreach (var proxSocket in ClientProxSocketList.Values)
                {
                    StopConnetct(proxSocket);
                }
                serverSocket.Close();
                serverSocket = null;
                btnStart.Text = "开启server";
            }
            else
            {
                //1 创建Socket
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                //2 绑定端口ip
                socket.Bind(new IPEndPoint(IPAddress.Parse(txtIP.Text), int.Parse(txtPort.Text)));

                //3 开启侦听
                socket.Listen(10);// 等待链接的队列：同时来了100链接请求，只能处理一个链接，队列里面放10个等待链接的客户端，其他的返回错误消息。
                servered = true;
                //4 开始接受客户端的链接
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.AcceptClientConnect), socket);
                btnStart.Text = "关闭server";
            }




        }

        Socket serverSocket;
        bool servered = false;

        #endregion

        #region 接受客户端连接
        public void AcceptClientConnect(object socket)
        {
            serverSocket = socket as Socket;

            this.AppendTextToTxtLog("服务器端开始接受客户端的链接。");

            while (servered)
            {

                try
                {
                    var proxSocket = serverSocket.Accept();
                    this.AppendTextToTxtLog(string.Format("客户端：{0} 链接上了", proxSocket.RemoteEndPoint.ToString()));

                    sendData(proxSocket, "{\"type\":\"init\"}");

                    ClientProxSocketList.Add(proxSocket.RemoteEndPoint.ToString(), proxSocket);

                    //不停的接受当前链接的客户端发送来的消息
                    //proxSocket.Receive()
                    ThreadPool.QueueUserWorkItem(ReceiveData, proxSocket);
                    refreshList();
                }
                catch (Exception error)
                {
                    Console.WriteLine(error);

                }


            }
        }
        #endregion

        #region 接受客户端的消息

        //接受客户端的消息
        public void ReceiveData(object socket)
        {
            var proxSocket = socket as Socket;
            byte[] data = new byte[1024 * 1024];
            while (true)
            {
                Thread.Sleep(100);
                if (!proxSocket.Connected)
                {  //异常退出
                    AppendTextToTxtLog(string.Format("客户端未连接"));
                    return;
                }
                int len = 0;
                try
                {
                    len = proxSocket.Receive(data, 0, data.Length, SocketFlags.None);
                }
                catch (Exception ex)
                {
                    //异常退出
                    AppendTextToTxtLog(string.Format("客户端：{0}非正常退出", ex.Message));

                    /*   if (ClientProxSocketList.ContainsKey(proxSocket.RemoteEndPoint.ToString()))
                       {
                           ClientProxSocketList.Remove(proxSocket.RemoteEndPoint.ToString());
                           StopConnetct(proxSocket);
                       }*/

                    return;
                }

                if (len <= 0)
                {
                    //客户端正常退出
                    AppendTextToTxtLog(string.Format("客户端：{0}正常退出", proxSocket.RemoteEndPoint.ToString()));
                    if (ClientProxSocketList.ContainsKey(proxSocket.RemoteEndPoint.ToString()))
                    {
                        ClientProxSocketList.Remove(proxSocket.RemoteEndPoint.ToString());
                        refreshList();
                        StopConnetct(proxSocket);
                    }
                    return;//让方法结束，终结当前接受客户端数据的异步线程。
                }

                //把接收到的数据放到文本框上去
                string str = Encoding.UTF8.GetString(data, 0, len);
                if (str.Contains("ping"))
                {
                    sendData(proxSocket, "{\"type\":\"pong\"}");
                }
                AppendTextToTxtLog(string.Format("接收到客户端：{0}的消息是：{1}", proxSocket.RemoteEndPoint.ToString(), str));
            }
        }
        #endregion

        #region 断开连接
        private void StopConnetct(Socket proxSocket)
        {
            try
            {
                if (proxSocket.Connected)
                {
                    proxSocket.Shutdown(SocketShutdown.Both);
                    proxSocket.Close(0);
                    proxSocket = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("【StopConnetct】" + ex.ToString());
            }
        }
        #endregion

        #region 往日志的文本框上追加数据

        //往日志的文本框上追加数据
        public void AppendTextToTxtLog(string txt)
        {

            //Action<string> sdel = a => { };

            //sdel =new Action<string>(a => { });

            ////sdel+=


            //sdel("sss");
            //sdel.Invoke("sss");

            //sdel.BeginInvoke("sss", null, null);

            if (txtLog.InvokeRequired)
            {
                txtLog.BeginInvoke(new Action<string>(s =>
                {
                    this.txtLog.Text = string.Format("{0}\r\n{1}", s, txtLog.Text);
                }), txt);

                //同步方法。
                //txtLog.Invoke(new Action<string>(s =>
                //{
                //    this.txtLog.Text = string.Format("{0}\r\n{1}", s, txtLog.Text);
                //}), txt);
            }
            else
            {
                this.txtLog.Text = string.Format("{0}\r\n{1}", txt, txtLog.Text);
            }

        }
        #endregion

        #region 发送字符串
        //发送消息
        private void btnSendMsg_Click(object sender, EventArgs e)
        {
            foreach (var proxSocket in ClientProxSocketList.Values)
            {
                if (proxSocket.Connected)
                {
                    sendData(proxSocket, txtMsg.Text);

                    //  sendData(proxSocket, txtMsg.Text);
                }
            }
        }

        public void sendData(Socket socket, string txt)
        {
            //原始的字符串转成的字节数组。
            // byte[] data = Encoding.Default.GetBytes(txtMsg.Text);
            byte[] data = Encoding.UTF8.GetBytes(txt + "\n");

            //对原始的数据数组加上协议的头部字节。
            byte[] result = new byte[data.Length + 10];
            //设置当前的协议头部字节 是1： 1代表字符串
            result[0] = 1;
            //把原始的数据放到 最终的的字节数组里去
            char[] len = data.Length.ToString().ToCharArray();//获取长度字符串

            for (int i = 0; i < len.Length; i++)
            {
                result[i + 1] = (byte)len[i];
            }

            Buffer.BlockCopy(data, 0, result, 10, data.Length);
            Console.WriteLine(result.Length);
            socket.Send(data, 0, data.Length, SocketFlags.None);

        }

        #endregion

        #region 发送闪屏
        private void btnSendShake_Click(object sender, EventArgs e)
        {
            //把窗体最原始的坐标记住。
            Point oldLocation = this.Location;
            Random r = new Random();

            for (int i = 0; i < 20; i++)
            {
                this.Location = new Point(r.Next(oldLocation.X - 3, oldLocation.X + 3),
                    r.Next(oldLocation.Y - 3, oldLocation.Y + 3)
                    );
                Thread.Sleep(20);
                this.Location = oldLocation;

            }
        }
        #endregion

        #region 发送文件
        private void btnSendFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                byte[] data = File.ReadAllBytes(ofd.FileName);
                byte[] result = new byte[data.Length + 10];
                result[0] = 3;

                char[] len = data.Length.ToString().ToCharArray();//获取长度字符串

                for (int i = 0; i < len.Length; i++)
                {
                    result[i + 1] = (byte)len[i];
                }


                Buffer.BlockCopy(data, 0, result, 10, data.Length);

                Console.WriteLine(result.Length);

                foreach (var proxSocket in ClientProxSocketList.Values)
                {
                    if (!proxSocket.Connected)
                    {
                        continue;
                    }
                    //把要发送的文件读取来。
                    proxSocket.Send(result, SocketFlags.None);
                }
            }


        }
        #endregion

        private void btmHtml_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "网页文件(*.html)|*.html";
                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                //  byte[] data = File.ReadAllBytes(ofd.FileName);
                string content = File.ReadAllText(ofd.FileName, Encoding.UTF8);
                Console.WriteLine(content);

                byte[] data = Encoding.UTF8.GetBytes(content);

                byte[] result = new byte[data.Length + 10];
                result[0] = 4;

                char[] len = data.Length.ToString().ToCharArray();//获取长度字符串

                for (int i = 0; i < len.Length; i++)
                {
                    result[i + 1] = (byte)len[i];
                }


                Buffer.BlockCopy(data, 0, result, 10, data.Length);

                Console.WriteLine(result.Length);

                foreach (var proxSocket in ClientProxSocketList.Values)
                {
                    if (!proxSocket.Connected)
                    {
                        continue;
                    }
                    //把要发送的文件读取来。
                    proxSocket.Send(result, SocketFlags.None);
                }
            }
        }

        private void BtnSendZip_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "压缩文件(*.zip)|*.zip";
                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                byte[] data = File.ReadAllBytes(ofd.FileName);
                // string content = File.ReadAllText(ofd.FileName, Encoding.UTF8);

                //  byte[] data = Encoding.UTF8.GetBytes(content);

                byte[] result = new byte[data.Length + 10];
                result[0] = 3;

                char[] len = data.Length.ToString().ToCharArray();//获取长度字符串

                for (int i = 0; i < len.Length; i++)
                {
                    result[i + 1] = (byte)len[i];
                }


                Buffer.BlockCopy(data, 0, result, 10, data.Length);

                Console.WriteLine(result.Length);

                foreach (var proxSocket in ClientProxSocketList.Values)
                {
                    if (!proxSocket.Connected)
                    {
                        continue;
                    }
                    //把要发送的文件读取来。
                    proxSocket.Send(result, SocketFlags.None);
                }
            }
        }

        private void btnSendMsg2_Click(object sender, EventArgs e)
        {
            foreach (var proxSocket in ClientProxSocketList.Values)
            {
                if (proxSocket.Connected)
                {
                    //原始的字符串转成的字节数组。
                    // byte[] data = Encoding.Default.GetBytes(txtMsg.Text);
                    byte[] data = Encoding.UTF8.GetBytes(txtMsg.Text);


                    proxSocket.Send(data, 0, data.Length, SocketFlags.None);
                }
            }
        }

        private void TxtMsg_TextChanged(object sender, EventArgs e)
        {

        }

        void refreshList()
        {
            List<string> clinets = new List<string>();
            clinets.AddRange(ClientProxSocketList.Keys);

            cbClient.Invoke(new Action(() =>
            {
                cbClient.DataSource = null;
                cbClient.DataSource = clinets;
                if (clinets.Count > 0)
                    cbClient.SelectedIndex = 0;
            }));

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (cbClient.SelectedValue != null)
                if (ClientProxSocketList.ContainsKey(cbClient.SelectedValue.ToString()))
                {
                    socket = ClientProxSocketList[cbClient.SelectedValue.ToString()];
                    ClientProxSocketList.Remove(cbClient.SelectedValue.ToString());
                    StopConnetct(socket);
                    refreshList();

                }

        }
    }
}
