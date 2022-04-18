using System;
using System.Net;

namespace WinMTRSharp.Utility
{
    class IPAddrHelper
    {
        public static bool IsValidDomainName(string name)
        {
            if (!string.IsNullOrEmpty(name.Trim()))
                return true;
            else
                return false;
        }

        public static bool IsValidIPAddress(string ip)
        {
            IPAddress address;
            if (IPAddress.TryParse(ip, out address))
            {
                switch (address.AddressFamily)
                {
                    case System.Net.Sockets.AddressFamily.InterNetwork:
                        int First = Convert.ToInt32(ip.Split('.')[0]);
                        if (First == 0 || First >= 224)
                            return false;
                        break;
                    case System.Net.Sockets.AddressFamily.InterNetworkV6:
                        if (ip == "::0")
                            return false;
                        break;
                }
                return true;
            }
            return false;
        }

        public static bool IsIPv6(string addr)
        {
            IPAddress ip;
            if (IPAddress.TryParse(addr, out ip))
            {
                return ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6;
            }
            else
            {
                return false;
            }
        }
    }
}
