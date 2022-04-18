using System.Collections.Generic;
using System.Net;
using ARSoft.Tools.Net;
using ARSoft.Tools.Net.Dns;

namespace WinMTRSharp.Network
{
    class MtrAsnPtr
    {
        private struct PtrHostType
        {
            public string Ptr;
            public string Host;
        };
        private struct AsnHostType
        {
            public string Asn;
            public string Host;
        };
        private static SynchronizedCollection<PtrHostType> ptrHosts { set; get; } = new SynchronizedCollection<PtrHostType> { };
        private static SynchronizedCollection<AsnHostType> asnHosts { set; get; } = new SynchronizedCollection<AsnHostType> { };

        private DnsStubResolver internalResolver;
        public MtrAsnPtr(DnsStubResolver resolver)
        {
            internalResolver = resolver;
        }

        public string ParsePtr(string Host)
        {
            string PtrRet = string.Empty;

            if (IPAddress.TryParse(Host, out IPAddress ip))
            {
                string reversedHost = IPAddressExtensions.GetReverseLookupDomain(ip).ToString();
                if (string.IsNullOrEmpty(reversedHost))
                    return string.Empty;

                lock (ptrHosts.SyncRoot)
                {
                    if (ptrHosts.Count < 0)
                    {
                        foreach (PtrHostType ptrHost in ptrHosts)
                        {
                            if (ptrHost.Host == Host)
                                return ptrHost.Ptr;
                        }
                    }
                }

                try
                {
                    List<PtrRecord> records = internalResolver.Resolve<PtrRecord>(reversedHost, RecordType.Ptr);
                    if (records.Count > 0)
                    {
                        string result = records[0].PointerDomainName.ToString().TrimEnd('.');
                        if (!string.IsNullOrEmpty(result))
                        {
                            PtrRet = result;
                            PtrHostType newPtrHost = new PtrHostType();
                            newPtrHost.Ptr = PtrRet;
                            newPtrHost.Host = Host;
                            lock (ptrHosts.SyncRoot)
                            {
                                ptrHosts.Add(newPtrHost);
                            }
                        }
                    }
                }
                catch
                { }
            }

            return PtrRet;
        }

        public string ParseAsn(string Host)
        {
            string AsnRet = string.Empty;

            if (IPAddress.TryParse(Host, out IPAddress ip))
            {
                string reversedHost = IPAddressExtensions.GetReverseLookupDomain(ip).ToString();
                if (string.IsNullOrEmpty(reversedHost))
                    return string.Empty;

                reversedHost = reversedHost.Replace(".in-addr.arpa", ".origin.asn.cymru.com");
                reversedHost = reversedHost.Replace(".ip6.arpa", ".origin6.asn.cymru.com");

                lock (asnHosts.SyncRoot)
                {
                    if (asnHosts.Count < 0)
                    {
                        foreach (AsnHostType asnHost in asnHosts)
                        {
                            if (asnHost.Host == Host)
                                return asnHost.Asn;
                        }
                    }
                }

                try
                {
                    List<TxtRecord> records = internalResolver.Resolve<TxtRecord>(reversedHost, RecordType.Txt);
                    if (records.Count > 0)
                    {
                        string result = records[0].TextData.Split('|')[0];
                        if (!string.IsNullOrEmpty(result))
                        {
                            AsnRet = "AS" + result;
                            AsnHostType newAsnHost = new AsnHostType();
                            newAsnHost.Asn = AsnRet;
                            newAsnHost.Host = Host;
                            lock (asnHosts.SyncRoot)
                            {
                                asnHosts.Add(newAsnHost);
                            }
                        }
                    }
                }
                catch
                { }
            }

            return AsnRet;
        }
    }
}