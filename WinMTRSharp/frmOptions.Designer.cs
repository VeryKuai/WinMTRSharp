using System.Windows.Forms;

namespace WinMTRSharp
{
    partial class frmOptions
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOptions));
            this.gbAbout = new System.Windows.Forms.GroupBox();
            this.icoLogo = new System.Windows.Forms.PictureBox();
            this.lblInfo1 = new System.Windows.Forms.Label();
            this.lblInfo2 = new System.Windows.Forms.Label();
            this.lblInfo3 = new System.Windows.Forms.Label();
            this.lblLink = new System.Windows.Forms.LinkLabel();
            this.gbSettings = new System.Windows.Forms.GroupBox();
            this.chkAsLookup = new System.Windows.Forms.CheckBox();
            this.chkNoDns = new System.Windows.Forms.CheckBox();
            this.lblTimeout = new System.Windows.Forms.Label();
            this.txtTimeout = new System.Windows.Forms.MaskedTextBox();
            this.lblInterval = new System.Windows.Forms.Label();
            this.txtInterval = new System.Windows.Forms.MaskedTextBox();
            this.lblMaxHops = new System.Windows.Forms.Label();
            this.txtMaxHops = new System.Windows.Forms.MaskedTextBox();
            this.lblCountSize = new System.Windows.Forms.Label();
            this.txtCountSize = new System.Windows.Forms.MaskedTextBox();
            this.lblPacketSize = new System.Windows.Forms.Label();
            this.txtPacketSize = new System.Windows.Forms.MaskedTextBox();
            this.chkEnableGeoIP = new System.Windows.Forms.CheckBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnDefault = new System.Windows.Forms.Button();
            this.gbAbout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.icoLogo)).BeginInit();
            this.gbSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbAbout
            // 
            this.gbAbout.Controls.Add(this.icoLogo);
            this.gbAbout.Controls.Add(this.lblInfo1);
            this.gbAbout.Controls.Add(this.lblInfo2);
            this.gbAbout.Controls.Add(this.lblInfo3);
            this.gbAbout.Controls.Add(this.lblLink);
            this.gbAbout.Location = new System.Drawing.Point(12, 5);
            this.gbAbout.Name = "gbAbout";
            this.gbAbout.Size = new System.Drawing.Size(411, 98);
            this.gbAbout.TabIndex = 0;
            this.gbAbout.TabStop = false;
            // 
            // icoLogo
            // 
            this.icoLogo.Location = new System.Drawing.Point(6, 12);
            this.icoLogo.Name = "icoLogo";
            this.icoLogo.Size = new System.Drawing.Size(32, 32);
            this.icoLogo.TabIndex = 0;
            this.icoLogo.TabStop = false;
            this.icoLogo.WaitOnLoad = true;
            // 
            // lblInfo1
            // 
            this.lblInfo1.AutoSize = true;
            this.lblInfo1.Location = new System.Drawing.Point(42, 12);
            this.lblInfo1.Name = "lblInfo1";
            this.lblInfo1.Size = new System.Drawing.Size(41, 12);
            this.lblInfo1.TabIndex = 1;
            this.lblInfo1.Text = "WinMTR";
            // 
            // lblInfo2
            // 
            this.lblInfo2.AutoSize = true;
            this.lblInfo2.Location = new System.Drawing.Point(42, 28);
            this.lblInfo2.Name = "lblInfo2";
            this.lblInfo2.Size = new System.Drawing.Size(245, 12);
            this.lblInfo2.TabIndex = 2;
            this.lblInfo2.Text = "适用于 Windows 的 traceroute && ping 工具";
            // 
            // lblInfo3
            // 
            this.lblInfo3.AutoSize = true;
            this.lblInfo3.Location = new System.Drawing.Point(104, 57);
            this.lblInfo3.Name = "lblInfo3";
            this.lblInfo3.Size = new System.Drawing.Size(227, 12);
            this.lblInfo3.TabIndex = 3;
            this.lblInfo3.Text = "VeryKuai VK加速器 - 只做极限延迟效果!";
            this.lblInfo3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblLink
            // 
            this.lblLink.AutoSize = true;
            this.lblLink.Location = new System.Drawing.Point(161, 73);
            this.lblLink.Name = "lblLink";
            this.lblLink.Size = new System.Drawing.Size(101, 12);
            this.lblLink.TabIndex = 4;
            this.lblLink.TabStop = true;
            this.lblLink.Text = "www.verykuai.com";
            this.lblLink.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblLink_LinkClicked);
            // 
            // gbSettings
            // 
            this.gbSettings.Controls.Add(this.chkAsLookup);
            this.gbSettings.Controls.Add(this.chkNoDns);
            this.gbSettings.Controls.Add(this.lblTimeout);
            this.gbSettings.Controls.Add(this.txtTimeout);
            this.gbSettings.Controls.Add(this.lblInterval);
            this.gbSettings.Controls.Add(this.txtInterval);
            this.gbSettings.Controls.Add(this.lblMaxHops);
            this.gbSettings.Controls.Add(this.txtMaxHops);
            this.gbSettings.Controls.Add(this.lblCountSize);
            this.gbSettings.Controls.Add(this.txtCountSize);
            this.gbSettings.Controls.Add(this.lblPacketSize);
            this.gbSettings.Controls.Add(this.txtPacketSize);
            this.gbSettings.Controls.Add(this.chkEnableGeoIP);
            this.gbSettings.Location = new System.Drawing.Point(12, 103);
            this.gbSettings.Name = "gbSettings";
            this.gbSettings.Size = new System.Drawing.Size(411, 132);
            this.gbSettings.TabIndex = 1;
            this.gbSettings.TabStop = false;
            // 
            // chkAsLookup
            // 
            this.chkAsLookup.AutoSize = true;
            this.chkAsLookup.Location = new System.Drawing.Point(208, 101);
            this.chkAsLookup.Name = "chkAsLookup";
            this.chkAsLookup.Size = new System.Drawing.Size(120, 16);
            this.chkAsLookup.TabIndex = 11;
            this.chkAsLookup.Text = "显示自治系统编号";
            this.chkAsLookup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkAsLookup.UseVisualStyleBackColor = true;
            // 
            // chkNoDns
            // 
            this.chkNoDns.AutoSize = true;
            this.chkNoDns.Checked = true;
            this.chkNoDns.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNoDns.Location = new System.Drawing.Point(208, 74);
            this.chkNoDns.Name = "chkNoDns";
            this.chkNoDns.Size = new System.Drawing.Size(168, 16);
            this.chkNoDns.TabIndex = 11;
            this.chkNoDns.Text = "不解析追踪结果中的主机名";
            this.chkNoDns.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkNoDns.UseVisualStyleBackColor = true;
            // 
            // lblTimeout
            // 
            this.lblTimeout.AutoSize = true;
            this.lblTimeout.Location = new System.Drawing.Point(6, 21);
            this.lblTimeout.Name = "lblTimeout";
            this.lblTimeout.Size = new System.Drawing.Size(95, 12);
            this.lblTimeout.TabIndex = 0;
            this.lblTimeout.Text = "超时时间(毫秒):";
            // 
            // txtTimeout
            // 
            this.txtTimeout.Location = new System.Drawing.Point(106, 18);
            this.txtTimeout.Name = "txtTimeout";
            this.txtTimeout.PromptChar = ' ';
            this.txtTimeout.Size = new System.Drawing.Size(75, 21);
            this.txtTimeout.TabIndex = 1;
            this.txtTimeout.Text = "2000";
            this.txtTimeout.ValidatingType = typeof(int);
            this.txtTimeout.TypeValidationCompleted += new System.Windows.Forms.TypeValidationEventHandler(this.txtTimeout_TypeValidationCompleted);
            // 
            // lblInterval
            // 
            this.lblInterval.AutoSize = true;
            this.lblInterval.Location = new System.Drawing.Point(6, 48);
            this.lblInterval.Name = "lblInterval";
            this.lblInterval.Size = new System.Drawing.Size(71, 12);
            this.lblInterval.TabIndex = 2;
            this.lblInterval.Text = "间隔(毫秒):";
            // 
            // txtInterval
            // 
            this.txtInterval.Location = new System.Drawing.Point(106, 45);
            this.txtInterval.Name = "txtInterval";
            this.txtInterval.PromptChar = ' ';
            this.txtInterval.Size = new System.Drawing.Size(75, 21);
            this.txtInterval.TabIndex = 3;
            this.txtInterval.Text = "500";
            this.txtInterval.ValidatingType = typeof(int);
            this.txtInterval.TypeValidationCompleted += new System.Windows.Forms.TypeValidationEventHandler(this.txtInterval_TypeValidationCompleted);
            // 
            // lblMaxHops
            // 
            this.lblMaxHops.AutoSize = true;
            this.lblMaxHops.Location = new System.Drawing.Point(6, 75);
            this.lblMaxHops.Name = "lblMaxHops";
            this.lblMaxHops.Size = new System.Drawing.Size(71, 12);
            this.lblMaxHops.TabIndex = 4;
            this.lblMaxHops.Text = "最大跃点数:";
            // 
            // txtMaxHops
            // 
            this.txtMaxHops.Location = new System.Drawing.Point(106, 72);
            this.txtMaxHops.Name = "txtMaxHops";
            this.txtMaxHops.PromptChar = ' ';
            this.txtMaxHops.Size = new System.Drawing.Size(75, 21);
            this.txtMaxHops.TabIndex = 5;
            this.txtMaxHops.Text = "30";
            this.txtMaxHops.ValidatingType = typeof(int);
            this.txtMaxHops.TypeValidationCompleted += new System.Windows.Forms.TypeValidationEventHandler(this.txtMaxHops_TypeValidationCompleted);
            // 
            // lblCountSize
            // 
            this.lblCountSize.AutoSize = true;
            this.lblCountSize.Location = new System.Drawing.Point(206, 21);
            this.lblCountSize.Name = "lblCountSize";
            this.lblCountSize.Size = new System.Drawing.Size(83, 12);
            this.lblCountSize.TabIndex = 8;
            this.lblCountSize.Text = "最大测试次数:";
            // 
            // txtCountSize
            // 
            this.txtCountSize.Location = new System.Drawing.Point(306, 18);
            this.txtCountSize.Name = "txtCountSize";
            this.txtCountSize.PromptChar = ' ';
            this.txtCountSize.Size = new System.Drawing.Size(75, 21);
            this.txtCountSize.TabIndex = 9;
            this.txtCountSize.Text = "1000";
            this.txtCountSize.ValidatingType = typeof(int);
            this.txtCountSize.TypeValidationCompleted += new System.Windows.Forms.TypeValidationEventHandler(this.txtCountSize_TypeValidationCompleted);
            // 
            // lblPacketSize
            // 
            this.lblPacketSize.AutoSize = true;
            this.lblPacketSize.Location = new System.Drawing.Point(6, 102);
            this.lblPacketSize.Name = "lblPacketSize";
            this.lblPacketSize.Size = new System.Drawing.Size(71, 12);
            this.lblPacketSize.TabIndex = 6;
            this.lblPacketSize.Text = "数据包大小:";
            // 
            // txtPacketSize
            // 
            this.txtPacketSize.Location = new System.Drawing.Point(106, 99);
            this.txtPacketSize.Name = "txtPacketSize";
            this.txtPacketSize.PromptChar = ' ';
            this.txtPacketSize.Size = new System.Drawing.Size(75, 21);
            this.txtPacketSize.TabIndex = 7;
            this.txtPacketSize.Text = "64";
            this.txtPacketSize.ValidatingType = typeof(int);
            this.txtPacketSize.TypeValidationCompleted += new System.Windows.Forms.TypeValidationEventHandler(this.txtPacketSize_TypeValidationCompleted);
            // 
            // chkEnableGeoIP
            // 
            this.chkEnableGeoIP.AutoSize = true;
            this.chkEnableGeoIP.Checked = true;
            this.chkEnableGeoIP.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnableGeoIP.Location = new System.Drawing.Point(208, 47);
            this.chkEnableGeoIP.Name = "chkEnableGeoIP";
            this.chkEnableGeoIP.Size = new System.Drawing.Size(120, 16);
            this.chkEnableGeoIP.TabIndex = 10;
            this.chkEnableGeoIP.Text = "查询地理位置信息";
            this.chkEnableGeoIP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkEnableGeoIP.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(267, 241);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 21);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "确定(&O)";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(348, 241);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 21);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消(&C)";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // toolTip
            // 
            this.toolTip.IsBalloon = true;
            this.toolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Error;
            // 
            // btnDefault
            // 
            this.btnDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDefault.Location = new System.Drawing.Point(12, 241);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(75, 21);
            this.btnDefault.TabIndex = 4;
            this.btnDefault.Text = "默认值(&D)";
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // frmOptions
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(435, 273);
            this.Controls.Add(this.btnDefault);
            this.Controls.Add(this.gbAbout);
            this.Controls.Add(this.gbSettings);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = Properties.Resources.WinMTR;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOptions";
            this.Padding = new System.Windows.Forms.Padding(9, 8, 9, 8);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "选项";
            this.Load += new System.EventHandler(this.frmOptions_Load);
            this.gbAbout.ResumeLayout(false);
            this.gbAbout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.icoLogo)).EndInit();
            this.gbSettings.ResumeLayout(false);
            this.gbSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox gbAbout;
        private PictureBox icoLogo;
        private Label lblInfo1;
        private Label lblInfo2;
        private Label lblInfo3;
        private LinkLabel lblLink;
        private GroupBox gbSettings;
        private Label lblTimeout;
        private Label lblInterval;
        private Label lblMaxHops;
        private Label lblPacketSize;
        private Label lblCountSize;
        private MaskedTextBox txtTimeout;
        private MaskedTextBox txtInterval;
        private MaskedTextBox txtMaxHops;
        private MaskedTextBox txtCountSize;
        private MaskedTextBox txtPacketSize;
        private CheckBox chkEnableGeoIP;
        private Button btnOk;
        private Button btnCancel;
        private ToolTip toolTip;
        private Button btnDefault;
        private CheckBox chkNoDns;
        private CheckBox chkAsLookup;
    }
}
