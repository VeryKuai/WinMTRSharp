using System;
using System.Collections.Generic;

namespace WinMTRSharp.Network
{
    class MtrResultItem
    {
        public int Hop { set; get; } = 0;
        public string Asn { set; get; } = string.Empty;
        public string Host { set; get; } = string.Empty;
        public string HostPtr { set; get; } = string.Empty;
        public double Loss { set; get; } = 0;
        public int Sent { set; get; } = 0;
        public int Recv { set; get; } = 0;
        public double Best { set; get; } = Double.NegativeInfinity;
        public double Avg { set; get; } = Double.NegativeInfinity;
        public double Wrst { set; get; } = Double.NegativeInfinity;
        public double Last { set; get; } = Double.NegativeInfinity;
        public double StDev { set; get; } = Double.NegativeInfinity;
        public string Geo { set; get; } = string.Empty;
        public int LoCn { set; get; } = 0;
        public SynchronizedCollection<double> Rtts { set; get; } = new SynchronizedCollection<double> { };
    }
}
