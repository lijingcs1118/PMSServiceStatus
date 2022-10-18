using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PMSServiceStatus
{
    public partial class CloseFrm : Form
    {
        public CloseFrm()
        {
            InitializeComponent();
            SetLanguage();
        }

        private void SetLanguage()
        {
            this.Text = ServerConfig.getInstance().Language ? "Prompt" : "提示";
            lblPrompt.Text = ServerConfig.getInstance().Language ? "Are you sure you want to exit?" : "确定要退出么？";
            btnMinimumBox.Text = ServerConfig.getInstance().Language ? "Minimize to tray" : "最小化到托盘";
            btnExit.Text = ServerConfig.getInstance().Language ? "Exit application" : "直接退出";
        }

        private void btnMinimumBox_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// 按ESC关闭窗体
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            int WM_KEYDOWN = 256;
            int WM_SYSKEYDOWN = 260;

            if (msg.Msg == WM_KEYDOWN | msg.Msg == WM_SYSKEYDOWN)
            {
                switch (keyData)
                {
                    case Keys.Escape:
                        this.Close();
                        break;
                }
            }
            return false;
        }
    }
}
