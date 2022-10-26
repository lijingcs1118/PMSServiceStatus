using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using MakarovDev.ExpandCollapsePanel;
using pmsServiceLib;
using PMSServiceStatus.Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Baosight.FDAA.PackageDiagnosis;
using Baosight.FDAA.PackageDiagnosis.BLL.Packages;
using Baosight.FDAA.PackageDiagnosis.Model.Attributes;
using Baosight.FDAA.PackageDiagnosis.Model.Entities;
using Timer = System.Threading.Timer;

namespace PMSServiceStatus
{
    public partial class MainFrm : Form
    {
        #region 默认右键添加自定义按钮（关于）
        // P/Invoke constants
        private const int WM_SYSCOMMAND = 0x112;
        private const int MF_STRING = 0x0;
        private const int MF_SEPARATOR = 0x800;

        const int SC_MINIMIZE = 0xF020;

        // P/Invoke declarations
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool AppendMenu(IntPtr hMenu, int uFlags, int uIDNewItem, string lpNewItem);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool InsertMenu(IntPtr hMenu, int uPosition, int uFlags, int uIDNewItem, string lpNewItem);


        // ID for the About item on the system menu
        private int SYSMENU_ABOUT_ID = 0x1;

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            // Get a handle to a copy of this form's system (window) menu
            IntPtr hSysMenu = GetSystemMenu(this.Handle, false);

            // Add a separator
            AppendMenu(hSysMenu, MF_SEPARATOR, 0, string.Empty);

            // Add the About menu item
            AppendMenu(hSysMenu, MF_STRING, SYSMENU_ABOUT_ID, ServerConfig.getInstance().Language ? "&About PMSServiceStatus..." : "关于 PMSServiceStatus(&A)...");//"&About…""&关于 PMSServiceStatus(&A)..."
        }

        protected override void WndProc(ref Message m)
        {
            if (m.WParam.ToInt32() == SC_MINIMIZE) //是否点击最小化
            {
                this.Hide();
                return ;
            }

            base.WndProc(ref m);

            // Test if the About item was selected from the system menu
            if ((m.Msg == WM_SYSCOMMAND) && ((int)m.WParam == SYSMENU_ABOUT_ID))
            {
                AboutFrm aboutFrm = new AboutFrm();
                aboutFrm.ShowDialog();
            }
        }
        #endregion

        #region MessageBox窗体居中
        class CenterWinDialog : IDisposable
        {
            private int mTries = 0;
            private Form mOwner;

            public CenterWinDialog(Form owner)
            {
                mOwner = owner;
                owner.BeginInvoke(new MethodInvoker(findDialog));
            }

            private void findDialog()
            {
                // Enumerate windows to find the message box
                if (mTries < 0) return;
                EnumThreadWndProc callback = new EnumThreadWndProc(checkWindow);
                if (EnumThreadWindows(GetCurrentThreadId(), callback, IntPtr.Zero))
                {
                    if (++mTries < 10) mOwner.BeginInvoke(new MethodInvoker(findDialog));
                }
            }
            private bool checkWindow(IntPtr hWnd, IntPtr lp)
            {
                // Checks if <hWnd> is a dialog
                StringBuilder sb = new StringBuilder(260);
                GetClassName(hWnd, sb, sb.Capacity);
                if (sb.ToString() != "#32770") return true;
                // Got it
                Rectangle frmRect = new Rectangle(mOwner.Location, mOwner.Size);
                RECT dlgRect;
                GetWindowRect(hWnd, out dlgRect);
                MoveWindow(hWnd,
                    frmRect.Left + (frmRect.Width - dlgRect.Right + dlgRect.Left) / 2,
                    frmRect.Top + (frmRect.Height - dlgRect.Bottom + dlgRect.Top) / 2,
                    dlgRect.Right - dlgRect.Left,
                    dlgRect.Bottom - dlgRect.Top, true);
                return false;
            }
            public void Dispose()
            {
                mTries = -1;
            }

            // P/Invoke declarations
            private delegate bool EnumThreadWndProc(IntPtr hWnd, IntPtr lp);
            [DllImport("user32.dll")]
            private static extern bool EnumThreadWindows(int tid, EnumThreadWndProc callback, IntPtr lp);
            [DllImport("kernel32.dll")]
            private static extern int GetCurrentThreadId();
            [DllImport("user32.dll")]
            private static extern int GetClassName(IntPtr hWnd, StringBuilder buffer, int buflen);
            [DllImport("user32.dll")]
            private static extern bool GetWindowRect(IntPtr hWnd, out RECT rc);
            [DllImport("user32.dll")]
            private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int w, int h, bool repaint);
            private struct RECT { public int Left; public int Top; public int Right; public int Bottom; }
        }
        #endregion
        
        //  托盘状态
        enum TrayStatus
        {
            ServiceUnknown = 0,
            ServiceStopped = 1,
            ServiceRunningAcquisitionStopped = 2,
            AcuisitionStarted = 3
        };

        //  内部循环计数：时间/计数
        int currentInterruptsComboxIndex;

        //  当前模块是否是普通模块
        bool isDisplaySignalValues;

        //  FORM的Title
        string strMessageBoxTitle = string.Empty;

        //  IO页同时最大刷新多少个信号
        const int MaxRowsCount = 50;

        //  服务配置文件路径
        //string serverConfigName = @"C:\FDAA\Server\config\ServerConfig.xml";
        string serverConfigName = System.Windows.Forms.Application.StartupPath + @"\config\ServerConfig.xml";

        //  IO配置文件路径        
        string IOConfigName = System.Windows.Forms.Application.StartupPath + @"\config\CurrentIoConfig.io";

        //  控制刷新页面数据的频率
        static long times = 0;

        //  是否从托盘关闭
        bool closeFromTray = false;

        //  映射模块偏移量
        const ushort MappingOffset = 30000;

        //  循环中断计数器状态
        bool CyclicInterruptsAbnormal = false;
        
        //  是否处理过异常
        bool AlreadyHandle = false;

        //  未开始
        private string notStarted = string.Empty;

        //  导出按钮Text
        string strExportError = string.Empty;

        //  一件诊断Button Text
        private string strOneClickDiagnostic = string.Empty;

        //  诊断状态栏显示的文字
        private string diagnosticCompleted;

        //  诊断中
        private string diagnosing;

        private string strReady;

        private string strDiagnoseServiceNotRunning = string.Empty;

        private string strDiagnoseAcquisitionNotStarted = string.Empty;

        private List<ExpandCollapsePanel> panelList;

        private readonly object progressbarLocker = new Object();

        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public MainFrm()
        {
            InitializeComponent();

            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);
            ServerConfig.getInstance().LoadServerConfig(serverConfigName);
            IOConfig.getInstance().LoadIOConfig(IOConfigName);

            PMSServer.Instance.PMSServiceChangedEventHandler += OnServiceStatusChanged;
            PMSServer.Instance.PMSAcquisitionChangedEventHandler += OnAcquisitionChanged; 
            PMSServer.Instance.ClientStatusChangedEventHandler += UpdateOnlineClients;

            SetLanguage();
            InitControls();
            InitControlHalfDynamicData();
            UpdateFrameLostConfig();
            UpdateDataTimer.Start();
            ScrollStart();
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            try
            {
                if (notifyIcon1 != null)
                {
                    notifyIcon1.Visible = false;
                    notifyIcon1.Icon = null; // required to make icon disappear
                    notifyIcon1.Dispose();
                    notifyIcon1 = null;
                }

            }
            catch (Exception ex)
            {
                // handle the error
            }
        }

        #region 初始化及服务状态信息同步更新
        public void SetLanguage()
        {
            tabControlMain.TabPages[0].Text = ServerConfig.getInstance().Language ? "General" : "常规";
            tabControlMain.TabPages[1].Text = ServerConfig.getInstance().Language ? "IO Status" : "IO 状态";
            tabControlMain.TabPages[2].Text = ServerConfig.getInstance().Language ? "Diagnostic" : "诊断";
            strMessageBoxTitle = ServerConfig.getInstance().Language ? "PMS Service Status" : "采集服务状态";

            //  常规页双语
            groupBoxService.Text = ServerConfig.getInstance().Language ? "Service" : "服务";
            lblServiceName.Text = ServerConfig.getInstance().Language ? "Service Name :" : "服务名 :";
            lblExecutableFilePath.Text = ServerConfig.getInstance().Language ? "Executable File Path :" : "可执行文件路径 :";
            chkBoxAutostartService.Text = ServerConfig.getInstance().Language ? " Auto-start service when windows starts" : " Windows启动时自动启动服务";
            lblServiceStatus.Text = ServerConfig.getInstance().Language ? "Service Status :" : "服务状态监控 :";
            btnServiceStatus.Text = ServerConfig.getInstance().Language ? "Unknown" : "未知";
            btnStartService.Text = ServerConfig.getInstance().Language ? "Start" : "启动";
            btnStopService.Text = ServerConfig.getInstance().Language ? "Stop" : "停止";
            lblListeningPortNo.Text = ServerConfig.getInstance().Language ? "Listening Port No :" : "侦听端口号 :";
            lblLogPath.Text = ServerConfig.getInstance().Language ? "Log Path :" : "日志路径 :";
            lblLogLevel.Text = ServerConfig.getInstance().Language ? "Log Level :" : "日志等级 :";

            groupBoxAcquisition.Text = ServerConfig.getInstance().Language ? "Acquisition" : "采集";
            lblAcquisitionStatus.Text = ServerConfig.getInstance().Language ? "Acquisition Status :" : "采集状态监控 :";
            btnAcquisitionStatus.Text = ServerConfig.getInstance().Language ? "Unknown" : "未知";
            btnStartAcquisition.Text = ServerConfig.getInstance().Language ? "Start" : "启动";
            btnStopAcquisition.Text = ServerConfig.getInstance().Language ? "Stop" : "停止";
            chkAutoStartAcquisition.Text = ServerConfig.getInstance().Language ? "Auto-start data acquisition when service starts" : "服务启动时自动启动数据采集";
            lblDriverInfo.Text = ServerConfig.getInstance().Language ? "Driver Info :" : "采集驱动信息";
            lblSoftTimebase.Text = ServerConfig.getInstance().Language ? "Soft Timebase :" : "基准时钟周期 :";
            lblSoftTimebaseUnit.Text = ServerConfig.getInstance().Language ? "ms" : "毫秒";
            lblInternalCyclicInterrupts.Text = ServerConfig.getInstance().Language ? "Internal Cyclic Interrupts :" : "内部循环中断计数 :";

            groupBoxOnlineClient.Text = ServerConfig.getInstance().Language ? "Online Client(s)" : "在线客户端";
            btnDisconnectAllClients.Text = ServerConfig.getInstance().Language ? "Disconnect All Clients" : "断开所有客户端";
            lblClientCount.Text = ServerConfig.getInstance().Language ? "Count :" : "数量 :";

            //  IO状态页双语
            lblSignalTree.Text = ServerConfig.getInstance().Language ? "Signal Tree" : "信号树";
            lblMonitorWindow.Text = ServerConfig.getInstance().Language ? "Monitor Window" : "监视窗口";
            btnLaunchConfigurationTool.Text = ServerConfig.getInstance().Language ? "Launch Configuration Tool" : "加载配置工具";
            tabControlSignalValues.TabPages[0].Text = ServerConfig.getInstance().Language ? "Analog" : "模拟量";
            tabControlSignalValues.TabPages[1].Text = ServerConfig.getInstance().Language ? "Digital" : "开关量";
            tabControlStringValues.TabPages[0].Text = ServerConfig.getInstance().Language ? "Technostring" : "信息字串";

            //  诊断页双语
            tabPage2.Text = ServerConfig.getInstance().Language ? "General" : "常规";
            tabPage3.Text = ServerConfig.getInstance().Language ? "Advanced" : "高级";
            groupBoxAcquisitionHitRate.Text = ServerConfig.getInstance().Language ? "Acquisition Hit Rate" : "采样命中率";
            lblSignalUsed.Text = ServerConfig.getInstance().Language ? "Signal Used:" : "使用的诊断信号:";
            lblMaximumValue.Text = ServerConfig.getInstance().Language ? "Maximum Value:" : "最大值:";
            lblMinimumValue.Text = ServerConfig.getInstance().Language ? "Minimum Value:" : "最小值:";
            lblIncrementValue.Text = ServerConfig.getInstance().Language ? "Increment Value:" : "增量步长:";
            lblAcquisitionHitRate.Text = ServerConfig.getInstance().Language ? "Acquisition Hit Rate:" : "采样命中率:";
            btnReset.Text = ServerConfig.getInstance().Language ? "Reset" : "重置";
            groupBoxService2.Text = ServerConfig.getInstance().Language ? "Service" : "服务";
            btnAcquisitionServiceDiagnosis.Text = ServerConfig.getInstance().Language ? "Acquisition service diagnosis" : "采集服务状态诊断";
            btnAcquisitionEnvironmentInitialize.Text = ServerConfig.getInstance().Language ? "Acquisition environment initialize" : "采集环境初始化";
            btnExport.Text = ServerConfig.getInstance().Language ? "Export Diagnostic Result" : "导出诊断结果";
            strExportError = ServerConfig.getInstance().Language ? "No Record To Export!" : "没有可以导出的数据！";
            strOneClickDiagnostic = ServerConfig.getInstance().Language ? "One Click Diagnostic" : "一键诊断";
            btnExpand.Text = ServerConfig.getInstance().Language ? "Expand All" : "全部展开";
            btnCollapse.Text = ServerConfig.getInstance().Language ? "Collapse All" : "全部折叠";
            toolStripExpandAll.ToolTipText = ServerConfig.getInstance().Language ? "Expand All" : "全部展开";
            toolStripCollapseAll.ToolTipText = ServerConfig.getInstance().Language ? "Collapse All" : "全部折叠";
            toolStripExport.ToolTipText = ServerConfig.getInstance().Language ? "Export Diagnostic Result" : "导出诊断结果";
            toolStripViewInfo.ToolTipText = ServerConfig.getInstance().Language ? "View installation and license information" : "查看安装授权信息";
            notStarted = ServerConfig.getInstance().Language ? "Not started" : "未开始";
            diagnosticCompleted = ServerConfig.getInstance().Language ? "Diagnostic completed    |    Passing rate :" : "诊断完成    |    通过率 ： ";
            strReady = ServerConfig.getInstance().Language ? "Ready" : "就绪";
            diagnosing = ServerConfig.getInstance().Language ? "In progress" : "诊断中";

            strDiagnoseAcquisitionNotStarted = ServerConfig.getInstance().Language
                ? "One-click diagnosis is available after data acquisition is started"
                : "数据采集启动后，一键诊断方可使用";
            radioButtonDiagnoseConfiguredPackages.Text = ServerConfig.getInstance().Language ? "Diagnose configured packages" : "诊断已配置功能包";
            radioButtonDiagnoseAllPackages.Text = ServerConfig.getInstance().Language ? "Diagnose all packages" : "诊断所有功能包";
            btnOneClickDiagnostic.Text = strOneClickDiagnostic;

            //  托盘双语
            toolStripMenuItemStatus.Text = ServerConfig.getInstance().Language ? "Status(&s)" : "状态(&s)";
            toolStripMenuItemStartservice.Text = ServerConfig.getInstance().Language ? "Start service(&t)" : "启动服务(&t)";
            toolStripMenuItemStopservice.Text = ServerConfig.getInstance().Language ? "Stop service(&p)" : "停止服务(&p)";
            toolStripMenuItemAbout.Text = ServerConfig.getInstance().Language ? "About...(&a)" : "关于...(&a)";
            toolStripMenuItemExit.Text = ServerConfig.getInstance().Language ? "Exit(&x)" : "退出(&x)";

            this.notifyIcon1.Text = ServerConfig.getInstance().Language ? "PMS Service Unknown" : "PMS 服务状态未知";
        }

        private void InitControls()
        {
            //  设置Form Title
            if (PMSServer.Instance.ServerStatus == PMSServer.FDAA_SERVICE_STATUS.UNKNOWN)
            {
                this.Text = (ServerConfig.getInstance().Language
                                    ? "PMS Service Status : Disconnected"
                                    : "PMS 服务状态 : 已断开");
            }
            else
            {
                this.Text = (ServerConfig.getInstance().Language
                                    ? "PMS Service Status : Connected"
                                    : "PMS 服务状态: 已连接");
            }

            //  初始化计数/时间 comboBox
            if (ServerConfig.getInstance().Language)
            {
                comboBoxInternalCyclicInterrupts.Items.AddRange(new object[] {
                "     Count",
                "     Time"});
            }
            else
            {
                comboBoxInternalCyclicInterrupts.Items.AddRange(new object[] {
                "     计数",
                "     时间"});
            }
            comboBoxInternalCyclicInterrupts.Text = comboBoxInternalCyclicInterrupts.Items[0].ToString();
            currentInterruptsComboxIndex = 0;

            //  初始化驱动信息列表控件
            listViewDriverInfo.FullRowSelect = true;
            listViewDriverInfo.Columns.Add(ServerConfig.getInstance().Language ? "Component Name" : "组件名称", ServerConfig.getInstance().Language ? 120 : 110, HorizontalAlignment.Left);
            listViewDriverInfo.Columns.Add(ServerConfig.getInstance().Language ? "Version" : "版本", 75, HorizontalAlignment.Left);
            listViewDriverInfo.Columns.Add(ServerConfig.getInstance().Language ? "License" : "授权", 80, HorizontalAlignment.Left);
            listViewDriverInfo.View = View.Details;

            //  初始化客户端列表控件
            listViewOnlineClient.FullRowSelect = true;
            listViewOnlineClient.Columns.Add("ID", 80, HorizontalAlignment.Left);
            listViewOnlineClient.Columns.Add(ServerConfig.getInstance().Language ? "Name" : "名称", 300, HorizontalAlignment.Left);
            listViewOnlineClient.Columns.Add(ServerConfig.getInstance().Language ? "Connected Since" : "从何时开始连接", 220, HorizontalAlignment.Left);
            listViewOnlineClient.Columns.Add(ServerConfig.getInstance().Language ? "Request Signal Count" : "请求信号数量", 170, HorizontalAlignment.Left);
            listViewOnlineClient.View = View.Details;

            gridViewAnalog.IndicatorWidth = 49;
            gridViewDigital.IndicatorWidth = 49;
            gridViewTecnostring.IndicatorWidth = 49;

            advancedFlowLayoutPanel1.DoubleBufferedFlowLayoutPanel(true);

            toolStripPassRate.Text = strDiagnoseAcquisitionNotStarted;
            btnOneClickDiagnostic.Enabled = false;
            diagnosticProgressText.Visible = false;
        }

        //  如果不加locker，OnServiceStatusChanged和OnAcquisitionChanged有时会同时执行，
        //  会造成执行FillTreeControl函数时IOConfig.AllModules集合已修改，无法进行枚举操作"的错误
        private static readonly object statusChangedLocker = new object();
        /// <summary>
        /// 服务状态发生变化时 UI数据同时更新
        /// </summary>
        /// <param name="serviceStatus">服务状态</param>
        private void OnServiceStatusChanged(PMSServer.FDAA_SERVICE_STATUS serviceStatus)
        {
            lock (statusChangedLocker)
            {
                if (serviceStatus == PMSServer.FDAA_SERVICE_STATUS.UNKNOWN)
                {
                    CleareIOData();
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke(new Action(() => this.Text = (ServerConfig.getInstance().Language
                                        ? "PMS Service Status : Disconnected"
                                        : "PMS 服务状态 : 已断开")));
                    }
                }
                else if (serviceStatus == PMSServer.FDAA_SERVICE_STATUS.RUNNING)
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke(new Action(() => this.Text = (ServerConfig.getInstance().Language
                                        ? "PMS Service Status : Connected"
                                        : "PMS 服务状态: 已连接")));
                    }
                    if (PMSServer.Instance.AcqStatus == PMSServer.FDAA_ACQUISITION_STATUS.STARTED)
                    {
                        setTrayStatus(TrayStatus.AcuisitionStarted);
                        //如果数据采集被重新启动，则重新读取CurrentIoConfig.io配置文件 
                        IOConfig.getInstance().LoadIOConfig(IOConfigName);
                        log.Fatal("FillTreeControl From OnServiceStatusChanged");
                        FillUITreeControl();
                    }
                    else
                    {
                        CleareIOData();
                        setTrayStatus(TrayStatus.ServiceRunningAcquisitionStopped);
                    }
                }
                else if (serviceStatus == PMSServer.FDAA_SERVICE_STATUS.STOPPED)
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke(new Action(() => this.Text = (ServerConfig.getInstance().Language
                                        ? "PMS Service Status : Connected"
                                        : "PMS 服务状态: 已连接")));
                    }
                    CleareIOData();
                    // 更新托盘信息
                    setTrayStatus(TrayStatus.ServiceStopped);
                    // 更新软时钟显示为？？
                    UpdateGeneralData();
                }

                InitControlHalfDynamicData();
                InitControlsDynamicData();
            }
        }

        void OnAcquisitionChanged(PMSServer.FDAA_ACQUISITION_STATUS acquisitionStatus)
        {
            lock(statusChangedLocker)
            {
                if (acquisitionStatus == PMSServer.FDAA_ACQUISITION_STATUS.STARTED)
                {
                    setTrayStatus(TrayStatus.AcuisitionStarted);

                    // 如果数据采集被重新启动，则重新读取CurrentIoConfig.io配置文件 
                    IOConfig.getInstance().LoadIOConfig(IOConfigName);
                    log.Fatal("FillTreeControl From OnAcquisitionChanged");
                    FillUITreeControl();
                    UpdateFrameLostConfig();

                    // 更新基准时钟频率
                    InitControlHalfDynamicData();
                }
                else if (acquisitionStatus == PMSServer.FDAA_ACQUISITION_STATUS.STOPPED)
                {
                    CleareIOData();
                    setTrayStatus(TrayStatus.ServiceRunningAcquisitionStopped);
                }

                SetAcquistionStatus(acquisitionStatus);
            }
        }

        private void InitControlHalfDynamicData()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(InitControlHalfDynamic));//IAsyncResult ar = 
                //this.EndInvoke(ar);
            }
            else
            {
                InitControlHalfDynamic();
            }
        }

        private void InitControlHalfDynamic()
        {
            txtServiceName.Text = PMSServer.Instance.ServiceName;
            txtExecutableFilePath.Text = PMSServer.Instance.ServicePath;
            chkBoxAutostartService.Checked = ServerConfig.getInstance().AutoStartServerOnStartup;
            txtListeningPortNo.Text = ServerConfig.getInstance().ServerPortNr.ToString();
            txtLogPath.Text = ServerConfig.getInstance().LogPath;
            comboBoxLogLevel.Text = Helper.ConvertLogNumberToString(ServerConfig.getInstance().LogLevel);

            chkAutoStartAcquisition.Checked = ServerConfig.getInstance().StartAcquisitionOnStartup;
            txtSoftTimebase.Text = IOConfig.getInstance().SoftTimebase.ToString();

            // Driver list refreshment
            UpdateDriverInfos();
        }

        private void InitControlsDynamicData()
        {
            // 更新服务状态
            SetPMSServiceStatus(PMSServer.Instance.ServerStatus);

            // 更新数据采集状态
            SetAcquistionStatus(PMSServer.Instance.AcqStatus);

            // 更新在线客户端列表
            UpdateOnlineClients();
        }

        private void SetPMSServiceStatus(PMSServer.FDAA_SERVICE_STATUS serverStatus)
        {
            if (serverStatus == PMSServer.FDAA_SERVICE_STATUS.RUNNING)
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(() => btnStartService.Enabled = false));
                    this.BeginInvoke(new Action(() => btnStopService.Enabled = true));

                    this.BeginInvoke(new Action(() => btnServiceStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))))));
                    this.BeginInvoke(new Action(() => btnServiceStatus.Text = ServerConfig.getInstance().Language ? "Running" : "运行中"));

                    this.BeginInvoke(new Action(() => comboBoxLogLevel.Enabled = false));
                    //this.BeginInvoke(new Action(() => txtLogPath.Enabled = false));
                    this.BeginInvoke(new Action(() => txtListeningPortNo.Enabled = false));
                    this.BeginInvoke(new Action(() => chkBoxAutostartService.Enabled = false));
                    this.BeginInvoke(new Action(() => chkAutoStartAcquisition.Enabled = false));
                }
                else
                {
                    // 禁止使用开始按钮
                    btnStartService.Enabled = false;
                    // 可以使用停止按钮
                    btnStopService.Enabled = true;

                    // 绿色显示
                    btnServiceStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
                    btnServiceStatus.Text = ServerConfig.getInstance().Language ? "Running" : "运行中";

                    // 禁止那些可以编辑的控件
                    comboBoxLogLevel.Enabled = false;
                    //txtLogPath.Enabled = false;
                    txtListeningPortNo.Enabled = false;
                    chkBoxAutostartService.Enabled = false;
                    chkAutoStartAcquisition.Enabled = false;
                }
            }
            else if (serverStatus == PMSServer.FDAA_SERVICE_STATUS.STOPPED)
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(() => btnStartService.Enabled = true));
                    this.BeginInvoke(new Action(() => btnStopService.Enabled = false));

                    this.BeginInvoke(new Action(() => btnServiceStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))));
                    this.BeginInvoke(new Action(() => btnServiceStatus.Text = ServerConfig.getInstance().Language ? "Stopped" : "已停止"));

                    this.BeginInvoke(new Action(() => comboBoxLogLevel.Enabled = true));
                    //this.BeginInvoke(new Action(() => txtLogPath.Enabled = true));
                    this.BeginInvoke(new Action(() => txtListeningPortNo.Enabled = true));
                    this.BeginInvoke(new Action(() => chkBoxAutostartService.Enabled = true));
                    this.BeginInvoke(new Action(() => chkAutoStartAcquisition.Enabled = true));
                }
                else
                {
                    // 禁止使用开始按钮
                    btnStartService.Enabled = true;
                    // 可以使用停止按钮
                    btnStopService.Enabled = false;

                    // 绿色显示
                    btnServiceStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                    btnServiceStatus.Text = ServerConfig.getInstance().Language ? "Stopped" : "已停止";

                    // 禁止那些可以编辑的控件
                    comboBoxLogLevel.Enabled = true;
                    //txtLogPath.Enabled = true;
                    txtListeningPortNo.Enabled = true;
                    chkBoxAutostartService.Enabled = true;
                    chkAutoStartAcquisition.Enabled = true;

                    // When service is stopped, disable 'start acquisition' command
                    SetAcquistionStatus(PMSServer.FDAA_ACQUISITION_STATUS.UNAVAILABLE);
                }
            }
            else	// FDAA_SERVICE_UNKNOWN
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(() => btnStartService.Enabled = false));
                    this.BeginInvoke(new Action(() => btnStopService.Enabled = false));

                    this.BeginInvoke(new Action(() => btnServiceStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))))));
                    this.BeginInvoke(new Action(() => btnServiceStatus.Text = ServerConfig.getInstance().Language ? "Unknown" : "未知"));

                    this.BeginInvoke(new Action(() => comboBoxLogLevel.Enabled = true));
                    //this.BeginInvoke(new Action(() => txtLogPath.Enabled = true));
                    this.BeginInvoke(new Action(() => txtListeningPortNo.Enabled = true));
                    this.BeginInvoke(new Action(() => chkBoxAutostartService.Enabled = true));
                    this.BeginInvoke(new Action(() => chkAutoStartAcquisition.Enabled = true));

                    this.BeginInvoke(new Action(() => txtServiceName.Text = "??"));
                    this.BeginInvoke(new Action(() => txtExecutableFilePath.Text = "??"));
                }
                else
                {
                    // 禁止使用开始按钮
                    btnStartService.Enabled = false;
                    // 可以使用停止按钮
                    btnStopService.Enabled = false;

                    // 灰色显示
                    btnServiceStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
                    btnServiceStatus.Text = ServerConfig.getInstance().Language ? "Unknown" : "未知";

                    // 禁止那些可以编辑的控件
                    comboBoxLogLevel.Enabled = true;
                    //txtLogPath.Enabled = true;
                    txtListeningPortNo.Enabled = true;
                    chkBoxAutostartService.Enabled = true;
                    chkAutoStartAcquisition.Enabled = true;

                    // Unknown status
                    txtServiceName.Text = "??";
                    txtExecutableFilePath.Text = "??";

                    // Unknown internal interrupts
                    txtInternalCyclicInterrupts.Text = "??";

                    SetAcquistionStatus(PMSServer.FDAA_ACQUISITION_STATUS.UNAVAILABLE);
                    // List init
                    //m_wndClientList.DeleteAllItems();
                    //m_wndDriverList.DeleteAllItems();
                }
            }
        }

        private void SetAcquistionStatus(PMSServer.FDAA_ACQUISITION_STATUS acqStatus)
        {
            // 如果当前状态是数据采集状态
            if (acqStatus == PMSServer.FDAA_ACQUISITION_STATUS.STARTED)
            {
                // 仅在服务处于启动状态时，才将数据采集状态设置为采集中
                if (PMSServer.Instance.AcqStatus == PMSServer.FDAA_ACQUISITION_STATUS.STARTED)
                {
                    if (this.InvokeRequired)
                    {
                        // 禁止使用开始按钮
                        btnStartAcquisition.BeginInvoke(new Action(() => btnStartAcquisition.Enabled = false));
                        // 可以使用停止按钮
                        btnStopAcquisition.BeginInvoke(new Action(() => btnStopAcquisition.Enabled = true));
                        // 将数据采集状态显示为绿色，并显示相应文字信息
                        btnAcquisitionStatus.BeginInvoke(new Action(() => btnAcquisitionStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)0))), ((int)(((byte)(255)))), ((int)(((byte)(0)))))));
                        btnAcquisitionStatus.BeginInvoke(new Action(() => btnAcquisitionStatus.Text = ServerConfig.getInstance().Language ? "Started" : "采集中"));
                        btnAcquisitionStatus.BeginInvoke(new Action(() => toolStripPassRate.Text = strReady));
                        BeginInvoke(new Action(() => btnOneClickDiagnostic.Enabled = true));
                        BeginInvoke(new Action(() => diagnosticProgressText.Visible = true));
                        BeginInvoke(new Action(() => diagnosticProgressText.Text = string.Empty));
                    }
                    else
                    {
                        // 禁止使用开始按钮
                        btnStartAcquisition.Enabled = false;
                        // 可以使用停止按钮
                        btnStopAcquisition.Enabled = true;
                        // 将数据采集状态显示为绿色，并显示相应文字信息
                        btnAcquisitionStatus.BackColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
                        btnAcquisitionStatus.Text = ServerConfig.getInstance().Language ? "Started" : "采集中";
                        toolStripPassRate.Text = strReady;
                        btnOneClickDiagnostic.Enabled = true;
                        diagnosticProgressText.Visible = true;
                        diagnosticProgressText.Text = string.Empty;
                    }
                }
            }
            else if (PMSServer.Instance.AcqStatus == PMSServer.FDAA_ACQUISITION_STATUS.STOPPED)
            {
                if (this.InvokeRequired)
                {
                    // 可以使用开始按钮
                    btnStartAcquisition.BeginInvoke(new Action(() => btnStartAcquisition.Enabled = true));
                    // 禁止使用停止按钮
                    btnStopAcquisition.BeginInvoke(new Action(() => btnStopAcquisition.Enabled = false));
                    // 将数据采集状态显示为绿色，并显示相应文字信息
                    btnAcquisitionStatus.BeginInvoke(new Action(() => btnAcquisitionStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))));
                    btnAcquisitionStatus.BeginInvoke(new Action(() => btnAcquisitionStatus.Text = ServerConfig.getInstance().Language ? "Stopped" : "已停止"));
                    BeginInvoke(new Action(() => toolStripPassRate.Text = strDiagnoseAcquisitionNotStarted));
                    BeginInvoke(new Action(() => btnOneClickDiagnostic.Enabled = false));
                    BeginInvoke(new Action(() => diagnosticProgressText.Visible = false));
                }
                else
                {
                    // 可以使用开始按钮
                    btnStartAcquisition.Enabled = true;
                    // 禁止使用停止按钮
                    btnStopAcquisition.Enabled = false;
                    // 将数据采集状态显示为绿色，并显示相应文字信息
                    btnAcquisitionStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                    btnAcquisitionStatus.Text = ServerConfig.getInstance().Language ? "Stopped" : "已停止";
                    toolStripPassRate.Text = strDiagnoseAcquisitionNotStarted;
                    btnOneClickDiagnostic.Enabled = false;
                    diagnosticProgressText.Visible = false;
                }
            }
            else // ACQUISITION_UNAVAILABLE
            {
                if (this.InvokeRequired)
                {
                    // 禁止使用开始按钮
                    btnStartAcquisition.BeginInvoke(new Action(() => btnStartAcquisition.Enabled = false));
                    // 禁止使用停止按钮
                    btnStopAcquisition.BeginInvoke(new Action(() => btnStopAcquisition.Enabled = false));
                    // 将数据采集状态显示为绿色，并显示相应文字信息
                    btnAcquisitionStatus.BeginInvoke(new Action(() => btnAcquisitionStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))))));
                    btnAcquisitionStatus.BeginInvoke(new Action(() => btnAcquisitionStatus.Text = ServerConfig.getInstance().Language ? "Unknown" : "未知"));
                    BeginInvoke(new Action(() => toolStripPassRate.Text = strDiagnoseAcquisitionNotStarted));
                    BeginInvoke(new Action(() => btnOneClickDiagnostic.Enabled = false));
                    BeginInvoke(new Action(() => diagnosticProgressText.Visible = false));
                }
                else
                {
                    // 禁止使用开始按钮
                    btnStartAcquisition.Enabled = false;
                    // 禁止使用停止按钮
                    btnStopAcquisition.Enabled = false;
                    // 将数据采集状态显示为绿色，并显示相应文字信息
                    btnAcquisitionStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
                    btnAcquisitionStatus.Text = ServerConfig.getInstance().Language ? "Unknown" : "未知";
                    toolStripPassRate.Text = strDiagnoseAcquisitionNotStarted;
                    btnOneClickDiagnostic.Enabled = false;
                    diagnosticProgressText.Visible = false;
                }
            }
        }

        private void UpdateDataTimer_Tick(object sender, EventArgs e)
        {
            // 与采集服务的心跳不正常
            if (CyclicInterruptsAbnormal)
            {
                handleAbnormal();
            }

            if (tabControlMain.SelectedIndex == 0 && PMSServer.Instance.ServerStatus == PMSServer.FDAA_SERVICE_STATUS.RUNNING)
            {
                UpdateGeneralData();
            }
            else if (tabControlMain.SelectedIndex == 1 && PMSServer.Instance.ServerStatus == PMSServer.FDAA_SERVICE_STATUS.RUNNING)
            {
                if (times % 5 == 0)	// 不需要频繁地更新,1s刷新一次采样值
                {
                    UpdateIoActualData();
                    UpdateModuleStatus();
                }
                //if (times % 25 == 0) // 5s刷新一次链路状态
                //{
                //    UpdateModuleStatus();
                //}
            }
            else if (tabControlMain.SelectedIndex == 2)
            {
                UpdateFrameLostCounter(PMSServer.Instance.ServerStatus);
            }
            times++;
        }

        private void handleAbnormal()
        {
            if (AlreadyHandle) return;

	        UpdateDataTimer.Stop();

	        AlreadyHandle = true;

            string strInfo = ServerConfig.getInstance().Language
		        ? "Service abnormal status detected! acquisition environment NEED to be reset.\nWould you like to continue?"
		        : "检测到采集服务出现异常！\n点击“是”初始化采集环境，重新开始采集；点击“否”忽略异常。";
            string strTitle = ServerConfig.getInstance().Language
		        ? "PMS Service Status"
		        : "采集服务状态";
            using (new CenterWinDialog(this))
            {
                if (MessageBox.Show(strInfo, strMessageBoxTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    InitAcquisitionEnvironment();
                }
            }
        }
        #endregion

        #region 常规页
        /// <summary>
        /// 实时保存自动启动服务项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkBoxAutostartService_CheckedChanged(object sender, EventArgs e)
        {
            ServerConfig.getInstance().AutoStartServerOnStartup = this.chkBoxAutostartService.Checked;
            ServerConfig.getInstance().SaveServerConfig(serverConfigName);//System.Windows.Forms.Application.StartupPath + @"\config\ServerConfig.xml"
        }

        /// <summary>
        /// 实时保存自动启动采集项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkAutoStartAcquisition_CheckedChanged(object sender, EventArgs e)
        {
            ServerConfig.getInstance().StartAcquisitionOnStartup = this.chkAutoStartAcquisition.Checked;
            ServerConfig.getInstance().SaveServerConfig(serverConfigName);//System.Windows.Forms.Application.StartupPath + @"\config\ServerConfig.xml"
        }

        /// <summary>
        /// 实时保存日志等级项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxLogLevel_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ServerConfig.getInstance().LogLevel = Helper.ConvertStrLogNumberToInt(this.comboBoxLogLevel.Text);
            ServerConfig.getInstance().SaveServerConfig(serverConfigName);//System.Windows.Forms.Application.StartupPath + @"\config\ServerConfig.xml"
        }

        private void comboBoxInternalCyclicInterrupts_SelectionChangeCommitted(object sender, EventArgs e)
        {
            currentInterruptsComboxIndex = comboBoxInternalCyclicInterrupts.SelectedIndex;
        }
        /// <summary>
        /// 校验并实时保存端口号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtListeningPortNo_Validating(object sender, CancelEventArgs e)
        {
            Regex r = new Regex(@"^(102[4-9]|10[3-9]\d|1[1-9]\d\d|[2-9]\d{3}|[1-5]\d{4}|6[0-4]\d{3}|65[0-4]\d{2}|655[0-2]\d|6553[0-5])$");
            string content = ((TextBox)sender).Text;
            bool match = r.IsMatch(content);
            if (!match)
            {
                errorProvider1.SetError((Control)sender, ServerConfig.getInstance().Language ? " Invalid port number!" : " 请输入正确的端口号！");
                e.Cancel = true;
            }
            else
            {
                ServerConfig.getInstance().ServerPortNr = Convert.ToInt32(content);
                ServerConfig.getInstance().SaveServerConfig(serverConfigName);//System.Windows.Forms.Application.StartupPath + @"\config\ServerConfig.xml"
                errorProvider1.SetError((Control)sender, null);
            }
        }

        private void btnDisconnectAllClients_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            PMSServer.Instance.DisconnectAllClients();
            Cursor.Current = Cursors.Default;
        }


        private void btnStartService_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            PMSServer.Instance.StartService(true);
            Cursor.Current = Cursors.Default;
        }

        private void btnStopService_Click(object sender, EventArgs e)
        {
            //SendExitGuardianRequest();
            Cursor.Current = Cursors.WaitCursor;
            PMSServer.Instance.StopService(true);
            Cursor.Current = Cursors.Default;
        }

        private void btnStartAcquisition_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            PMSServer.Instance.StartAcquisition();
            Cursor.Current = Cursors.Default;
        }

        private void btnStopAcquisition_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            PMSServer.Instance.StopAcquisition();
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// 找到Guardian主窗体句柄，并请求点击“退出守卫”按钮
        /// </summary>
        private void SendExitGuardianRequest()
        {
            //  需要查找的窗体Title
            string targetwinname = " PMS服务守卫者";
            //  需要查找的Button的名称
            string lpszName_Submit = "退出守卫";
            //获取Guardian窗体句柄                   
            IntPtr guardianMaindHwnd = WinMsgHandler.FindWindow(null, targetwinname);
            //  如果未找到窗体句柄
            if (guardianMaindHwnd == IntPtr.Zero)
            {
                log.Fatal("The target window " + targetwinname + "was not found!");
                return;
            }
            else
            {
                IntPtr guardianChildHwnd = WinMsgHandler.FindWindowEx(guardianMaindHwnd, IntPtr.Zero, null, lpszName_Submit);   //获得按钮的句柄
                if (guardianChildHwnd == IntPtr.Zero)
                {
                    log.Fatal(lpszName_Submit + "button handler was not found!");
                }
                else
                {
                    //WinMsgHandler.SetForegroundWindow(guardianMaindHwnd);
                    WinMsgHandler.SendMessage(guardianChildHwnd, WinMsgHandler.WM_CLICK, IntPtr.Zero, null);
                }
            }
        }

        /// <summary>
        /// 更新内部循环时钟中断计数
        /// </summary>
        private void UpdateGeneralData()
        {
            long lInterrupts;

            // 如果PMSService处于停止状态
            if (PMSServer.Instance.PMSService == null) return;
            if (PMSServer.Instance.ServerStatus != PMSServer.FDAA_SERVICE_STATUS.RUNNING) return;

            try
            {
                PMSServer.Instance.PMSService.getSoftInterrupts(out lInterrupts);
                //  设置计数值
                if (currentInterruptsComboxIndex == 0)
                {
                    txtInternalCyclicInterrupts.Text = lInterrupts.ToString();
                }
                //  设置时间
                else
                {
                    TimeSpan ts = new TimeSpan(lInterrupts * 10000);

                    if (ServerConfig.getInstance().Language)
                    {
                        txtInternalCyclicInterrupts.Text = ts.Days + " Days " + string.Format("{0:D2}", ts.Hours) + ":" + string.Format("{0:D2}", ts.Minutes) + ":" + string.Format("{0:D2}", ts.Seconds);
                    }
                    else
                    {
                        txtInternalCyclicInterrupts.Text = ts.Days + " 天 " + string.Format("{0:D2}", ts.Hours) + ":" + string.Format("{0:D2}", ts.Minutes) + ":" + string.Format("{0:D2}", ts.Seconds);
                    }
                }
            }
            // 心跳不正常
            catch (Exception)
            {
                txtInternalCyclicInterrupts.Text = "??";
                CyclicInterruptsAbnormal = true;
            }
        }

        /// <summary>
        /// 更新驱动信息列表
        /// </summary>
        private void UpdateDriverInfos()
        {
            //  更新驱动信息列表
            //  清空所有项
            listViewDriverInfo.Items.Clear();
            //  填充项
            try
            {
                if (PMSServer.Instance.DriverInfos.GetValue(0) != null)
                {
                    for (int i = 0; i < PMSServer.Instance.DriverInfos.Length; i++)
                    {
                        string Name = ((_arrayItem)(PMSServer.Instance.DriverInfos.GetValue(i))).param0.ToString();
                        string Version = ((_arrayItem)(PMSServer.Instance.DriverInfos.GetValue(i))).param1.ToString();
                        string License = ((_arrayItem)(PMSServer.Instance.DriverInfos.GetValue(i))).param2.ToString();
                        if (Name == "DBStorageManager") continue;

                        ListViewItem viewitem = new ListViewItem(Name);
                        viewitem.SubItems.Add(Version);
                        if (Name == "pmsService" && License == "99999")
                        {
                            viewitem.SubItems.Add("unlimited");
                        }
                        else if (Name == "ConsistantUDP")
                        {
                            if (License == "OK")
                                viewitem.SubItems.Add("OK");
                            else
                                viewitem.SubItems.Add("1");
                        }
                        else
                        {
                            viewitem.SubItems.Add(License);
                        }
                        listViewDriverInfo.Items.Add(viewitem);
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 更新客户端信息列表
        /// </summary>
        private void UpdateOnlineClients()
        {
            if (this.InvokeRequired)
            {
                //  更新客户端信息列表
                //  清空所有项
                listViewOnlineClient.BeginInvoke(new Action(() => listViewOnlineClient.Items.Clear()));
                //  填充项
                foreach (var item in PMSServer.Instance.ClinetInfos)
                {
                    ListViewItem viewitem = new ListViewItem(item._id.ToString());
                    viewitem.SubItems.Add(item._name.ToString());
                    viewitem.SubItems.Add(item._logintime.ToString());
                    viewitem.SubItems.Add(item._signalcount.ToString());
                    listViewOnlineClient.BeginInvoke(new Action(() => listViewOnlineClient.Items.Add(viewitem)));
                }
                //  更新客户端数量
                lblClientCountNumber.BeginInvoke(new Action(() => lblClientCountNumber.Text = PMSServer.Instance.ClinetInfos.Count.ToString()));
            }
        }
        #endregion
        
        #region IO状态页

        static int usingTreeViewResource = 0;

        private void CleareIOData()
        {
            if (Interlocked.Exchange(ref usingTreeViewResource, 1) == 0)
            {
                ClearTreeControl();
                ClearGridViewData();
                Interlocked.Exchange(ref usingTreeViewResource, 0);
            }
        }

        private void ClearTreeControl()
        {
            if (treeView1.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(ClearTreeNodes));                
            }
            else
            {
                ClearTreeNodes();
            }
            FillTreeControlCount = 0;
        }

        private void ClearTreeNodes()
        {
            treeView1.BeginUpdate();

            this.treeView1.SelectedNode = null;
            treeView1.Nodes.Clear();

            treeView1.EndUpdate();

            lblTotalSignals.Text = "";
        }

        private void ClearGridViewData()
        {
            if(this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(ClearGridView));              
            }
            else
            {
                ClearGridView();
            }
        }

        private void ClearGridView()
        {
            gridControlDigital.DataSource = null;
            gridControlAnalog.DataSource = null;
            gridControlTecnostring.DataSource = null;
        }

        //  排查bug用
        static int time = 0;
        static int FillTreeControlCount = 0;

        private void FillUITreeControl()
        {
            if (treeView1.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(() => FillTreeControl()));
            }
            else
                FillTreeControl();
        }

        private void FillTreeControl()
        {
            treeView1.BeginUpdate();

            this.treeView1.SelectedNode = null;
            treeView1.Nodes.Clear();

            lock(statusChangedLocker)
            {
                foreach (KeyValuePair<UInt32, BaseModule> kvp in IOConfig.getInstance().AllModules)
                {
                    //  普通模块
                    if (kvp.Value.MdType != IOConfig.MODULE_TYPE.mtTsFAU && kvp.Value.MdType != IOConfig.MODULE_TYPE.mtTsOPC &&
                        kvp.Value.MdType != IOConfig.MODULE_TYPE.mtTsUDP && kvp.Value.MdType != IOConfig.MODULE_TYPE.mtVirtualforLuaTS)
                    {
                        //  模块内没有活动信号（空模块），不需要显示在信号树中
                        if (kvp.Value.AnalogDataTable.Rows.Count == 0 && kvp.Value.DigitalDataTable.Rows.Count == 0) continue;

                        TreeNode nodeModule = new TreeNode(kvp.Value.ModuleNo.ToString() + ". " + kvp.Value.ModuleName, 0, 0);
                        nodeModule.Name = kvp.Key.ToString();
                        nodeModule.Tag = kvp.Value.ModuleNo;

                        treeView1.Nodes.Add(nodeModule);
                    }
                    //  Tecnostring模块
                    else
                    {
                        //  模块内没有活动信号（空模块），不需要显示在信号树中
                        if (kvp.Value.SectionDataTable.Rows.Count == 0) continue;

                        TreeNode nodeModule = new TreeNode(kvp.Value.ModuleNo.ToString() + ". " + kvp.Value.ModuleName, 3, 3);
                        nodeModule.Name = kvp.Key.ToString();
                        nodeModule.Tag = kvp.Value.ModuleNo;

                        treeView1.Nodes.Add(nodeModule);
                    }
                }
                if (treeView1.Nodes.Count > 0)
                    treeView1.SelectedNode = treeView1.Nodes[0];
            }

            treeView1.EndUpdate();

            lblTotalSignals.Text = IOConfig.getInstance().AnalogCount + " A" + " + " +
                IOConfig.getInstance().DigitalCount + " D" + " + " +
                IOConfig.getInstance().TecnostringCount + " T";
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //  解决反复停止采集时，出现的“给定关键字不在字典中"的bug
            //  原因分析fillControl时用的BeginInvoke异步添s加节点，当自动选择第一个节点时，树节点并没有填充完毕。
            //Thread.Sleep(10);
            try
            {
                if (Interlocked.Exchange(ref usingTreeViewResource, 1) == 0)
                {
                    if (treeView1.SelectedNode != null)
                    {
                        gridViewAnalog.Columns.Clear();
                        gridViewDigital.Columns.Clear();
                        gridViewTecnostring.Columns.Clear();

                        uint moduleNo = Convert.ToUInt32(treeView1.SelectedNode.Tag);
                        if (!IOConfig.getInstance().AllModules.ContainsKey(moduleNo)) return;

                        SetIndicatorWidth(moduleNo, IOConfig.getInstance().AllModules[moduleNo].AnalogDataTable.Rows.Count,
                            IOConfig.getInstance().AllModules[moduleNo].DigitalDataTable.Rows.Count, 
                        IOConfig.getInstance().AllModules[moduleNo].SectionDataTable.Rows.Count);

                        PMSServiceStatus.IOConfig.MODULE_TYPE moduleType = IOConfig.getInstance().AllModules[moduleNo].MdType;
                        if (moduleType != PMSServiceStatus.IOConfig.MODULE_TYPE.mtTsFAU && moduleType != PMSServiceStatus.IOConfig.MODULE_TYPE.mtTsOPC
                             && moduleType != PMSServiceStatus.IOConfig.MODULE_TYPE.mtTsUDP && moduleType != PMSServiceStatus.IOConfig.MODULE_TYPE.mtVirtualforLuaTS)
                        {
                            SetTecnostringTabcontrolVisiable(false);

                            foreach (DataColumn col in IOConfig.getInstance().AllModules[moduleNo].AnalogDataTable.Columns)
                            {
                                GridColumn column = gridViewAnalog.Columns.AddVisible(col.ColumnName);
                                column.UnboundType = DevExpress.Data.UnboundColumnType.String;
                            }
                            gridControlAnalog.DataSource = IOConfig.getInstance().AllModules[moduleNo].AnalogDataTable;

                            //  设置列宽和布局
                            for (int i = 0; i < IOConfig.getInstance().AllModules[moduleNo].AnalogDataTable.Columns.Count; i++)
                            {
                                SetColumnWidth(moduleType, true, gridViewAnalog.Columns[i]);
                            }

                            foreach (DataColumn col in IOConfig.getInstance().AllModules[moduleNo].DigitalDataTable.Columns)
                            {
                                GridColumn column = gridViewDigital.Columns.AddVisible(col.ColumnName);
                                column.UnboundType = DevExpress.Data.UnboundColumnType.String;
                            }
                            gridControlDigital.DataSource = IOConfig.getInstance().AllModules[moduleNo].DigitalDataTable;

                            //  设置列宽和布局
                            for (int i = 0; i < IOConfig.getInstance().AllModules[moduleNo].DigitalDataTable.Columns.Count; i++)
                            {
                                SetColumnWidth(moduleType, false, gridViewDigital.Columns[i]);
                            }
                        }
                        else
                        {
                            SetTecnostringTabcontrolVisiable(true);
                            foreach (DataColumn col in IOConfig.getInstance().AllModules[moduleNo].SectionDataTable.Columns)
                            {
                                GridColumn column = gridViewTecnostring.Columns.AddVisible(col.ColumnName);
                                column.UnboundType = DevExpress.Data.UnboundColumnType.String;
                            }
                            gridControlTecnostring.DataSource = IOConfig.getInstance().AllModules[moduleNo].SectionDataTable;

                            //  设置列宽和布局
                            for (int i = 0; i < IOConfig.getInstance().AllModules[moduleNo].SectionDataTable.Columns.Count; i++)
                            {
                                SetColumnWidth(moduleType, false, gridViewTecnostring.Columns[i]);
                            }
                        }
                    }
                    Interlocked.Exchange(ref usingTreeViewResource, 0);
                }
            }
            catch (Exception ex)
            {
                Interlocked.Exchange(ref usingTreeViewResource, 0);
                log.Fatal(ex.StackTrace);
            }
        }

        /// <summary>
        /// 设置IndicatorWidth宽度 
        /// </summary>
        /// <param name="moduleNo"></param>
        /// <param name="analogSignalCount"></param>
        /// <param name="digitalSignalCount"></param>
        private void SetIndicatorWidth(uint moduleNo,int analogSignalCount,int digitalSignalCount,int technostringSignalCount)
        {
            //  计算模块号位数 10000=》5位 
            int moduleDigits = 0;

            if (moduleNo == 0)
            {
                moduleDigits = 1;
            }
            else
            {
                while (moduleNo != 0)
                {
                    moduleNo /= 10;
                    moduleDigits++;
                }
            }

            //  计算模拟量最大信号位数 
            int analogSignalDigits = 0;
            while (analogSignalCount != 0)
            {
                analogSignalCount /= 10;
                analogSignalDigits++;
            }

            //  计算开关量最大信号位数 
            int digitalSignalDigits = 0;
            while (digitalSignalCount != 0)
            {
                digitalSignalCount /= 10;
                digitalSignalDigits++;
            }

            //  计算模拟量最大信号位数 
            int technostringSignalDigits = 0;
            while (technostringSignalCount != 0)
            {
                technostringSignalCount /= 10;
                technostringSignalDigits++;
            }

            //  单个字符宽度
            int singleDigitWidth = 6;
            //  模拟量页面IndicatorWidth宽度 
            gridViewAnalog.IndicatorWidth = moduleDigits * singleDigitWidth + analogSignalDigits * singleDigitWidth + 19;
            //  开关量页面IndicatorWidth宽度 
            gridViewDigital.IndicatorWidth = moduleDigits * singleDigitWidth + digitalSignalDigits * singleDigitWidth + 19;
            //  Technostring页面IndicatorWidth宽度 
            gridViewTecnostring.IndicatorWidth = moduleDigits * singleDigitWidth + technostringSignalDigits * singleDigitWidth + 19;
        }

        /// <summary>
        /// 信号树鼠标右键让该节点处于选中状态 实现左右键都可以选中节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_MouseClick(object sender, MouseEventArgs e)
        {
            TreeView tv = sender as TreeView;
            if (tv == null)
            {
                return;
            }
            if (e.Button == MouseButtons.Right)
            {
                TreeNode selectedNode = tv.GetNodeAt(e.Location);
                tv.SelectedNode = selectedNode;
            }
        }

        void SetTecnostringTabcontrolVisiable(bool isTecnostringTabVisiable)
        {
            if (isTecnostringTabVisiable)
            {
                tabControlStringValues.Visible = true;
                tabControlSignalValues.Visible = false;
                isDisplaySignalValues = false;
            }
            else
            {
                tabControlStringValues.Visible = false;
                tabControlSignalValues.Visible = true;
                isDisplaySignalValues = true;
            }
        }

        private void SetColumnWidth(IOConfig.MODULE_TYPE moduleType, bool isAnalog, GridColumn column)
        {
            #region UDP DPLITE
            if (moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtUDPInteger || moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtUDPReal
                     || moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtUDPUserDefined ||
                moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtDPLiteInteger || moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtDPLiteReal
                         || moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtDPLiteDoubleInteger)
            {
                if (isAnalog)
                {
                    if (ServerConfig.getInstance().Language)
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.4f;
                        float colPercentageNameUnit = 0.08f;
                        float colPercentageGain = 0.08f;
                        float colPercentageOffset = 0.08f;
                        float colPercentageDataType = 0.12f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageNameUnit - colPercentageGain - colPercentageOffset - colPercentageDataType - colPercentageActualValue;

                        if (column.FieldName == "Address")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "Name")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "Unit")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageNameUnit * ActualWidth);
                        }
                        else if (column.FieldName == "Gain")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageGain * ActualWidth);
                        }
                        else if (column.FieldName == "Offset")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageOffset * ActualWidth);
                        }
                        else if (column.FieldName == "DataType")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "Actual Value")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                    else
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.38f;
                        float colPercentageNameUnit = 0.08f;
                        float colPercentageGain = 0.08f;
                        float colPercentageOffset = 0.12f;
                        float colPercentageDataType = 0.12f;
                        float colPercentageActualValue = 0.13f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageNameUnit - colPercentageGain - colPercentageOffset - colPercentageDataType - colPercentageActualValue;

                        if (column.FieldName == "地址")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "信号名称")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "单位")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageNameUnit * ActualWidth);
                        }
                        else if (column.FieldName == "增益")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageGain * ActualWidth);
                        }
                        else if (column.FieldName == "偏移地址")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageOffset * ActualWidth);
                        }
                        else if (column.FieldName == "数据类型")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "实际值")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                }
                //  开关量列宽布局
                else
                {
                    if (ServerConfig.getInstance().Language)
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.51f;
                        float colPercentageOffset = 0.11f;
                        float colPercentageDataType = 0.12f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageOffset - colPercentageDataType - colPercentageActualValue;

                        if (column.FieldName == "Address")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "Name")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "Offset")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageOffset * ActualWidth);
                        }
                        else if (column.FieldName == "DataType")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "Actual Value")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                    else
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.51f;
                        float colPercentageOffset = 0.11f;
                        float colPercentageDataType = 0.12f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageOffset - colPercentageDataType - colPercentageActualValue;

                        if (column.FieldName == "地址")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "信号名称")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "偏移地址")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageOffset * ActualWidth);
                        }
                        else if (column.FieldName == "数据类型")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "实际值")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                }
            }
            #endregion
            #region UDP MULTICAST
            if (moduleType == IOConfig.MODULE_TYPE.mtUDPMulticast)
            {
                if (isAnalog)
                {
                    if (ServerConfig.getInstance().Language)
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.4f;
                        float colPercentageNameUnit = 0.08f;
                        float colPercentageGain = 0.08f;
                        float colPercentageOffset = 0.08f;
                        float colPercentageDataType = 0.12f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageNameUnit - colPercentageGain - colPercentageOffset - colPercentageDataType - colPercentageActualValue;

                        if (column.FieldName == "Address")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "Name")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "Unit")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageNameUnit * ActualWidth);
                        }
                        else if (column.FieldName == "Gain")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageGain * ActualWidth);
                        }
                        else if (column.FieldName == "Offset")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageOffset * ActualWidth);
                        }
                        else if (column.FieldName == "DataType")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "Actual Value")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                    else
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.38f;
                        float colPercentageNameUnit = 0.08f;
                        float colPercentageGain = 0.08f;
                        float colPercentageOffset = 0.12f;
                        float colPercentageDataType = 0.12f;
                        float colPercentageActualValue = 0.13f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageNameUnit - colPercentageGain - colPercentageOffset - colPercentageDataType - colPercentageActualValue;

                        if (column.FieldName == "地址")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "信号名称")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "单位")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageNameUnit * ActualWidth);
                        }
                        else if (column.FieldName == "增益")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageGain * ActualWidth);
                        }
                        else if (column.FieldName == "偏移地址")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageOffset * ActualWidth);
                        }
                        else if (column.FieldName == "数据类型")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "实际值")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                }
                //  开关量列宽布局
                else
                {
                    if (ServerConfig.getInstance().Language)
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.4f;
                        float colPercentageOffset = 0.11f;
                        float colPercentageBitNo = 0.11f;
                        float colPercentageDataType = 0.12f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageOffset - colPercentageDataType - colPercentageActualValue;

                        if (column.FieldName == "Address")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "Name")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "Offset")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageOffset * ActualWidth);
                        }
                        else if (column.FieldName == "BitNo")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageBitNo * ActualWidth);
                        }
                        else if (column.FieldName == "DataType")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "Actual Value")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                    else
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.4f;
                        float colPercentageOffset = 0.11f;
                        float colPercentageBitNo = 0.11f;
                        float colPercentageDataType = 0.12f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageOffset - colPercentageDataType - colPercentageActualValue;

                        if (column.FieldName == "地址")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "信号名称")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "偏移地址")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageOffset * ActualWidth);
                        }
                        else if (column.FieldName == "比特位")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageBitNo * ActualWidth);
                        }
                        else if (column.FieldName == "数据类型")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "实际值")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                }
            }
            #endregion
            #region RFM
            else if (moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtRFM5565)
            {
                if (isAnalog)
                {
                    if (ServerConfig.getInstance().Language)
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.34f;
                        float colPercentageNameUnit = 0.08f;
                        float colPercentageGain = 0.08f;
                        float colPercentageRfmAddress = 0.14f;
                        float colPercentageDataType = 0.12f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageNameUnit - colPercentageGain - colPercentageRfmAddress - colPercentageDataType - colPercentageActualValue;

                        if (column.FieldName == "Address")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "Name")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "Unit")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageNameUnit * ActualWidth);
                        }
                        else if (column.FieldName == "Gain")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageGain * ActualWidth);
                        }
                        else if (column.FieldName == "Rfm Address")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageRfmAddress * ActualWidth);
                        }
                        else if (column.FieldName == "DataType")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "Actual Value")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                    else
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.33f;
                        float colPercentageNameUnit = 0.08f;
                        float colPercentageGain = 0.08f;
                        float colPercentageRfmAddress = 0.17f;
                        float colPercentageDataType = 0.12f;
                        float colPercentageActualValue = 0.13f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageNameUnit - colPercentageGain - colPercentageRfmAddress - colPercentageDataType - colPercentageActualValue;

                        if (column.FieldName == "地址")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "信号名称")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "单位")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageNameUnit * ActualWidth);
                        }
                        else if (column.FieldName == "增益")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageGain * ActualWidth);
                        }
                        else if (column.FieldName == "反射内存网地址")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageRfmAddress * ActualWidth);
                        }
                        else if (column.FieldName == "数据类型")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "实际值")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                }
                //  开关量列宽布局
                else
                {
                    if (ServerConfig.getInstance().Language)
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.38f;
                        float colPercentageRfmAddress = 0.14f;
                        float colPercentageBitNo = 0.10f;
                        float colPercentageDataType = 0.12f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageRfmAddress - colPercentageDataType - colPercentageActualValue - colPercentageBitNo;

                        if (column.FieldName == "Address")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "Name")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "Rfm Address")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageRfmAddress * ActualWidth);
                        }
                        else if (column.FieldName == "Bit No")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageBitNo * ActualWidth);
                        }
                        else if (column.FieldName == "DataType")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "Actual Value")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                    else
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.35f;
                        float colPercentageRfmAddress = 0.17f;
                        float colPercentageBitNo = 0.10f;
                        float colPercentageDataType = 0.12f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageRfmAddress - colPercentageDataType - colPercentageActualValue - colPercentageBitNo;

                        if (column.FieldName == "地址")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "信号名称")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "反射内存网地址")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageRfmAddress * ActualWidth);
                        }
                        else if (column.FieldName == "比特位")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageBitNo * ActualWidth);
                        }
                        else if (column.FieldName == "数据类型")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "实际值")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                }
            }
            #endregion
            #region OPC
            else if (moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtOPCClient)
            {
                if (isAnalog)
                {
                    if (ServerConfig.getInstance().Language)
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.3f;
                        float colPercentageNameUnit = 0.08f;
                        float colPercentageGain = 0.08f;
                        float colPercentageItemID = 0.3f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageNameUnit - colPercentageGain - colPercentageItemID - colPercentageActualValue;

                        if (column.FieldName == "Address")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "Name")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "Unit")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageNameUnit * ActualWidth);
                        }
                        else if (column.FieldName == "Gain")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageGain * ActualWidth);
                        }
                        else if (column.FieldName == "Item ID")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageItemID * ActualWidth);
                        }
                        else if (column.FieldName == "Actual Value")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                    else
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.3f;
                        float colPercentageNameUnit = 0.08f;
                        float colPercentageGain = 0.08f;
                        float colPercentageItemID = 0.3f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageNameUnit - colPercentageGain - colPercentageItemID - colPercentageActualValue;

                        if (column.FieldName == "地址")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "信号名称")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "单位")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageNameUnit * ActualWidth);
                        }
                        else if (column.FieldName == "增益")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageGain * ActualWidth);
                        }
                        else if (column.FieldName == "数据项识别号")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageItemID * ActualWidth);
                        }
                        else if (column.FieldName == "实际值")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                }
                //  开关量列宽布局
                else
                {
                    if (ServerConfig.getInstance().Language)
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.4f;
                        float colPercentageItemID = 0.4f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageItemID - colPercentageActualValue;

                        if (column.FieldName == "Address")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "Name")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "Item ID")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageItemID * ActualWidth);
                        }
                        else if (column.FieldName == "Actual Value")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                    else
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.4f;
                        float colPercentageItemID = 0.4f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageItemID - colPercentageActualValue;

                        if (column.FieldName == "地址")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "信号名称")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "数据项识别号")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageItemID * ActualWidth);
                        }
                        else if (column.FieldName == "实际值")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                }
            }
            #endregion
            #region S7
            else if ((moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtS7TCP300) || (moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtS7TCP400DB)
                            || (moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtS7TCP400NoneDB) || (moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtS7TCP) || moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtS7DPRequest || moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtS7PNRequest)
            {
                if (isAnalog)
                {
                    if (ServerConfig.getInstance().Language)
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.24f;
                        float colPercentageNameUnit = 0.08f;
                        float colPercentageGain = 0.08f;
                        float colPercentageS7Symbol = 0.12f;
                        float colPercentageS7Operand = 0.12f;
                        float colPercentageDataType = 0.12f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageNameUnit - colPercentageGain - colPercentageS7Symbol - colPercentageS7Operand - colPercentageDataType - colPercentageActualValue;

                        if (column.FieldName == "Address")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "Name")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "Unit")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageNameUnit * ActualWidth);
                        }
                        else if (column.FieldName == "Gain")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageGain * ActualWidth);
                        }
                        else if (column.FieldName == "S7 Symbol")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageS7Symbol * ActualWidth);
                        }
                        else if (column.FieldName == "S7 Operand")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageS7Operand * ActualWidth);
                        }
                        else if (column.FieldName == "DataType")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "Actual Value")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                    else
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.24f;
                        float colPercentageNameUnit = 0.08f;
                        float colPercentageGain = 0.08f;
                        float colPercentageS7Symbol = 0.12f;
                        float colPercentageS7Operand = 0.12f;
                        float colPercentageDataType = 0.12f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageNameUnit - colPercentageGain - colPercentageS7Symbol - colPercentageS7Operand - colPercentageDataType - colPercentageActualValue;

                        if (column.FieldName == "地址")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "信号名称")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "单位")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageNameUnit * ActualWidth);
                        }
                        else if (column.FieldName == "增益")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageGain * ActualWidth);
                        }
                        else if (column.FieldName == "S7 符号")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageS7Symbol * ActualWidth);
                        }
                        else if (column.FieldName == "S7 操作符")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageS7Operand * ActualWidth);
                        }
                        else if (column.FieldName == "数据类型")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "实际值")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                }
                //  开关量列宽布局
                else
                {
                    if (ServerConfig.getInstance().Language)
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.36f;
                        float colPercentageS7Symbol = 0.13f;
                        float colPercentageS7Operand = 0.13f;
                        float colPercentageDataType = 0.12f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageS7Symbol - colPercentageS7Operand - colPercentageDataType - colPercentageActualValue;

                        if (column.FieldName == "Address")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "Name")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "S7 Symbol")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageS7Symbol * ActualWidth);
                        }
                        else if (column.FieldName == "S7 Operand")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageS7Operand * ActualWidth);
                        }
                        else if (column.FieldName == "DataType")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "Actual Value")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                    else
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.36f;
                        float colPercentageS7Symbol = 0.13f;
                        float colPercentageS7Operand = 0.13f;
                        float colPercentageDataType = 0.12f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageS7Symbol - colPercentageS7Operand - colPercentageDataType - colPercentageActualValue;

                        if (column.FieldName == "地址")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "信号名称")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "S7 符号")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageS7Symbol * ActualWidth);
                        }
                        else if (column.FieldName == "S7 操作符")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageS7Operand * ActualWidth);
                        }
                        else if (column.FieldName == "数据类型")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "实际值")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                }
            }
            #endregion
            #region NISDAS SME
            else if ((moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtNisdas) || (moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtSme))
            {
                if (isAnalog)
                {
                    if (ServerConfig.getInstance().Language)
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.30f;
                        float colPercentageNameUnit = 0.08f;
                        float colPercentageGain = 0.08f;
                        float colPercentageVariable = 0.12f;
                        float colPercentageNisdasAddress = 0.12f;
                        float colPercentageDataType = 0.09f;
                        float colPercentageActualValue = 0.12f;
                        //float colPercentageAddress = 1 - colPercentageName - colPercentageNameUnit - colPercentageGain - colPercentageS7Symbol - colPercentageS7Operand - colPercentageDataType - colPercentageActualValue;

                        if (column.FieldName == "Address")
                        {
                            column.Visible = false;
                            //column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "Name")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "Unit")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageNameUnit * ActualWidth);
                        }
                        else if (column.FieldName == "Gain")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageGain * ActualWidth);
                        }
                        else if (column.FieldName == "Variable")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageVariable * ActualWidth);
                        }
                        else if (column.FieldName == " Address")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageNisdasAddress * ActualWidth);
                        }
                        else if (column.FieldName == "DataType")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "Actual Value")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                    else
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.30f;
                        float colPercentageNameUnit = 0.08f;
                        float colPercentageGain = 0.08f;
                        float colPercentageVariable = 0.12f;
                        float colPercentageNisdasAddress = 0.12f;
                        float colPercentageDataType = 0.09f;
                        float colPercentageActualValue = 0.12f;
                        //float colPercentageAddress = 1 - colPercentageName - colPercentageNameUnit - colPercentageGain - colPercentageVariable - colPercentageNisdasAddress - colPercentageDataType - colPercentageActualValue;

                        if (column.FieldName == "地址")
                        {
                            column.Visible = false;
                            //column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "信号名称")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "单位")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageNameUnit * ActualWidth);
                        }
                        else if (column.FieldName == "增益")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageGain * ActualWidth);
                        }
                        else if (column.FieldName == "变量名")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageVariable * ActualWidth);
                        }
                        else if (column.FieldName == " 地址")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageNisdasAddress * ActualWidth);
                        }
                        else if (column.FieldName == "数据类型")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "实际值")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                }
                //  开关量列宽布局
                else
                {
                    if (ServerConfig.getInstance().Language)
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.36f;
                        float colPercentageVariable = 0.13f;
                        float colPercentageAddress = 0.13f;
                        float colPercentageDataType = 0.12f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageSignalAddress = 1 - colPercentageName - colPercentageVariable - colPercentageAddress - colPercentageDataType - colPercentageActualValue;

                        if (column.FieldName == "Address")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageSignalAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "Name")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "Variable")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageVariable * ActualWidth);
                        }
                        else if (column.FieldName == " Address")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                        }
                        else if (column.FieldName == "DataType")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "Actual Value")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                    else
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.36f;
                        float colPercentageS7Symbol = 0.13f;
                        float colPercentageS7Operand = 0.13f;
                        float colPercentageDataType = 0.12f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageS7Symbol - colPercentageS7Operand - colPercentageDataType - colPercentageActualValue;

                        if (column.FieldName == "地址")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "信号名称")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "变量名")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageS7Symbol * ActualWidth);
                        }
                        else if (column.FieldName == " 地址")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageS7Operand * ActualWidth);
                        }
                        else if (column.FieldName == "数据类型")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "实际值")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                }
            }
            #endregion
            #region ARTI3
            else if (moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtArti3)
            {
                if (isAnalog)
                {
                    if (ServerConfig.getInstance().Language)
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.3f;
                        float colPercentageNameUnit = 0.08f;
                        float colPercentageGain = 0.08f;
                        float colPercentageARTI3Symbol = 0.3f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageNameUnit - colPercentageGain - colPercentageARTI3Symbol - colPercentageActualValue;

                        if (column.FieldName == "Address")
                        {
                            column.Visible = false;
                            //column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "Name")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "Unit")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageNameUnit * ActualWidth);
                        }
                        else if (column.FieldName == "Gain")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageGain * ActualWidth);
                        }
                        else if (column.FieldName == "ARTI3 Symbol")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageARTI3Symbol * ActualWidth);
                        }
                        else if (column.FieldName == "Actual Value")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                    else
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.3f;
                        float colPercentageNameUnit = 0.08f;
                        float colPercentageGain = 0.08f;
                        float colPercentageARTI3Symbol = 0.3f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageNameUnit - colPercentageGain - colPercentageARTI3Symbol - colPercentageActualValue;

                        if (column.FieldName == "地址")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "信号名称")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "单位")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageNameUnit * ActualWidth);
                        }
                        else if (column.FieldName == "增益")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageGain * ActualWidth);
                        }
                        else if (column.FieldName == "ARTI3 符号")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageARTI3Symbol * ActualWidth);
                        }
                        else if (column.FieldName == "实际值")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                }
                //  开关量列宽布局
                else
                {
                    if (ServerConfig.getInstance().Language)
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.4f;
                        float colPercentageARTI3Symbol = 0.4f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageARTI3Symbol - colPercentageActualValue;

                        if (column.FieldName == "Address")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "Name")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "ARTI3 Symbol")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageARTI3Symbol * ActualWidth);
                        }
                        else if (column.FieldName == "Actual Value")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                    else
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.4f;
                        float colPercentageARTI3Symbol = 0.4f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageARTI3Symbol - colPercentageActualValue;

                        if (column.FieldName == "地址")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "信号名称")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "ARTI3 符号")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageARTI3Symbol * ActualWidth);
                        }
                        else if (column.FieldName == "实际值")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                }
            }
            #endregion
            #region MELSEC
            if (moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtMelsecQ)
            {
                if (isAnalog)
                {
                    if (ServerConfig.getInstance().Language)
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.3f;
                        float colPercentageNameUnit = 0.08f;
                        float colPercentageGain = 0.08f;
                        float colPercentageAddressNotation = 0.2f;
                        float colPercentageDataType = 0.12f;
                        float colPercentageActualValue = 0.13f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageNameUnit - colPercentageGain - colPercentageAddressNotation - colPercentageDataType - colPercentageActualValue;

                        if (column.FieldName == "Address")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "Name")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "Unit")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageNameUnit * ActualWidth);
                        }
                        else if (column.FieldName == "Gain")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageGain * ActualWidth);
                        }
                        else if (column.FieldName == "Address Notation")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageAddressNotation * ActualWidth);
                        }
                        else if (column.FieldName == "DataType")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "Actual Value")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                    else
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.32f;
                        float colPercentageNameUnit = 0.08f;
                        float colPercentageGain = 0.08f;
                        float colPercentageAddressNotation = 0.18f;
                        float colPercentageDataType = 0.12f;
                        float colPercentageActualValue = 0.13f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageNameUnit - colPercentageGain - colPercentageAddressNotation - colPercentageDataType - colPercentageActualValue;

                        if (column.FieldName == "地址")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "信号名称")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "单位")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageNameUnit * ActualWidth);
                        }
                        else if (column.FieldName == "增益")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageGain * ActualWidth);
                        }
                        else if (column.FieldName == "软元件地址符")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageAddressNotation * ActualWidth);
                        }
                        else if (column.FieldName == "数据类型")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "实际值")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                }
                //  开关量列宽布局
                else
                {
                    if (ServerConfig.getInstance().Language)
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.42f;
                        float colPercentageAddressNotation = 0.2f;
                        float colPercentageDataType = 0.12f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageAddressNotation - colPercentageDataType - colPercentageActualValue;

                        if (column.FieldName == "Address")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "Name")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "Address Notation")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageAddressNotation * ActualWidth);
                        }
                        else if (column.FieldName == "DataType")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "Actual Value")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                    else
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.42f;
                        float colPercentageAddressNotation = 0.2f;
                        float colPercentageDataType = 0.12f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageAddressNotation - colPercentageDataType - colPercentageActualValue;

                        if (column.FieldName == "地址")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "信号名称")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "软元件地址符")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageAddressNotation * ActualWidth);
                        }
                        else if (column.FieldName == "数据类型")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "实际值")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                }
            }
            #endregion
            #region VC
            else if (moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtVideoCapture)
            {
                if (isAnalog)
                {
                    if (ServerConfig.getInstance().Language)
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.56f;
                        float colPercentageNameUnit = 0.10f;
                        float colPercentageGain = 0.10f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageNameUnit - colPercentageGain  - colPercentageActualValue;

                        if (column.FieldName == "Address")
                        {
                            column.Visible = false;
                            //column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "Name")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "Unit")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageNameUnit * ActualWidth);
                        }
                        else if (column.FieldName == "Gain")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageGain * ActualWidth);
                        }
                        else if (column.FieldName == "Actual Value")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                    else
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.56f;
                        float colPercentageNameUnit = 0.10f;
                        float colPercentageGain = 0.10f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageNameUnit - colPercentageGain - colPercentageActualValue;

                        if (column.FieldName == "地址")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "信号名称")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "单位")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageNameUnit * ActualWidth);
                        }
                        else if (column.FieldName == "增益")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageGain * ActualWidth);
                        }
                        else if (column.FieldName == "实际值")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                }
            }
            #endregion
            #region CPNET TCNET USIGMA FDAA
            else if (moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtCP3550 
                || moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtToshibaV3000 
                || moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtToshibaNV
                || moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtHitachiR700 
                || moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtHitachiR900
                || moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtFDAA)
            {
                if (isAnalog)
                {
                    if (ServerConfig.getInstance().Language)
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.28f;
                        float colPercentageNameUnit = 0.08f;
                        float colPercentageGain = 0.08f;
                        float colPercentageOperandNotation = 0.2f;
                        float colPercentageDataType = 0.12f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageNameUnit - colPercentageGain - colPercentageOperandNotation - colPercentageDataType - colPercentageActualValue;

                        if (column.FieldName == "Address")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "Name")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "Unit")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageNameUnit * ActualWidth);
                        }
                        else if (column.FieldName == "Gain")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageGain * ActualWidth);
                        }
                        else if (column.FieldName == "Operand Notation")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageOperandNotation * ActualWidth);
                        }
                        else if (column.FieldName == "DataType")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "Actual Value")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                    else
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.28f;
                        float colPercentageNameUnit = 0.08f;
                        float colPercentageGain = 0.08f;
                        float colPercentageOperandNotation = 0.2f;
                        float colPercentageDataType = 0.12f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageNameUnit - colPercentageGain - colPercentageOperandNotation - colPercentageDataType - colPercentageActualValue;

                        if (column.FieldName == "地址")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "信号名称")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "单位")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageNameUnit * ActualWidth);
                        }
                        else if (column.FieldName == "增益")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageGain * ActualWidth);
                        }
                        else if (column.FieldName == "操作符标志")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageOperandNotation * ActualWidth);
                        }
                        else if (column.FieldName == "数据类型")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "实际值")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                }
                //  开关量列宽布局
                else
                {
                    if (ServerConfig.getInstance().Language)
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.42f;
                        float colPercentageOperandNotation = 0.2f;
                        float colPercentageDataType = 0.12f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageOperandNotation - colPercentageDataType - colPercentageActualValue;

                        if (column.FieldName == "Address")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "Name")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "Operand Notation")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageOperandNotation * ActualWidth);
                        }
                        else if (column.FieldName == "DataType")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "Actual Value")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                    else
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.42f;
                        float colPercentageOperandNotation = 0.2f;
                        float colPercentageDataType = 0.12f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageOperandNotation - colPercentageDataType - colPercentageActualValue;

                        if (column.FieldName == "地址")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "信号名称")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "操作符标志")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageOperandNotation * ActualWidth);
                        }
                        else if (column.FieldName == "数据类型")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "实际值")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                }
            }
            #endregion
            #region PLAYBACK
            else if (moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtPlayback)
            {
                if (isAnalog)
                {
                    if (ServerConfig.getInstance().Language)
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.48f;
                        float colPercentageNameUnit = 0.08f;
                        float colPercentageGain = 0.08f;
                        float colPercentageDataType = 0.12f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageNameUnit - colPercentageGain  - colPercentageDataType - colPercentageActualValue;

                        if (column.FieldName == "Address")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "Name")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "Unit")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageNameUnit * ActualWidth);
                        }
                        else if (column.FieldName == "Gain")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageGain * ActualWidth);
                        }
                        else if (column.FieldName == "DataType")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "Actual Value")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                    else
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.48f;
                        float colPercentageNameUnit = 0.08f;
                        float colPercentageGain = 0.08f;
                        float colPercentageDataType = 0.12f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageNameUnit - colPercentageGain - colPercentageDataType - colPercentageActualValue;

                        if (column.FieldName == "地址")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "信号名称")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "单位")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageNameUnit * ActualWidth);
                        }
                        else if (column.FieldName == "增益")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageGain * ActualWidth);
                        }
                        else if (column.FieldName == "数据类型")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "实际值")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                }
                //  开关量列宽布局
                else
                {
                    if (ServerConfig.getInstance().Language)
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.62f;
                        float colPercentageDataType = 0.12f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageDataType - colPercentageActualValue;

                        if (column.FieldName == "Address")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "Name")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "DataType")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "Actual Value")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                    else
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.62f;
                        float colPercentageDataType = 0.12f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageDataType - colPercentageActualValue;

                        if (column.FieldName == "地址")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "信号名称")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "数据类型")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "实际值")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                }
            }
            #endregion
            #region VIRTUAL
            else if (moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtVirtualforLua)
            {
                if (isAnalog)
                {
                    if (ServerConfig.getInstance().Language)
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.2f;
                        float colPercentageNameUnit = 0.08f;
                        float colPercentageGain = 0.08f;
                        float colPercentagePath = 0.28f;
                        float colPercentageDataType = 0.12f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageNameUnit - colPercentageGain - colPercentagePath - colPercentageDataType - colPercentageActualValue;

                        if (column.FieldName == "Address")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "Name")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "Unit")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageNameUnit * ActualWidth);
                        }
                        else if (column.FieldName == "Gain")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageGain * ActualWidth);
                        }
                        else if (column.FieldName == "Path")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentagePath * ActualWidth);
                        }
                        else if (column.FieldName == "DataType")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "Actual Value")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                    else
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.2f;
                        float colPercentageNameUnit = 0.08f;
                        float colPercentageGain = 0.08f;
                        float colPercentagePath = 0.28f;
                        float colPercentageDataType = 0.12f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentageNameUnit - colPercentageGain - colPercentagePath - colPercentageDataType - colPercentageActualValue;

                        if (column.FieldName == "地址")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "信号名称")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "单位")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageNameUnit * ActualWidth);
                        }
                        else if (column.FieldName == "增益")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageGain * ActualWidth);
                        }
                        else if (column.FieldName == "脚本文件")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentagePath * ActualWidth);
                        }
                        else if (column.FieldName == "数据类型")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "实际值")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                }
                //  开关量列宽布局
                else
                {
                    if (ServerConfig.getInstance().Language)
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.25f;
                        float colPercentagePath = 0.37f;
                        float colPercentageDataType = 0.12f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentagePath - colPercentageDataType - colPercentageActualValue;

                        if (column.FieldName == "Address")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "Name")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "Path")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentagePath * ActualWidth);
                        }
                        else if (column.FieldName == "DataType")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "Actual Value")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                    else
                    {
                        //  Grid实际宽度 如果有滚动条 把滚动条计算上
                        int ActualWidth = gridControlAnalog.Width - 10;

                        float colPercentageName = 0.25f;
                        float colPercentagePath = 0.37f;
                        float colPercentageDataType = 0.12f;
                        float colPercentageActualValue = 0.15f;
                        float colPercentageAddress = 1 - colPercentageName - colPercentagePath - colPercentageDataType - colPercentageActualValue;

                        if (column.FieldName == "地址")
                        {
                            column.Visible = false;
                            column.Width = (int)(colPercentageAddress * ActualWidth);
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        }
                        else if (column.FieldName == "信号名称")
                        {
                            column.Width = (int)(colPercentageName * ActualWidth);
                        }
                        else if (column.FieldName == "脚本文件")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentagePath * ActualWidth);
                        }
                        else if (column.FieldName == "数据类型")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageDataType * ActualWidth);
                        }
                        else if (column.FieldName == "实际值")
                        {
                            column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                            column.Width = (int)(colPercentageActualValue * ActualWidth);
                        }
                    }
                }
            }
            #endregion
            #region VIRTUALTS
            else if (moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtVirtualforLuaTS)
            {
                if (ServerConfig.getInstance().Language)
                {
                    //  Grid实际宽度 如果有滚动条 把滚动条计算上
                    int ActualWidth = gridControlTecnostring.Width - 10;

                    float colPercentageName = 0.28f;
                    float colPercentageNamePath = 0.34f;
                    float colPercentageDataType = 0.14f;
                    float colPercentageActualValue = 0.15f;
                    float colPercentageAddress = 1 - colPercentageName - colPercentageNamePath - colPercentageDataType - colPercentageActualValue;

                    if (column.FieldName == "Address")
                    {
                        column.Visible = false;
                        //column.Width = (int)(colPercentageAddress * ActualWidth);
                        column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    }
                    else if (column.FieldName == "Name")
                    {
                        column.Width = (int)(colPercentageName * ActualWidth);
                    }
                    else if (column.FieldName == "Path")
                    {
                        column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        column.Width = (int)(colPercentageNamePath * ActualWidth);
                    }
                    else if (column.FieldName == "Data Type")
                    {
                        column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        column.Width = (int)(colPercentageDataType * ActualWidth);
                    }
                    else if (column.FieldName == "Actual Value")
                    {
                        column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        column.Width = (int)(colPercentageActualValue * ActualWidth);
                    }
                }
                else
                {
                    //  Grid实际宽度 如果有滚动条 把滚动条计算上
                    int ActualWidth = gridControlTecnostring.Width - 10;

                    float colPercentageName = 0.28f;
                    float colPercentageNamePath = 0.36f;
                    float colPercentageDataType = 0.12f;
                    float colPercentageActualValue = 0.15f;
                    float colPercentageAddress = 1 - colPercentageName - colPercentageNamePath - colPercentageDataType - colPercentageActualValue;

                    if (column.FieldName == "地址")
                    {
                        column.Visible = false;
                        column.Width = (int)(colPercentageAddress * ActualWidth);
                        column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    }
                    else if (column.FieldName == "信号名称")
                    {
                        column.Width = (int)(colPercentageName * ActualWidth);
                    }
                    else if (column.FieldName == "脚本文件")
                    {
                        column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        column.Width = (int)(colPercentageNamePath * ActualWidth);
                    }
                    else if (column.FieldName == "数据类型")
                    {
                        column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        column.Width = (int)(colPercentageDataType * ActualWidth);
                    }
                    else if (column.FieldName == "实际值")
                    {
                        column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        column.Width = (int)(colPercentageActualValue * ActualWidth);
                    }
                }
            }
            #endregion
            #region TSFAU TSOPC TSUDP
            else if (moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtTsFAU
                || moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtTsOPC
                || moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtTsUDP)
            {
                if (ServerConfig.getInstance().Language)
                {
                    //  Grid实际宽度 如果有滚动条 把滚动条计算上
                    int ActualWidth = gridControlTecnostring.Width - 10;

                    float colPercentageName = 0.37f;
                    float colPercentageNameTime = 0.26f;
                    float colPercentageActualValue = 0.28f;
                    float colPercentageAddress = 1 - colPercentageName - colPercentageNameTime - colPercentageActualValue;

                    if (column.FieldName == "Address")
                    {
                        column.Visible = false;
                        //column.Width = (int)(colPercentageAddress * ActualWidth);
                        column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    }
                    else if (column.FieldName == "Name")
                    {
                        column.Width = (int)(colPercentageName * ActualWidth);
                    }
                    else if (column.FieldName == "Time")
                    {
                        column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        column.Width = (int)(colPercentageNameTime * ActualWidth);
                    }
                    else if (column.FieldName == "Actual Value")
                    {
                        column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        column.Width = (int)(colPercentageActualValue * ActualWidth);
                    }
                }
                else
                {
                    //  Grid实际宽度 如果有滚动条 把滚动条计算上
                    int ActualWidth = gridControlTecnostring.Width - 10;

                    float colPercentageName = 0.37f;
                    float colPercentageNameTime = 0.26f;
                    float colPercentageActualValue = 0.28f;
                    float colPercentageAddress = 1 - colPercentageName - colPercentageNameTime - colPercentageActualValue;

                    if (column.FieldName == "地址")
                    {
                        column.Visible = false;
                        column.Width = (int)(colPercentageAddress * ActualWidth);
                        column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    }
                    else if (column.FieldName == "信号名称")
                    {
                        column.Width = (int)(colPercentageName * ActualWidth);
                    }
                    else if (column.FieldName == "时间")
                    {
                        column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        column.Width = (int)(colPercentageNameTime * ActualWidth);
                    }
                    else if (column.FieldName == "实际值")
                    {
                        column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        column.Width = (int)(colPercentageActualValue * ActualWidth);
                    }
                }
            }
            #endregion
        }

        private void btnLaunchConfigurationTool_Click(object sender, EventArgs e)
        {
            //  防止打开多个IOconfig
            //foreach (Form f in Application.OpenForms)
            //{
            //    if (f is Baosight.FDAA.IOConfig.MainFrm)
            //    {

            //        f.WindowState = FormWindowState.Normal;
            //        f.BringToFront();
            //        return;
            //    }
            //}
            //Baosight.FDAA.IOConfig.MainFrm frm = new Baosight.FDAA.IOConfig.MainFrm();
            //frm.ShowInTaskbar = true;
            //frm.Show();

            string strPath = System.Windows.Forms.Application.StartupPath + @"\IOConfig.exe";
            System.IO.Directory.SetCurrentDirectory(System.Windows.Forms.Application.StartupPath);

            if (System.IO.File.Exists(strPath))
            {
                Process.Start(strPath);
            }
            else
            {
                using (new CenterWinDialog(this))
                {
                    MessageBox.Show("Can not find IOConfig.exe! Please contact your provider.", strMessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UpdateIoActualData()
        {
            try
            {
                //log.Fatal("Start Updating Io Actual Data...");
                //  如果Grid中无signal，则直接返回
                if (isDisplaySignalValues)
                {
                    if (tabControlSignalValues.SelectedIndex == 0)
                    {
                        if (gridViewAnalog.RowCount <= 0)
                        {
                            log.Debug("UpdateIoActualData(): gridViewAnalog.RowCount <= 0");
                            return;
                        }
                        
                    }
                    else if (tabControlSignalValues.SelectedIndex == 1)
                    {
                        if (gridViewDigital.RowCount <= 0)
                        {
                            log.Debug("UpdateIoActualData(): gridViewDigital.RowCount <= 0");
                            return;
                        }
                    }
                    //  获取选中树节点的模块号
                    ushort moduleNo = Convert.ToUInt16(this.treeView1.SelectedNode.Tag);

                    //  根据模块号获取信号值
                    PMSServer.Instance.GetSignalValues(moduleNo);

                    if (tabControlSignalValues.SelectedIndex == 0)
                    {
                        int top = gridViewAnalog.TopRowIndex;
                        int count = (gridViewAnalog.RowCount - top) > MaxRowsCount ? MaxRowsCount : gridViewAnalog.RowCount - top;
                        string address;

                        for (int i = top; i < top + count; i++)
                        {
                            if (ServerConfig.getInstance().Language)
                            {
                                address = gridViewAnalog.GetDataRow(i)["Address"].ToString();
                                IOConfig.getInstance().AllModules[moduleNo].AnalogDataTable.Rows[i]["Actual Value"] = PMSServer.Instance.SignalValues[address].ToString("0.000");
                            }
                            else
                            {
                                address = gridViewAnalog.GetDataRow(i)["地址"].ToString();
                                IOConfig.getInstance().AllModules[moduleNo].AnalogDataTable.Rows[i]["实际值"] = PMSServer.Instance.SignalValues[address].ToString("0.000");
                            }
                        }
                    }
                    else
                    {
                        int top = gridViewDigital.TopRowIndex;
                        int count = (gridViewDigital.RowCount - top) > MaxRowsCount ? MaxRowsCount : gridViewDigital.RowCount - top;
                        string address;

                        for (int i = top; i < top + count; i++)
                        {
                            if (ServerConfig.getInstance().Language)
                            {
                                address = gridViewDigital.GetDataRow(i)["Address"].ToString();
                                IOConfig.getInstance().AllModules[moduleNo].DigitalDataTable.Rows[i]["Actual Value"] = PMSServer.Instance.SignalValues[address].ToString("0");
                            }
                            else
                            {
                                address = gridViewDigital.GetDataRow(i)["地址"].ToString();
                                IOConfig.getInstance().AllModules[moduleNo].DigitalDataTable.Rows[i]["实际值"] = PMSServer.Instance.SignalValues[address].ToString("0");
                            }
                        }
                    }
                }
                else
                {
                    if (gridViewTecnostring.RowCount < 1) return;
                    //  获取选中树节点的模块号
                    ushort moduleNo = Convert.ToUInt16(this.treeView1.SelectedNode.Tag);
                    //  根据模块号获取信号值
                    PMSServer.Instance.GetStringValues(moduleNo);

                    int top = gridViewTecnostring.TopRowIndex;
                    int count = (gridViewTecnostring.RowCount - top) > MaxRowsCount ? MaxRowsCount : gridViewTecnostring.RowCount - top;
                    string address;

                    for (int i = top; i < top + count; i++)
                    {
                        if (ServerConfig.getInstance().Language)
                        {
                            address = gridViewTecnostring.GetDataRow(i)["Address"].ToString();
                            if (IOConfig.getInstance().AllModules[moduleNo].MdType != IOConfig.MODULE_TYPE.mtVirtualforLuaTS)
                                IOConfig.getInstance().AllModules[moduleNo].SectionDataTable.Rows[i]["Time"] = PMSServer.Instance.StringValues[address]._dTime;
                            IOConfig.getInstance().AllModules[moduleNo].SectionDataTable.Rows[i]["Actual Value"] = PMSServer.Instance.StringValues[address]._strValue;
                        }
                        else
                        {
                            address = gridViewTecnostring.GetDataRow(i)["地址"].ToString();
                            if (IOConfig.getInstance().AllModules[moduleNo].MdType != IOConfig.MODULE_TYPE.mtVirtualforLuaTS)
                                IOConfig.getInstance().AllModules[moduleNo].SectionDataTable.Rows[i]["时间"] = PMSServer.Instance.StringValues[address]._dTime;
                            IOConfig.getInstance().AllModules[moduleNo].SectionDataTable.Rows[i]["实际值"] = PMSServer.Instance.StringValues[address]._strValue;
                        }
                    }
                }
                log.Debug("End Updating Io Actual Data");
                log.Debug("---------------------------------------------------------------------------------------------------------------------");
            }
            catch (Exception ex)
            {
                log.Fatal(ex);
            }
        }

        private void gridViewAnalog_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            try
            {
                //  如果当前模块链路状态不通，则不进行红色显示判断
                if (treeView1.SelectedNode.ImageIndex != 0) return;
                //  获取选中树节点的模块号
                ushort moduleNo = Convert.ToUInt16(this.treeView1.SelectedNode.Tag);
                bool language = ServerConfig.getInstance().Language;
                if (!(IOConfig.getInstance().AllModules[moduleNo].MdType == IOConfig.MODULE_TYPE.mtS7TCP ||
                    IOConfig.getInstance().AllModules[moduleNo].MdType == IOConfig.MODULE_TYPE.mtS7TCP300 ||
                    IOConfig.getInstance().AllModules[moduleNo].MdType == IOConfig.MODULE_TYPE.mtS7TCP400DB ||
                    IOConfig.getInstance().AllModules[moduleNo].MdType == IOConfig.MODULE_TYPE.mtS7TCP400NoneDB ||
                    IOConfig.getInstance().AllModules[moduleNo].MdType == IOConfig.MODULE_TYPE.mtS7PNRequest ||
                    IOConfig.getInstance().AllModules[moduleNo].MdType == IOConfig.MODULE_TYPE.mtS7DPRequest||
                    IOConfig.getInstance().AllModules[moduleNo].MdType == IOConfig.MODULE_TYPE.mtUDPMulticast))
                    return;

                GridView gv = sender as GridView;
                if (e.RowHandle >= 0)
                {
                    string actualValue = string.Empty;
                    if(language)
                        actualValue = gv.GetRowCellDisplayText(e.RowHandle, gv.Columns["Actual Value"]);
                    else
                        actualValue = gv.GetRowCellDisplayText(e.RowHandle, gv.Columns["实际值"]);

                    if (actualValue == "-99999.000")
                    {
                        e.Appearance.ForeColor = Color.Red;
                    }
                    else
                    {
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void gridViewDigital_RowStyle(object sender, RowStyleEventArgs e)
        {
            try
            {
                //  如果当前模块链路状态不通，则不进行红色显示判断
                if (treeView1.SelectedNode.ImageIndex != 0) return;
                //  获取选中树节点的模块号
                ushort moduleNo = Convert.ToUInt16(this.treeView1.SelectedNode.Tag);
                bool language = ServerConfig.getInstance().Language;
                if (!(IOConfig.getInstance().AllModules[moduleNo].MdType == IOConfig.MODULE_TYPE.mtS7TCP ||
                    IOConfig.getInstance().AllModules[moduleNo].MdType == IOConfig.MODULE_TYPE.mtS7TCP300 ||
                    IOConfig.getInstance().AllModules[moduleNo].MdType == IOConfig.MODULE_TYPE.mtS7TCP400DB ||
                    IOConfig.getInstance().AllModules[moduleNo].MdType == IOConfig.MODULE_TYPE.mtS7TCP400NoneDB ||
                    IOConfig.getInstance().AllModules[moduleNo].MdType == IOConfig.MODULE_TYPE.mtS7PNRequest ||
                    IOConfig.getInstance().AllModules[moduleNo].MdType == IOConfig.MODULE_TYPE.mtS7DPRequest))
                    return;

                GridView gv = sender as GridView;
                if (e.RowHandle >= 0)
                {
                    string actualValue = string.Empty;
                    if (language)
                        actualValue = gv.GetRowCellDisplayText(e.RowHandle, gv.Columns["Actual Value"]);
                    else
                        actualValue = gv.GetRowCellDisplayText(e.RowHandle, gv.Columns["实际值"]);

                    if (actualValue == "-99999")
                    {
                        e.Appearance.ForeColor = Color.Red;
                    }
                    else
                    {
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void UpdateModuleStatus()
        {
            try
            {
                if (IOConfig.getInstance().EnableModuleMapping == false) return;
                foreach (TreeNode node in treeView1.Nodes)
                {
                    ushort moduleNo = Convert.ToUInt16(node.Tag);
                    if (!IOConfig.getInstance().AllModules.ContainsKey(moduleNo)) continue;
                    var moduleType = IOConfig.getInstance().AllModules[moduleNo].MdType;
                    if (moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtUDPInteger
                        || moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtUDPReal
                        || moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtUDPUserDefined
                        || moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtCP3550
                        || moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtOPCClient
                        || moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtMelsecQ
                        || moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtHitachiR700
                        || moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtHitachiR900
                        || moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtToshibaV3000
                        || moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtToshibaNV
                        || moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtS7TCP300
                        || moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtS7TCP400DB
                        || moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtS7TCP400NoneDB
                        || moduleType == PMSServiceStatus.IOConfig.MODULE_TYPE.mtS7TCP)
                    {
                        ushort mappingModuleNo = (ushort)(moduleNo + MappingOffset);
                        PMSServer.Instance.GetSignalValues(mappingModuleNo);
                        if (PMSServer.Instance.SignalValues.Count == 0)
                            return;
                        if (PMSServer.Instance.SignalValues.First().Value == 1)
                        {
                            // 仅在采集状态变化时刷新
                            if (node.ImageIndex != 0)
                            {
                                node.ImageIndex = 0;
                                node.SelectedImageIndex = 0;
                            }
                        }
                        else
                        {
                            // 仅在采集状态变化时刷新
                            if (node.ImageIndex != 6)
                            {
                                node.ImageIndex = 6;
                                node.SelectedImageIndex = 6;
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                log.Fatal(exception.Message);
            }
        }

        private void gridViewDigital_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            // 新增的行RowHandle为负值，无需显示row indicator
            if (e.RowHandle < 0) return;

            // 仅对Row indicator进行处理
            if (e.Info.IsRowIndicator)
            {
                string address = string.Empty;
                if (ServerConfig.getInstance().Language)
                {
                    address = gridViewDigital.GetDataRow(e.RowHandle)["Address"].ToString();
                    //if (!Helper.IsNumeric(address))
                    //{
                    //    address = gridViewDigital.GetDataRow(e.RowHandle)["SignalAddress"].ToString();
                    //}
                }
                    
                else
                {
                    address = gridViewDigital.GetDataRow(e.RowHandle)["地址"].ToString();
                    //if (!Helper.IsNumeric(address))
                    //{
                    //    address = gridViewDigital.GetDataRow(e.RowHandle)["信号地址"].ToString();
                    //}
                }
                    
                // 显示行数
                e.Info.DisplayText = address;
                // 不显示图标
                e.Info.ImageIndex = -1;
            }
        }

        private void gridViewAnalog_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            // 新增的行RowHandle为负值，无需显示row indicator
            if (e.RowHandle < 0) return;

            // 仅对Row indicator进行处理
            if (e.Info.IsRowIndicator)
            {
                string address = string.Empty;
                if (ServerConfig.getInstance().Language)
                {
                    address = gridViewAnalog.GetDataRow(e.RowHandle)["Address"].ToString();
                    if(!Helper.IsNumeric(address))
                    {
                        address = gridViewAnalog.GetDataRow(e.RowHandle)["SignalAddress"].ToString();
                    }
                }
                    
                else
                {
                    address = gridViewAnalog.GetDataRow(e.RowHandle)["地址"].ToString();
                    if (!Helper.IsNumeric(address))
                    {
                        address = gridViewAnalog.GetDataRow(e.RowHandle)["信号地址"].ToString();
                    }
                }
                // 显示行数
                e.Info.DisplayText = address;
                // 不显示图标
                e.Info.ImageIndex = -1;
            }
        }

        private void gridViewTecnostring_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            // 新增的行RowHandle为负值，无需显示row indicator
            if (e.RowHandle < 0) return;

            // 仅对Row indicator进行处理
            if (e.Info.IsRowIndicator)
            {
                string address = string.Empty;
                if (ServerConfig.getInstance().Language)
                    address = gridViewTecnostring.GetDataRow(e.RowHandle)["Address"].ToString();
                else
                    address = gridViewTecnostring.GetDataRow(e.RowHandle)["地址"].ToString();
                // 显示行数
                e.Info.DisplayText = address;
                // 不显示图标
                e.Info.ImageIndex = -1;
            }
        }

        #endregion

        #region 诊断页

        #region General diagnostic

        void UpdateFrameLostConfig()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(UpdateFrameLost));
            }
            else
            {
                UpdateFrameLost();
            }
        }

        void UpdateFrameLost()
        {
            try
            {
                txtSignalUsed.Text = IOConfig.getInstance().GetSignalName(IOConfig.getInstance().LostFrameModuleNo, IOConfig.getInstance().LostFrameSignalNo);
                txtMaximumValue.Text = IOConfig.getInstance().MaxValue.ToString();
                txtMinimumValue.Text = IOConfig.getInstance().MinValue.ToString();
                txtIncrementValue.Text = IOConfig.getInstance().IncrementValue.ToString();
            }
            catch (Exception ex)
            {
                log.Fatal(ex.Message);
            }
        }

        void UpdateFrameLostCounter(PMSServer.FDAA_SERVICE_STATUS svrStatus)
        {
            try
            {
                if (IOConfig.getInstance().LostFrameModuleNo == -1 && IOConfig.getInstance().LostFrameSignalNo == -1)
                {
                    lblAcquisitionHitRateValue.Text = "??";
                    progressBarAcquisitionHitRate.Value = 0;
                    return;
                }

                string strText = "??";
                if (svrStatus != PMSServer.FDAA_SERVICE_STATUS.RUNNING)
                {
                    lblAcquisitionHitRateValue.Text = strText;
                    progressBarAcquisitionHitRate.Value = 0;
                    return;
                }

                // 更新丢帧计数器
                long lFrameLostCounter;

                // 如果PMSService处于停止状态
                if(PMSServer.Instance.PMSService == null) return;

                PMSServer.Instance.PMSService.getLostFrameCounter(out lFrameLostCounter);

                if (PMSServer.Instance.AcqStatus == PMSServer.FDAA_ACQUISITION_STATUS.STARTED)
                {
                    long lInterrupts;
                    // 更新百分比
                    PMSServer.Instance.PMSService.getSoftInterrupts(out lInterrupts);

                    // 获得丢帧个数
                    float a = (float)lFrameLostCounter;
                    //  如果用于统计采样命中率的信号不存在，则退出计算
                    if (!IOConfig.getInstance().AllModules.ContainsKey((uint)IOConfig.getInstance().LostFrameModuleNo)) return;
                    // 计算用于统计采样命中率模块的总采样点数
                    uint timebase = IOConfig.getInstance().AllModules[(uint)IOConfig.getInstance().LostFrameModuleNo].TimeBase;
                    float b = (float)(lInterrupts / timebase);

                    float fResult =1-a/b;
                    if (fResult >= 0 && fResult <= 100)
                    {
                        lblAcquisitionHitRateValue.Text = Math.Round(fResult * 100, 2).ToString("f2") + "%";
                        progressBarAcquisitionHitRate.Value = Convert.ToInt32(Math.Round((double)(fResult * 100)));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Fatal(ex.Message);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            // 如果PMSService处于停止状态
            if(PMSServer.Instance.PMSService == null) return;
            // 清零计数器
            PMSServer.Instance.PMSService.resetCounter();
            //  暂时有疑问
            //CPMSServer::getInst().endPeriod();
            UpdateFrameLostCounter(PMSServer.Instance.ServerStatus);
        }

        private void btnAcquisitionServiceDiagnosis_Click(object sender, EventArgs e)
        {
            string strInfo;

            if (PMSServer.Instance.ServerStatus == PMSServer.FDAA_SERVICE_STATUS.UNKNOWN)
            {
                strInfo = ServerConfig.getInstance().Language
                    ? ("Service status unknown, need to initialize!")
                    : ("服务状态未知，需要初始化采集环境！");
                using (new CenterWinDialog(this)) {
                    MessageBox.Show(strInfo, strMessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return;
            }
            else if (PMSServer.Instance.ServerStatus == PMSServer.FDAA_SERVICE_STATUS.RUNNING)
            {
                strInfo = ServerConfig.getInstance().Language
                    ? ("Service status abnormal, need to initialize!")
                    : ("服务状态异常，需要初始化采集环境！");

                if (PMSServer.Instance.PMSService == null)
                {
                    using (new CenterWinDialog(this))
                    {
                        MessageBox.Show(strInfo, strMessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    return;
                }
                long lInterrupts;
                try 
                {	        
                    PMSServer.Instance.PMSService.getSoftInterrupts(out lInterrupts);
                }
                catch (Exception)
                {
                    using (new CenterWinDialog(this))
                    {
                        MessageBox.Show(strInfo, strMessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    return;
                }
                using (new CenterWinDialog(this)) {
                    MessageBox.Show(ServerConfig.getInstance().Language ? "Service status OK!" : "服务状态正常！", strMessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }	
            else if (PMSServer.Instance.ServerStatus == PMSServer.FDAA_SERVICE_STATUS.STOPPED)
            {
                strInfo = ServerConfig.getInstance().Language
                    ? "Service status stopped (or disabled) !"
                    : "服务处于停止（或禁用）状态！";
                using (new CenterWinDialog(this))
                {
                    MessageBox.Show(strInfo, strMessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnAcquisitionEnvironmentInitialize_Click(object sender, EventArgs e)
        {
            string strInfo = ServerConfig.getInstance().Language
                ? "Acquisition environment will be reset, all related programs will be shutdown.\nWould you like to continue?"
                : "将对采集环境执行初始化操作，所有相关的程序会被强制关闭\n您确定是否继续？";

            using (new CenterWinDialog(this))
            {
                if (MessageBox.Show(strInfo, strMessageBoxTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }
            }
            SendExitGuardianRequest();
            InitAcquisitionEnvironment();
        }

        private void InitAcquisitionEnvironment()
        {
            string strPath = Application.StartupPath + @"\ServiceInstall.bat";
            if (File.Exists(strPath))
            {
                Process.Start(strPath);
            }
            else
            {
                using (new CenterWinDialog(this))
                {
                    MessageBox.Show("Can not find ServiceInstall.bat! Please contact your provider.", strMessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        StaffListPanel c;
        Timer tmr;
        private void ScrollStart()
        {
            panel1.Controls.Clear();

            int height;
            do
            {
                c = new StaffListPanel();
                panel1.Controls.Add(c);
                height = SetLocationY(0);
            } while (height - c.Height < panel1.Height);

            c = (StaffListPanel)panel1.Controls[0];
            c.Dock = DockStyle.None;
            tmr = new Timer(dowork, null, 200, 200);
        }

        private void dowork(object state)
        {
            try
            {
                // 每次移动2像素。可以自由调整
                var d = 2;
                panel1.BeginInvoke((Action)delegate
                {
                    var loc = c.Location;
                    var yd = loc.Y - d;
                    if (yd <= -c.Height)
                        SetLocationY(0);
                    else
                        SetLocationY(yd);
                });
            }
            catch (Exception e)
            {
            }
        }

        private int SetLocationY(int y)
        {
            foreach (StaffListPanel c in panel1.Controls)
            {
                c.Location = new Point(0, y);
                y += c.Height;
            }
            return y;
        }

            #endregion

        #region Advanced diagnostic

        private void OneClickDiagnostic_Click(object sender, EventArgs e)
        {
            if (CheckAcquisitionStatus()) return;

            InitBeforeDiagnosticControl();

            List<Task> taskList = new List<Task>();
            //  开启诊断
            taskList.Add(Task.Run(() => Diagnose()));
            //  诊断完成后设置按钮按钮状态
            Task.WhenAll(taskList.ToArray()).ContinueWith(t =>
            {
                BeginInvoke(new MethodInvoker(InitAfterDiagnosticControl));
            });
        }

        /// <summary>
        ///     检查采集状态
        /// </summary>
        /// <returns>开启false 未开启true</returns>
        private bool CheckAcquisitionStatus()
        {
            if (PMSServer.Instance.AcqStatus != PMSServer.FDAA_ACQUISITION_STATUS.STARTED)
            {
                MessageBox.Show(strDiagnoseAcquisitionNotStarted, "Prompt", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return true;
            }

            return false;
        }

        /// <summary>
        ///     设置诊断开始前高级诊断页初始状态
        /// </summary>
        private void InitAfterDiagnosticControl()
        {
            btnOneClickDiagnostic.Enabled = true;
            btnExport.Enabled = true;
            btnExpand.Enabled = true;
            btnCollapse.Enabled = true;
            toolStripAdvancedDiagnose.Enabled = true;
            diagnosticProgressBar.Value = diagnosticProgressBar.Maximum;
            diagnosticProgressBar.Value = diagnosticProgressBar.Maximum - 1;
            statusStrip1.Refresh();
            Thread.Sleep(300);
            diagnosticProgressBar.Visible = false;
            diagnosticProgressText.Text = diagnosticCompleted;
            var passedCount = panelList.Count(p => p.Status == "OK" || p.Status == "通过");
            var passRate = passedCount * 100 / panelList.Count;
            toolStripPassRate.Text = passedCount + " / " + panelList.Count + " = " + passRate + " %" ;
            toolStripPassRate.Visible = true;
        }

        /// <summary>
        ///     设置诊断结束高级诊断页初始状态
        /// </summary>
        private void InitBeforeDiagnosticControl()
        {
            //  检测中禁用导出按钮
            btnOneClickDiagnostic.Enabled = false;
            toolStripAdvancedDiagnose.Enabled = false;
            toolStripPassRate.Visible = false;
            btnExport.Enabled = false;
            btnExpand.Enabled = false;
            btnCollapse.Enabled = false;
            diagnosticProgressBar.Value = 0;
            diagnosticProgressBar.Visible = true;
            //  清空上次诊断结果
            RemoveFlowlayoutpannel();
        }

        /// <summary>
        /// 清空advancedFlowLayoutPanel下所有延展收缩控件
        /// </summary>
        private void RemoveFlowlayoutpannel()
        {
            var listControls = new List<Control>();

            foreach (Control control in advancedFlowLayoutPanel1.Controls) listControls.Add(control);

            foreach (var control in listControls)
                if (control is ExpandCollapsePanel)
                {
                    advancedFlowLayoutPanel1.Controls.Remove(control);
                    control.Dispose();
                }
        }

        /// <summary>
        ///     已配置的功能模块填充FlowLayoutPanel，进行诊断，更新界面UI
        /// </summary>
        private void Diagnose()
        {
            var packageManager = new PackageManager();

            FillFlowLayoutPanel(packageManager);

            ExecuteDiagnose(packageManager);
        }

        /// <summary>
        ///     多线程进行诊断所有已配置的功能模块
        /// </summary>
        /// <param name="packageManager"></param>
        private void ExecuteDiagnose(PackageManager packageManager)
        {
            var taskList = new List<Task>();
            var count = 0;

            foreach (var panel in panelList)
            {
                Action action = () =>
                {
                    try
                    {
                        var package = packageManager.Packages.FirstOrDefault(p => p.PackageName == panel.Text);
                        if (InitPackage(package, panel)) return;

                        //  获取诊断结果
                        var codeInfos = package.Diagnose();

                        //  更新进度条和完成状态文字
                        lock (progressbarLocker)
                        {
                            count = UpdateProgressBar(count);
                        }

                        //  更新诊断结果dataGridView
                        BeginInvoke(new MethodInvoker(() => { FillDiagnosticResult(codeInfos, panel); }));
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        BeginInvoke(new MethodInvoker(() =>
                        {
                            panel.Status = ServerConfig.getInstance().Language ? "Exception interrupt" : "异常中断";
                        }));
                    }
                };
                taskList.Add(Task.Run(action));
            }

            Task.WaitAll(taskList.ToArray());
        }

        /// <summary>
        ///     更新进度条
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        private int UpdateProgressBar(int count)
        {
            diagnosticProgressText.Text = string.Format("{0} ({1}/{2}) ", diagnosing, ++count, panelList.Count);
            Invoke(new MethodInvoker(() =>
            {
                diagnosticProgressBar.Value += 1 * diagnosticProgressBar.Maximum / panelList.Count;
            }));
            return count;
        }

        /// <summary>
        ///     初始化功能包
        /// </summary>
        /// <param name="package"></param>
        /// <param name="panel"></param>
        /// <returns></returns>
        private bool InitPackage(BasePackage package, ExpandCollapsePanel panel)
        {
            if (package == null) return true;
            package.StatusEvent += (sender, args) =>
            {
                BeginInvoke(new MethodInvoker(() =>
                {
                    panel.Status = ServerConfig.getInstance().Language
                        ? args.DiagnosticStatus.GetAttribute<EnglishAttribute>().Name
                        : args.DiagnosticStatus.GetAttribute<ChineseAttribute>().Name;
                }));
            };
            return false;
        }

        /// <summary>
        ///     用诊断结果填充dataGridView
        /// </summary>
        /// <param name="codeInfos">诊断结果集合</param>
        /// <param name="panel">可伸缩面板</param>
        private void FillDiagnosticResult(List<CodeInfo> codeInfos, ExpandCollapsePanel panel)
        {
            var resultDataTable = GetResultDataTable();

            foreach (var codeInfo in codeInfos)
            {
                if (codeInfo.Id == 0) continue;
                var dr = resultDataTable.NewRow();
                dr[Constant.ColumnHearCode] = codeInfo.Id.GetHashCode();
                dr[Constant.ColumnHearDetail] = codeInfo.SpecificInformation == string.Empty
                    ? codeInfo.Detail
                    : codeInfo.Detail + "（" + codeInfo.SpecificInformation + "）";
                dr[Constant.ColumnHearSuggestion] =
                    string.Join("\n\n", codeInfo.RecommendedMeasures.Split('&'));
                if (codeInfo.Id.GetHashCode() != 0)
                    resultDataTable.Rows.Add(dr);
            }

            SetDiagnosticResultProperties(panel, resultDataTable);
        }

        /// <summary>
        ///     获取诊断结果DataTable
        /// </summary>
        /// <returns></returns>
        private DataTable GetResultDataTable()
        {
            var resultDataTable = new DataTable();
            resultDataTable.Columns.Add(Constant.ColumnHearCode);
            resultDataTable.Columns.Add(Constant.ColumnHearDetail);
            resultDataTable.Columns.Add(Constant.ColumnHearSuggestion);
            return resultDataTable;
        }

        /// <summary>
        ///     设置诊断结果dataGridView属性
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="resultDataTable"></param>
        private void SetDiagnosticResultProperties(ExpandCollapsePanel panel, DataTable resultDataTable)
        {
            if (resultDataTable.Rows.Count > 0)
            {
                panel.dataGridView1.DataSource = resultDataTable;

                //  dataGridView 列宽设为百分比
                panel.dataGridView1.Columns[Constant.ColumnHearCode].FillWeight = 7;
                panel.dataGridView1.Columns[Constant.ColumnHearDetail].FillWeight = 33;
                panel.dataGridView1.Columns[Constant.ColumnHearSuggestion].FillWeight = 60;
                panel.dataGridView1.ColumnHeadersHeight = 28;
                panel.dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            }
        }

        /// <summary>
        ///     用已配置的功能模块初始化FlowLayoutPanel
        /// </summary>
        /// <param name="packageManager"></param>
        private void FillFlowLayoutPanel(PackageManager packageManager)
        {
            panelList = new List<ExpandCollapsePanel>();

            foreach (var package in packageManager.Packages)
            {
                //  设置ExpandCollapsePanel属性
                var panel = new ExpandCollapsePanel();
                panel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                panel.BorderStyle = BorderStyle.FixedSingle;
                panel.Status = notStarted;
                panel.Text = package.PackageName;
                panel.UseAnimation = false;
                panel.IsExpanded = false;

                if (radioButtonDiagnoseConfiguredPackages.Checked)
                {
                    //  仅诊断已配置的功能模块
                    if (!package.IsConfigured) continue;
                    if (!advancedFlowLayoutPanel1.InvokeRequired) continue;

                    panelList.Add(panel);
                }
                else
                {
                    if (!advancedFlowLayoutPanel1.InvokeRequired) continue;
                    panelList.Add(panel);
                }
            }

            advancedFlowLayoutPanel1.BeginInvoke(new MethodInvoker(() =>
            {
                advancedFlowLayoutPanel1.Controls.AddRange(panelList.ToArray());
            }));
        }

        /// <summary>
        /// 展开全部诊断结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExpand_Click(object sender, EventArgs e)
        {
            if(panelList == null || panelList.Count == 0) return;
            foreach (var panel in panelList)
            {
                if (panel.Status == "未通过" || panel.Status == "Not passed")
                {
                    panel.IsExpanded = true;
                }
            }
        }

        /// <summary>
        /// 收缩全部诊断结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCollapse_Click(object sender, EventArgs e)
        {
            if (panelList == null || panelList.Count == 0) return;
            foreach (var panel in panelList)
            {
                panel.IsExpanded = false;
            }
        }

        /// <summary>
        ///     导出诊断结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (panelList == null || panelList.Count == 0)
            {
                MessageBox.Show(strExportError, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var exportFrm = new ExportFrm(panelList.Select(p => p.dataGridView1));
            exportFrm.ShowDialog();
        }

        private void toolStripViewInfo_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            var frm = new InstallationLicenseFrm();
            frm.ShowDialog();
            Cursor.Current = Cursors.Default;
        }

        #endregion

        #endregion

        #region 窗体属性及托盘

        /// <summary>
        /// 最小化到托盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainFrm_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                this.Hide();
            }
        }

        /// <summary>
        /// 点击主窗体右上角关闭 最小化到托盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                //  右上角的关闭按钮 最小化托盘
                if(!closeFromTray)
                {
                    CloseFrm closeFrm = new CloseFrm();
                    //  closeForm 点击最小化DialogResult设置的是DialogResult.OK
                    if(closeFrm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        this.Hide();
                    }
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// 左键双击托盘显示主窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        private void toolStripMenuItemStatus_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        private void toolStripMenuItemStartservice_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            PMSServer.Instance.StartService(true);
            Cursor.Current = Cursors.Default;
        }

        private void toolStripMenuItemStopservice_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            PMSServer.Instance.StopService(true);
            Cursor.Current = Cursors.Default;
        }

        private void toolStripMenuItemAbout_Click(object sender, EventArgs e)
        {
            //  防止打开多个关于页
            foreach (Form f in Application.OpenForms)
            {
                if (f is AboutFrm)
                {
                    f.WindowState = FormWindowState.Normal;
                    f.BringToFront();
                    return;
                }
            }

            AboutFrm aboutFrm = new AboutFrm();
            aboutFrm.ShowDialog();
        }

        private void toolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            closeFromTray = true;
            this.Close();
        }

        void setTrayStatus(TrayStatus ts)
        {
            if (ts == TrayStatus.ServiceUnknown)
            {
                notifyIcon1.Icon = global::PMSServiceStatus.Properties.Resources.serviceUnknown;
                notifyIcon1.Text = ServerConfig.getInstance().Language ? "PMS Service Unknown" : "PMS 服务状态未知";
            }
            else if (ts == TrayStatus.ServiceStopped)
            {
                notifyIcon1.Icon = global::PMSServiceStatus.Properties.Resources.serviceStop;
                notifyIcon1.Text = ServerConfig.getInstance().Language ? "PMS Service Stopped" : "PMS 服务停止";
            }
            else if (ts == TrayStatus.ServiceRunningAcquisitionStopped)
            {
                notifyIcon1.Icon = global::PMSServiceStatus.Properties.Resources.acquisitionStop;
                notifyIcon1.Text = ServerConfig.getInstance().Language ? "Acquisition Stopped" : "采集已停止";
            }
            else if (ts == TrayStatus.AcuisitionStarted)
            {
                notifyIcon1.Icon = global::PMSServiceStatus.Properties.Resources.acquisitionStart;
                notifyIcon1.Text = ServerConfig.getInstance().Language ? "Acquisition Started" : "采集进行中";
            }
        }

        #endregion
    }
}
