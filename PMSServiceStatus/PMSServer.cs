using pmsServiceLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PMSServiceStatus
{
    class PMSServer
    {
        static PMSServer instance = null;
        public static PMSServer Instance
        {
            get
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = new PMSServer();
                    }
                }
                return instance;
            }
        }
        #region 属性

        public enum FDAA_SERVICE_STATUS
        {
            STOPPED = 0,		// 服务处于启动状态
            RUNNING,			// 服务处于停止状态
            UNKNOWN		        // 服务状态未知，画面命令按钮不可用
        };
        public enum FDAA_ACQUISITION_STATUS
        {
            STOPPED = 0,        // 数据采集处于停止状态
            STARTED,	        // 数据采集处于启动状态
            UNAVAILABLE	        // 数据采集状态未知，画面命令按钮不可用
        };

        public static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private pmsServiceLib.PMSSupervisor pmsService = null;
        public pmsServiceLib.PMSSupervisor PMSService
        {
            get { return pmsService; }
            set { pmsService = value; }
        }

        private FDAA_SERVICE_STATUS serverStatus;
        public FDAA_SERVICE_STATUS ServerStatus
        {
            get { return serverStatus; }
            set 
            { 
                serverStatus = value; 
            }
        }

        // 用于判断服务状态是否发生了变化
        FDAA_SERVICE_STATUS serverOldStatus;

        private string serviceName;
        public string ServiceName
        {
            get { return serviceName; }
            set { serviceName = value; }
        }

        private string servicePath;
        public string ServicePath
        {
            get { return servicePath; }
            set { servicePath = value; }
        }

        private FDAA_ACQUISITION_STATUS acqStatus;
        public FDAA_ACQUISITION_STATUS AcqStatus
        {
            get { return acqStatus; }
            set { acqStatus = value; }
        }

        // Base sampling rate
        private string baseSamplingRate;
        public string BaseSamplingRate
        {
            get { return baseSamplingRate; }
            set { baseSamplingRate = value; }
        }

        // Machine Code
        private string machineCode;
        public string MachineCode
        {
            get { return machineCode; }
            set { machineCode = value; }
        }

        // 软时钟（用于计算丢帧百分比）
        private long inturrpts;
        public long Inturrpts
        {
            get { return inturrpts; }
            set { inturrpts = value; }
        }

        private Dictionary<string, double> signalValues = new Dictionary<string,double>();
        public Dictionary<string, double> SignalValues
        {
            get { return signalValues; }
            set { signalValues = value; }
        }

        public class TECHNOSTRING
	    {
		    public double _dTime;
            public string _strValue;
	    };

        private Dictionary<string, TECHNOSTRING>stringValues = new Dictionary<string,TECHNOSTRING>();
        public Dictionary<string, TECHNOSTRING> StringValues
        {
            get { return stringValues; }
            set { stringValues = value; }
        }

        public class ClientItem
        {
            public uint _id;
            public string _logintime;
            public string _name;
            public ushort _signalcount;
        }

        private List<ClientItem> clinetInfos = new List<ClientItem>();
        public List<ClientItem> ClinetInfos
        {
            get { return clinetInfos; }
            set { clinetInfos = value; }
        }

        //public Array ClinetInfos;
        public Array DriverInfos;

        private static readonly object locker = new object();

        System.Threading.Timer queryServiceStatusTimer; 
        #endregion
        
        #region 事件
        public delegate void PMSServiceStatusChangedHandler(FDAA_SERVICE_STATUS serviceStatus);
        public delegate void PMSAcquisitionStatusChangedHandler(FDAA_ACQUISITION_STATUS acquisitionStatus);
        public event PMSServiceStatusChangedHandler PMSServiceChangedEventHandler;
        public event PMSAcquisitionStatusChangedHandler PMSAcquisitionChangedEventHandler;
        public Action ClientStatusChangedEventHandler;
        #endregion

        private PMSServer()
        {
            InitQueryTimer();
            SetServiceStatus(FDAA_SERVICE_STATUS.UNKNOWN);
        }

        private void InitUnknownStatus()
        {
            serverStatus = FDAA_SERVICE_STATUS.UNKNOWN;
            serverOldStatus = FDAA_SERVICE_STATUS.UNKNOWN;
            acqStatus = FDAA_ACQUISITION_STATUS.UNAVAILABLE;

            serviceName = "PMSService";
            servicePath = "??";

            baseSamplingRate = "??";
            machineCode = "???";

            inturrpts = 1;
        }

        public void StartService(bool bIsFromUI)
        {
            try
            {
                // 如果服务已经处于运行状态，无需再运行
                if (serverStatus == FDAA_SERVICE_STATUS.RUNNING) return;

                if (bIsFromUI)
                {
                    ProgressFrm progressFrm = new ProgressFrm(true);
                    progressFrm.ShowDialog();
                }
                else
                {
                    // 通过创建COM对象启动服务,如果成功，说明Service可用
                    // 如果PMSService未装载至SCM或被禁用,则创建COM对象会失败
                    // 此处添加一个延迟函数，解决新版本在PMSServer程序start服务不启动情况下，通过IOConfig工具直接apply导致PMSServer程序启动报错的问题
                    Thread.Sleep(3000);
                    InitServer();
                    GetServerInfo();
                }
            }
            catch (Exception ex)
            {
                log.Fatal(ex);
            }
        }

        public void StopService(bool bIsFromUI)
        {
            try
            {
                // 如果服务已经处于停止状态，无需再停止
                if (serverStatus == FDAA_SERVICE_STATUS.STOPPED) return;

                if (bIsFromUI)
                {
                    ProgressFrm progressFrm = new ProgressFrm(false);
                    progressFrm.ShowDialog();

                }
                else
                    PMSServer.Instance.PMSService.stopService();
            }
            catch (Exception ex)
            {
                log.Fatal(ex);
            }
        }

        static bool flag = true;
        public void InitServer()
        {
            try
            {
                if(flag)
                {
                    flag = false;
                    if (PMSServer.Instance.PMSService == null)
                    {
                        PMSServer.Instance.PMSService = new pmsServiceLib.PMSSupervisor();

                        StopQueryServiceStatusTimer();
                        // 获取Service相关属性数据（初始化）
                        //PMSServer.Instance.PMSService.ServiceStatusNotify += PMSService_ServiceStatusNotify;
                        PMSServer.Instance.PMSService.ServiceStatusNotify += new _IPMSSupervisorEvents_ServiceStatusNotifyEventHandler(PMSService_ServiceStatusNotify);
                        PMSServer.Instance.PMSService.AcquisitionStatusNotify += new _IPMSSupervisorEvents_AcquisitionStatusNotifyEventHandler(PMSService_AcquisitionStatusNotify);
                        PMSServer.Instance.PMSService.updateRequestSignalCount += PMSService_updateRequestSignalCount;
                        PMSServer.Instance.PMSService.aClientLogin += PMSService_aClientLogin;
                        PMSServer.Instance.PMSService.aClientExit += PMSService_aClientExit;
                    }
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                log.Fatal("InitServer()" + ex.Message);
                PMSServer.Instance.PMSService = null;
                SetServiceStatus(FDAA_SERVICE_STATUS.UNKNOWN);
                return;
            }
            
        }

        void PMSService_aClientExit(uint clientid)
        {
            #region ARRAY版本 不用了
            ////  因为ARRAY是定长，所以不能动态删除
            //Type t = ClinetInfos.GetType().GetElementType();
            ////  先创建一个新的数组，数组长度-1
            //Array newArray = Array.CreateInstance(t, ClinetInfos.Length - 1);
            ////  把旧数组赋值到新数组上。
            ////Array.Copy(ClinetInfos, 0, newArray, 0, newArray.Length);
            //int index = 0;
            //for (int i = 0; i < ClinetInfos.Length; i++)
            //{
            //    if (((_clientItem)(PMSServer.Instance.ClinetInfos.GetValue(i)))._id == clientid) continue;

            //    _clientItem clientItem = new _clientItem();
            //    clientItem = (_clientItem)(PMSServer.Instance.ClinetInfos.GetValue(i));
            //    newArray.SetValue(clientItem, index);
            //    index++;
            //}
            ////  更新到实体
            //ClinetInfos = newArray;
            //ClientStatusChangedEventHandler();
            #endregion
            for (int i = ClinetInfos.Count - 1; i >= 0; i--)
            {
                if (ClinetInfos[i]._id == clientid)
                    ClinetInfos.Remove(ClinetInfos[i]);
            }
            ClientStatusChangedEventHandler();
        }

        void PMSService_aClientLogin(uint clientid, string name, string logintime, ushort count)
        {
            #region ARRAY版本 不用了
            ////  因为ARRAY是定长，所以不能动态添加
            //Type t = ClinetInfos.GetType().GetElementType();
            ////  先创建一个新的数组，数组长度+1
            //Array newArray = Array.CreateInstance(t,ClinetInfos.Length + 1);
            ////  把旧数组赋值到新数组上。
            //Array.Copy(ClinetInfos, 0, newArray, 0, ClinetInfos.Length);
            ////  把新的ClinetItem设置到最后
            //_clientItem clientItem = new _clientItem();
            //clientItem._id = clientid;
            //clientItem._name = name;
            //clientItem._logintime = logintime;
            //clientItem._signalcount = count;
            //newArray.SetValue(clientItem,newArray.Length -1); 
            ////  更新到实体
            //ClinetInfos = newArray;
            ////  通知表现层
            //ClientStatusChangedEventHandler();
            #endregion
            ClientItem item = new ClientItem();
            item._id = clientid;
            item._logintime = logintime;
            item._name = name;
            item._signalcount = count;
            ClinetInfos.Add(item);
            //  通知表现层
            ClientStatusChangedEventHandler();
        }

        void PMSService_updateRequestSignalCount(uint clientid, ushort count)
        {
            #region ARRAY版本 不用了
		    //for (int i = 0; i < ClinetInfos.Length; i++)
            //{
            //    if (((_clientItem)(PMSServer.Instance.ClinetInfos.GetValue(i)))._id != clientid) continue;

            //    _clientItem clientItem = (_clientItem)ClinetInfos.GetValue(i);
            //    clientItem._signalcount = count;
            //    ClinetInfos.SetValue(clientItem, i);
            //}
	        #endregion
            
            foreach (var item in ClinetInfos)
            {
                if (item._id == clientid)
                {
                    item._signalcount = count;
                }
            }
            //  通知表现层
            ClientStatusChangedEventHandler();
        }

        public void GetServerInfo()
        {
            if (PMSServer.Instance.PMSService == null)
            {
                SetServiceStatus(FDAA_SERVICE_STATUS.STOPPED);
                return;
            }

            while (true)
            {
                // 服务名称
                string serviceName;
                PMSServer.Instance.PMSService.getServiceName(out serviceName);
                PMSServer.Instance.ServiceName = serviceName;

                // 服务EXE路径
                string servicePath;
                PMSServer.Instance.PMSService.getServicePath(out servicePath);
                PMSServer.Instance.ServicePath = servicePath;

                ServerConfig.getInstance().LogPath = System.IO.Path.GetDirectoryName(servicePath) + @"\log";

                //服务有可能还没初始化成功，能取到值证明初始化成功
                //必须取到值之后才继续
                try
                {
                    long lInterrupts;
                    PMSServer.Instance.PMSService.getSoftInterrupts(out lInterrupts);
                }
                catch (Exception)
                {
                    Thread.Sleep(100);
                    continue;
                }

                //  采集状态
                bool bIsStarted = false;
                PMSServer.Instance.PMSService.getAcquisitionStatus(out bIsStarted);
                if (bIsStarted)
                    PMSServer.Instance.AcqStatus = FDAA_ACQUISITION_STATUS.STARTED;
                else
                    PMSServer.Instance.AcqStatus = FDAA_ACQUISITION_STATUS.STOPPED;

                //  机器码
                string machineCode;
                PMSServer.Instance.PMSService.getMachineCode(out machineCode);
                PMSServer.Instance.MachineCode = machineCode;

                //  连接客户端信息
                List<_clientItem> lstClinetInfos = new List<_clientItem>();
                Array arrayClinetInfos = lstClinetInfos.ToArray();
                PMSServer.Instance.PMSService.getAllClientInfo(out arrayClinetInfos);

                PMSServer.Instance.ClinetInfos.Clear();
                for (int i = 0; i < arrayClinetInfos.Length; i++)
                {
                    ClientItem item = new ClientItem();
                    item._id = ((_clientItem)(arrayClinetInfos.GetValue(i)))._id;
                    item._logintime = ((_clientItem)(arrayClinetInfos.GetValue(i)))._logintime;
                    item._name = ((_clientItem)(arrayClinetInfos.GetValue(i)))._name;
                    item._signalcount = ((_clientItem)(arrayClinetInfos.GetValue(i)))._signalcount;
                    ClinetInfos.Add(item);
                }

                //  驱动信息_arrayItem
                List<_arrayItem> lstDriverInfos = new List<_arrayItem>();
                DriverInfos = lstDriverInfos.ToArray();
                PMSServer.Instance.PMSService.getDriversInfo(out DriverInfos);


                log.Fatal(((_arrayItem)(PMSServer.Instance.DriverInfos.GetValue(0))).param2.ToString());

                // 如果COM初始化成功，则说明服务正常启动，将其状态设置为Running
                SetServiceStatus(FDAA_SERVICE_STATUS.RUNNING);
                break;
            }
        }

        void PMSService_AcquisitionStatusNotify(int bRunningNow)
        {
            // 更新PMServer数据采集状态标志
	        if (Convert.ToBoolean(bRunningNow))
		        setAcquisitionStatus(FDAA_ACQUISITION_STATUS.STARTED);
	        else
                setAcquisitionStatus(FDAA_ACQUISITION_STATUS.STOPPED);
        }

        private void setAcquisitionStatus(FDAA_ACQUISITION_STATUS fDAA_ACQUISITION_STATUS)
        {
            // 更新数据采集状态标志为开始
            this.acqStatus = fDAA_ACQUISITION_STATUS;

            PMSAcquisitionChangedEventHandler(fDAA_ACQUISITION_STATUS);
        }

        public bool isAlreadyLoadServerInfo = false;

        private void PMSService_ServiceStatusNotify(int bRunningNow)
        {
            if (Convert.ToBoolean(bRunningNow))
            {
                isAlreadyLoadServerInfo = false;
                GetServerInfo();
                isAlreadyLoadServerInfo = true;
                SetServiceStatus(FDAA_SERVICE_STATUS.RUNNING);
            }
            else
            {
                isAlreadyLoadServerInfo = false;
                SetServiceStatus(FDAA_SERVICE_STATUS.STOPPED);
            }
        }

        static int SetServiceStatusTime = 0;

        private void SetServiceStatus(FDAA_SERVICE_STATUS serverStatus)
        {
            log.Debug("Status:" + serverStatus);

            this.serverStatus = serverStatus;
            // 如果将服务设置为启动状态
            if (serverStatus == FDAA_SERVICE_STATUS.RUNNING)
            {
                // KillTimer，停止查询SCM列表。此时服务状态信息通过连接点发送
                StopQueryServiceStatusTimer();
            }
            else if (serverStatus == FDAA_SERVICE_STATUS.STOPPED)
            {
                // 更新服务状态和数据采集状态标志为停止
                acqStatus = FDAA_ACQUISITION_STATUS.STOPPED;
                PMSServer.Instance.PMSService = null;
                flag = true;
                StartQueryServiceStatusTimer();

                // 清空Client和Driver列表
                PMSServer.Instance.ClinetInfos.Clear();
                if (DriverInfos != null)
                    Array.Clear(DriverInfos, 0, DriverInfos.Length);
            }
            else if (serverStatus == FDAA_SERVICE_STATUS.UNKNOWN)
            {
                // 更新PMSServer服务状态为未知
                InitUnknownStatus();
                // 启动定时器，查询SCM列表，是否用户在SCM中启动了服务。
                // 此时COM对象不存在，无法通过连接点获知服务状态
                StartQueryServiceStatusTimer();
            }

            // 初始状态为Unknown时，或当状态改变时，通知
            if ((serverStatus != serverOldStatus) || serverStatus == FDAA_SERVICE_STATUS.UNKNOWN)
            {
                // 将消息发往主窗口
                if (PMSServiceChangedEventHandler != null)
                {
                    PMSServiceChangedEventHandler(serverStatus);
                    Thread.Sleep(1000);
                    TrayArea.RefreshAsync();
                }
                serverOldStatus = serverStatus;
            }
            else if (serverStatus == serverOldStatus)
            {
                // 将消息发往主窗口
                if (PMSServiceChangedEventHandler != null)
                    PMSServiceChangedEventHandler(serverStatus);
            }

            SetServiceStatusTime++;

            log.Debug("SetServiceStatusTime:" + SetServiceStatusTime);
        }

        private void InitQueryTimer()
        {
            queryServiceStatusTimer = new Timer(QueryServiceStatusTimer_tick, null, 0, 1000);
        }

        //  连接资源是否被占用
        private static int usingResource = 0;

        private void QueryServiceStatusTimer_tick(object state)
        {
            var serviceControllers = ServiceController.GetServices();
            //  从Windows服务列表中获取pmsService服务
            var pmsService = serviceControllers.FirstOrDefault(service => service.ServiceName == "PMSService");
            if (pmsService == null) return;

            if (Interlocked.Exchange(ref usingResource, 1) == 0)
            {
                if (pmsService.Status == ServiceControllerStatus.Running)
                {
                    PMSServer.Instance.StartService(false);
                }
                else if (pmsService.Status == ServiceControllerStatus.Stopped)
                {
                    PMSServer.Instance.SetServiceStatus(FDAA_SERVICE_STATUS.STOPPED);
                }
            }
            Interlocked.Exchange(ref usingResource, 0);
        }

        static bool isQueryServiceStatusTimerStarted = false;

        private void StartQueryServiceStatusTimer()
        {
            if (!isQueryServiceStatusTimerStarted)
            {
                isQueryServiceStatusTimerStarted = true;
                queryServiceStatusTimer.Change(0, 1000);
            }
                
        }

        private void StopQueryServiceStatusTimer()
        {
            if (isQueryServiceStatusTimerStarted)
            {
                isQueryServiceStatusTimerStarted = false;
                queryServiceStatusTimer.Change(-1, 0);
            }
        }

        public void StartAcquisition()
        {
            try
            {
                if (PMSServer.Instance.PMSService != null)
                {
                    PMSServer.Instance.PMSService.startAcquisition();
                }
            }
            catch (Exception ex)
            {
                log.Fatal(ex);
            }
        }

        public void StopAcquisition()
        {
            try
            {
                if (PMSServer.Instance.PMSService != null)
                {
                    PMSServer.Instance.PMSService.stopAcquisition();
                }
            }
            catch (Exception ex)
            {
                log.Fatal(ex);
            }
        }

        internal void DisconnectAllClients()
        {
            if (PMSServer.Instance.PMSService != null)
            {
                PMSServer.Instance.PMSService.disconnectAllClients(); 
            }
        }

        /// <summary>
        /// 获取模拟量或开关量值
        /// </summary>
        /// <param name="ModuelNo"></param>
        public void GetSignalValues(ushort ModuelNo)
        {
            try
            {
                if (PMSServer.Instance.PMSService == null)
                {
                    log.Fatal("PMSServer.Instance.PMSService = null then return");
                    return;
                }

                // 清空保存Signal值的map
                SignalValues.Clear();

                List<_signalValueItem> lstSignalValueItems = new List<_signalValueItem>();
                Array SignalValueItems = lstSignalValueItems.ToArray();
                PMSServer.Instance.PMSService.getSignalValues(ModuelNo, out SignalValueItems);
                for (int i = 0; i < SignalValueItems.Length; i++)
                {
                    signalValues.Add(((_signalValueItem)(SignalValueItems.GetValue(i)))._no, ((_signalValueItem)(SignalValueItems.GetValue(i)))._value);
                    if (ModuelNo == 0)
                    {
                        log.Debug("从后台获取0号模块值为");
                        log.Debug(((_signalValueItem)(SignalValueItems.GetValue(i)))._value);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Fatal(ex);
            }
        }

        /// <summary>
        /// 获取Tecnostring值
        /// </summary>
        /// <param name="ModuelNo"></param>
        public void GetStringValues(ushort ModuelNo)
        {
            if (PMSServer.Instance.PMSService == null) return;

            // 清空保存Signal值的map
            StringValues.Clear();

            List<_stringValueItem> lstStringValueItemValueItems = new List<_stringValueItem>();
            Array StringValueItems = lstStringValueItemValueItems.ToArray();
            PMSServer.Instance.PMSService.getStringValues(ModuelNo, out StringValueItems);
            for (int i = 0; i < StringValueItems.Length; i++)
            {
                TECHNOSTRING temp = new TECHNOSTRING();
                temp._dTime = ((_stringValueItem)(StringValueItems.GetValue(i)))._time;
                temp._strValue = ((_stringValueItem)(StringValueItems.GetValue(i)))._value;
                stringValues.Add(((_stringValueItem)(StringValueItems.GetValue(i)))._no, temp);
            }
        }
    }
}
