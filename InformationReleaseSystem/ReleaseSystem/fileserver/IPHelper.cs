using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace ReleaseSystem.fileserver
{
    public class IPHelper
    {
        #region Methods

        public static IPAddress[] GetAllLocalIP()
        {
            var lst = new List<IPAddress>();
            try
            {
                string HostName = Dns.GetHostName(); //得到主机名
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                foreach (var ip in IpEntry.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        lst.Add(ip);
                    }
                }
            }
            catch (Exception)
            {
            }
            return lst.ToArray();
        }

        public static string GetLocalIP()
        {
            try
            {
                string HostName = Dns.GetHostName(); //得到主机名
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        return IpEntry.AddressList[i].ToString();
                    }
                }
                return "127.0.0.1";
            }
            catch (Exception)
            {
                return "127.0.0.1";
            }
        }

        #endregion Methods
    }
}