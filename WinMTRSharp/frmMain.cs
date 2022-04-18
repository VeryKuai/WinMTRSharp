using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using WinMTRSharp.Network;
using WinMTRSharp.Utility;
using QQWry;
using Microsoft.Win32;
using System.Text;
using System.IO;
using ARSoft.Tools.Net.Dns;

namespace WinMTRSharp
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        public static volatile int isTraceRunning = 0;

        private static QQWryLocator QQWry;
        private static MtrIcmp MtrIcmpTrace;
        private static CustomDnsResolver CustomDnsResolver = new CustomDnsResolver();
        private static MtrAsnPtr MtrAsnPtr = new MtrAsnPtr(CustomDnsResolver.resolver);
        private static SynchronizedCollection<MtrResultItem> MtrResultList;
        private static MtrRuntimeParams rtParams = new MtrRuntimeParams();
        private static System.Timers.Timer TracerTimer = new System.Timers.Timer();
        private static System.Timers.Timer DisplayTimer = new System.Timers.Timer();
        private static int asColumnWidth = 60;

        private readonly double MTR_TIMEDOUT = double.NegativeInfinity;
        private readonly int MAX_STDEV = 200;
        private readonly int MAX_LRU = 10;
        private readonly string ASNUMBER_COLUMN = "AS";

        private struct MtrRuntimeParams
        {
            public bool ipv6;
            public string hostName;
            public long firstHopCount;
            public int maxHop;
            public int traceCount;
        };

        private void Tracer(object sender, ElapsedEventArgs e)
        {
            if (Interlocked.Exchange(ref isTraceRunning, 1) != 0)
                return;

            int maxhop = rtParams.maxHop;
            for (int i = maxhop - 1; i >= 0; i--)
            {
                int Num = i;
                int Hop = Num + 1;

                new Thread(delegate ()
                {
#if !DEBUG
                    try
#endif
                    {
                        if (MtrResultList[Num].Sent > rtParams.traceCount)
                            return;

                        if (Hop == 1)
                        {
                            long firstHopCount = rtParams.firstHopCount;
                            rtParams.firstHopCount = firstHopCount + 1L;
                            if (firstHopCount % 2L != 0L)
                            {
                                return;
                            }
                        }

                        string NewHost = MtrIcmpTrace.GetRouterIpByHop(Hop);
                        MtrResultList[Num].Hop = Hop;

                        if (!string.IsNullOrEmpty(NewHost))
                            MtrResultList[Num].Host = NewHost;

                        double LastPing = MtrIcmpTrace.GetDestRttByHop(Hop);
                        double BestPing = MtrResultList[Num].Best;
                        double WrstPing = MtrResultList[Num].Wrst;
                        double BestPingCbrt = BestPing;

                        MtrResultList[Num].Last = LastPing;

                        if (BestPing == MTR_TIMEDOUT)
                        {
                            BestPingCbrt = MtrOptions.Timeout;
                        }
                        if (LastPing < BestPingCbrt && LastPing != MTR_TIMEDOUT)
                        {
                            MtrResultList[Num].Best = LastPing;
                        }
                        if (LastPing > WrstPing)
                        {
                            MtrResultList[Num].Wrst = LastPing;
                        }

                        object syncRoot = MtrResultList[Num].Rtts.SyncRoot;
                        lock (syncRoot)
                        {
                            if (LastPing > MTR_TIMEDOUT)
                            {
                                if (MtrResultList[Num].Rtts.Count < MAX_STDEV)
                                {
                                    MtrResultList[Num].Rtts.Add(LastPing);
                                }
                                else
                                {
                                    MtrResultList[Num].Rtts.RemoveAt(0);
                                    MtrResultList[Num].Rtts.Add(LastPing);
                                }
                            }

                            if (MtrResultList[Num].Rtts.Count > 0)
                            {
                                MtrResultList[Num].Avg = Math.Round(MtrResultList[Num].Rtts.ToList().Average(), 1);
                            }

                            if (MtrResultList[Num].Rtts.Count > 1)
                            {
                                MtrResultList[Num].StDev = Math.Round(MtrResultList[Num].Rtts.ToList().StandardDeviation(), 1);
                            }
                        }

                        if (LastPing == MTR_TIMEDOUT)
                            MtrResultList[Num].LoCn++;
                        else
                            MtrResultList[Num].Recv++;

                        long TotalSent = ++MtrResultList[Num].Sent;
                        float LossCount = MtrResultList[Num].LoCn;

                        MtrResultList[Num].Loss = Math.Round((LossCount / TotalSent) * 100, 1);

                        bool shouldQueryGeoIP = QQWryLocator.IsOkay && MtrOptions.EnableGeoIP && !rtParams.ipv6;
                        if (shouldQueryGeoIP && IPAddress.TryParse(MtrResultList[Num].Host, out _))
                        {
                            try
                            {
                                IPLocation Location = QQWry.Query(MtrResultList[Num].Host);
                                MtrResultList[Num].Geo = Location.Country + " " + Location.Local;
                            }
                            catch
                            { }
                        }

                        if (IPAddress.TryParse(MtrResultList[Num].Host, out _))
                        {
                            try
                            {
                                if (!MtrOptions.NoDns)
                                    MtrResultList[Num].HostPtr = MtrAsnPtr.ParsePtr(MtrResultList[Num].Host);
                                if (MtrOptions.AsLookup)
                                    MtrResultList[Num].Asn = MtrAsnPtr.ParseAsn(MtrResultList[Num].Host);
                            }
                            catch
                            { }
                        }
                    }
#if !DEBUG
                    catch { }
#endif
                }).Start();

                int sleeptime = (int)Math.Floor(Math.Min(30f, (MtrOptions.Interval / maxhop) / 2f));
                Thread.Sleep(sleeptime);
            }

            rtParams.traceCount++;
            if (TracerTimer.Enabled && rtParams.traceCount >= MtrOptions.CountSize)
            {
                MtrStop(sender, null);
                MessageBox.Show("路由追踪已结束", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            Interlocked.Exchange(ref isTraceRunning, 0);
        }
        private void Display(object sender, ElapsedEventArgs e)
        {
            int MtrResultListLength = MtrResultList.Count();
            ListViewItem[] listViewItems = new ListViewItem[MtrResultListLength];

            for (int i = 0; i < MtrResultListLength; i++)
            {
                int Num = i;

                MtrResultItem Item = MtrResultList[Num];

                if (MtrOptions.AsLookup)
                {
                    string[] row = new string[] {
                        Item.Hop == 0 ? "?" : Item.Hop.ToString("D"),
                        string.IsNullOrEmpty(Item.Asn) ? "AS???" : Item.Asn,
                        (!MtrOptions.NoDns && !string.IsNullOrEmpty(Item.HostPtr)) ? Item.HostPtr :
                            (string.IsNullOrEmpty(Item.Host) ? "请求超时" : Item.Host),
                        Item.Loss.ToString("F1"),
                        Item.Sent.ToString("D"),
                        Item.Recv.ToString("D"),
                        Item.Best == MTR_TIMEDOUT ? "*" : Item.Best.ToString("F1"),
                        Item.Avg == MTR_TIMEDOUT ? "*" : Item.Avg.ToString("F1"),
                        Item.Wrst == MTR_TIMEDOUT ? "*" : Item.Wrst.ToString("F1"),
                        Item.Last == MTR_TIMEDOUT ? "*" : Item.Last.ToString("F1"),
                        Item.StDev == MTR_TIMEDOUT ? "*" : Item.StDev.ToString("F1"),
                        !MtrOptions.EnableGeoIP ? "暂缺" :
                            (string.IsNullOrEmpty(Item.Geo) ? string.Empty : Item.Geo)
                    };
                    listViewItems[i] = new ListViewItem(row);
                }
                else
                {
                    string[] row = new string[] {
                        Item.Hop == 0 ? "?" : Item.Hop.ToString("D"),
                        (!MtrOptions.NoDns && !string.IsNullOrEmpty(Item.HostPtr)) ? Item.HostPtr :
                            (string.IsNullOrEmpty(Item.Host) ? "请求超时" : Item.Host),
                        Item.Loss.ToString("F1"),
                        Item.Sent.ToString("D"),
                        Item.Recv.ToString("D"),
                        Item.Best == MTR_TIMEDOUT ? "*" : Item.Best.ToString("F1"),
                        Item.Avg == MTR_TIMEDOUT ? "*" : Item.Avg.ToString("F1"),
                        Item.Wrst == MTR_TIMEDOUT ? "*" : Item.Wrst.ToString("F1"),
                        Item.Last == MTR_TIMEDOUT ? "*" : Item.Last.ToString("F1"),
                        Item.StDev == MTR_TIMEDOUT ? "*" : Item.StDev.ToString("F1"),
                        !MtrOptions.EnableGeoIP ? "暂缺" :
                            (string.IsNullOrEmpty(Item.Geo) ? string.Empty : Item.Geo)
                    };
                    listViewItems[i] = new ListViewItem(row);
                }

                if (Num < MtrResultListLength - 1 && Item.Host == MtrIcmpTrace.HostName)
                {
                    rtParams.maxHop = Num + 1;
                    break;
                }
                else if (Num == MtrResultListLength - 1 && Item.Host != MtrIcmpTrace.HostName)
                {
                    rtParams.maxHop = MtrOptions.HopLimit;
                    break;
                }
            }
            UpdateListViewMTR(listViewItems);
        }

        delegate void ListViewMTRAddItemDelegate(ListViewItem[] listViewItems);
        private void UpdateListViewMTR(ListViewItem[] listViewItems)
        {
            if (listViewMTR.InvokeRequired)
            {
                ListViewMTRAddItemDelegate del = UpdateListViewMTR;
                listViewMTR.Invoke(del, new[] { listViewItems });
            }
            else
            {
                lock (listViewMTR.Tag)
                {
                    listViewMTR.Tag = "Updating";
                    listViewMTR.BeginUpdate();
                    listViewMTR.Items.Clear();
                    for (int i = 0; i < rtParams.maxHop; i++)
                    {
                        if (listViewItems[i] != null)
                        {
                            listViewMTR.Items.Add(listViewItems[i]);
                            listViewItems[i] = null;
                        }
                    }
                    listViewItems = null;
                    listViewMTR.EndUpdate();
                    listViewMTR.Tag = "Not Updating";
                }
                Application.DoEvents();
            }
        }
        private bool MtrRun(object sender, EventArgs e)
        {
            btnStartStop.Text = "停止(&S)";
            btnStartStop.Enabled = false;
            btnOption.Enabled = false;
            cboHostList.Enabled = false;
            toolStripStatusLabelHostIP.Text = string.Empty;
            optionsToolStripMenuItem.Enabled = false;

            listViewMTR.Items.Clear();
            listViewMTR.AutoResizeColumn(listViewMTR.Columns.Count - 1, ColumnHeaderAutoResizeStyle.HeaderSize);

            if (!IPAddrHelper.IsValidIPAddress(cboHostList.Text.Trim()) &&
                !IPAddrHelper.IsValidDomainName(cboHostList.Text.Trim()))
            {
                MessageBox.Show("IP地址或主机名格式不正确!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                goto onErr;
            }

            rtParams = new MtrRuntimeParams();
            rtParams.hostName = cboHostList.Text.Trim();
            rtParams.firstHopCount = 0;
            rtParams.maxHop = MtrOptions.HopLimit;
            rtParams.traceCount = 0;

            string destip = string.Empty;
            if (IPAddrHelper.IsValidDomainName(rtParams.hostName))
            {
                try
                {
                    IPAddress[] records = Dns.GetHostAddresses(rtParams.hostName);
                    if (records.Length > 0)
                        destip = records[0].ToString();
                    if (string.IsNullOrEmpty(destip))
                        throw new IOException();
                }
                catch
                {
                    try
                    {
                        List<ARecord> records = CustomDnsResolver.resolver.Resolve<ARecord>(rtParams.hostName, RecordType.A);
                        if (records.Count > 0)
                            destip = records[0].Address.ToString();
                        if (string.IsNullOrEmpty(destip))
                            throw new IOException();
                    }
                    catch
                    {
                        try
                        {
                            List<AaaaRecord> records = CustomDnsResolver.resolver.Resolve<AaaaRecord>(rtParams.hostName, RecordType.Aaaa);
                            if (records.Count > 0)
                                destip = records[0].Address.ToString();
                            if (string.IsNullOrEmpty(destip))
                                throw new IOException();
                        }
                        catch
                        {
                            MessageBox.Show("域名解析失败!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            goto onErr;
                        }
                    }
                }
            }
            else
            {
                destip = rtParams.hostName;
            }
            rtParams.ipv6 = IPAddrHelper.IsIPv6(destip);

            MtrResultList = new SynchronizedCollection<MtrResultItem>();
            for (int i = 0; i < rtParams.maxHop; i++)
            {
                MtrResultList.Add(new MtrResultItem());
            }
            MtrIcmpTrace = new MtrIcmp(MtrOptions.PacketSize);
            MtrIcmpTrace.HostName = destip;
            MtrIcmpTrace.Timeout = MtrOptions.Timeout;
            MtrIcmpTrace.Ttl = Math.Max(64, MtrOptions.HopLimit);

            notifyIcon.Text = Application.ProductName + " - " + MtrIcmpTrace.HostName;
            toolStripStatusLabelHostIP.AutoSize = true;
            if (rtParams.ipv6)
                toolStripStatusLabelHostIP.Text = "IPv6地址: " + MtrIcmpTrace.HostName;
            else
                toolStripStatusLabelHostIP.Text = "IPv4地址: " + MtrIcmpTrace.HostName;

            Application.DoEvents();
            TracerTimer.Elapsed += new ElapsedEventHandler(Tracer);
            TracerTimer.Interval = MtrOptions.Interval;
            TracerTimer.Enabled = true;
            TracerTimer.AutoReset = true;
            TracerTimer.Start();

            Thread.Sleep(500);
            DisplayTimer.Elapsed += new ElapsedEventHandler(Display);
            DisplayTimer.Interval = Math.Max(500, MtrOptions.Interval);
            DisplayTimer.Enabled = true;
            DisplayTimer.AutoReset = true;
            DisplayTimer.Start();

            btnStartStop.Enabled = true;
            return true;

        onErr:
            btnStartStop.Enabled = true;
            ResetStartStopButton();
            return false;
        }

        delegate void ResetStartStopButtonDelegate();
        private void ResetStartStopButton()
        {
            if (btnStartStop.InvokeRequired)
            {
                ResetStartStopButtonDelegate del = ResetStartStopButton;
                btnStartStop.Invoke(del);
            }
            else
                btnStartStop.Text = "开始(&S)";
            if (btnOption.InvokeRequired)
            {
                ResetStartStopButtonDelegate del = ResetStartStopButton;
                btnOption.Invoke(del);
            }
            else
                btnOption.Enabled = true;
            if (cboHostList.InvokeRequired)
            {
                ResetStartStopButtonDelegate del = ResetStartStopButton;
                cboHostList.Invoke(del);
            }
            else
                cboHostList.Enabled = true;

            notifyIcon.Text = this.Text;
            optionsToolStripMenuItem.Enabled = true;
        }
        private void MtrStop(object sender, EventArgs e)
        {
            TracerTimer.Stop();
            TracerTimer.Enabled = false;
            TracerTimer.Elapsed -= Tracer;

            Display(sender, null);
            DisplayTimer.Stop();
            DisplayTimer.Enabled = false;
            DisplayTimer.Elapsed -= Display;

            GC.Collect();
            GC.WaitForPendingFinalizers();

            ResetStartStopButton();
        }
        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (!TracerTimer.Enabled)
            {
                if (cboHostList.SelectedIndex == cboHostList.Items.Count - 1)
                {
                    clearCboHostList();
                    return;
                }
                changeColumn();
                if (MtrRun(sender, null))
                {
                    string host = cboHostList.Text.Trim();
                    if (cboHostList.Items.Count > MAX_LRU)
                    {
                        cboHostList.Items.Remove(cboHostList.Items[0]);
                    }
                    if (cboHostList.FindString(host) == -1)
                    {
                        cboHostList.Items.Insert(cboHostList.Items.Count - 1, host);
                    }
                }
            }
            else
            {
                MtrStop(sender, null);
            }
        }

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool BitBlt([In] IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, [In] IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
        private const int SRC_COPY = 0xCC0020;

        private void btnScreenshot_Click(object sender, EventArgs e)
        {
            Form capturedForm = this;
            int width = capturedForm.ClientSize.Width;
            int height = capturedForm.ClientSize.Height;

            Bitmap bmpScreenshot = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            Graphics gb = Graphics.FromImage(bmpScreenshot);
            Graphics gc = Graphics.FromHwnd(this.Handle);

            IntPtr hdcDest;
            IntPtr hdcSrc;

            lock (listViewMTR.Tag)
            {
                this.Refresh();
                listViewMTR.Refresh();
                Application.DoEvents();
                try
                {
                    hdcDest = gb.GetHdc();
                    hdcSrc = gc.GetHdc();
                    BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, SRC_COPY);
                }
                catch
                {
                    MessageBox.Show("获取截图出错!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (hdcDest != IntPtr.Zero)
                gb.ReleaseHdc(hdcDest);
            if (hdcSrc != IntPtr.Zero)
                gc.ReleaseHdc(hdcSrc);

            Clipboard.SetDataObject(bmpScreenshot);

            DialogResult result = sfdScreenShot.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    bmpScreenshot.Save(sfdScreenShot.FileName, ImageFormat.Png);
                }
                catch
                {
                    MessageBox.Show("保存截图出错!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                notifyIcon.ShowBalloonTip(20000, "提示", "截图已复制到剪贴板并保存。", ToolTipIcon.Info);
            }
            else
            {
                notifyIcon.ShowBalloonTip(20000, "提示", "截图已复制到剪贴板。", ToolTipIcon.Info);
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.WinMTR;
            notifyIcon.Icon = Properties.Resources.WinMTR;
            notifyIcon.Visible = true;

            listViewMTR.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            asColumnWidth = listViewMTR.Columns[1].Width;
            listViewMTR.Columns.RemoveAt(1);
            listViewMTR.AutoResizeColumn(listViewMTR.Columns.Count - 1, ColumnHeaderAutoResizeStyle.HeaderSize);
            listViewMTR.Items.Clear();
            listViewMTR.DoubleBuffered(true);

            toolStripStatusLabelVersion.Text = Application.ProductName
                                               + " " + Program.appVersion.Major.ToString()
                                               + "." + Program.appVersion.Minor.ToString();
            timerDateTime_Tick(sender, null);

            notifyIcon.Text = this.Text;
            loadSettings();
            readLRU();
            changeColumn();

            try
            {
                QQWry = new QQWryLocator(AppDomain.CurrentDomain.BaseDirectory + "\\qqwry.daz", true);
            }
            catch
            {
                try
                {
                    QQWry = new QQWryLocator(Path.GetTempPath() + "\\qqwry.daz", true);
                }
                catch
                {
                    MtrOptions.EnableGeoIP = false;
                    notifyIcon.ShowBalloonTip(20000, "提示", "正在下载地理位置数据库。", ToolTipIcon.Info);
                    new Thread(delegate ()
                    {
                        Thread.Sleep(1000);
                        qqWryDownloader();
                    }).Start();
                }
            }
        }

        private void qqWryDownloader()
        {
            string fileName = Path.GetTempPath() + "\\qqwry.daz";
            Uri uri = new Uri("https://dl.verykuai.com/tools/qqwry.daz");

            using (WebClient client = new WebClient())
            {
                client.Encoding = System.Text.Encoding.UTF8;
                client.Headers.Add("User-Agent", Application.ProductName + " " + Program.appVersion.Major.ToString() + "." + Program.appVersion.Minor.ToString());
                try
                {
                    client.DownloadFile(uri, fileName);
                }
                catch
                {
                    try
                    {
                        uri = new Uri("http://dl.verykuai.com/tools/qqwry.daz");
                        client.DownloadFile(uri, fileName);
                    }
                    catch
                    {
                        goto onErr;
                    }
                }
            }

            try
            {
                File.Copy(fileName, AppDomain.CurrentDomain.BaseDirectory + "\\qqwry.daz", true);
                QQWry = new QQWryLocator(AppDomain.CurrentDomain.BaseDirectory + "\\qqwry.daz", true);
                try
                {
                    File.Delete(Path.GetTempPath() + "\\qqwry.daz");
                }
                catch { }
            }
            catch
            {
                try
                {
                    QQWry = new QQWryLocator(Path.GetTempPath() + "\\qqwry.daz", true);
                }
                catch
                {
                    goto onErr;
                }
            }

            MtrOptions.EnableGeoIP = true;
            notifyIcon.Visible = false;
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(20000, "提示", "地理位置数据库下载成功。", ToolTipIcon.Info);
            return;

        onErr:
            notifyIcon.Visible = false;
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(20000, "错误", "地理位置数据库下载失败，无法显示地理位置信息。", ToolTipIcon.Error);
            return;
        }

        private void ListViewToCSV(ListView listView, string filePath, bool includeHidden = true)
        {
            StringBuilder result = new StringBuilder();

            result.Append("," + Application.ProductName + " 统计信息");
            result.Append(",,,,,,," + "IP地址: " + MtrIcmpTrace.HostName);
            result.AppendLine();

            WriteCSVRow(result, listView.Columns.Count, i => includeHidden || listView.Columns[i].Width > 0, i => listView.Columns[i].Text);

            foreach (ListViewItem listItem in listView.Items)
                WriteCSVRow(result, listView.Columns.Count, i => includeHidden || listView.Columns[i].Width > 0, i => listItem.SubItems[i].Text);

            result.Append("," + Application.ProductName + " " + Program.appVersion.Major.ToString() + "." + Program.appVersion.Minor.ToString()
                + " https://www.verykuai.com");
            result.Append(",,,,,,," + "系统时间: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            result.AppendLine();

            try
            {
                File.WriteAllText(filePath, result.ToString());
            }
            catch
            {
                throw new System.IO.IOException();
            }
        }
        private void WriteCSVRow(StringBuilder result, int itemsCount, Func<int, bool> isColumnNeeded, Func<int, string> columnValue)
        {
            bool isFirstTime = true;
            for (int i = 0; i < itemsCount; i++)
            {
                if (!isColumnNeeded(i))
                    continue;

                if (!isFirstTime)
                    result.Append(",");
                isFirstTime = false;

                result.Append(String.Format("\"{0}\"", columnValue(i)));
            }
            result.AppendLine();
        }
        private void btnExport_Click(object sender, EventArgs e)
        {
            DialogResult result = sfdCSV.ShowDialog();
            if (result == DialogResult.OK)
            {
                lock (listViewMTR.Tag)
                {
                    try
                    {
                        ListViewToCSV(listViewMTR, sfdCSV.FileName);
                    }
                    catch
                    {
                        MessageBox.Show("保存CSV异常!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                notifyIcon.ShowBalloonTip(20000, "提示", "统计信息已保存至CSV文件。", ToolTipIcon.Info);
            }
        }
        private void timerDateTime_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = "系统时间: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }
        private void clearCboHostList()
        {
            cboHostList.Items.Clear();
            cboHostList.Text = string.Empty;
            cboHostList.Items.AddRange(new object[] { "114.114.114.114", "223.5.5.5", "清除历史记录" });
        }
        private void readLRU()
        {
            List<string> list = new List<string>();
            int lruCounter = 0;

            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\WinMTR\LRU");
                if (key != null)
                {
                    int nrLRUValue = 0;
                    if (key.GetValue("NrLRU") != null && key.GetValueKind("NrLRU").Equals(RegistryValueKind.DWord))
                        nrLRUValue = Convert.ToInt32(key.GetValue("NrLRU").ToString());

                    if (nrLRUValue < 0 || nrLRUValue > MAX_LRU)
                        nrLRUValue = MAX_LRU;

                    for (int i = 1; i <= nrLRUValue; i++)
                    {
                        string hostKey = "Host" + i.ToString();
                        string hostValue = string.Empty;
                        if (key.GetValue(hostKey) != null && key.GetValueKind(hostKey).Equals(RegistryValueKind.String))
                            hostValue = key.GetValue(hostKey).ToString();

                        if (!string.IsNullOrEmpty(hostValue))
                        {
                            list.Add(hostValue);
                            lruCounter++;
                        }
                    }
                    key.Close();
                }
            }
            catch { }

            if (lruCounter != 0)
            {
                list = list.Distinct().ToList();
                cboHostList.Items.AddRange(list.ToArray());
                cboHostList.Items.Add("清除历史记录");
            }
            else
            {
                clearCboHostList();
            }
        }
        private void loadSettings()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\WinMTR\Config");
                if (key != null)
                {
                    if (key.GetValue("AsLookup") != null && key.GetValueKind("AsLookup").Equals(RegistryValueKind.DWord))
                        MtrOptions.AsLookup = Convert.ToInt32(key.GetValue("AsLookup").ToString()) != 0 ? true : false;
                    if (key.GetValue("CountSize") != null && key.GetValueKind("CountSize").Equals(RegistryValueKind.DWord))
                        MtrOptions.CountSize = Convert.ToInt32(key.GetValue("CountSize").ToString());
                    if (key.GetValue("EnableGeoIP") != null && key.GetValueKind("EnableGeoIP").Equals(RegistryValueKind.DWord))
                        MtrOptions.EnableGeoIP = Convert.ToInt32(key.GetValue("EnableGeoIP").ToString()) != 0 ? true : false;
                    if (key.GetValue("HopLimit") != null && key.GetValueKind("HopLimit").Equals(RegistryValueKind.DWord))
                        MtrOptions.HopLimit = Convert.ToInt32(key.GetValue("HopLimit").ToString());
                    if (key.GetValue("Interval") != null && key.GetValueKind("Interval").Equals(RegistryValueKind.DWord))
                        MtrOptions.Interval = Convert.ToInt32(key.GetValue("Interval").ToString());
                    if (key.GetValue("PingSize") != null && key.GetValueKind("PingSize").Equals(RegistryValueKind.DWord))
                        MtrOptions.PacketSize = Convert.ToInt32(key.GetValue("PingSize").ToString());
                    if (key.GetValue("Timeout") != null && key.GetValueKind("Timeout").Equals(RegistryValueKind.DWord))
                        MtrOptions.Timeout = Convert.ToInt32(key.GetValue("Timeout").ToString());
                    if (key.GetValue("UseDNS") != null && key.GetValueKind("UseDNS").Equals(RegistryValueKind.DWord))
                        MtrOptions.NoDns = Convert.ToInt32(key.GetValue("UseDNS").ToString()) != 0 ? false : true;
                    key.Close();
                }
            }
            catch { }
        }
        private void saveSettings()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\WinMTR");
                if (key != null)
                {
                    key.SetValue("HomePage", "https://www.verykuai.com");
                    key.SetValue("Version", Program.appVersion.Major.ToString() + "." + Program.appVersion.Minor.ToString());
                    key.DeleteValue("License", false);
                    key.DeleteSubKeyTree("LRU", false);
                    key.Close();
                }

                key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\WinMTR\Config");
                if (key != null)
                {
                    key.SetValue("AsLookup", MtrOptions.AsLookup ? 1 : 0);
                    key.SetValue("CountSize", MtrOptions.CountSize);
                    key.SetValue("EnableGeoIP", QQWryLocator.IsOkay && !MtrOptions.EnableGeoIP ? 0 : 1);
                    key.SetValue("HopLimit", MtrOptions.HopLimit);
                    key.SetValue("Interval", MtrOptions.Interval);
                    key.SetValue("MaxLRU", MAX_LRU);
                    key.SetValue("PingSize", MtrOptions.PacketSize);
                    key.SetValue("Timeout", MtrOptions.Timeout);
                    key.SetValue("UseDNS", MtrOptions.NoDns ? 0 : 1);
                    key.SetValue("UseIPV6", 1);
                    key.Close();
                }

                key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\WinMTR\LRU");
                if (key != null)
                {
                    int NrLRU = Math.Min(MAX_LRU, cboHostList.Items.Count - 1);
                    for (int i = 0; i <= NrLRU - 1; i++)
                    {
                        key.SetValue("Host" + (i + 1).ToString(), cboHostList.Items[i].ToString());
                    }
                    key.SetValue("NrLRU", NrLRU);
                    key.Close();
                }
            }
            catch { }
        }
        private void cboHostList_DropDownClosed(object sender, EventArgs e)
        {
            if (cboHostList.SelectedIndex == cboHostList.Items.Count - 1)
                clearCboHostList();
        }
        private void btnOption_Click(object sender, EventArgs e)
        {
            frmOptions optionsForm = new frmOptions();
            optionsForm.Owner = this;
            optionsForm.ShowDialog(this);
        }
        private void toolStripStatusLabelWebsite_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.verykuai.com");
        }
        public void changeColumn()
        {
            if (MtrOptions.AsLookup && this.listViewMTR.Columns[1].Text != "AS")
            {
                listViewMTR.Items.Clear();
                listViewMTR.Columns.Insert(1, ASNUMBER_COLUMN);
                listViewMTR.Columns[1].Width = asColumnWidth;
                listViewMTR.AutoResizeColumn(listViewMTR.Columns.Count - 1, ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            else if (!MtrOptions.AsLookup && listViewMTR.Columns[1].Text == "AS")
            {
                listViewMTR.Items.Clear();
                listViewMTR.Columns.RemoveAt(1);
                listViewMTR.AutoResizeColumn(listViewMTR.Columns.Count - 1, ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            if (MtrOptions.AsLookup)
            {
                if (MtrOptions.NoDns)
                    listViewMTR.Columns[2].Text = "IP地址";
                else
                    listViewMTR.Columns[2].Text = "IP地址/主机名";
            }
            else
            {
                if (MtrOptions.NoDns)
                    listViewMTR.Columns[1].Text = "IP地址";
                else
                    listViewMTR.Columns[1].Text = "IP地址/主机名";
            }
        }
        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (sender == this)
            {
                listViewMTR.Size = new Size(this.Width - 40, this.Height - 103);
                listViewMTR.AutoResizeColumn(listViewMTR.Columns.Count - 1, ColumnHeaderAutoResizeStyle.HeaderSize);
                if (listViewMTR.ClientSize.Width > listViewMTR.Width)
                {
                    listViewMTR.Columns[listViewMTR.Columns.Count - 1].Width -= (listViewMTR.Width - listViewMTR.ClientSize.Width);
                }
            }
        }
        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveSettings();
            notifyIcon.Visible = false;
        }
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (btnOption.Enabled)
                btnOption_Click(sender, null);
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listViewMTR_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
                e.Item.Selected = false;
        }

        private void cboHostList_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnStartStop.Focus();
                btnStartStop_Click(sender, e);
            }
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon_MouseDoubleClick(sender, null);
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
            }
        }
    }
}
