using System.Windows.Forms;

namespace WinMTRSharp
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "000",
            "AS888888",
            "8888:8888:8888:8888::1",
            "888.8%",
            "88888",
            "88888",
            "888.8",
            "888.8",
            "888.8",
            "888.8",
            "888.8",
            ""}, -1);
            this.lblHost = new System.Windows.Forms.Label();
            this.cboHostList = new System.Windows.Forms.ComboBox();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.btnOption = new System.Windows.Forms.Button();
            this.btnScreenshot = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelHostIP = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelWebsite = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblDateTime = new System.Windows.Forms.Label();
            this.timerDateTime = new System.Windows.Forms.Timer(this.components);
            this.sfdScreenShot = new System.Windows.Forms.SaveFileDialog();
            this.sfdCSV = new System.Windows.Forms.SaveFileDialog();
            this.listViewMTR = new System.Windows.Forms.ListView();
            this.chHop = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chAsn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chHost = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chLoss = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chSent = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chRecv = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chBest = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chAvg = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chWrst = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chLast = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chStDev = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chGeo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmsIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip.SuspendLayout();
            this.cmsIcon.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblHost
            // 
            this.lblHost.AutoSize = true;
            this.lblHost.Location = new System.Drawing.Point(12, 15);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(47, 12);
            this.lblHost.TabIndex = 0;
            this.lblHost.Text = "主机名:";
            // 
            // cboHostList
            // 
            this.cboHostList.FormattingEnabled = true;
            this.cboHostList.Location = new System.Drawing.Point(65, 12);
            this.cboHostList.Name = "cboHostList";
            this.cboHostList.Size = new System.Drawing.Size(203, 20);
            this.cboHostList.TabIndex = 1;
            this.cboHostList.DropDownClosed += new System.EventHandler(this.cboHostList_DropDownClosed);
            this.cboHostList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cboHostList_KeyUp);
            // 
            // btnStartStop
            // 
            this.btnStartStop.Location = new System.Drawing.Point(283, 10);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(60, 23);
            this.btnStartStop.TabIndex = 2;
            this.btnStartStop.Text = "开始(&S)";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // btnOption
            // 
            this.btnOption.Location = new System.Drawing.Point(349, 10);
            this.btnOption.Name = "btnOption";
            this.btnOption.Size = new System.Drawing.Size(60, 23);
            this.btnOption.TabIndex = 3;
            this.btnOption.Text = "选项(&O)";
            this.btnOption.UseVisualStyleBackColor = true;
            this.btnOption.Click += new System.EventHandler(this.btnOption_Click);
            // 
            // btnScreenshot
            // 
            this.btnScreenshot.Location = new System.Drawing.Point(415, 10);
            this.btnScreenshot.Name = "btnScreenshot";
            this.btnScreenshot.Size = new System.Drawing.Size(60, 23);
            this.btnScreenshot.TabIndex = 4;
            this.btnScreenshot.Text = "截图(&C)";
            this.btnScreenshot.UseVisualStyleBackColor = true;
            this.btnScreenshot.Click += new System.EventHandler(this.btnScreenshot_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(481, 10);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(60, 23);
            this.btnExport.TabIndex = 5;
            this.btnExport.Text = "导出(&E)";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelVersion,
            this.toolStripStatusLabelHostIP,
            this.toolStripStatusLabelWebsite});
            this.statusStrip.Location = new System.Drawing.Point(0, 379);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(824, 22);
            this.statusStrip.TabIndex = 6;
            // 
            // toolStripStatusLabelVersion
            // 
            this.toolStripStatusLabelVersion.Margin = new System.Windows.Forms.Padding(0, 3, 3, 2);
            this.toolStripStatusLabelVersion.Name = "toolStripStatusLabelVersion";
            this.toolStripStatusLabelVersion.Size = new System.Drawing.Size(500, 17);
            this.toolStripStatusLabelVersion.Spring = true;
            this.toolStripStatusLabelVersion.Text = "WinMTR";
            this.toolStripStatusLabelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabelHostIP
            // 
            this.toolStripStatusLabelHostIP.AutoSize = false;
            this.toolStripStatusLabelHostIP.Margin = new System.Windows.Forms.Padding(3, 3, 3, 2);
            this.toolStripStatusLabelHostIP.Name = "toolStripStatusLabelHostIP";
            this.toolStripStatusLabelHostIP.Size = new System.Drawing.Size(180, 17);
            this.toolStripStatusLabelHostIP.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolStripStatusLabelWebsite
            // 
            this.toolStripStatusLabelWebsite.IsLink = true;
            this.toolStripStatusLabelWebsite.Margin = new System.Windows.Forms.Padding(3, 3, 3, 2);
            this.toolStripStatusLabelWebsite.Name = "toolStripStatusLabelWebsite";
            this.toolStripStatusLabelWebsite.Size = new System.Drawing.Size(114, 17);
            this.toolStripStatusLabelWebsite.Text = "www.verykuai.com";
            this.toolStripStatusLabelWebsite.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolStripStatusLabelWebsite.Click += new System.EventHandler(this.toolStripStatusLabelWebsite_Click);
            // 
            // lblDateTime
            // 
            this.lblDateTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDateTime.Location = new System.Drawing.Point(633, 15);
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.Size = new System.Drawing.Size(179, 12);
            this.lblDateTime.TabIndex = 7;
            this.lblDateTime.Text = "系统时间: 0000/00/00 00:00:00";
            this.lblDateTime.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // timerDateTime
            // 
            this.timerDateTime.Enabled = true;
            this.timerDateTime.Interval = 1000;
            this.timerDateTime.Tick += new System.EventHandler(this.timerDateTime_Tick);
            // 
            // sfdScreenShot
            // 
            this.sfdScreenShot.DefaultExt = "png";
            this.sfdScreenShot.FileName = "WinMTR.png";
            this.sfdScreenShot.Filter = "PNG 图片|*.png";
            // 
            // sfdCSV
            // 
            this.sfdCSV.DefaultExt = "csv";
            this.sfdCSV.FileName = "WinMTR.csv";
            this.sfdCSV.Filter = "CSV (逗号分隔)|*.csv";
            // 
            // listViewMTR
            // 
            this.listViewMTR.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chHop,
            this.chAsn,
            this.chHost,
            this.chLoss,
            this.chSent,
            this.chRecv,
            this.chBest,
            this.chAvg,
            this.chWrst,
            this.chLast,
            this.chStDev,
            this.chGeo});
            this.listViewMTR.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewMTR.HideSelection = false;
            this.listViewMTR.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.listViewMTR.LabelWrap = false;
            this.listViewMTR.Location = new System.Drawing.Point(12, 39);
            this.listViewMTR.MultiSelect = false;
            this.listViewMTR.Name = "listViewMTR";
            this.listViewMTR.ShowGroups = false;
            this.listViewMTR.Size = new System.Drawing.Size(800, 337);
            this.listViewMTR.TabIndex = 8;
            this.listViewMTR.Tag = "Not Updating";
            this.listViewMTR.UseCompatibleStateImageBehavior = false;
            this.listViewMTR.View = System.Windows.Forms.View.Details;
            this.listViewMTR.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listViewMTR_ItemSelectionChanged);
            // 
            // chHop
            // 
            this.chHop.Text = "#";
            // 
            // chAsn
            // 
            this.chAsn.Text = "AS";
            // 
            // chHost
            // 
            this.chHost.Text = "IP地址/主机名";
            // 
            // chLoss
            // 
            this.chLoss.Text = "丢包%";
            this.chLoss.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // chSent
            // 
            this.chSent.Text = "发送";
            this.chSent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // chRecv
            // 
            this.chRecv.Text = "接收";
            this.chRecv.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // chBest
            // 
            this.chBest.Text = "最好";
            this.chBest.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // chAvg
            // 
            this.chAvg.Text = "平均";
            this.chAvg.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // chWrst
            // 
            this.chWrst.Text = "最差";
            this.chWrst.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // chLast
            // 
            this.chLast.Text = "上次";
            this.chLast.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // chStDev
            // 
            this.chStDev.Text = "标准差";
            this.chStDev.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // chGeo
            // 
            this.chGeo.Text = "地理位置";
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.cmsIcon;
            this.notifyIcon.Icon = global::WinMTRSharp.Properties.Resources.WinMTR;
            this.notifyIcon.Text = "WinMTR";
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // cmsIcon
            // 
            this.cmsIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.cmsIcon.Name = "cmsIcon";
            this.cmsIcon.Size = new System.Drawing.Size(119, 70);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.showToolStripMenuItem.Text = "显示(&S)";
            this.showToolStripMenuItem.Click += new System.EventHandler(this.showToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.optionsToolStripMenuItem.Text = "选项(&O)";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.exitToolStripMenuItem.Text = "退出(&X)";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(824, 401);
            this.Controls.Add(this.listViewMTR);
            this.Controls.Add(this.lblDateTime);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.btnOption);
            this.Controls.Add(this.btnScreenshot);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.cboHostList);
            this.Controls.Add(this.lblHost);
            this.MaximumSize = new System.Drawing.Size(1920, 1200);
            this.MinimumSize = new System.Drawing.Size(840, 440);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WinMTR - VeryKuai VK加速器专用版";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.cmsIcon.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblHost;
        private ComboBox cboHostList;
        private Button btnStartStop;
        private Button btnOption;
        private Button btnScreenshot;
        private Button btnExport;
        private StatusStrip statusStrip;
        private Label lblDateTime;
        private Timer timerDateTime;
        private ToolStripStatusLabel toolStripStatusLabelVersion;
        private ToolStripStatusLabel toolStripStatusLabelWebsite;
        private SaveFileDialog sfdScreenShot;
        private SaveFileDialog sfdCSV;
        private ListView listViewMTR;
        private ColumnHeader chHop;
        private ColumnHeader chAsn;
        private ColumnHeader chHost;
        private ColumnHeader chLoss;
        private ColumnHeader chSent;
        private ColumnHeader chRecv;
        private ColumnHeader chBest;
        private ColumnHeader chAvg;
        private ColumnHeader chWrst;
        private ColumnHeader chLast;
        private ColumnHeader chStDev;
        private ColumnHeader chGeo;
        private NotifyIcon notifyIcon;
        private ContextMenuStrip cmsIcon;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem showToolStripMenuItem;
        private ToolStripStatusLabel toolStripStatusLabelHostIP;
    }
}