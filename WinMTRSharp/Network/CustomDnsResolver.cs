using ARSoft.Tools.Net;
using ARSoft.Tools.Net.Dns;
using System.Collections.Generic;
using System.Net;

namespace WinMTRSharp.Network
{
    class CustomDnsResolver
    {
        public static DnsStubResolver resolver;

        public CustomDnsResolver()
        {
            List<IPAddress> dnsServers = DnsClient.GetLocalConfiguredDnsServers();
            dnsServers.Add(IPAddress.Parse("114.114.114.114"));
            dnsServers.Add(IPAddress.Parse("223.5.5.5"));
            dnsServers.Add(IPAddress.Parse("119.29.29.29"));
            DnsClient dnsClient = new DnsClient(dnsServers, 2000);
            dnsClient.Is0x20ValidationEnabled = false;
            dnsClient.IsResponseValidationEnabled = false;
            dnsClient.IsTcpEnabled = true;
            dnsClient.IsUdpEnabled = true;
            resolver = new DnsStubResolver(dnsClient);
        }
    }
}
