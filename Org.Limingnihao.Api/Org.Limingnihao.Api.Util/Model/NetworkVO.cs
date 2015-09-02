using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace Org.Limingnihao.Api.Util.Model
{
    public class NetworkVO
    {
        public bool IpEnabled { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Speed { get; set; }
        public string MacAddress { get; set; }
        public string Address { get; set; }
        public string SubnetMask { get; set; }
        public string Gateway { get; set; }
        public string DnsHostName { get; set; }
        public IList<String> DnsServers { get; set; }
        public string DhcpServer { get; set; }
        public bool DhcpEnabled { get; set; }

        public override string ToString()
        {
            string s = "[NetworkVO] - ipEnabled=" + IpEnabled + ", name=" + Name + ", description=" + Description + ", speed=" + Speed + ", macAddress=" + MacAddress + ", address=" + Address + ", subnetMask=" + SubnetMask + "， gateway=" + Gateway + ", dnsHostName=" + DnsHostName;
            if (DnsServers != null && DnsServers.Count > 0)
            {
                s+= ", dns=";
                foreach (string dns in DnsServers)
                {
                    s+= dns + ", ";
                }
            }
            else
            {
                s += ", dns=null, ";
            }
            s += "dhcpServer=" + DhcpServer + ", dhcpEnabled=" + DhcpEnabled;
            return s;
        }
       
    }
}
