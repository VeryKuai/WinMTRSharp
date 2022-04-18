using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text;

namespace WinMTRSharp.Network
{
    class MtrIcmp
    {
        public string HostName { set; get; }
        public int Timeout { set; get; } = 2000;
        public int Ttl { set; get; } = 0;

        private readonly string data = "LOREMIPSUMDOLORSITAMETCONSECTETURADIPISCINGELITSEDDOEIUSMODTEMPORINCIDIDUNTUTLABOREETDOLOREMAGNAALIQUAUTENIMADMINIMVENIAMQUISNOSTRUDEXERCITATIONULLAMCOLABORISNISIUTALIQUIPEXEACOMMODOCONSEQUATDUISAUTEIRUREDOLORINREPREHENDERITINVOLUPTATEVELITESSECILLUMDOLOREEUFUGIATNULLAPARIATUREXCEPTEURSINTOCCAECATCUPIDATATNONPROIDENTSUNTINCULPAQUIOFFICIADESERUNTMOLLITANIMIDESTLABORUMLOREMIPSUMDOLORSITAMETCONSECTETURADIPISCINGELITSEDDOEIUSMODTEMPORINCIDIDUNTUTLABOREETDOLOREMAGNAALIQUAUTENIMADMINIMVENIAMQUISNOSTRUDEXERCITATIONULLAMCOLABORISNISIUTALIQUIPEXEACOMMODOCONSEQUATDUISAUTEIRUREDOLORINREPREHENDERITINVOLUPTATEVELITESSECILLUMDOLOREEUFUGIATNULLAPARIATUREXCEPTEURSINTOCCAECATCUPIDATATNONPROIDENTSUNTINCULPAQUIOFFICIADESERUNTMOLLITANIMIDESTLABORUMLOREMIPSUMDOLORSITAMETCONSECTETURADIPISCINGELITSEDDOEIUSMODTEMPORINCIDIDUNTUTLABOREETDOLOREMAGNAALIQUAUTENIMADMINIMVENIAMQUISNOSTRUDEXERCITATIONULLAMCOLABORISNISIUTALIQUIPEXEACOMMODOCONSEQUATDUISAUTEIRUREDOLORINREPREHENDERITINVOLUPTATEVELITESSECILLUMDOLOREEUFUGIATNULLAPARIATUREXCEPTEURSINTOCCAECATCUPIDATATNONPROIDENTSUNTINCULPAQUIOFFICIADESERUNTMOLLITANIMIDESTLABORUMLOREMIPSUMDOLORSITAMETCONSECTETURADIPISCINGELITSEDDOEIUSMODTEMPORINCIDIDUNTUTLABOREETDOLOREMAGNAALIQUAUTENIMADMINIMVENIAMQUISNOSTRUDEXERCITATIONULLAMCOLABORISNISIUTALIQUIPEXEACOMMODOCONSEQUATDUISAUTEIRUREDOLORINREPREHENDERITINVOLUPTATEVELITESSECILLUMDOLOREEUFUGIATNULLAPARIATUREXCEPTEURSINTOCCAECATCUPIDATATNONPROIDENTSUNTINCULPAQUIOFFICIADESERUNTMOLLITANIMIDESTLAB";
        private byte[] buffer;

        public MtrIcmp(int PacketSize)
        {
            if (PacketSize > 0 && PacketSize <= 1472)
                buffer = Encoding.ASCII.GetBytes(data.Substring(0, PacketSize));
            else
                buffer = Encoding.ASCII.GetBytes(data.Substring(0, 64));
        }

        private PingReply Ping()
        {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();

            options.DontFragment = true;
            if (Ttl > 0)
                options.Ttl = Ttl;

            PingReply reply;
            try
            {
                reply = pingSender.Send(HostName, Timeout, buffer, options);
            }
            catch
            {
                reply = null;
            }
            return reply;
        }

        public long GetOriginTtl()
        {
            Ttl = 0;
            PingReply reply = Ping();

            if (reply == null)
                return 0;

            if (reply.Status == IPStatus.Success
                || reply.Status == IPStatus.TtlExpired)
            {
                return reply.Options.Ttl;
            }

            return 0;
        }

        public string GetRouterIpByHop(int Hop)
        {
            Ttl = Hop;
            PingReply reply = Ping();

            if (reply == null)
                return string.Empty;

            if (reply.Status == IPStatus.Success
                || reply.Status == IPStatus.TtlExpired)
            {
                return reply.Address.ToString();
            }

            return string.Empty;
        }

        public double GetDestRttByHop(int Hop)
        {
            Ttl = Hop;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PingReply reply = Ping();
            stopWatch.Stop();

            if (reply == null)
                return Double.NegativeInfinity;

            if (reply.Status == IPStatus.Success
                || reply.Status == IPStatus.TtlExpired)
            {
                return Math.Round(stopWatch.Elapsed.TotalMilliseconds, 1);
            }

            return Double.NegativeInfinity;
        }
    }
}
