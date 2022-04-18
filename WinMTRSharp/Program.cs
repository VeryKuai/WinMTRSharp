using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Windows.Forms;

namespace WinMTRSharp
{
    static class Program
    {
        public static Version appVersion;

        private static Mutex mutex = null;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Form MainForm = new frmMain();

            bool createdNew;
            mutex = new Mutex(true, "WinMTRSharp", out createdNew);
            if (!createdNew)
            {
                MainForm.StartPosition = FormStartPosition.WindowsDefaultLocation;
                //ProcessUtils.SetFocusToPreviousInstance(MainForm.Text);
                //Environment.Exit(0);
                //return;
            }

            appVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            ServicePointManager.ServerCertificateValidationCallback = RemoteCertValidate;

            try
            {
                FileInfo fi = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "\\qqwry.daz");
                if (fi.LastAccessTime < DateTime.Now.AddMonths(-3))
                    fi.Delete();
            }
            catch { }

            if (File.Exists("\\qqwry.dat") && !File.Exists("\\qqwry.daz"))
                ConvertQQWry();

            Application.Run(MainForm);
            mutex.ReleaseMutex();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {

        }
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
        }
        private static bool RemoteCertValidate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
        {
            return true;
        }

        private static void ConvertQQWry()
        {
            try
            {
                FileStream sourceFile = File.Open("qqwry.dat", FileMode.Open, FileAccess.Read, FileShare.Read);
                FileStream targetFile = File.Create("qqwry.daz");
                DeflateStream targetEncoder = new DeflateStream(targetFile, CompressionMode.Compress);
                sourceFile.CopyTo(targetEncoder);
                sourceFile.Close();
                targetFile.Close();
            }
            catch { }
        }
    }

    public static class ProcessUtils
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_SHOWNORMAL = 1;

        [DllImport("user32.dll")]
        static extern IntPtr GetLastActivePopup(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool IsWindowEnabled(IntPtr hWnd);

        public static void SetFocusToPreviousInstance(string windowCaption)
        {
            IntPtr hWnd = FindWindow(null, windowCaption);
            if (hWnd != null)
            {
                IntPtr hPopupWnd = GetLastActivePopup(hWnd);
                if (hPopupWnd != null && IsWindowEnabled(hPopupWnd))
                {
                    hWnd = hPopupWnd;
                }
                ShowWindow(hWnd, SW_SHOWNORMAL);
                SetForegroundWindow(hWnd);
            }
        }
    }
}
