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
    public partial class AboutFrm : Form
    {
        public AboutFrm()
        {
            InitializeComponent();
            SetLanguage();
            SetMachineCode();
        }   

        private void SetLanguage()
        {
            this.Text = ServerConfig.getInstance().Language ? "About PMS Service Status" : "关于 PMS 服务状态";
            lblPMSServiceStatusVersion.Text = ServerConfig.getInstance().Language ? "PMS Service Status, Version 3.9.1.5094" : "PMS 服务状态，版本 3.9.1.5094";
            lblCopyright.Text = ServerConfig.getInstance().Language ? "Copyright(C) 2003 - 2022 by Baosight Corporation" : "宝信公司版权所有(C) 2003 - 2022";
            lblURL.Text = ServerConfig.getInstance().Language ? "URL：" : "网址：";
            lblMachineCode.Text = ServerConfig.getInstance().Language ? "Machine Code :" : "机器码：";
            lblWarning.Text = ServerConfig.getInstance().Language ? "Warning: this computer program is protected by copyright law and international treaties. Any unauthorized reproduction or distribution of this program may result in severe civil and criminal penalties." : "警告 \n 此计算机程序受版权法和国际条约保护\n未经授权擅自复制或传播本程序可能导致严厉的民事和刑事处罚";
        }

        private void SetMachineCode()
        {
            txtMachineCodeValue.Text = PMSServer.Instance.MachineCode;
        }

        private void linkLabelURL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string linkdata1 = "http://www.baosight.com";
            System.Diagnostics.Process.Start(linkdata1);
        }
    }
}
