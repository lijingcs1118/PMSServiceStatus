using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PMSServiceStatus
{
    public partial class ProgressFrm : Form
    {
        bool isStart;
        public ProgressFrm()
        {
            InitializeComponent();
            // 设置不出现关闭按钮
            this.ControlBox = false;
        }

        public ProgressFrm(bool isstart)
        {
            InitializeComponent();
            // 设置不出现关闭按钮
            this.ControlBox = false;
            isStart = isstart;
            this.backgroundWorker1.RunWorkerAsync();    
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            backgroundWorker1.WorkerReportsProgress = true;

            if (isStart)
            {
                try
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke(new MethodInvoker((GenerateStartTitle)));
                    }
                    else
                    {
                        GenerateStartTitle();
                    }
                }
                catch (Exception ex)
                {
                    PMSServer.log.Fatal("GenerateStartTitle" + ex);
                }

                backgroundWorker1.ReportProgress(10);
                PMSServer.Instance.InitServer();
                for (int index = 11; index <= 50; index++)
                {
                    backgroundWorker1.ReportProgress(index);
                    Thread.Sleep(50);
                }
                
                int loadInfoIndex = 50;
                do
                {
                    loadInfoIndex++;
                    if(loadInfoIndex <= 80)
                        backgroundWorker1.ReportProgress(loadInfoIndex);
                    Thread.Sleep(50);
                } while (!PMSServer.Instance.isAlreadyLoadServerInfo);

                backgroundWorker1.ReportProgress(80);
                for (int i = 80; i <= 100; i++)
                {
                    //PMSServer.Instance.StartService(true);
                    //调用 ReportProgress 方法，会触发ProgressChanged事件
                    backgroundWorker1.ReportProgress(i, String.Format("{0}%", i));
                    Thread.Sleep(10);
                }
            }
            else
            {
                if(this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(() => this.Text = ServerConfig.getInstance().Language ? "Stopping PMS Service" : "正在停止数据采集服务，请稍候..."));
                }
                else
                {
                    this.Text = ServerConfig.getInstance().Language ? "Stopping PMS Service" : "正在停止数据采集服务，请稍候...";
                }
                
                backgroundWorker1.ReportProgress(10);
                PMSServer.Instance.PMSService.stopService();
                backgroundWorker1.ReportProgress(80);
                for (int i = 80; i <= 100; i++)
                {
                    //PMSServer.Instance.StartService(true);
                    //调用 ReportProgress 方法，会触发ProgressChanged事件
                    backgroundWorker1.ReportProgress(i, String.Format("{0}%", i));
                    System.Threading.Thread.Sleep(10);
                }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
        }

        /// <summary>
        /// 完成后关闭FORM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }

        private void GenerateStartTitle()
        {
            this.Text = ServerConfig.getInstance().Language ? "Starting PMS Service" : "数据采集服务启动中，请稍候...";
        }

        private void GenerateStopTitle()
        {
            this.Text = ServerConfig.getInstance().Language ? "Stopping PMS Service" : "正在停止数据采集服务，请稍候...";
        }
    }
}
