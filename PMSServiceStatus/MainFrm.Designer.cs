namespace PMSServiceStatus
{
    partial class MainFrm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrm));
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.groupBoxOnlineClient = new System.Windows.Forms.GroupBox();
            this.listViewOnlineClient = new System.Windows.Forms.ListView();
            this.btnDisconnectAllClients = new System.Windows.Forms.Button();
            this.lblClientCountNumber = new System.Windows.Forms.Label();
            this.lblClientCount = new System.Windows.Forms.Label();
            this.groupBoxAcquisition = new System.Windows.Forms.GroupBox();
            this.listViewDriverInfo = new System.Windows.Forms.ListView();
            this.comboBoxInternalCyclicInterrupts = new System.Windows.Forms.ComboBox();
            this.lblAcquisitionStatus = new System.Windows.Forms.Label();
            this.btnAcquisitionStatus = new System.Windows.Forms.Button();
            this.btnStopAcquisition = new System.Windows.Forms.Button();
            this.btnStartAcquisition = new System.Windows.Forms.Button();
            this.chkAutoStartAcquisition = new System.Windows.Forms.CheckBox();
            this.lblInternalCyclicInterrupts = new System.Windows.Forms.Label();
            this.txtInternalCyclicInterrupts = new PMSServiceStatus.ReadOnlyTextBox();
            this.txtSoftTimebase = new PMSServiceStatus.ReadOnlyTextBox();
            this.lblSoftTimebaseUnit = new System.Windows.Forms.Label();
            this.lblSoftTimebase = new System.Windows.Forms.Label();
            this.lblDriverInfo = new System.Windows.Forms.Label();
            this.groupBoxService = new System.Windows.Forms.GroupBox();
            this.comboBoxLogLevel = new System.Windows.Forms.ComboBox();
            this.btnServiceStatus = new System.Windows.Forms.Button();
            this.btnStopService = new System.Windows.Forms.Button();
            this.btnStartService = new System.Windows.Forms.Button();
            this.txtListeningPortNo = new System.Windows.Forms.TextBox();
            this.txtLogPath = new PMSServiceStatus.ReadOnlyTextBox();
            this.txtExecutableFilePath = new PMSServiceStatus.ReadOnlyTextBox();
            this.txtServiceName = new System.Windows.Forms.TextBox();
            this.chkBoxAutostartService = new System.Windows.Forms.CheckBox();
            this.lblLogLevel = new System.Windows.Forms.Label();
            this.lblLogPath = new System.Windows.Forms.Label();
            this.lblListeningPortNo = new System.Windows.Forms.Label();
            this.lblServiceStatus = new System.Windows.Forms.Label();
            this.lblExecutableFilePath = new System.Windows.Forms.Label();
            this.lblServiceName = new System.Windows.Forms.Label();
            this.tabPageIOStatus = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.lblSignalTree = new System.Windows.Forms.Label();
            this.lblTotalSignals = new System.Windows.Forms.Label();
            this.btnLaunchConfigurationTool = new System.Windows.Forms.Button();
            this.tabControlStringValues = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.gridControlTecnostring = new DevExpress.XtraGrid.GridControl();
            this.gridViewTecnostring = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.tabControlSignalValues = new System.Windows.Forms.TabControl();
            this.tabPageAnalog = new System.Windows.Forms.TabPage();
            this.gridControlAnalog = new DevExpress.XtraGrid.GridControl();
            this.gridViewAnalog = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.tabPageDigital = new System.Windows.Forms.TabPage();
            this.gridControlDigital = new DevExpress.XtraGrid.GridControl();
            this.gridViewDigital = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.repositoryItemCheckEditDigitalActive = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.repositoryItemSpinEditDigitalOffset = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.repositoryItemComboBoxDigitalDataType = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemSpinEditDigSamplePoints = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.repositoryItemButtonEditDigName = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.lblMonitorWindow = new System.Windows.Forms.Label();
            this.tabPageDiagnostic = new System.Windows.Forms.TabPage();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBoxAcquisitionHitRate = new System.Windows.Forms.GroupBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.progressBarAcquisitionHitRate = new PMSServiceStatus.MyProgressBar();
            this.txtIncrementValue = new PMSServiceStatus.ReadOnlyTextBox();
            this.txtMinimumValue = new PMSServiceStatus.ReadOnlyTextBox();
            this.txtMaximumValue = new PMSServiceStatus.ReadOnlyTextBox();
            this.txtSignalUsed = new PMSServiceStatus.ReadOnlyTextBox();
            this.lblAcquisitionHitRateValue = new System.Windows.Forms.Label();
            this.lblAcquisitionHitRate = new System.Windows.Forms.Label();
            this.lblIncrementValue = new System.Windows.Forms.Label();
            this.lblMinimumValue = new System.Windows.Forms.Label();
            this.lblMaximumValue = new System.Windows.Forms.Label();
            this.lblSignalUsed = new System.Windows.Forms.Label();
            this.groupBoxService2 = new System.Windows.Forms.GroupBox();
            this.btnAcquisitionEnvironmentInitialize = new System.Windows.Forms.Button();
            this.btnAcquisitionServiceDiagnosis = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnOneClickDiagnostic = new System.Windows.Forms.Button();
            this.toolStripAdvancedDiagnose = new System.Windows.Forms.ToolStrip();
            this.toolStripExpandAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripCollapseAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripExport = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripViewInfo = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.diagnosticProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.diagnosticProgressText = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripPassRate = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnExpand = new System.Windows.Forms.Button();
            this.btnCollapse = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.radioButtonDiagnoseConfiguredPackages = new System.Windows.Forms.RadioButton();
            this.gridControlDiagResults = new DevExpress.XtraGrid.GridControl();
            this.gridViewDiagResults = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Description = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Suggestion = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Package = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.radioButtonDiagnoseAllPackages = new System.Windows.Forms.RadioButton();
            this.advancedFlowLayoutPanel1 = new MakarovDev.ExpandCollapsePanel.AdvancedFlowLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.UpdateDataTimer = new System.Windows.Forms.Timer(this.components);
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStripNotifyIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemStatus = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemStartservice = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemStopservice = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControlMain.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.groupBoxOnlineClient.SuspendLayout();
            this.groupBoxAcquisition.SuspendLayout();
            this.groupBoxService.SuspendLayout();
            this.tabPageIOStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControlStringValues.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlTecnostring)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTecnostring)).BeginInit();
            this.tabControlSignalValues.SuspendLayout();
            this.tabPageAnalog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlAnalog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewAnalog)).BeginInit();
            this.tabPageDigital.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDigital)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDigital)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEditDigitalActive)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEditDigitalOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxDigitalDataType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEditDigSamplePoints)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEditDigName)).BeginInit();
            this.tabPageDiagnostic.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBoxAcquisitionHitRate.SuspendLayout();
            this.groupBoxService2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.toolStripAdvancedDiagnose.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDiagResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDiagResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).BeginInit();
            this.advancedFlowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.contextMenuStripNotifyIcon.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageGeneral);
            this.tabControlMain.Controls.Add(this.tabPageIOStatus);
            this.tabControlMain.Controls.Add(this.tabPageDiagnostic);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.ImageList = this.imageList1;
            this.tabControlMain.ItemSize = new System.Drawing.Size(70, 20);
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(942, 578);
            this.tabControlMain.TabIndex = 0;
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageGeneral.Controls.Add(this.groupBoxOnlineClient);
            this.tabPageGeneral.Controls.Add(this.groupBoxAcquisition);
            this.tabPageGeneral.Controls.Add(this.groupBoxService);
            this.tabPageGeneral.ImageIndex = 0;
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 24);
            this.tabPageGeneral.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPageGeneral.Size = new System.Drawing.Size(934, 550);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            // 
            // groupBoxOnlineClient
            // 
            this.groupBoxOnlineClient.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxOnlineClient.Controls.Add(this.listViewOnlineClient);
            this.groupBoxOnlineClient.Controls.Add(this.btnDisconnectAllClients);
            this.groupBoxOnlineClient.Controls.Add(this.lblClientCountNumber);
            this.groupBoxOnlineClient.Controls.Add(this.lblClientCount);
            this.groupBoxOnlineClient.Location = new System.Drawing.Point(15, 345);
            this.groupBoxOnlineClient.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBoxOnlineClient.Name = "groupBoxOnlineClient";
            this.groupBoxOnlineClient.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBoxOnlineClient.Size = new System.Drawing.Size(901, 201);
            this.groupBoxOnlineClient.TabIndex = 3;
            this.groupBoxOnlineClient.TabStop = false;
            this.groupBoxOnlineClient.Text = "Online Client(s)";
            // 
            // listViewOnlineClient
            // 
            this.listViewOnlineClient.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listViewOnlineClient.GridLines = true;
            this.listViewOnlineClient.Location = new System.Drawing.Point(14, 60);
            this.listViewOnlineClient.Name = "listViewOnlineClient";
            this.listViewOnlineClient.Size = new System.Drawing.Size(871, 130);
            this.listViewOnlineClient.TabIndex = 8;
            this.listViewOnlineClient.UseCompatibleStateImageBehavior = false;
            this.listViewOnlineClient.View = System.Windows.Forms.View.Details;
            // 
            // btnDisconnectAllClients
            // 
            this.btnDisconnectAllClients.Location = new System.Drawing.Point(14, 24);
            this.btnDisconnectAllClients.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDisconnectAllClients.Name = "btnDisconnectAllClients";
            this.btnDisconnectAllClients.Size = new System.Drawing.Size(170, 27);
            this.btnDisconnectAllClients.TabIndex = 4;
            this.btnDisconnectAllClients.Text = "Disconnect All Clients";
            this.btnDisconnectAllClients.UseVisualStyleBackColor = true;
            this.btnDisconnectAllClients.Click += new System.EventHandler(this.btnDisconnectAllClients_Click);
            // 
            // lblClientCountNumber
            // 
            this.lblClientCountNumber.AutoSize = true;
            this.lblClientCountNumber.Location = new System.Drawing.Point(246, 29);
            this.lblClientCountNumber.Name = "lblClientCountNumber";
            this.lblClientCountNumber.Size = new System.Drawing.Size(16, 17);
            this.lblClientCountNumber.TabIndex = 0;
            this.lblClientCountNumber.Text = "0";
            // 
            // lblClientCount
            // 
            this.lblClientCount.AutoSize = true;
            this.lblClientCount.Location = new System.Drawing.Point(208, 29);
            this.lblClientCount.Name = "lblClientCount";
            this.lblClientCount.Size = new System.Drawing.Size(55, 17);
            this.lblClientCount.TabIndex = 0;
            this.lblClientCount.Text = "Count :";
            // 
            // groupBoxAcquisition
            // 
            this.groupBoxAcquisition.Controls.Add(this.listViewDriverInfo);
            this.groupBoxAcquisition.Controls.Add(this.comboBoxInternalCyclicInterrupts);
            this.groupBoxAcquisition.Controls.Add(this.lblAcquisitionStatus);
            this.groupBoxAcquisition.Controls.Add(this.btnAcquisitionStatus);
            this.groupBoxAcquisition.Controls.Add(this.btnStopAcquisition);
            this.groupBoxAcquisition.Controls.Add(this.btnStartAcquisition);
            this.groupBoxAcquisition.Controls.Add(this.chkAutoStartAcquisition);
            this.groupBoxAcquisition.Controls.Add(this.lblInternalCyclicInterrupts);
            this.groupBoxAcquisition.Controls.Add(this.txtInternalCyclicInterrupts);
            this.groupBoxAcquisition.Controls.Add(this.txtSoftTimebase);
            this.groupBoxAcquisition.Controls.Add(this.lblSoftTimebaseUnit);
            this.groupBoxAcquisition.Controls.Add(this.lblSoftTimebase);
            this.groupBoxAcquisition.Controls.Add(this.lblDriverInfo);
            this.groupBoxAcquisition.Location = new System.Drawing.Point(460, 12);
            this.groupBoxAcquisition.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBoxAcquisition.Name = "groupBoxAcquisition";
            this.groupBoxAcquisition.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBoxAcquisition.Size = new System.Drawing.Size(456, 325);
            this.groupBoxAcquisition.TabIndex = 1;
            this.groupBoxAcquisition.TabStop = false;
            this.groupBoxAcquisition.Text = "Acquisition";
            // 
            // listViewDriverInfo
            // 
            this.listViewDriverInfo.GridLines = true;
            this.listViewDriverInfo.Location = new System.Drawing.Point(137, 112);
            this.listViewDriverInfo.Name = "listViewDriverInfo";
            this.listViewDriverInfo.Size = new System.Drawing.Size(289, 112);
            this.listViewDriverInfo.TabIndex = 8;
            this.listViewDriverInfo.UseCompatibleStateImageBehavior = false;
            // 
            // comboBoxInternalCyclicInterrupts
            // 
            this.comboBoxInternalCyclicInterrupts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxInternalCyclicInterrupts.FormattingEnabled = true;
            this.comboBoxInternalCyclicInterrupts.Location = new System.Drawing.Point(347, 284);
            this.comboBoxInternalCyclicInterrupts.Name = "comboBoxInternalCyclicInterrupts";
            this.comboBoxInternalCyclicInterrupts.Size = new System.Drawing.Size(75, 24);
            this.comboBoxInternalCyclicInterrupts.TabIndex = 7;
            this.comboBoxInternalCyclicInterrupts.SelectionChangeCommitted += new System.EventHandler(this.comboBoxInternalCyclicInterrupts_SelectionChangeCommitted);
            // 
            // lblAcquisitionStatus
            // 
            this.lblAcquisitionStatus.AutoSize = true;
            this.lblAcquisitionStatus.Location = new System.Drawing.Point(18, 35);
            this.lblAcquisitionStatus.Name = "lblAcquisitionStatus";
            this.lblAcquisitionStatus.Size = new System.Drawing.Size(124, 17);
            this.lblAcquisitionStatus.TabIndex = 7;
            this.lblAcquisitionStatus.Text = "Acquisition Status :";
            // 
            // btnAcquisitionStatus
            // 
            this.btnAcquisitionStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnAcquisitionStatus.Enabled = false;
            this.btnAcquisitionStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAcquisitionStatus.Location = new System.Drawing.Point(157, 31);
            this.btnAcquisitionStatus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAcquisitionStatus.Name = "btnAcquisitionStatus";
            this.btnAcquisitionStatus.Size = new System.Drawing.Size(75, 25);
            this.btnAcquisitionStatus.TabIndex = 6;
            this.btnAcquisitionStatus.Text = "Unknown";
            this.btnAcquisitionStatus.UseVisualStyleBackColor = false;
            // 
            // btnStopAcquisition
            // 
            this.btnStopAcquisition.Enabled = false;
            this.btnStopAcquisition.Location = new System.Drawing.Point(329, 30);
            this.btnStopAcquisition.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnStopAcquisition.Name = "btnStopAcquisition";
            this.btnStopAcquisition.Size = new System.Drawing.Size(75, 27);
            this.btnStopAcquisition.TabIndex = 5;
            this.btnStopAcquisition.Text = "Stop";
            this.btnStopAcquisition.UseVisualStyleBackColor = true;
            this.btnStopAcquisition.Click += new System.EventHandler(this.btnStopAcquisition_Click);
            // 
            // btnStartAcquisition
            // 
            this.btnStartAcquisition.Enabled = false;
            this.btnStartAcquisition.Location = new System.Drawing.Point(243, 30);
            this.btnStartAcquisition.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnStartAcquisition.Name = "btnStartAcquisition";
            this.btnStartAcquisition.Size = new System.Drawing.Size(75, 27);
            this.btnStartAcquisition.TabIndex = 4;
            this.btnStartAcquisition.Text = "Start";
            this.btnStartAcquisition.UseVisualStyleBackColor = true;
            this.btnStartAcquisition.Click += new System.EventHandler(this.btnStartAcquisition_Click);
            // 
            // chkAutoStartAcquisition
            // 
            this.chkAutoStartAcquisition.AutoSize = true;
            this.chkAutoStartAcquisition.Location = new System.Drawing.Point(21, 77);
            this.chkAutoStartAcquisition.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkAutoStartAcquisition.Name = "chkAutoStartAcquisition";
            this.chkAutoStartAcquisition.Size = new System.Drawing.Size(315, 21);
            this.chkAutoStartAcquisition.TabIndex = 1;
            this.chkAutoStartAcquisition.Text = " Auto-start data acquisition when service starts";
            this.chkAutoStartAcquisition.UseVisualStyleBackColor = true;
            this.chkAutoStartAcquisition.CheckedChanged += new System.EventHandler(this.chkAutoStartAcquisition_CheckedChanged);
            // 
            // lblInternalCyclicInterrupts
            // 
            this.lblInternalCyclicInterrupts.AutoSize = true;
            this.lblInternalCyclicInterrupts.Location = new System.Drawing.Point(18, 286);
            this.lblInternalCyclicInterrupts.Name = "lblInternalCyclicInterrupts";
            this.lblInternalCyclicInterrupts.Size = new System.Drawing.Size(167, 17);
            this.lblInternalCyclicInterrupts.TabIndex = 0;
            this.lblInternalCyclicInterrupts.Text = "Internal Cyclic Interrupts :";
            // 
            // txtInternalCyclicInterrupts
            // 
            this.txtInternalCyclicInterrupts.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.txtInternalCyclicInterrupts.Enabled = false;
            this.txtInternalCyclicInterrupts.Location = new System.Drawing.Point(233, 284);
            this.txtInternalCyclicInterrupts.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtInternalCyclicInterrupts.Name = "txtInternalCyclicInterrupts";
            this.txtInternalCyclicInterrupts.Size = new System.Drawing.Size(104, 24);
            this.txtInternalCyclicInterrupts.TabIndex = 2;
            this.txtInternalCyclicInterrupts.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtSoftTimebase
            // 
            this.txtSoftTimebase.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.txtSoftTimebase.Enabled = false;
            this.txtSoftTimebase.Location = new System.Drawing.Point(233, 244);
            this.txtSoftTimebase.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSoftTimebase.Name = "txtSoftTimebase";
            this.txtSoftTimebase.Size = new System.Drawing.Size(104, 24);
            this.txtSoftTimebase.TabIndex = 2;
            this.txtSoftTimebase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblSoftTimebaseUnit
            // 
            this.lblSoftTimebaseUnit.AutoSize = true;
            this.lblSoftTimebaseUnit.Location = new System.Drawing.Point(349, 248);
            this.lblSoftTimebaseUnit.Name = "lblSoftTimebaseUnit";
            this.lblSoftTimebaseUnit.Size = new System.Drawing.Size(26, 17);
            this.lblSoftTimebaseUnit.TabIndex = 0;
            this.lblSoftTimebaseUnit.Text = "ms";
            // 
            // lblSoftTimebase
            // 
            this.lblSoftTimebase.AutoSize = true;
            this.lblSoftTimebase.Location = new System.Drawing.Point(18, 250);
            this.lblSoftTimebase.Name = "lblSoftTimebase";
            this.lblSoftTimebase.Size = new System.Drawing.Size(103, 17);
            this.lblSoftTimebase.TabIndex = 0;
            this.lblSoftTimebase.Text = "Soft Timebase :";
            // 
            // lblDriverInfo
            // 
            this.lblDriverInfo.AutoSize = true;
            this.lblDriverInfo.Location = new System.Drawing.Point(18, 119);
            this.lblDriverInfo.Name = "lblDriverInfo";
            this.lblDriverInfo.Size = new System.Drawing.Size(82, 17);
            this.lblDriverInfo.TabIndex = 0;
            this.lblDriverInfo.Text = "Driver Info :";
            // 
            // groupBoxService
            // 
            this.groupBoxService.Controls.Add(this.comboBoxLogLevel);
            this.groupBoxService.Controls.Add(this.btnServiceStatus);
            this.groupBoxService.Controls.Add(this.btnStopService);
            this.groupBoxService.Controls.Add(this.btnStartService);
            this.groupBoxService.Controls.Add(this.txtListeningPortNo);
            this.groupBoxService.Controls.Add(this.txtLogPath);
            this.groupBoxService.Controls.Add(this.txtExecutableFilePath);
            this.groupBoxService.Controls.Add(this.txtServiceName);
            this.groupBoxService.Controls.Add(this.chkBoxAutostartService);
            this.groupBoxService.Controls.Add(this.lblLogLevel);
            this.groupBoxService.Controls.Add(this.lblLogPath);
            this.groupBoxService.Controls.Add(this.lblListeningPortNo);
            this.groupBoxService.Controls.Add(this.lblServiceStatus);
            this.groupBoxService.Controls.Add(this.lblExecutableFilePath);
            this.groupBoxService.Controls.Add(this.lblServiceName);
            this.groupBoxService.Location = new System.Drawing.Point(15, 12);
            this.groupBoxService.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBoxService.Name = "groupBoxService";
            this.groupBoxService.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBoxService.Size = new System.Drawing.Size(420, 325);
            this.groupBoxService.TabIndex = 2;
            this.groupBoxService.TabStop = false;
            this.groupBoxService.Text = "Service";
            // 
            // comboBoxLogLevel
            // 
            this.comboBoxLogLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLogLevel.FormattingEnabled = true;
            this.comboBoxLogLevel.Items.AddRange(new object[] {
            "Debug",
            "Info",
            "Warning",
            "Error",
            "Fatal",
            "Off"});
            this.comboBoxLogLevel.Location = new System.Drawing.Point(131, 283);
            this.comboBoxLogLevel.Name = "comboBoxLogLevel";
            this.comboBoxLogLevel.Size = new System.Drawing.Size(84, 24);
            this.comboBoxLogLevel.TabIndex = 7;
            this.comboBoxLogLevel.SelectionChangeCommitted += new System.EventHandler(this.comboBoxLogLevel_SelectionChangeCommitted);
            // 
            // btnServiceStatus
            // 
            this.btnServiceStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnServiceStatus.Enabled = false;
            this.btnServiceStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnServiceStatus.Location = new System.Drawing.Point(131, 156);
            this.btnServiceStatus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnServiceStatus.Name = "btnServiceStatus";
            this.btnServiceStatus.Size = new System.Drawing.Size(75, 25);
            this.btnServiceStatus.TabIndex = 6;
            this.btnServiceStatus.Text = "Unknown";
            this.btnServiceStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnServiceStatus.UseVisualStyleBackColor = false;
            // 
            // btnStopService
            // 
            this.btnStopService.Enabled = false;
            this.btnStopService.Location = new System.Drawing.Point(303, 155);
            this.btnStopService.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnStopService.Name = "btnStopService";
            this.btnStopService.Size = new System.Drawing.Size(75, 27);
            this.btnStopService.TabIndex = 5;
            this.btnStopService.Text = "Stop";
            this.btnStopService.UseVisualStyleBackColor = true;
            this.btnStopService.Click += new System.EventHandler(this.btnStopService_Click);
            // 
            // btnStartService
            // 
            this.btnStartService.Enabled = false;
            this.btnStartService.Location = new System.Drawing.Point(217, 155);
            this.btnStartService.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnStartService.Name = "btnStartService";
            this.btnStartService.Size = new System.Drawing.Size(75, 27);
            this.btnStartService.TabIndex = 4;
            this.btnStartService.Text = "Start";
            this.btnStartService.UseVisualStyleBackColor = true;
            this.btnStartService.Click += new System.EventHandler(this.btnStartService_Click);
            // 
            // txtListeningPortNo
            // 
            this.txtListeningPortNo.Location = new System.Drawing.Point(131, 201);
            this.txtListeningPortNo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtListeningPortNo.Name = "txtListeningPortNo";
            this.txtListeningPortNo.Size = new System.Drawing.Size(95, 24);
            this.txtListeningPortNo.TabIndex = 3;
            this.txtListeningPortNo.Validating += new System.ComponentModel.CancelEventHandler(this.txtListeningPortNo_Validating);
            // 
            // txtLogPath
            // 
            this.txtLogPath.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.txtLogPath.Enabled = false;
            this.txtLogPath.Location = new System.Drawing.Point(131, 243);
            this.txtLogPath.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtLogPath.Name = "txtLogPath";
            this.txtLogPath.ReadOnly = true;
            this.txtLogPath.Size = new System.Drawing.Size(249, 24);
            this.txtLogPath.TabIndex = 2;
            // 
            // txtExecutableFilePath
            // 
            this.txtExecutableFilePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtExecutableFilePath.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.txtExecutableFilePath.Enabled = false;
            this.txtExecutableFilePath.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.txtExecutableFilePath.Location = new System.Drawing.Point(131, 74);
            this.txtExecutableFilePath.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtExecutableFilePath.Name = "txtExecutableFilePath";
            this.txtExecutableFilePath.ReadOnly = true;
            this.txtExecutableFilePath.Size = new System.Drawing.Size(249, 24);
            this.txtExecutableFilePath.TabIndex = 2;
            // 
            // txtServiceName
            // 
            this.txtServiceName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtServiceName.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.txtServiceName.Enabled = false;
            this.txtServiceName.ForeColor = System.Drawing.Color.Red;
            this.txtServiceName.Location = new System.Drawing.Point(131, 33);
            this.txtServiceName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtServiceName.Name = "txtServiceName";
            this.txtServiceName.ReadOnly = true;
            this.txtServiceName.Size = new System.Drawing.Size(249, 24);
            this.txtServiceName.TabIndex = 2;
            // 
            // chkBoxAutostartService
            // 
            this.chkBoxAutostartService.AutoSize = true;
            this.chkBoxAutostartService.Location = new System.Drawing.Point(20, 117);
            this.chkBoxAutostartService.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkBoxAutostartService.Name = "chkBoxAutostartService";
            this.chkBoxAutostartService.Size = new System.Drawing.Size(273, 21);
            this.chkBoxAutostartService.TabIndex = 1;
            this.chkBoxAutostartService.Text = " Auto-start service when windows starts";
            this.chkBoxAutostartService.UseVisualStyleBackColor = true;
            this.chkBoxAutostartService.CheckedChanged += new System.EventHandler(this.chkBoxAutostartService_CheckedChanged);
            // 
            // lblLogLevel
            // 
            this.lblLogLevel.AutoSize = true;
            this.lblLogLevel.Location = new System.Drawing.Point(17, 285);
            this.lblLogLevel.Name = "lblLogLevel";
            this.lblLogLevel.Size = new System.Drawing.Size(75, 17);
            this.lblLogLevel.TabIndex = 0;
            this.lblLogLevel.Text = "Log Level :";
            // 
            // lblLogPath
            // 
            this.lblLogPath.AutoSize = true;
            this.lblLogPath.Location = new System.Drawing.Point(17, 245);
            this.lblLogPath.Name = "lblLogPath";
            this.lblLogPath.Size = new System.Drawing.Size(72, 17);
            this.lblLogPath.TabIndex = 0;
            this.lblLogPath.Text = "Log Path :";
            // 
            // lblListeningPortNo
            // 
            this.lblListeningPortNo.AutoSize = true;
            this.lblListeningPortNo.Location = new System.Drawing.Point(17, 201);
            this.lblListeningPortNo.Name = "lblListeningPortNo";
            this.lblListeningPortNo.Size = new System.Drawing.Size(121, 17);
            this.lblListeningPortNo.TabIndex = 0;
            this.lblListeningPortNo.Text = "Listening Port No :";
            // 
            // lblServiceStatus
            // 
            this.lblServiceStatus.AutoSize = true;
            this.lblServiceStatus.Location = new System.Drawing.Point(17, 161);
            this.lblServiceStatus.Name = "lblServiceStatus";
            this.lblServiceStatus.Size = new System.Drawing.Size(104, 17);
            this.lblServiceStatus.TabIndex = 0;
            this.lblServiceStatus.Text = "Service Status :";
            // 
            // lblExecutableFilePath
            // 
            this.lblExecutableFilePath.AutoSize = true;
            this.lblExecutableFilePath.Location = new System.Drawing.Point(17, 77);
            this.lblExecutableFilePath.Name = "lblExecutableFilePath";
            this.lblExecutableFilePath.Size = new System.Drawing.Size(138, 17);
            this.lblExecutableFilePath.TabIndex = 0;
            this.lblExecutableFilePath.Text = "Executable File Path :";
            // 
            // lblServiceName
            // 
            this.lblServiceName.AutoSize = true;
            this.lblServiceName.Location = new System.Drawing.Point(17, 37);
            this.lblServiceName.Name = "lblServiceName";
            this.lblServiceName.Size = new System.Drawing.Size(100, 17);
            this.lblServiceName.TabIndex = 0;
            this.lblServiceName.Text = "Service Name :";
            // 
            // tabPageIOStatus
            // 
            this.tabPageIOStatus.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageIOStatus.Controls.Add(this.splitContainer1);
            this.tabPageIOStatus.ImageIndex = 1;
            this.tabPageIOStatus.Location = new System.Drawing.Point(4, 24);
            this.tabPageIOStatus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPageIOStatus.Name = "tabPageIOStatus";
            this.tabPageIOStatus.Padding = new System.Windows.Forms.Padding(3, 4, 3, 3);
            this.tabPageIOStatus.Size = new System.Drawing.Size(934, 550);
            this.tabPageIOStatus.TabIndex = 1;
            this.tabPageIOStatus.Text = "IO Status";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            this.splitContainer1.Panel1.Controls.Add(this.lblSignalTree);
            this.splitContainer1.Panel1.Controls.Add(this.lblTotalSignals);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnLaunchConfigurationTool);
            this.splitContainer1.Panel2.Controls.Add(this.tabControlStringValues);
            this.splitContainer1.Panel2.Controls.Add(this.tabControlSignalValues);
            this.splitContainer1.Panel2.Controls.Add(this.lblMonitorWindow);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.splitContainer1.Size = new System.Drawing.Size(928, 543);
            this.splitContainer1.SplitterDistance = 214;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeView1.HideSelection = false;
            this.treeView1.HotTracking = true;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList;
            this.treeView1.Location = new System.Drawing.Point(0, 33);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(214, 476);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseClick);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Modual.bmp");
            this.imageList.Images.SetKeyName(1, "analog.ico");
            this.imageList.Images.SetKeyName(2, "digital.ico");
            this.imageList.Images.SetKeyName(3, "technostring_new.ico");
            this.imageList.Images.SetKeyName(4, "section_new.ico");
            this.imageList.Images.SetKeyName(5, "motherboard.ico");
            this.imageList.Images.SetKeyName(6, "deletered.ico");
            this.imageList.Images.SetKeyName(7, "ModuleBlocked.bmp");
            // 
            // lblSignalTree
            // 
            this.lblSignalTree.AutoSize = true;
            this.lblSignalTree.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSignalTree.Location = new System.Drawing.Point(0, 10);
            this.lblSignalTree.Name = "lblSignalTree";
            this.lblSignalTree.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.lblSignalTree.Size = new System.Drawing.Size(74, 23);
            this.lblSignalTree.TabIndex = 0;
            this.lblSignalTree.Text = "Signal Tree";
            // 
            // lblTotalSignals
            // 
            this.lblTotalSignals.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTotalSignals.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblTotalSignals.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.lblTotalSignals.ForeColor = System.Drawing.Color.Red;
            this.lblTotalSignals.Location = new System.Drawing.Point(0, 512);
            this.lblTotalSignals.Name = "lblTotalSignals";
            this.lblTotalSignals.Size = new System.Drawing.Size(214, 31);
            this.lblTotalSignals.TabIndex = 1;
            this.lblTotalSignals.Text = "??";
            this.lblTotalSignals.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnLaunchConfigurationTool
            // 
            this.btnLaunchConfigurationTool.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLaunchConfigurationTool.Image = ((System.Drawing.Image)(resources.GetObject("btnLaunchConfigurationTool.Image")));
            this.btnLaunchConfigurationTool.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLaunchConfigurationTool.Location = new System.Drawing.Point(529, 2);
            this.btnLaunchConfigurationTool.Name = "btnLaunchConfigurationTool";
            this.btnLaunchConfigurationTool.Size = new System.Drawing.Size(177, 27);
            this.btnLaunchConfigurationTool.TabIndex = 2;
            this.btnLaunchConfigurationTool.Text = "Launch Configuration Tool";
            this.btnLaunchConfigurationTool.UseVisualStyleBackColor = true;
            this.btnLaunchConfigurationTool.Click += new System.EventHandler(this.btnLaunchConfigurationTool_Click);
            // 
            // tabControlStringValues
            // 
            this.tabControlStringValues.Controls.Add(this.tabPage1);
            this.tabControlStringValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlStringValues.Font = new System.Drawing.Font("Tahoma", 8F);
            this.tabControlStringValues.ImageList = this.imageList;
            this.tabControlStringValues.ItemSize = new System.Drawing.Size(52, 22);
            this.tabControlStringValues.Location = new System.Drawing.Point(0, 5);
            this.tabControlStringValues.Name = "tabControlStringValues";
            this.tabControlStringValues.SelectedIndex = 0;
            this.tabControlStringValues.Size = new System.Drawing.Size(710, 538);
            this.tabControlStringValues.TabIndex = 4;
            this.tabControlStringValues.Visible = false;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.gridControlTecnostring);
            this.tabPage1.ImageIndex = 4;
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(702, 508);
            this.tabPage1.TabIndex = 1;
            this.tabPage1.Text = "Tecnostring";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // gridControlTecnostring
            // 
            this.gridControlTecnostring.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlTecnostring.Font = new System.Drawing.Font("Tahoma", 7.5F);
            this.gridControlTecnostring.Location = new System.Drawing.Point(0, 0);
            this.gridControlTecnostring.LookAndFeel.SkinName = "Black";
            this.gridControlTecnostring.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gridControlTecnostring.MainView = this.gridViewTecnostring;
            this.gridControlTecnostring.Name = "gridControlTecnostring";
            this.gridControlTecnostring.Size = new System.Drawing.Size(702, 508);
            this.gridControlTecnostring.TabIndex = 1;
            this.gridControlTecnostring.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewTecnostring});
            // 
            // gridViewTecnostring
            // 
            this.gridViewTecnostring.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridViewTecnostring.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gridViewTecnostring.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.gridViewTecnostring.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridViewTecnostring.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.gridViewTecnostring.Appearance.Row.Options.UseFont = true;
            this.gridViewTecnostring.Appearance.TopNewRow.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridViewTecnostring.Appearance.TopNewRow.Options.UseBackColor = true;
            this.gridViewTecnostring.GridControl = this.gridControlTecnostring;
            this.gridViewTecnostring.IndicatorWidth = 32;
            this.gridViewTecnostring.Name = "gridViewTecnostring";
            this.gridViewTecnostring.NewItemRowText = "click here to add a new signal";
            this.gridViewTecnostring.OptionsBehavior.AutoSelectAllInEditor = false;
            this.gridViewTecnostring.OptionsBehavior.Editable = false;
            this.gridViewTecnostring.OptionsBehavior.ReadOnly = true;
            this.gridViewTecnostring.OptionsCustomization.AllowFilter = false;
            this.gridViewTecnostring.OptionsCustomization.AllowSort = false;
            this.gridViewTecnostring.OptionsMenu.EnableColumnMenu = false;
            this.gridViewTecnostring.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gridViewTecnostring.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.gridViewTecnostring.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridViewTecnostring.OptionsView.ShowGroupPanel = false;
            this.gridViewTecnostring.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridViewTecnostring_CustomDrawRowIndicator);
            // 
            // tabControlSignalValues
            // 
            this.tabControlSignalValues.Controls.Add(this.tabPageAnalog);
            this.tabControlSignalValues.Controls.Add(this.tabPageDigital);
            this.tabControlSignalValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlSignalValues.Font = new System.Drawing.Font("Tahoma", 8F);
            this.tabControlSignalValues.ImageList = this.imageList;
            this.tabControlSignalValues.ItemSize = new System.Drawing.Size(52, 22);
            this.tabControlSignalValues.Location = new System.Drawing.Point(0, 5);
            this.tabControlSignalValues.Name = "tabControlSignalValues";
            this.tabControlSignalValues.SelectedIndex = 0;
            this.tabControlSignalValues.Size = new System.Drawing.Size(710, 538);
            this.tabControlSignalValues.TabIndex = 3;
            // 
            // tabPageAnalog
            // 
            this.tabPageAnalog.Controls.Add(this.gridControlAnalog);
            this.tabPageAnalog.ImageIndex = 1;
            this.tabPageAnalog.Location = new System.Drawing.Point(4, 26);
            this.tabPageAnalog.Name = "tabPageAnalog";
            this.tabPageAnalog.Size = new System.Drawing.Size(702, 508);
            this.tabPageAnalog.TabIndex = 1;
            this.tabPageAnalog.Text = "Analog";
            this.tabPageAnalog.UseVisualStyleBackColor = true;
            // 
            // gridControlAnalog
            // 
            this.gridControlAnalog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlAnalog.Font = new System.Drawing.Font("Tahoma", 7.5F);
            this.gridControlAnalog.Location = new System.Drawing.Point(0, 0);
            this.gridControlAnalog.LookAndFeel.SkinName = "Black";
            this.gridControlAnalog.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gridControlAnalog.MainView = this.gridViewAnalog;
            this.gridControlAnalog.Name = "gridControlAnalog";
            this.gridControlAnalog.Size = new System.Drawing.Size(702, 508);
            this.gridControlAnalog.TabIndex = 1;
            this.gridControlAnalog.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewAnalog});
            // 
            // gridViewAnalog
            // 
            this.gridViewAnalog.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridViewAnalog.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gridViewAnalog.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.gridViewAnalog.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridViewAnalog.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.gridViewAnalog.Appearance.Row.Options.UseFont = true;
            this.gridViewAnalog.Appearance.TopNewRow.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridViewAnalog.Appearance.TopNewRow.Options.UseBackColor = true;
            this.gridViewAnalog.GridControl = this.gridControlAnalog;
            this.gridViewAnalog.IndicatorWidth = 32;
            this.gridViewAnalog.Name = "gridViewAnalog";
            this.gridViewAnalog.NewItemRowText = "click here to add a new signal";
            this.gridViewAnalog.OptionsBehavior.AutoSelectAllInEditor = false;
            this.gridViewAnalog.OptionsBehavior.Editable = false;
            this.gridViewAnalog.OptionsBehavior.ReadOnly = true;
            this.gridViewAnalog.OptionsCustomization.AllowFilter = false;
            this.gridViewAnalog.OptionsCustomization.AllowSort = false;
            this.gridViewAnalog.OptionsMenu.EnableColumnMenu = false;
            this.gridViewAnalog.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gridViewAnalog.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridViewAnalog.OptionsView.ShowGroupPanel = false;
            this.gridViewAnalog.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridViewAnalog_CustomDrawRowIndicator);
            this.gridViewAnalog.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(this.gridViewAnalog_RowStyle);
            // 
            // tabPageDigital
            // 
            this.tabPageDigital.Controls.Add(this.gridControlDigital);
            this.tabPageDigital.ImageIndex = 2;
            this.tabPageDigital.Location = new System.Drawing.Point(4, 26);
            this.tabPageDigital.Name = "tabPageDigital";
            this.tabPageDigital.Size = new System.Drawing.Size(702, 508);
            this.tabPageDigital.TabIndex = 2;
            this.tabPageDigital.Text = "Digital";
            this.tabPageDigital.UseVisualStyleBackColor = true;
            // 
            // gridControlDigital
            // 
            this.gridControlDigital.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlDigital.Location = new System.Drawing.Point(0, 0);
            this.gridControlDigital.LookAndFeel.SkinName = "Black";
            this.gridControlDigital.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gridControlDigital.MainView = this.gridViewDigital;
            this.gridControlDigital.Name = "gridControlDigital";
            this.gridControlDigital.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEditDigitalActive,
            this.repositoryItemSpinEditDigitalOffset,
            this.repositoryItemComboBoxDigitalDataType,
            this.repositoryItemSpinEditDigSamplePoints,
            this.repositoryItemButtonEditDigName});
            this.gridControlDigital.Size = new System.Drawing.Size(702, 508);
            this.gridControlDigital.TabIndex = 2;
            this.gridControlDigital.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewDigital});
            // 
            // gridViewDigital
            // 
            this.gridViewDigital.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridViewDigital.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gridViewDigital.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.gridViewDigital.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridViewDigital.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.gridViewDigital.Appearance.Row.Options.UseFont = true;
            this.gridViewDigital.Appearance.TopNewRow.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridViewDigital.Appearance.TopNewRow.Options.UseBackColor = true;
            this.gridViewDigital.GridControl = this.gridControlDigital;
            this.gridViewDigital.IndicatorWidth = 32;
            this.gridViewDigital.Name = "gridViewDigital";
            this.gridViewDigital.NewItemRowText = "click here to add a new signal";
            this.gridViewDigital.OptionsBehavior.Editable = false;
            this.gridViewDigital.OptionsBehavior.ReadOnly = true;
            this.gridViewDigital.OptionsCustomization.AllowFilter = false;
            this.gridViewDigital.OptionsCustomization.AllowSort = false;
            this.gridViewDigital.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gridViewDigital.OptionsSelection.MultiSelect = true;
            this.gridViewDigital.OptionsView.ShowGroupPanel = false;
            this.gridViewDigital.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridViewDigital_CustomDrawRowIndicator);
            this.gridViewDigital.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(this.gridViewDigital_RowStyle);
            // 
            // repositoryItemCheckEditDigitalActive
            // 
            this.repositoryItemCheckEditDigitalActive.Name = "repositoryItemCheckEditDigitalActive";
            // 
            // repositoryItemSpinEditDigitalOffset
            // 
            this.repositoryItemSpinEditDigitalOffset.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.repositoryItemSpinEditDigitalOffset.AutoHeight = false;
            this.repositoryItemSpinEditDigitalOffset.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemSpinEditDigitalOffset.MaxValue = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.repositoryItemSpinEditDigitalOffset.MinValue = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.repositoryItemSpinEditDigitalOffset.Name = "repositoryItemSpinEditDigitalOffset";
            // 
            // repositoryItemComboBoxDigitalDataType
            // 
            this.repositoryItemComboBoxDigitalDataType.AutoHeight = false;
            this.repositoryItemComboBoxDigitalDataType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBoxDigitalDataType.Items.AddRange(new object[] {
            "BOOL",
            "BYTE",
            "WORD",
            "DWORD",
            "INT",
            "DINT",
            "REAL"});
            this.repositoryItemComboBoxDigitalDataType.Name = "repositoryItemComboBoxDigitalDataType";
            this.repositoryItemComboBoxDigitalDataType.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // repositoryItemSpinEditDigSamplePoints
            // 
            this.repositoryItemSpinEditDigSamplePoints.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.repositoryItemSpinEditDigSamplePoints.AutoHeight = false;
            this.repositoryItemSpinEditDigSamplePoints.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemSpinEditDigSamplePoints.IsFloatValue = false;
            this.repositoryItemSpinEditDigSamplePoints.Mask.EditMask = "N00";
            this.repositoryItemSpinEditDigSamplePoints.MaxValue = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.repositoryItemSpinEditDigSamplePoints.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.repositoryItemSpinEditDigSamplePoints.Name = "repositoryItemSpinEditDigSamplePoints";
            this.repositoryItemSpinEditDigSamplePoints.NullText = "1";
            // 
            // repositoryItemButtonEditDigName
            // 
            this.repositoryItemButtonEditDigName.AutoHeight = false;
            this.repositoryItemButtonEditDigName.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemButtonEditDigName.Name = "repositoryItemButtonEditDigName";
            // 
            // lblMonitorWindow
            // 
            this.lblMonitorWindow.AutoSize = true;
            this.lblMonitorWindow.Location = new System.Drawing.Point(4, 1);
            this.lblMonitorWindow.Name = "lblMonitorWindow";
            this.lblMonitorWindow.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.lblMonitorWindow.Size = new System.Drawing.Size(108, 20);
            this.lblMonitorWindow.TabIndex = 0;
            this.lblMonitorWindow.Text = "Monitor Window";
            this.lblMonitorWindow.Visible = false;
            // 
            // tabPageDiagnostic
            // 
            this.tabPageDiagnostic.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageDiagnostic.Controls.Add(this.tabControl1);
            this.tabPageDiagnostic.ImageIndex = 2;
            this.tabPageDiagnostic.Location = new System.Drawing.Point(4, 24);
            this.tabPageDiagnostic.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPageDiagnostic.Name = "tabPageDiagnostic";
            this.tabPageDiagnostic.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPageDiagnostic.Size = new System.Drawing.Size(934, 550);
            this.tabPageDiagnostic.TabIndex = 2;
            this.tabPageDiagnostic.Text = "Diagnostic";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.ImageList = this.imageList1;
            this.tabControl1.Location = new System.Drawing.Point(8, 7);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(918, 535);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Controls.Add(this.groupBoxAcquisitionHitRate);
            this.tabPage2.Controls.Add(this.groupBoxService2);
            this.tabPage2.ImageIndex = 0;
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(910, 506);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(302, 913);
            this.panel1.TabIndex = 1;
            // 
            // groupBoxAcquisitionHitRate
            // 
            this.groupBoxAcquisitionHitRate.Controls.Add(this.btnReset);
            this.groupBoxAcquisitionHitRate.Controls.Add(this.progressBarAcquisitionHitRate);
            this.groupBoxAcquisitionHitRate.Controls.Add(this.txtIncrementValue);
            this.groupBoxAcquisitionHitRate.Controls.Add(this.txtMinimumValue);
            this.groupBoxAcquisitionHitRate.Controls.Add(this.txtMaximumValue);
            this.groupBoxAcquisitionHitRate.Controls.Add(this.txtSignalUsed);
            this.groupBoxAcquisitionHitRate.Controls.Add(this.lblAcquisitionHitRateValue);
            this.groupBoxAcquisitionHitRate.Controls.Add(this.lblAcquisitionHitRate);
            this.groupBoxAcquisitionHitRate.Controls.Add(this.lblIncrementValue);
            this.groupBoxAcquisitionHitRate.Controls.Add(this.lblMinimumValue);
            this.groupBoxAcquisitionHitRate.Controls.Add(this.lblMaximumValue);
            this.groupBoxAcquisitionHitRate.Controls.Add(this.lblSignalUsed);
            this.groupBoxAcquisitionHitRate.Location = new System.Drawing.Point(329, 17);
            this.groupBoxAcquisitionHitRate.Name = "groupBoxAcquisitionHitRate";
            this.groupBoxAcquisitionHitRate.Size = new System.Drawing.Size(574, 314);
            this.groupBoxAcquisitionHitRate.TabIndex = 0;
            this.groupBoxAcquisitionHitRate.TabStop = false;
            this.groupBoxAcquisitionHitRate.Text = "Acquisition Hit Rate";
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(394, 253);
            this.btnReset.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(92, 27);
            this.btnReset.TabIndex = 5;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // progressBarAcquisitionHitRate
            // 
            this.progressBarAcquisitionHitRate.Location = new System.Drawing.Point(54, 258);
            this.progressBarAcquisitionHitRate.Name = "progressBarAcquisitionHitRate";
            this.progressBarAcquisitionHitRate.Size = new System.Drawing.Size(272, 15);
            this.progressBarAcquisitionHitRate.TabIndex = 2;
            // 
            // txtIncrementValue
            // 
            this.txtIncrementValue.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.txtIncrementValue.Enabled = false;
            this.txtIncrementValue.Location = new System.Drawing.Point(160, 177);
            this.txtIncrementValue.Name = "txtIncrementValue";
            this.txtIncrementValue.Size = new System.Drawing.Size(100, 24);
            this.txtIncrementValue.TabIndex = 1;
            // 
            // txtMinimumValue
            // 
            this.txtMinimumValue.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.txtMinimumValue.Enabled = false;
            this.txtMinimumValue.Location = new System.Drawing.Point(160, 134);
            this.txtMinimumValue.Name = "txtMinimumValue";
            this.txtMinimumValue.Size = new System.Drawing.Size(100, 24);
            this.txtMinimumValue.TabIndex = 1;
            // 
            // txtMaximumValue
            // 
            this.txtMaximumValue.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.txtMaximumValue.Enabled = false;
            this.txtMaximumValue.Location = new System.Drawing.Point(160, 90);
            this.txtMaximumValue.Name = "txtMaximumValue";
            this.txtMaximumValue.Size = new System.Drawing.Size(100, 24);
            this.txtMaximumValue.TabIndex = 1;
            // 
            // txtSignalUsed
            // 
            this.txtSignalUsed.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.txtSignalUsed.Enabled = false;
            this.txtSignalUsed.Location = new System.Drawing.Point(160, 46);
            this.txtSignalUsed.Name = "txtSignalUsed";
            this.txtSignalUsed.Size = new System.Drawing.Size(326, 24);
            this.txtSignalUsed.TabIndex = 1;
            // 
            // lblAcquisitionHitRateValue
            // 
            this.lblAcquisitionHitRateValue.AutoSize = true;
            this.lblAcquisitionHitRateValue.Location = new System.Drawing.Point(332, 258);
            this.lblAcquisitionHitRateValue.Name = "lblAcquisitionHitRateValue";
            this.lblAcquisitionHitRateValue.Size = new System.Drawing.Size(22, 17);
            this.lblAcquisitionHitRateValue.TabIndex = 0;
            this.lblAcquisitionHitRateValue.Text = "??";
            // 
            // lblAcquisitionHitRate
            // 
            this.lblAcquisitionHitRate.AutoSize = true;
            this.lblAcquisitionHitRate.Location = new System.Drawing.Point(51, 224);
            this.lblAcquisitionHitRate.Name = "lblAcquisitionHitRate";
            this.lblAcquisitionHitRate.Size = new System.Drawing.Size(129, 17);
            this.lblAcquisitionHitRate.TabIndex = 0;
            this.lblAcquisitionHitRate.Text = "Acquisition Hit Rate:";
            // 
            // lblIncrementValue
            // 
            this.lblIncrementValue.AutoSize = true;
            this.lblIncrementValue.Location = new System.Drawing.Point(51, 180);
            this.lblIncrementValue.Name = "lblIncrementValue";
            this.lblIncrementValue.Size = new System.Drawing.Size(112, 17);
            this.lblIncrementValue.TabIndex = 0;
            this.lblIncrementValue.Text = "Increment Value:";
            // 
            // lblMinimumValue
            // 
            this.lblMinimumValue.AutoSize = true;
            this.lblMinimumValue.Location = new System.Drawing.Point(51, 136);
            this.lblMinimumValue.Name = "lblMinimumValue";
            this.lblMinimumValue.Size = new System.Drawing.Size(103, 17);
            this.lblMinimumValue.TabIndex = 0;
            this.lblMinimumValue.Text = "Minimum Value:";
            // 
            // lblMaximumValue
            // 
            this.lblMaximumValue.AutoSize = true;
            this.lblMaximumValue.Location = new System.Drawing.Point(51, 92);
            this.lblMaximumValue.Name = "lblMaximumValue";
            this.lblMaximumValue.Size = new System.Drawing.Size(108, 17);
            this.lblMaximumValue.TabIndex = 0;
            this.lblMaximumValue.Text = "Maximum Value:";
            // 
            // lblSignalUsed
            // 
            this.lblSignalUsed.AutoSize = true;
            this.lblSignalUsed.Location = new System.Drawing.Point(51, 48);
            this.lblSignalUsed.Name = "lblSignalUsed";
            this.lblSignalUsed.Size = new System.Drawing.Size(82, 17);
            this.lblSignalUsed.TabIndex = 0;
            this.lblSignalUsed.Text = "Signal Used:";
            // 
            // groupBoxService2
            // 
            this.groupBoxService2.Controls.Add(this.btnAcquisitionEnvironmentInitialize);
            this.groupBoxService2.Controls.Add(this.btnAcquisitionServiceDiagnosis);
            this.groupBoxService2.Location = new System.Drawing.Point(329, 340);
            this.groupBoxService2.Name = "groupBoxService2";
            this.groupBoxService2.Size = new System.Drawing.Size(574, 160);
            this.groupBoxService2.TabIndex = 0;
            this.groupBoxService2.TabStop = false;
            this.groupBoxService2.Text = "Service";
            // 
            // btnAcquisitionEnvironmentInitialize
            // 
            this.btnAcquisitionEnvironmentInitialize.Location = new System.Drawing.Point(309, 51);
            this.btnAcquisitionEnvironmentInitialize.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAcquisitionEnvironmentInitialize.Name = "btnAcquisitionEnvironmentInitialize";
            this.btnAcquisitionEnvironmentInitialize.Size = new System.Drawing.Size(202, 27);
            this.btnAcquisitionEnvironmentInitialize.TabIndex = 5;
            this.btnAcquisitionEnvironmentInitialize.Text = "Acquisition environment initialize";
            this.btnAcquisitionEnvironmentInitialize.UseVisualStyleBackColor = true;
            this.btnAcquisitionEnvironmentInitialize.Click += new System.EventHandler(this.btnAcquisitionEnvironmentInitialize_Click);
            // 
            // btnAcquisitionServiceDiagnosis
            // 
            this.btnAcquisitionServiceDiagnosis.Location = new System.Drawing.Point(54, 51);
            this.btnAcquisitionServiceDiagnosis.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAcquisitionServiceDiagnosis.Name = "btnAcquisitionServiceDiagnosis";
            this.btnAcquisitionServiceDiagnosis.Size = new System.Drawing.Size(202, 27);
            this.btnAcquisitionServiceDiagnosis.TabIndex = 5;
            this.btnAcquisitionServiceDiagnosis.Text = "Acquisition service diagnosis";
            this.btnAcquisitionServiceDiagnosis.UseVisualStyleBackColor = true;
            this.btnAcquisitionServiceDiagnosis.Click += new System.EventHandler(this.btnAcquisitionServiceDiagnosis_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btnOneClickDiagnostic);
            this.tabPage3.Controls.Add(this.toolStripAdvancedDiagnose);
            this.tabPage3.Controls.Add(this.statusStrip1);
            this.tabPage3.Controls.Add(this.btnExpand);
            this.tabPage3.Controls.Add(this.btnCollapse);
            this.tabPage3.Controls.Add(this.btnExport);
            this.tabPage3.Controls.Add(this.radioButtonDiagnoseConfiguredPackages);
            this.tabPage3.Controls.Add(this.gridControlDiagResults);
            this.tabPage3.Controls.Add(this.radioButtonDiagnoseAllPackages);
            this.tabPage3.Controls.Add(this.advancedFlowLayoutPanel1);
            this.tabPage3.ImageIndex = 3;
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(910, 506);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btnOneClickDiagnostic
            // 
            this.btnOneClickDiagnostic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOneClickDiagnostic.Image = global::PMSServiceStatus.Properties.Resources.icons8_system_report_16;
            this.btnOneClickDiagnostic.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOneClickDiagnostic.Location = new System.Drawing.Point(694, 5);
            this.btnOneClickDiagnostic.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOneClickDiagnostic.Name = "btnOneClickDiagnostic";
            this.btnOneClickDiagnostic.Size = new System.Drawing.Size(210, 27);
            this.btnOneClickDiagnostic.TabIndex = 11;
            this.btnOneClickDiagnostic.Text = "One Click Diagnostic";
            this.btnOneClickDiagnostic.UseVisualStyleBackColor = true;
            this.btnOneClickDiagnostic.Click += new System.EventHandler(this.OneClickDiagnostic_Click);
            // 
            // toolStripAdvancedDiagnose
            // 
            this.toolStripAdvancedDiagnose.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripAdvancedDiagnose.Enabled = false;
            this.toolStripAdvancedDiagnose.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripAdvancedDiagnose.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripExpandAll,
            this.toolStripCollapseAll,
            this.toolStripSeparator3,
            this.toolStripExport,
            this.toolStripSeparator4,
            this.toolStripViewInfo});
            this.toolStripAdvancedDiagnose.Location = new System.Drawing.Point(3, 3);
            this.toolStripAdvancedDiagnose.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.toolStripAdvancedDiagnose.Name = "toolStripAdvancedDiagnose";
            this.toolStripAdvancedDiagnose.Size = new System.Drawing.Size(116, 25);
            this.toolStripAdvancedDiagnose.TabIndex = 19;
            this.toolStripAdvancedDiagnose.Text = "toolStrip1";
            // 
            // toolStripExpandAll
            // 
            this.toolStripExpandAll.AutoSize = false;
            this.toolStripExpandAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripExpandAll.Image = global::PMSServiceStatus.Properties.Resources.split_files_16px;
            this.toolStripExpandAll.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripExpandAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripExpandAll.Name = "toolStripExpandAll";
            this.toolStripExpandAll.Size = new System.Drawing.Size(23, 22);
            this.toolStripExpandAll.Text = "toolStripButton1";
            this.toolStripExpandAll.Click += new System.EventHandler(this.btnExpand_Click);
            // 
            // toolStripCollapseAll
            // 
            this.toolStripCollapseAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripCollapseAll.Image = global::PMSServiceStatus.Properties.Resources.merge_files_16px;
            this.toolStripCollapseAll.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripCollapseAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripCollapseAll.Name = "toolStripCollapseAll";
            this.toolStripCollapseAll.Size = new System.Drawing.Size(23, 22);
            this.toolStripCollapseAll.Text = "toolStripButton2";
            this.toolStripCollapseAll.Click += new System.EventHandler(this.btnCollapse_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripExport
            // 
            this.toolStripExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripExport.Image = global::PMSServiceStatus.Properties.Resources.icons8_export_16;
            this.toolStripExport.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripExport.Name = "toolStripExport";
            this.toolStripExport.Size = new System.Drawing.Size(23, 22);
            this.toolStripExport.Text = "toolStripButton3";
            this.toolStripExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripViewInfo
            // 
            this.toolStripViewInfo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripViewInfo.Image = global::PMSServiceStatus.Properties.Resources.software_16px;
            this.toolStripViewInfo.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripViewInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripViewInfo.Name = "toolStripViewInfo";
            this.toolStripViewInfo.Size = new System.Drawing.Size(23, 22);
            this.toolStripViewInfo.Text = "toolStripButton4";
            this.toolStripViewInfo.Click += new System.EventHandler(this.toolStripViewInfo_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.Font = new System.Drawing.Font("Tahoma", 8F);
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.diagnosticProgressBar,
            this.diagnosticProgressText,
            this.toolStripPassRate});
            this.statusStrip1.Location = new System.Drawing.Point(3, 479);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(904, 24);
            this.statusStrip1.TabIndex = 18;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // diagnosticProgressBar
            // 
            this.diagnosticProgressBar.Margin = new System.Windows.Forms.Padding(1, 5, 1, 3);
            this.diagnosticProgressBar.Name = "diagnosticProgressBar";
            this.diagnosticProgressBar.Size = new System.Drawing.Size(580, 16);
            this.diagnosticProgressBar.Visible = false;
            // 
            // diagnosticProgressText
            // 
            this.diagnosticProgressText.Name = "diagnosticProgressText";
            this.diagnosticProgressText.Size = new System.Drawing.Size(12, 19);
            this.diagnosticProgressText.Text = " ";
            // 
            // toolStripPassRate
            // 
            this.toolStripPassRate.Font = new System.Drawing.Font("Tahoma", 8F);
            this.toolStripPassRate.ForeColor = System.Drawing.SystemColors.ControlText;
            this.toolStripPassRate.Name = "toolStripPassRate";
            this.toolStripPassRate.Size = new System.Drawing.Size(63, 19);
            this.toolStripPassRate.Text = "Pass rate";
            // 
            // btnExpand
            // 
            this.btnExpand.Image = ((System.Drawing.Image)(resources.GetObject("btnExpand.Image")));
            this.btnExpand.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExpand.Location = new System.Drawing.Point(155, 1400);
            this.btnExpand.Name = "btnExpand";
            this.btnExpand.Size = new System.Drawing.Size(140, 23);
            this.btnExpand.TabIndex = 17;
            this.btnExpand.Text = "Expand All";
            this.btnExpand.UseVisualStyleBackColor = true;
            this.btnExpand.Visible = false;
            this.btnExpand.Click += new System.EventHandler(this.btnExpand_Click);
            // 
            // btnCollapse
            // 
            this.btnCollapse.Image = global::PMSServiceStatus.Properties.Resources.CollapseAll;
            this.btnCollapse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCollapse.Location = new System.Drawing.Point(9, 1400);
            this.btnCollapse.Name = "btnCollapse";
            this.btnCollapse.Size = new System.Drawing.Size(140, 23);
            this.btnCollapse.TabIndex = 16;
            this.btnCollapse.Text = "Collapse All";
            this.btnCollapse.UseVisualStyleBackColor = true;
            this.btnCollapse.Visible = false;
            this.btnCollapse.Click += new System.EventHandler(this.btnCollapse_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Enabled = false;
            this.btnExport.Image = global::PMSServiceStatus.Properties.Resources.icons8_export_16;
            this.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExport.Location = new System.Drawing.Point(547, 1400);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(176, 27);
            this.btnExport.TabIndex = 15;
            this.btnExport.Text = "Export Diagnostic Result";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // radioButtonDiagnoseConfiguredPackages
            // 
            this.radioButtonDiagnoseConfiguredPackages.AutoSize = true;
            this.radioButtonDiagnoseConfiguredPackages.Checked = true;
            this.radioButtonDiagnoseConfiguredPackages.Location = new System.Drawing.Point(389, 1400);
            this.radioButtonDiagnoseConfiguredPackages.Name = "radioButtonDiagnoseConfiguredPackages";
            this.radioButtonDiagnoseConfiguredPackages.Size = new System.Drawing.Size(223, 21);
            this.radioButtonDiagnoseConfiguredPackages.TabIndex = 13;
            this.radioButtonDiagnoseConfiguredPackages.TabStop = true;
            this.radioButtonDiagnoseConfiguredPackages.Text = "Diagnose configured packages  ";
            this.radioButtonDiagnoseConfiguredPackages.UseVisualStyleBackColor = true;
            this.radioButtonDiagnoseConfiguredPackages.Visible = false;
            // 
            // gridControlDiagResults
            // 
            this.gridControlDiagResults.Font = new System.Drawing.Font("Tahoma", 7.5F);
            this.gridControlDiagResults.Location = new System.Drawing.Point(6, 1600);
            this.gridControlDiagResults.LookAndFeel.SkinName = "Black";
            this.gridControlDiagResults.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gridControlDiagResults.MainView = this.gridViewDiagResults;
            this.gridControlDiagResults.Name = "gridControlDiagResults";
            this.gridControlDiagResults.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemMemoEdit1});
            this.gridControlDiagResults.Size = new System.Drawing.Size(887, 231);
            this.gridControlDiagResults.TabIndex = 2;
            this.gridControlDiagResults.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewDiagResults});
            this.gridControlDiagResults.Visible = false;
            // 
            // gridViewDiagResults
            // 
            this.gridViewDiagResults.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridViewDiagResults.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gridViewDiagResults.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.gridViewDiagResults.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridViewDiagResults.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.gridViewDiagResults.Appearance.Row.Options.UseFont = true;
            this.gridViewDiagResults.Appearance.TopNewRow.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridViewDiagResults.Appearance.TopNewRow.Options.UseBackColor = true;
            this.gridViewDiagResults.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.Description,
            this.Suggestion,
            this.Package,
            this.Id});
            this.gridViewDiagResults.GridControl = this.gridControlDiagResults;
            this.gridViewDiagResults.IndicatorWidth = 32;
            this.gridViewDiagResults.Name = "gridViewDiagResults";
            this.gridViewDiagResults.NewItemRowText = "click here to add a new signal";
            this.gridViewDiagResults.OptionsBehavior.AutoSelectAllInEditor = false;
            this.gridViewDiagResults.OptionsBehavior.Editable = false;
            this.gridViewDiagResults.OptionsBehavior.ReadOnly = true;
            this.gridViewDiagResults.OptionsCustomization.AllowFilter = false;
            this.gridViewDiagResults.OptionsCustomization.AllowSort = false;
            this.gridViewDiagResults.OptionsMenu.EnableColumnMenu = false;
            this.gridViewDiagResults.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gridViewDiagResults.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.gridViewDiagResults.OptionsView.RowAutoHeight = true;
            this.gridViewDiagResults.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridViewDiagResults.OptionsView.ShowGroupPanel = false;
            this.gridViewDiagResults.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.Id, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // Description
            // 
            this.Description.Caption = "Description";
            this.Description.FieldName = "Description";
            this.Description.Name = "Description";
            this.Description.Visible = true;
            this.Description.VisibleIndex = 0;
            // 
            // Suggestion
            // 
            this.Suggestion.Caption = "Suggestion";
            this.Suggestion.FieldName = "Suggestion";
            this.Suggestion.Name = "Suggestion";
            this.Suggestion.Visible = true;
            this.Suggestion.VisibleIndex = 1;
            // 
            // Package
            // 
            this.Package.Caption = "Package";
            this.Package.FieldName = "Package";
            this.Package.Name = "Package";
            this.Package.Visible = true;
            this.Package.VisibleIndex = 2;
            // 
            // Id
            // 
            this.Id.Caption = "Id";
            this.Id.FieldName = "Id";
            this.Id.Name = "Id";
            // 
            // repositoryItemMemoEdit1
            // 
            this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
            // 
            // radioButtonDiagnoseAllPackages
            // 
            this.radioButtonDiagnoseAllPackages.AutoSize = true;
            this.radioButtonDiagnoseAllPackages.Location = new System.Drawing.Point(301, 1400);
            this.radioButtonDiagnoseAllPackages.Name = "radioButtonDiagnoseAllPackages";
            this.radioButtonDiagnoseAllPackages.Size = new System.Drawing.Size(169, 21);
            this.radioButtonDiagnoseAllPackages.TabIndex = 12;
            this.radioButtonDiagnoseAllPackages.Text = "Diagnose all packages  ";
            this.radioButtonDiagnoseAllPackages.UseVisualStyleBackColor = true;
            this.radioButtonDiagnoseAllPackages.Visible = false;
            // 
            // advancedFlowLayoutPanel1
            // 
            this.advancedFlowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.advancedFlowLayoutPanel1.AutoScroll = true;
            this.advancedFlowLayoutPanel1.Controls.Add(this.button1);
            this.advancedFlowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.advancedFlowLayoutPanel1.Location = new System.Drawing.Point(6, 35);
            this.advancedFlowLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.advancedFlowLayoutPanel1.Name = "advancedFlowLayoutPanel1";
            this.advancedFlowLayoutPanel1.Size = new System.Drawing.Size(898, 434);
            this.advancedFlowLayoutPanel1.TabIndex = 9;
            this.advancedFlowLayoutPanel1.WrapContents = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 2);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(892, 1);
            this.button1.TabIndex = 0;
            this.button1.TabStop = false;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "general.ico");
            this.imageList1.Images.SetKeyName(1, "icons8_opera_glasses_32.png");
            this.imageList1.Images.SetKeyName(2, "icons8_system_report.ico");
            this.imageList1.Images.SetKeyName(3, "icons8_mental_health.ico");
            // 
            // UpdateDataTimer
            // 
            this.UpdateDataTimer.Interval = 200;
            this.UpdateDataTimer.Tick += new System.EventHandler(this.UpdateDataTimer_Tick);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStripNotifyIcon;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "test";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStripNotifyIcon
            // 
            this.contextMenuStripNotifyIcon.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.5F);
            this.contextMenuStripNotifyIcon.ImageScalingSize = new System.Drawing.Size(5, 10);
            this.contextMenuStripNotifyIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemStatus,
            this.toolStripSeparator1,
            this.toolStripMenuItemStartservice,
            this.toolStripMenuItemStopservice,
            this.toolStripSeparator2,
            this.toolStripMenuItemAbout,
            this.toolStripMenuItemExit});
            this.contextMenuStripNotifyIcon.Name = "contextMenuStripNotifyIcon";
            this.contextMenuStripNotifyIcon.Size = new System.Drawing.Size(189, 126);
            this.contextMenuStripNotifyIcon.Text = "Status(&s)";
            // 
            // toolStripMenuItemStatus
            // 
            this.toolStripMenuItemStatus.AutoSize = false;
            this.toolStripMenuItemStatus.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.toolStripMenuItemStatus.Name = "toolStripMenuItemStatus";
            this.toolStripMenuItemStatus.Size = new System.Drawing.Size(188, 22);
            this.toolStripMenuItemStatus.Text = "Status(&s)";
            this.toolStripMenuItemStatus.Click += new System.EventHandler(this.toolStripMenuItemStatus_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(185, 6);
            // 
            // toolStripMenuItemStartservice
            // 
            this.toolStripMenuItemStartservice.AutoSize = false;
            this.toolStripMenuItemStartservice.Name = "toolStripMenuItemStartservice";
            this.toolStripMenuItemStartservice.Size = new System.Drawing.Size(188, 22);
            this.toolStripMenuItemStartservice.Text = "Start service(&t)";
            this.toolStripMenuItemStartservice.Click += new System.EventHandler(this.toolStripMenuItemStartservice_Click);
            // 
            // toolStripMenuItemStopservice
            // 
            this.toolStripMenuItemStopservice.AutoSize = false;
            this.toolStripMenuItemStopservice.Name = "toolStripMenuItemStopservice";
            this.toolStripMenuItemStopservice.Size = new System.Drawing.Size(188, 22);
            this.toolStripMenuItemStopservice.Text = "Stop service(&p)";
            this.toolStripMenuItemStopservice.Click += new System.EventHandler(this.toolStripMenuItemStopservice_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(185, 6);
            // 
            // toolStripMenuItemAbout
            // 
            this.toolStripMenuItemAbout.AutoSize = false;
            this.toolStripMenuItemAbout.Name = "toolStripMenuItemAbout";
            this.toolStripMenuItemAbout.Size = new System.Drawing.Size(188, 22);
            this.toolStripMenuItemAbout.Text = "About...(&A)";
            this.toolStripMenuItemAbout.Click += new System.EventHandler(this.toolStripMenuItemAbout_Click);
            // 
            // toolStripMenuItemExit
            // 
            this.toolStripMenuItemExit.AutoSize = false;
            this.toolStripMenuItemExit.Name = "toolStripMenuItemExit";
            this.toolStripMenuItemExit.Size = new System.Drawing.Size(188, 22);
            this.toolStripMenuItemExit.Text = "Exit(&x)";
            this.toolStripMenuItemExit.Click += new System.EventHandler(this.toolStripMenuItemExit_Click);
            // 
            // MainFrm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(942, 578);
            this.Controls.Add(this.tabControlMain);
            this.Font = new System.Drawing.Font("Tahoma", 8F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "MainFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFrm_FormClosing);
            this.Resize += new System.EventHandler(this.MainFrm_Resize);
            this.tabControlMain.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.groupBoxOnlineClient.ResumeLayout(false);
            this.groupBoxOnlineClient.PerformLayout();
            this.groupBoxAcquisition.ResumeLayout(false);
            this.groupBoxAcquisition.PerformLayout();
            this.groupBoxService.ResumeLayout(false);
            this.groupBoxService.PerformLayout();
            this.tabPageIOStatus.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControlStringValues.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlTecnostring)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTecnostring)).EndInit();
            this.tabControlSignalValues.ResumeLayout(false);
            this.tabPageAnalog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlAnalog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewAnalog)).EndInit();
            this.tabPageDigital.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDigital)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDigital)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEditDigitalActive)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEditDigitalOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxDigitalDataType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEditDigSamplePoints)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEditDigName)).EndInit();
            this.tabPageDiagnostic.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBoxAcquisitionHitRate.ResumeLayout(false);
            this.groupBoxAcquisitionHitRate.PerformLayout();
            this.groupBoxService2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.toolStripAdvancedDiagnose.ResumeLayout(false);
            this.toolStripAdvancedDiagnose.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDiagResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDiagResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).EndInit();
            this.advancedFlowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.contextMenuStripNotifyIcon.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.TabPage tabPageIOStatus;
        private System.Windows.Forms.TabPage tabPageDiagnostic;
        private System.Windows.Forms.GroupBox groupBoxOnlineClient;
        private System.Windows.Forms.GroupBox groupBoxAcquisition;
        private System.Windows.Forms.GroupBox groupBoxService;
        private System.Windows.Forms.CheckBox chkBoxAutostartService;
        private System.Windows.Forms.Label lblLogLevel;
        private System.Windows.Forms.Label lblLogPath;
        private System.Windows.Forms.Label lblListeningPortNo;
        private System.Windows.Forms.Label lblServiceStatus;
        private System.Windows.Forms.Label lblExecutableFilePath;
        private System.Windows.Forms.Label lblServiceName;
        private System.Windows.Forms.Button btnStopService;
        private System.Windows.Forms.Button btnStartService;
        private System.Windows.Forms.TextBox txtListeningPortNo;
        private System.Windows.Forms.Button btnServiceStatus;
        private System.Windows.Forms.Label lblAcquisitionStatus;
        private System.Windows.Forms.ComboBox comboBoxLogLevel;
        private System.Windows.Forms.ListView listViewDriverInfo;
        private System.Windows.Forms.ComboBox comboBoxInternalCyclicInterrupts;
        private System.Windows.Forms.Button btnAcquisitionStatus;
        private System.Windows.Forms.Button btnStopAcquisition;
        private System.Windows.Forms.Button btnStartAcquisition;
        private System.Windows.Forms.CheckBox chkAutoStartAcquisition;
        private System.Windows.Forms.Label lblInternalCyclicInterrupts;
        private System.Windows.Forms.Label lblSoftTimebaseUnit;
        private System.Windows.Forms.Label lblSoftTimebase;
        private System.Windows.Forms.Label lblDriverInfo;
        private System.Windows.Forms.ListView listViewOnlineClient;
        private System.Windows.Forms.Button btnDisconnectAllClients;
        private System.Windows.Forms.Label lblClientCountNumber;
        private System.Windows.Forms.Label lblClientCount;
        private System.Windows.Forms.Timer UpdateDataTimer;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.TextBox txtServiceName;
        private ReadOnlyTextBox txtLogPath;
        private ReadOnlyTextBox txtExecutableFilePath;
        private ReadOnlyTextBox txtInternalCyclicInterrupts;
        private ReadOnlyTextBox txtSoftTimebase;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Label lblSignalTree;
        private System.Windows.Forms.Button btnLaunchConfigurationTool;
        private System.Windows.Forms.TabControl tabControlSignalValues;
        private System.Windows.Forms.TabPage tabPageAnalog;
        private System.Windows.Forms.TabPage tabPageDigital;
        private System.Windows.Forms.Label lblMonitorWindow;
        private System.Windows.Forms.Label lblTotalSignals;
        private System.Windows.Forms.ImageList imageList;
        private DevExpress.XtraGrid.GridControl gridControlAnalog;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewAnalog;
        private DevExpress.XtraGrid.GridControl gridControlDigital;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewDigital;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEditDigName;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit repositoryItemSpinEditDigitalOffset;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxDigitalDataType;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit repositoryItemSpinEditDigSamplePoints;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEditDigitalActive;
        private System.Windows.Forms.TabControl tabControlStringValues;
        private System.Windows.Forms.TabPage tabPage1;
        private DevExpress.XtraGrid.GridControl gridControlTecnostring;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewTecnostring;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.GroupBox groupBoxService2;
        private System.Windows.Forms.Button btnAcquisitionEnvironmentInitialize;
        private System.Windows.Forms.Button btnAcquisitionServiceDiagnosis;
        private System.Windows.Forms.GroupBox groupBoxAcquisitionHitRate;
        private System.Windows.Forms.Button btnReset;
        private MyProgressBar progressBarAcquisitionHitRate;
        private ReadOnlyTextBox txtIncrementValue;
        private ReadOnlyTextBox txtMinimumValue;
        private ReadOnlyTextBox txtMaximumValue;
        private ReadOnlyTextBox txtSignalUsed;
        private System.Windows.Forms.Label lblAcquisitionHitRateValue;
        private System.Windows.Forms.Label lblAcquisitionHitRate;
        private System.Windows.Forms.Label lblIncrementValue;
        private System.Windows.Forms.Label lblMinimumValue;
        private System.Windows.Forms.Label lblMaximumValue;
        private System.Windows.Forms.Label lblSignalUsed;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripNotifyIcon;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemStatus;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemStartservice;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemStopservice;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAbout;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnOneClickDiagnostic;
        private MakarovDev.ExpandCollapsePanel.AdvancedFlowLayoutPanel advancedFlowLayoutPanel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton radioButtonDiagnoseConfiguredPackages;
        private System.Windows.Forms.RadioButton radioButtonDiagnoseAllPackages;
        private DevExpress.XtraGrid.GridControl gridControlDiagResults;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewDiagResults;
        private DevExpress.XtraGrid.Columns.GridColumn Description;
        private DevExpress.XtraGrid.Columns.GridColumn Suggestion;
        private DevExpress.XtraGrid.Columns.GridColumn Package;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn Id;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnExpand;
        private System.Windows.Forms.Button btnCollapse;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar diagnosticProgressBar;
        private System.Windows.Forms.ToolStripStatusLabel diagnosticProgressText;
        private System.Windows.Forms.ToolStrip toolStripAdvancedDiagnose;
        private System.Windows.Forms.ToolStripButton toolStripExpandAll;
        private System.Windows.Forms.ToolStripButton toolStripCollapseAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripExport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolStripViewInfo;
        private System.Windows.Forms.ToolStripStatusLabel toolStripPassRate;
    }
}

