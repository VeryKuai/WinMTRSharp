namespace WinMTRSharp
{
    class MtrOptions
    {
        public static int Timeout { set; get; } = 2000;
        public static int Interval { set; get; } = 500;
        public static int HopLimit { set; get; } = 30;
        public static int CountSize { set; get; } = 1000;
        public static int PacketSize { set; get; } = 64;
        public static bool EnableGeoIP { set; get; } = true;
        public static bool NoDns { set; get; } = true;
        public static bool AsLookup { set; get; } = false;
    }
}
