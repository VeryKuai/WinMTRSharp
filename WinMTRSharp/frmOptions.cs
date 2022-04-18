using QQWry;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace WinMTRSharp
{
    public partial class frmOptions : Form
    {
        public frmOptions()
        {
            InitializeComponent();
        }

        private void frmOptions_Load(object sender, EventArgs e)
        {
            icoLogo.Image = this.Icon.ToBitmap();

            txtTimeout.Text = MtrOptions.Timeout.ToString();
            txtInterval.Text = MtrOptions.Interval.ToString();
            txtMaxHops.Text = MtrOptions.HopLimit.ToString();
            txtPacketSize.Text = MtrOptions.PacketSize.ToString();
            txtCountSize.Text = MtrOptions.CountSize.ToString();
            chkEnableGeoIP.Checked = MtrOptions.EnableGeoIP;
            chkEnableGeoIP.Enabled = QQWryLocator.IsOkay;
            chkNoDns.Checked = MtrOptions.NoDns;
            chkAsLookup.Checked = MtrOptions.AsLookup;
        }
        private void lblLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"https://www.verykuai.com");
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            int number;
            bool success;

            success = int.TryParse(txtTimeout.Text.Trim(), out number);
            if (success && number >= 1000 && number <= 60 * 1000)
                MtrOptions.Timeout = number;
            else goto onErr;

            success = int.TryParse(txtInterval.Text.Trim(), out number);
            if (success && number >= 100 && number <= 5 * 1000)
                MtrOptions.Interval = number;
            else goto onErr;

            success = int.TryParse(txtMaxHops.Text.Trim(), out number);
            if (success && number >= 1 && number <= 255)
                MtrOptions.HopLimit = number;
            else goto onErr;

            success = int.TryParse(txtPacketSize.Text.Trim(), out number);
            if (success && number >= 18 && number <= 1472)
                MtrOptions.PacketSize = number;
            else goto onErr;

            success = int.TryParse(txtCountSize.Text.Trim(), out number);
            if (success && number >= 10 && number <= 10000)
                MtrOptions.CountSize = number;
            else goto onErr;

            MtrOptions.EnableGeoIP = chkEnableGeoIP.Checked;
            MtrOptions.NoDns = chkNoDns.Checked;
            MtrOptions.AsLookup = chkAsLookup.Checked;

            (this.Owner as frmMain).changeColumn();
            Close();
            return;

        onErr:
            MessageBox.Show("错误的参数!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void txtTimeout_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
        {
            int number;
            bool success = int.TryParse(txtTimeout.Text.Trim(), out number);

            if (!success || number < 1000 || number > 60 * 1000)
            {
                string toolTipText = "超时时间范围: 1000 - 60000 (毫秒)";
                toolTip.ToolTipTitle = "错误的超时时间";
                toolTip.SetToolTip(txtTimeout, toolTipText);
                toolTip.Show(toolTipText, txtTimeout, 5000);
            }
        }
        private void txtInterval_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
        {
            int number;
            bool success = int.TryParse(txtInterval.Text.Trim(), out number);

            if (!success || number < 100 || number > 5 * 1000)
            {
                string toolTipText = "间隔范围: 100 - 5000 (毫秒)";
                toolTip.ToolTipTitle = "错误的间隔";
                toolTip.SetToolTip(txtInterval, toolTipText);
                toolTip.Show(toolTipText, txtInterval, 5000);
            }
        }
        private void txtMaxHops_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
        {
            int number;
            bool success = int.TryParse(txtMaxHops.Text.Trim(), out number);

            if (!success || number < 1 || number > 255)
            {
                string toolTipText = "最大跃点数范围: 1 - 255";
                toolTip.ToolTipTitle = "错误的最大跃点数";
                toolTip.SetToolTip(txtMaxHops, toolTipText);
                toolTip.Show(toolTipText, txtMaxHops, 5000);
            }
        }
        private void txtPacketSize_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
        {
            int number;
            bool success = int.TryParse(txtPacketSize.Text.Trim(), out number);

            if (!success || number < 18 || number > 1472)
            {
                string toolTipText = "数据包大小范围: 18 - 1472 (字节)";
                toolTip.ToolTipTitle = "错误的数据包大小";
                toolTip.SetToolTip(txtPacketSize, toolTipText);
                toolTip.Show(toolTipText, txtPacketSize, 5000);
            }
        }
        private void txtCountSize_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
        {
            int number;
            bool success = int.TryParse(txtCountSize.Text.Trim(), out number);

            if (!success || number < 10 || number > 10000)
            {
                string toolTipText = "最大测试次数范围: 10 - 10000";
                toolTip.ToolTipTitle = "错误的最大测试次数";
                toolTip.SetToolTip(txtCountSize, toolTipText);
                toolTip.Show(toolTipText, txtCountSize, 5000);
            }
        }
        private void btnDefault_Click(object sender, EventArgs e)
        {
            txtTimeout.Text = "2000";
            txtInterval.Text = "500";
            txtMaxHops.Text = "30";
            txtCountSize.Text = "1000";
            txtPacketSize.Text = "64";

            chkEnableGeoIP.Enabled = QQWryLocator.IsOkay;
            chkEnableGeoIP.Checked = chkEnableGeoIP.Enabled;
            chkNoDns.Checked = true;
            chkAsLookup.Checked = false;
        }
    }
}

