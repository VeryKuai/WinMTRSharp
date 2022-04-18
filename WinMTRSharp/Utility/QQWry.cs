﻿using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace QQWry
{
    public class IPLocation
    {
        public string IP { get; set; }
        public string Country { get; set; }
        public string Local { get; set; }
    }
    public class QQWryLocator
    {
        [DllImport("psapi.dll")]
        private static extern int EmptyWorkingSet(IntPtr hwProc);

        public static bool IsOkay { set; get; } = false;

        private static byte[] data;
        private static readonly Regex regex = new Regex(@"(((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))\.){3}((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))");
        private static long firstStartIpOffset;
        private static long lastStartIpOffset;
        private static long ipCount;

        public QQWryLocator(string dataPath, bool compressed = false)
        {
            if (!compressed)
            {
                FileStream fs = File.Open(dataPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                MemoryStream ms = new MemoryStream();
                fs.CopyTo(ms);
                data = ms.ToArray();
                fs.Close();
                ms.Close();
            }
            else
            {
                FileStream fz = File.Open(dataPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                DeflateStream ds = new DeflateStream(fz, CompressionMode.Decompress);
                MemoryStream ms = new MemoryStream();
                ds.CopyTo(ms);
                data = ms.ToArray();
                fz.Close();
                ds.Close();
                ms.Close();
            }

            byte[] buffer = new byte[8];
            Array.Copy(data, 0, buffer, 0, 8);
            firstStartIpOffset = ((buffer[0] + (buffer[1] * 0x100)) + ((buffer[2] * 0x100) * 0x100)) + (((buffer[3] * 0x100) * 0x100) * 0x100);
            lastStartIpOffset = ((buffer[4] + (buffer[5] * 0x100)) + ((buffer[6] * 0x100) * 0x100)) + (((buffer[7] * 0x100) * 0x100) * 0x100);
            ipCount = Convert.ToInt64((double)(((double)(lastStartIpOffset - firstStartIpOffset)) / 7.0));

            EmptyWorkingSet(Process.GetCurrentProcess().Handle);

            if (ipCount <= 1L)
            {
                IsOkay = false;
                throw new ArgumentException("读取GeoIP数据库失败");
            }
            else
            {
                IsOkay = true;
            }
        }

        private static long IpToInt(string ip)
        {
            char[] separator = new char[] { '.' };
            if (ip.Split(separator).Length == 3)
            {
                ip += ".0";
            }
            string[] strArray = ip.Split(separator);
            long num2 = ((long.Parse(strArray[0]) * 0x100L) * 0x100L) * 0x100L;
            long num3 = (long.Parse(strArray[1]) * 0x100L) * 0x100L;
            long num4 = long.Parse(strArray[2]) * 0x100L;
            long num5 = long.Parse(strArray[3]);
            return (((num2 + num3) + num4) + num5);
        }

        public IPLocation Query(string ip)
        {
            if (!regex.Match(ip).Success)
            {
                throw new ArgumentException("IP格式错误");
            }
            IPLocation ipLocation = new IPLocation() { IP = ip };
            long intIP = IpToInt(ip);
            if ((intIP >= IpToInt("127.0.0.1") && (intIP <= IpToInt("127.255.255.255"))))
            {
                ipLocation.Country = "本机内部环回地址";
                ipLocation.Local = "";
            }
            else
            {
                if ((((intIP >= IpToInt("0.0.0.0")) && (intIP <= IpToInt("2.255.255.255"))) || ((intIP >= IpToInt("64.0.0.0")) && (intIP <= IpToInt("126.255.255.255")))) ||
                ((intIP >= IpToInt("58.0.0.0")) && (intIP <= IpToInt("60.255.255.255"))))
                {
                    ipLocation.Country = "网络保留地址";
                    ipLocation.Local = "";
                }
            }
            long right = ipCount;
            long left = 0L;
            long startIp;
            while (left < (right - 1L))
            {
                long middle = (right + left) / 2L;
                startIp = GetStartIp(middle, out _);
                if (intIP == startIp)
                {
                    left = middle;
                    break;
                }
                if (intIP > startIp)
                {
                    left = middle;
                }
                else
                {
                    right = middle;
                }
            }
            startIp = GetStartIp(left, out long endIpOff);
            long endIp = GetEndIp(endIpOff, out int countryFlag);
            if ((startIp <= intIP) && (endIp >= intIP))
            {
                ipLocation.Country = GetCountry(endIpOff, countryFlag, out string local);
                if (Regex.IsMatch(local, @"(?i)(CZ88|对方|同一)"))
                    ipLocation.Local = "";
                else
                    ipLocation.Local = local;
            }
            else
            {
                ipLocation.Country = "未知";
                ipLocation.Local = "";
            }
            return ipLocation;
        }

        private long GetStartIp(long left, out long endIpOff)
        {
            long leftOffset = firstStartIpOffset + (left * 7L);
            byte[] buffer = new byte[7];
            Array.Copy(data, leftOffset, buffer, 0, 7);

            endIpOff = (Convert.ToInt64(buffer[4].ToString()) + (Convert.ToInt64(buffer[5].ToString()) * 0x100L)) + ((Convert.ToInt64(buffer[6].ToString()) * 0x100L) * 0x100L);
            return ((Convert.ToInt64(buffer[0].ToString()) + (Convert.ToInt64(buffer[1].ToString()) * 0x100L)) + ((Convert.ToInt64(buffer[2].ToString()) * 0x100L) * 0x100L)) + (((Convert.ToInt64(buffer[3].ToString()) * 0x100L) * 0x100L) * 0x100L);
        }

        private long GetEndIp(long endIpOff, out int countryFlag)
        {
            byte[] buffer = new byte[5];
            Array.Copy(data, endIpOff, buffer, 0, 5);

            countryFlag = buffer[4];
            return ((Convert.ToInt64(buffer[0].ToString()) + (Convert.ToInt64(buffer[1].ToString()) * 0x100L)) + ((Convert.ToInt64(buffer[2].ToString()) * 0x100L) * 0x100L)) + (((Convert.ToInt64(buffer[3].ToString()) * 0x100L) * 0x100L) * 0x100L);
        }

        private string GetCountry(long endIpOff, int countryFlag, out string local)
        {
            string country;
            long offset = endIpOff + 4L;

            switch (countryFlag)
            {
                case 1:
                case 2:
                    country = GetFlagStr(ref offset, ref countryFlag, ref endIpOff);
                    offset = endIpOff + 8L;
                    local = (1 == countryFlag) ? "" : GetFlagStr(ref offset, ref countryFlag, ref endIpOff);
                    break;
                default:
                    country = GetFlagStr(ref offset, ref countryFlag, ref endIpOff);
                    local = GetFlagStr(ref offset, ref countryFlag, ref endIpOff);
                    break;
            }

            return country;
        }

        private string GetFlagStr(ref long offset, ref int countryFlag, ref long endIpOff)
        {
            byte[] buffer = new byte[3];

            while (true)
            {
                long forwardOffset = offset;
                int flag = data[forwardOffset++];

                if (flag != 1 && flag != 2)
                {
                    break;
                }
                Array.Copy(data, forwardOffset, buffer, 0, 3);
                if (flag == 2)
                {
                    countryFlag = 2;
                    endIpOff = offset - 4L;
                }
                offset = (Convert.ToInt64(buffer[0].ToString()) + (Convert.ToInt64(buffer[1].ToString()) * 0x100L)) + ((Convert.ToInt64(buffer[2].ToString()) * 0x100L) * 0x100L);
            }

            if (offset < 12L)
            {
                return "";
            }
            return GetStr(ref offset);
        }

        private string GetStr(ref long offset)
        {
            byte[] bytes = new byte[2];
            StringBuilder stringBuilder = new StringBuilder();
            Encoding encoding = Encoding.GetEncoding("GBK");

            while (true)
            {
                byte lowByte = data[offset++];
                if (lowByte == 0)
                {
                    return stringBuilder.ToString();
                }
                if (lowByte > 0x7f)
                {
                    byte highByte = data[offset++];
                    bytes[0] = lowByte;
                    bytes[1] = highByte;
                    if (highByte == 0)
                    {
                        return stringBuilder.ToString();
                    }
                    stringBuilder.Append(encoding.GetString(bytes));
                }
                else
                {
                    stringBuilder.Append((char)lowByte);
                }
            }
        }
    }
}
