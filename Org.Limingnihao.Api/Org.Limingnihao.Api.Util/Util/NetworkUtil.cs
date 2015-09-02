using Common.Logging;
using Org.Limingnihao.Api.Util.Model;
using System;
using System.Collections.Generic;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;


namespace Org.Limingnihao.Api.Util
{
    /// <summary>
    /// 网络相关功能
    /// </summary>
    public class NetworkUtil
    {
        private static readonly ILog logger = LogManager.GetLogger("NetworkUtil");

        /// <summary>
        /// 确定字符串是否为有效的 IP 地址。
        /// </summary>
        /// <param name="ipString">要验证的字符串。</param>
        /// <param name="address">字符串的 System.Net.IPAddress 版本</param>
        /// <returns>如果 ipString 是有效 IP 地址，则为 true；否则为 false。</returns>
        public static bool TryParse(string ipString, out System.Net.IPAddress address)
        {
            return System.Net.IPAddress.TryParse(ipString, out address);
        }
        public static bool TryParse(string ipString)
        {
            System.Net.IPAddress address;
            return TryParse(ipString, out address);
        }

        /// <summary>
        /// 指示是否有任何可用的网络连接。
        /// </summary>
        /// <returns></returns>
        public static bool IsNetworkLink()
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }

        /// <summary>
        /// 获取当前可用的网络连接列表。
        /// </summary>
        /// <returns></returns>
        public static List<NetworkVO> GetNetworkListEnabled()
        {
            return GetNetworkList(NetworkInterfaceType.Unknown, OperationalStatus.Up);
        }

        /// <summary>
        /// 获取当前可用的网络连接列表。本地连接+wifi
        /// </summary>
        /// <returns></returns>
        public static List<NetworkVO> GetNetworkListEW()
        {
            List<NetworkVO> l1 = GetNetworkList(NetworkInterfaceType.Ethernet, OperationalStatus.Up);
            List<NetworkVO> l2 = GetNetworkList(NetworkInterfaceType.Wireless80211, OperationalStatus.Up);
            l1.AddRange(l2);
            return l1;
        }
        /// <summary>
        /// 获取当前本地连接列表，根据网络接口类型，和操作状态
        /// </summary>
        /// <param name="type">网络接口类型</param>
        /// <param name="ipEnabled">网络接口的操作状态</param>
        public static List<NetworkVO> GetNetworkList(NetworkInterfaceType type, OperationalStatus status)
        {
            List<NetworkVO> list = new List<NetworkVO>();
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {
                //过滤网络接口类型
                if (!NetworkInterfaceType.Unknown.Equals(type) && !type.Equals(adapter.NetworkInterfaceType))
                {
                    logger.Debug("跳过的其他类型网络，name=" + adapter.Name + ", NetworkInterfaceType=" + adapter.NetworkInterfaceType + ", OperationalStatus=" + adapter.OperationalStatus);
                    continue;
                }
                //过滤网络接口的操作状态
                if (!status.Equals(adapter.OperationalStatus))
                {
                    logger.Debug("跳过的不是上行网络，name=" + adapter.Name + ", NetworkInterfaceType=" + adapter.NetworkInterfaceType + ", OperationalStatus=" + adapter.OperationalStatus);
                    continue;
                }
                NetworkVO vo = new NetworkVO();
                vo.IpEnabled = true;
                IPInterfaceProperties property = adapter.GetIPProperties();
                vo.DnsHostName = Dns.GetHostName();//本机名
                vo.Name = adapter.Name;
                vo.Description = adapter.Description;
                vo.Speed = adapter.Speed;

                //macAddress
                if (adapter.GetPhysicalAddress() != null && adapter.GetPhysicalAddress().ToString().Length > 0)
                {
                    char[] mac = adapter.GetPhysicalAddress().ToString().ToCharArray();
                    vo.MacAddress = mac[0] + mac[1] + "-" + mac[2] + mac[3] + "-" + mac[4] + mac[5] + "-" + mac[6] + mac[7] + "-" + mac[8] + mac[9] + "-" + mac[10] + mac[11];
                }

                //ipAddress subnetMask 
                if (property.UnicastAddresses != null && property.UnicastAddresses.Count > 0)
                {
                    foreach (UnicastIPAddressInformation ip in property.UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily.Equals(AddressFamily.InterNetwork))
                        {
                            if (ip.Address != null)
                            {
                                vo.Address = ip.Address.ToString();
                            }
                            if (ip.IPv4Mask != null)
                            {
                                vo.SubnetMask = ip.IPv4Mask.ToString();
                            }
                        }
                    }
                }

                // gateway
                if (property.GatewayAddresses != null && property.GatewayAddresses.Count > 0)
                {
                    foreach (GatewayIPAddressInformation uip in property.GatewayAddresses)
                    {
                        if (uip.Address.AddressFamily.Equals(AddressFamily.InterNetwork))
                        {
                            vo.Gateway = uip.Address.ToString();
                        }
                    }
                }

                // dns server
                if (property.DnsAddresses != null && property.DnsAddresses.Count > 0)
                {
                    vo.DnsServers = new List<string>();
                    foreach (IPAddress ip in property.DnsAddresses)
                    {
                        if (ip.AddressFamily.Equals(AddressFamily.InterNetwork))
                        {
                            vo.DnsServers.Add(ip.ToString());
                        }
                    }
                }

                // dhcp server
                if (property.DhcpServerAddresses != null && property.DhcpServerAddresses.Count > 0)
                {
                    foreach (IPAddress ip in property.DhcpServerAddresses)
                    {
                        if (ip.AddressFamily.Equals(AddressFamily.InterNetwork))
                        {
                            vo.DhcpServer = ip.ToString();
                            vo.DhcpEnabled = true;
                        }
                    }
                }
                else
                {
                    vo.DhcpEnabled = false;
                }
                list.Add(vo);
            }
            return list;
        }

        /// <summary>
        /// 使用win32方式获取网络连接。获取不到名称。
        /// </summary>
        /// <returns></returns>
        public static List<NetworkVO> GetNetworkListWin32()
        {
            List<NetworkVO> list = new List<NetworkVO>();
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                PropertyDataCollection coll = mo.Properties;
                bool ipEnabled = (bool)mo["IPEnabled"];
                if (!ipEnabled)
                {
                    continue;
                }
                string description = (string)mo["Description"];
                string dnsHostName = (string)mo["DNSHostName"];
                string macAddress = (string)mo["MACAddress"];
                string dhcpServer = (string)mo["DHCPServer"];
                bool dhcpEnabled = (bool)mo["DHCPEnabled"];

                string[] addresses = (string[])mo["IPAddress"];
                string[] subnets = (string[])mo["IPSubnet"];
                string[] gateways = (string[])mo["DefaultIPGateway"];
                string[] dnses = (string[])mo["DNSServerSearchOrder"];

                NetworkVO v = new NetworkVO();
                v.IpEnabled = ipEnabled;
                if (addresses != null && addresses.Length > 0 && addresses[0] != null)
                {
                    v.Address = addresses[0];
                }
                if (subnets != null && subnets.Length > 0 && subnets[0] != null)
                {
                    v.SubnetMask = subnets[0];
                }
                if (gateways != null && gateways.Length > 0 && gateways[0] != null)
                {
                    v.Gateway = gateways[0];
                }
                v.DnsServers = new List<string>();
                foreach (string dns in dnses)
                {
                    v.DnsServers.Add(dns);
                }
                v.Description = description;
                v.MacAddress = macAddress;
                v.DnsHostName = dnsHostName;
                v.DhcpServer = dhcpServer;
                v.DhcpEnabled = dhcpEnabled;
                list.Add(v);
            }
            return list;
        }

        private void SetNetworkAdapter()
        {
            ManagementBaseObject inPar = null;
            ManagementBaseObject outPar = null;

            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (!(bool)mo["IPEnabled"])
                    continue;
                //
                //设置ip地址和子网掩码 
                inPar = mo.GetMethodParameters("EnableStatic");
                inPar["IPAddress"] = new string[] { "10.22.21.111", "192.168.10.9" };
                inPar["SubnetMask"] = new string[] { "255.255.255.0", "255.255.255.0" };
                outPar = mo.InvokeMethod("EnableStatic", inPar, null);

                //设置网关地址 
                inPar = mo.GetMethodParameters("SetGateways");
                inPar["DefaultIPGateway"] = new string[] { "10.22.21.1", "192.168.10.1" };
                outPar = mo.InvokeMethod("SetGateways", inPar, null);

                //设置DNS 
                inPar = mo.GetMethodParameters("SetDNSServerSearchOrder");
                inPar["DNSServerSearchOrder"] = new string[] { "179.32.42.4", "179.32.42.5" };
                outPar = mo.InvokeMethod("SetDNSServerSearchOrder", inPar, null);
                break;
            }
        }
    }
}
