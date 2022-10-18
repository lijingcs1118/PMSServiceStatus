using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Baosight.FDAA.PackageDiagnosis.Model.Enums;
using Baosight.FDAA.PackageDiagnosis.Model.Modules;
using Baosight.FDAA.Utility;

namespace Baosight.FDAA.PackageDiagnosis.DAL.Configs
{
    public class IOConfig
    {
        // Module type definition

        private FAUDeviceManager _fauDeviceManager = new FAUDeviceManager();

        //  连接资源是否被占用
        //public static int usingResource = 0;
        private FDAAPMSServiceManager _fdaaPMSServiceManager = new FDAAPMSServiceManager();

        public Dictionary<uint, BaseModule> AllModules = new Dictionary<uint, BaseModule>();

        private ObservableCollection<ARTI3Module> arti3Modules = new ObservableCollection<ARTI3Module>();

        private ObservableCollection<CPNetModule> cpNetModules = new ObservableCollection<CPNetModule>();

        private ObservableCollection<DPLiteModule> dpLiteModules = new ObservableCollection<DPLiteModule>();

        private ObservableCollection<FDAAModule> fdaaModules = new ObservableCollection<FDAAModule>();

        private ObservableCollection<MELSECModule> melsecModules = new ObservableCollection<MELSECModule>();

        private ObservableCollection<NisdasModule> nisdasModules = new ObservableCollection<NisdasModule>();

        private ObservableCollection<OPCClientModule> opcClientModules = new ObservableCollection<OPCClientModule>();

        private ObservableCollection<PlaybackModule> playbackModules = new ObservableCollection<PlaybackModule>();

        private Dictionary<uint, FDAAPMSService> pmsServiceCollection = new Dictionary<uint, FDAAPMSService>();

        private ObservableCollection<RFM5565Module> rfm5565Modules = new ObservableCollection<RFM5565Module>();

        private ObservableCollection<S7DpRequestModule> s7DpRequestModules =
            new ObservableCollection<S7DpRequestModule>();

        private ObservableCollection<S7PnRequestModule> s7PnRequestModules =
            new ObservableCollection<S7PnRequestModule>();

        private ObservableCollection<S7TcpModule> s7TcpModules = new ObservableCollection<S7TcpModule>();

        private ObservableCollection<SmeModule> sseModules = new ObservableCollection<SmeModule>();

        private ObservableCollection<TCNetModule> tcNetModules = new ObservableCollection<TCNetModule>();

        private ObservableCollection<TechnostringModule> technostringModules =
            new ObservableCollection<TechnostringModule>();

        private ObservableCollection<UDPUnicastModule> udpUnicastModules = new ObservableCollection<UDPUnicastModule>();

        private ObservableCollection<UDPMulticastModule> udpMulticastModules = new ObservableCollection<UDPMulticastModule>();

        private ObservableCollection<USigmaModule> uSigmaModules = new ObservableCollection<USigmaModule>();

        private ObservableCollection<VideoCaptureModule> videoCaptureModules =
            new ObservableCollection<VideoCaptureModule>();

        private ObservableCollection<VirtualModule> virtualModules = new ObservableCollection<VirtualModule>();


        public IOConfig()
        {
            EnableModuleMapping = false;
            TecnostringCount = 0;
            DigitalCount = 0;
            AnalogCount = 0;
        }

        public ObservableCollection<UDPUnicastModule> UdpUnicastModules
        {
            get { return udpUnicastModules; }
            set { udpUnicastModules = value; }
        }

        public ObservableCollection<UDPMulticastModule> UdpMulticastModules
        {
            get { return udpMulticastModules; }
            set { udpMulticastModules = value; }
        }

        public ObservableCollection<ARTI3Module> Arti3Modules
        {
            get { return arti3Modules; }
            set { arti3Modules = value; }
        }

        public ObservableCollection<OPCClientModule> OpcClientModules
        {
            get { return opcClientModules; }
            set { opcClientModules = value; }
        }

        public ObservableCollection<DPLiteModule> DpLiteModules
        {
            get { return dpLiteModules; }
            set { dpLiteModules = value; }
        }

        public ObservableCollection<RFM5565Module> Rfm5565Modules
        {
            get { return rfm5565Modules; }
            set { rfm5565Modules = value; }
        }

        public ObservableCollection<S7DpRequestModule> S7DpRequestModules
        {
            get { return s7DpRequestModules; }
            set { s7DpRequestModules = value; }
        }

        public ObservableCollection<S7PnRequestModule> S7PnRequestModules
        {
            get { return s7PnRequestModules; }
            set { s7PnRequestModules = value; }
        }

        public ObservableCollection<S7TcpModule> S7TcpModules
        {
            get { return s7TcpModules; }
            set { s7TcpModules = value; }
        }

        public ObservableCollection<VirtualModule> VirtualModules
        {
            get { return virtualModules; }
            set { virtualModules = value; }
        }

        public ObservableCollection<MELSECModule> MelsecModules
        {
            get { return melsecModules; }
            set { melsecModules = value; }
        }

        public ObservableCollection<CPNetModule> CpNetModules
        {
            get { return cpNetModules; }
            set { cpNetModules = value; }
        }

        public ObservableCollection<TCNetModule> TcNetModules
        {
            get { return tcNetModules; }
            set { tcNetModules = value; }
        }

        public ObservableCollection<USigmaModule> USigmaModules
        {
            get { return uSigmaModules; }
            set { uSigmaModules = value; }
        }

        public ObservableCollection<NisdasModule> NisdasModules
        {
            get { return nisdasModules; }
            set { nisdasModules = value; }
        }

        public ObservableCollection<SmeModule> SseModules
        {
            get { return sseModules; }
            set { sseModules = value; }
        }

        public ObservableCollection<VideoCaptureModule> VideoCaptureModules
        {
            get { return videoCaptureModules; }
            set { videoCaptureModules = value; }
        }

        public ObservableCollection<TechnostringModule> TechnostringModules
        {
            get { return technostringModules; }
            set { technostringModules = value; }
        }

        public ObservableCollection<PlaybackModule> PlaybackModules
        {
            get { return playbackModules; }
            set { playbackModules = value; }
        }

        public ObservableCollection<FDAAModule> FdaaModules
        {
            get { return fdaaModules; }
            set { fdaaModules = value; }
        }

        public uint SoftTimebase { get; set; }

        public int LostFrameModuleNo { get; set; }

        public int LostFrameSignalNo { get; set; }

        public int MaxValue { get; set; }

        public int MinValue { get; set; }

        public int IncrementValue { get; set; }

        public int AnalogCount { get; set; }

        public int DigitalCount { get; set; }

        public int TecnostringCount { get; set; }

        public bool EnableModuleMapping { get; set; }

        public string DbName { get; set; }

        public FAUDeviceManager FauDeviceManager
        {
            get { return _fauDeviceManager; }
            set { _fauDeviceManager = value; }
        }

        public FDAAPMSServiceManager FDAAPMSServiceManager
        {
            get { return _fdaaPMSServiceManager; }
            set { _fdaaPMSServiceManager = value; }
        }

        public Dictionary<uint, FDAAPMSService> PmsServiceCollection
        {
            get { return pmsServiceCollection; }
            set { pmsServiceCollection = value; }
        }

        public void LoadIOConfig(string fileName)
        {
            AnalogCount = 0;
            DigitalCount = 0;
            TecnostringCount = 0;
            AllModules.Clear();

            // 创建XML文件读取对象
            var parser = new XmlParser();

            try
            {
                // 加载指定的xml文件
                parser.LoadConfig(fileName);
            }
            catch (Exception ex)
            {
                // 如果读取文件有异常，则提示
                MessageBox.Show(ex.Message, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                throw;
            }

            // 读取General信息
            var nodeGeneral = parser.ParsedNode.NodeList[0];
            SoftTimebase = Convert.ToUInt32(nodeGeneral.GetValueByName("SoftTimebase"));

            // 读取DataBase信息
            if (nodeGeneral.NodeList.Count > 5)
            {
                var nodeDataBase = nodeGeneral.NodeList[5];
                DbName = nodeDataBase.GetValueByName("DBName");
            }

            // 获取UDP工作模式
            if (nodeGeneral.NodeList.Count > 16)
                EnableModuleMapping =
                    Convert.ToBoolean(Convert.ToInt32(nodeGeneral.GetValueByName("EnableModuleMapping")));

            // 读取FrameLostCounter信息
            if (nodeGeneral.NodeList.Count > 1)
            {
                var nodeFrameLostCounter = nodeGeneral.NodeList[1];
                var sigAddress = nodeFrameLostCounter.GetValueByName("SignalAddress");
                // 从信号地址中截取module号和signal号
                var len = sigAddress.Length;
                var spaceIndex = sigAddress.IndexOf(":");
                if (spaceIndex != -1)
                {
                    var temp = sigAddress.Substring(0, spaceIndex);
                    LostFrameModuleNo = Convert.ToInt32(temp);

                    temp = sigAddress.Substring(spaceIndex + 1, len - spaceIndex - 1);
                    LostFrameSignalNo = Convert.ToInt32(temp);
                }

                MaxValue = Convert.ToInt32(nodeFrameLostCounter.GetValueByName("MaxValue"));
                MinValue = Convert.ToInt32(nodeFrameLostCounter.GetValueByName("MinValue"));
                IncrementValue = Convert.ToInt32(nodeFrameLostCounter.GetValueByName("IncrementValue"));
            }

            // 读取所有FAU设备
            if (nodeGeneral.NodeList.Count > 9)
            {
                var nodeFAUDevices = nodeGeneral.NodeList[9];

                foreach (var nodeFAU in nodeFAUDevices.NodeList)
                {
                    var no = Convert.ToUInt32(nodeFAU.GetValueByName("Number"));
                    var name = nodeFAU.GetValueByName("Name");
                    var type = nodeFAU.GetValueByName("Type");
                    var ip = nodeFAU.GetValueByName("IPAddress");
                    var fau = new FAUDevice(no, name, type, ip);
                    FauDeviceManager.addFAU(fau);
                }
            }

            // 获取所有PMSServices
            if (nodeGeneral.NodeList.Count > 13)
            {
                var nodePMSservices = nodeGeneral.NodeList[13];

                foreach (var nodePMSservice in nodePMSservices.NodeList)
                {
                    var PMSService = new FDAAPMSService();

                    PMSService.No = Convert.ToUInt32(nodePMSservice.GetValueByName("Number"));
                    PMSService.Name = nodePMSservice.GetValueByName("Name");
                    PMSService.Ip = nodePMSservice.GetValueByName("IPAddress");
                    PMSService.Port = Convert.ToInt32(nodePMSservice.GetValueByName("ListeningPort"));
                    PMSService.ReconnectionPeriod =
                        Convert.ToInt32(nodePMSservice.GetValueByName("ReconnectionPeriod"));
                    PMSService.prefix = nodePMSservice.GetValueByName("Prefix");
                    PMSService.usePrefixName =
                        Convert.ToBoolean(Convert.ToUInt32(nodePMSservice.GetValueByName("UsePrefixName")));

                    FDAAPMSServiceManager.addPMSService(PMSService);
                }

                pmsServiceCollection = FDAAPMSServiceManager.PMSServiceCollection;
            }

            var nodeModules = parser.ParsedNode.NodeList[1];
            uint moduleNo;
            bool enabled;
            ModuleType moduleType;
            try
            {
                foreach (var nodeModule in nodeModules.NodeList)
                {
                    // 获取ModuleNo和模块类型
                    moduleNo = Convert.ToUInt32(nodeModule.GetValueByName("ModuleNo"));
                    moduleType = (ModuleType) Convert.ToUInt32(nodeModule.GetValueByName("ModuleType"));
                    enabled = Convert.ToBoolean(Convert.ToInt32(nodeModule.GetValueByName("Enabled")));

                    if (enabled == false) continue;

                    #region UDP

                    // 如果模块是UDP单播类型的
                    if (moduleType == ModuleType.mtUDPInteger || moduleType == ModuleType.mtUDPReal
                                                               || moduleType == ModuleType.mtUDPUserDefined)
                    {
                        // 创建一个新的UDP Module对象
                        var udpUnicastModule = new UDPUnicastModule(moduleNo, moduleType);

                        udpUnicastModule.ModuleName = nodeModule.GetValueByName("Name");
                        udpUnicastModule.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        udpUnicastModule.Port = Convert.ToInt32(nodeModule.GetValueByName("PortNo"));
                        udpUnicastModule.ModuleIndex = Convert.ToUInt32(nodeModule.GetValueByName("ModuleIndex"));
                        udpUnicastModule.TelegramLength = Convert.ToUInt32(nodeModule.GetValueByName("TelegramLength"));
                        udpUnicastModule.Nbo = Helper.NboNumber2String(nodeModule.GetValueByName("NBO"));

                        // 获取所有的Analog数据
                        var nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (var nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = udpUnicastModule.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                                dr["Offset"] = nodeSignal.GetValueByName("Offset");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                                dr["偏移地址"] = nodeSignal.GetValueByName("Offset");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }

                            udpUnicastModule.AnalogDataTable.Rows.Add(dr);
                            AnalogCount++;
                        }

                        // 获取所有的Digital数据
                        var nodeDigital = nodeModule.GetNodeByName("Digital");
                        foreach (var nodeSignal in nodeDigital.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = udpUnicastModule.DigitalDataTable.NewRow();
                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Offset"] = nodeSignal.GetValueByName("Offset");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["偏移地址"] = nodeSignal.GetValueByName("Offset");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }

                            udpUnicastModule.DigitalDataTable.Rows.Add(dr);
                            DigitalCount++;
                        }

                        udpUnicastModules.Add(udpUnicastModule);
                        AllModules.Add(moduleNo, udpUnicastModule);
                    }

                    #endregion#region UDP

                    #region UDP multicast
                    // 如果模块是UDP多播类型的
                    if (moduleType == ModuleType.mtUDPMulticast)
                    {
                        // 创建一个新的UDP Module对象
                        var udpMulticastModule = new UDPMulticastModule(moduleNo, moduleType);

                        udpMulticastModule.ModuleName = nodeModule.GetValueByName("Name");
                        udpMulticastModule.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        udpMulticastModule.MulticastAddress = nodeModule.GetValueByName("MulticastAddress");
                        udpMulticastModule.MulticastPort = Convert.ToInt32(nodeModule.GetValueByName("DestinationPort"));
                        udpMulticastModule.SourceAddress = nodeModule.GetValueByName("SourceAddress");
                        udpMulticastModule.Nbo = Helper.NboNumber2String(nodeModule.GetValueByName("NBO"));

                        // 获取所有的Analog数据
                        var nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (var nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = udpMulticastModule.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                                dr["Offset"] = nodeSignal.GetValueByName("Offset");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                                dr["偏移地址"] = nodeSignal.GetValueByName("Offset");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }

                            udpMulticastModule.AnalogDataTable.Rows.Add(dr);
                            AnalogCount++;
                        }

                        // 获取所有的Digital数据
                        var nodeDigital = nodeModule.GetNodeByName("Digital");
                        foreach (var nodeSignal in nodeDigital.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = udpMulticastModule.DigitalDataTable.NewRow();
                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Offset"] = nodeSignal.GetValueByName("Offset");
                                dr["BitNo"] = nodeSignal.GetValueByName("BitNo");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["偏移地址"] = nodeSignal.GetValueByName("Offset");
                                dr["比特位"] = nodeSignal.GetValueByName("BitNo");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }

                            udpMulticastModule.DigitalDataTable.Rows.Add(dr);
                            DigitalCount++;
                        }

                        UdpMulticastModules.Add(udpMulticastModule);
                        AllModules.Add(moduleNo, udpMulticastModule);
                    }

                    #endregion

                    #region DP

                    // 如果模块类型是Profibus DP类型的
                    else if (moduleType == ModuleType.mtDPLiteInteger || moduleType == ModuleType.mtDPLiteReal
                                                                       || moduleType ==
                                                                       ModuleType.mtDPLiteDoubleInteger)
                    {
                        // 创建一个新的DP Module对象
                        var dpLiteModule = new DPLiteModule(moduleNo, moduleType);

                        dpLiteModule.ModuleName = nodeModule.GetValueByName("Name");
                        dpLiteModule.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        // 获取所有的Analog数据
                        var nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (var nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = dpLiteModule.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                                dr["Offset"] = nodeSignal.GetValueByName("Offset");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                                dr["偏移地址"] = nodeSignal.GetValueByName("Offset");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }

                            dpLiteModule.AnalogDataTable.Rows.Add(dr);
                            AnalogCount++;
                        }

                        // 获取所有的Digital数据
                        var nodeDigital = nodeModule.GetNodeByName("Digital");
                        foreach (var nodeSignal in nodeDigital.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = dpLiteModule.DigitalDataTable.NewRow();
                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Offset"] = nodeSignal.GetValueByName("Offset");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["偏移地址"] = nodeSignal.GetValueByName("Offset");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }

                            dpLiteModule.DigitalDataTable.Rows.Add(dr);
                            DigitalCount++;
                        }

                        dpLiteModules.Add(dpLiteModule);
                        AllModules.Add(moduleNo, dpLiteModule);
                    }

                    #endregion

                    #region mtRFM5565

                    // 如果模块类型是RFM 5565类型的
                    else if (moduleType == ModuleType.mtRFM5565)
                    {
                        // 创建一个新的RFM5565 Module对象
                        var rfm5566Module = new RFM5565Module(moduleNo, moduleType);

                        rfm5566Module.ModuleName = nodeModule.GetValueByName("Name");
                        rfm5566Module.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        // 获取所有的Analog数据
                        var nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (var nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = rfm5566Module.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                                dr["Rfm Address"] = string.Format("0x{0:X8}",
                                    Convert.ToInt32(nodeSignal.GetValueByName("Address")));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                                dr["反射内存网地址"] = string.Format("0x{0:X8}",
                                    Convert.ToInt32(nodeSignal.GetValueByName("Address")));
                            }

                            rfm5566Module.AnalogDataTable.Rows.Add(dr);
                            AnalogCount++;
                        }

                        // 获取所有的Digital数据
                        var nodeDigital = nodeModule.GetNodeByName("Digital");
                        foreach (var nodeSignal in nodeDigital.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = rfm5566Module.DigitalDataTable.NewRow();
                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Rfm Address"] = string.Format("0x{0:X8}",
                                    Convert.ToInt32(nodeSignal.GetValueByName("Address")));
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                                dr["Bit No"] = nodeSignal.GetValueByName("BitNo");
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["反射内存网地址"] = string.Format("0x{0:X8}",
                                    Convert.ToInt32(nodeSignal.GetValueByName("Address")));
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                                dr["比特位"] = nodeSignal.GetValueByName("BitNo");
                            }

                            rfm5566Module.DigitalDataTable.Rows.Add(dr);
                            DigitalCount++;
                        }

                        rfm5565Modules.Add(rfm5566Module);
                        AllModules.Add(moduleNo, rfm5566Module);
                    }

                    #endregion

                    #region OPC

                    // 如果模块类型是OPC Client类型的
                    else if (moduleType == ModuleType.mtOPCClient)
                    {
                        // 创建一个新的OPCClient Module对象
                        var opcClientModule = new OPCClientModule(moduleNo, moduleType);

                        opcClientModule.ModuleName = nodeModule.GetValueByName("Name");
                        opcClientModule.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        // 获取所有的Analog数据
                        var nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (var nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = opcClientModule.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                                dr["Item ID"] = nodeSignal.GetValueByName("ItemID");
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                                dr["数据项识别号"] = nodeSignal.GetValueByName("ItemID");
                            }

                            opcClientModule.AnalogDataTable.Rows.Add(dr);
                            AnalogCount++;
                        }

                        // 获取所有的Digital数据
                        var nodeDigital = nodeModule.GetNodeByName("Digital");
                        foreach (var nodeSignal in nodeDigital.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = opcClientModule.DigitalDataTable.NewRow();
                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Item ID"] = nodeSignal.GetValueByName("ItemID");
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["数据项识别号"] = nodeSignal.GetValueByName("ItemID");
                            }

                            opcClientModule.DigitalDataTable.Rows.Add(dr);
                            DigitalCount++;
                        }

                        opcClientModules.Add(opcClientModule);
                        AllModules.Add(moduleNo, opcClientModule);
                    }

                    #endregion

                    #region S7TCP

                    // 如果模块类型是S7类型的
                    else if (moduleType == ModuleType.mtS7TCP300 || moduleType == ModuleType.mtS7TCP400DB
                                                                  || moduleType == ModuleType.mtS7TCP400NoneDB ||
                                                                  moduleType == ModuleType.mtS7TCP)
                    {
                        // 创建一个新的S7TCP Module对象
                        var s7TCPModule = new S7TcpModule(moduleNo, moduleType);

                        s7TCPModule.ModuleName = nodeModule.GetValueByName("Name");
                        s7TCPModule.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        // 获取所有的Analog数据
                        var nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (var nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = s7TCPModule.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                                dr["S7 Symbol"] = nodeSignal.GetValueByName("S7Symbol");
                                dr["S7 Operand"] = nodeSignal.GetValueByName("S7Operand");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                                dr["S7 符号"] = nodeSignal.GetValueByName("S7Symbol");
                                dr["S7 操作符"] = nodeSignal.GetValueByName("S7Operand");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }

                            s7TCPModule.AnalogDataTable.Rows.Add(dr);
                            AnalogCount++;
                        }

                        // 获取所有的Digital数据
                        var nodeDigital = nodeModule.GetNodeByName("Digital");
                        foreach (var nodeSignal in nodeDigital.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = s7TCPModule.DigitalDataTable.NewRow();
                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["S7 Symbol"] = nodeSignal.GetValueByName("S7Symbol");
                                dr["S7 Operand"] = nodeSignal.GetValueByName("S7Operand");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["S7 符号"] = nodeSignal.GetValueByName("S7Symbol");
                                dr["S7 操作符"] = nodeSignal.GetValueByName("S7Operand");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }

                            s7TCPModule.DigitalDataTable.Rows.Add(dr);
                            DigitalCount++;
                        }

                        s7TcpModules.Add(s7TCPModule);
                        AllModules.Add(moduleNo, s7TCPModule);
                    }

                    #endregion

                    #region S7DpRequest

                    // 如果模块类型是S7类型的
                    else if (moduleType == ModuleType.mtS7DPRequest)
                    {
                        // 创建一个新的S7TCP Module对象
                        var s7DpRequestModule = new S7DpRequestModule(moduleNo, moduleType);

                        s7DpRequestModule.ModuleName = nodeModule.GetValueByName("Name");
                        s7DpRequestModule.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        // 获取所有的Analog数据
                        var nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (var nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = s7DpRequestModule.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                                dr["S7 Symbol"] = nodeSignal.GetValueByName("S7Symbol");
                                dr["S7 Operand"] = nodeSignal.GetValueByName("S7Operand");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                                dr["S7 符号"] = nodeSignal.GetValueByName("S7Symbol");
                                dr["S7 操作符"] = nodeSignal.GetValueByName("S7Operand");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }

                            s7DpRequestModule.AnalogDataTable.Rows.Add(dr);
                            AnalogCount++;
                        }

                        // 获取所有的Digital数据
                        var nodeDigital = nodeModule.GetNodeByName("Digital");
                        foreach (var nodeSignal in nodeDigital.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = s7DpRequestModule.DigitalDataTable.NewRow();
                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["S7 Symbol"] = nodeSignal.GetValueByName("S7Symbol");
                                dr["S7 Operand"] = nodeSignal.GetValueByName("S7Operand");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["S7 符号"] = nodeSignal.GetValueByName("S7Symbol");
                                dr["S7 操作符"] = nodeSignal.GetValueByName("S7Operand");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }

                            s7DpRequestModule.DigitalDataTable.Rows.Add(dr);
                            DigitalCount++;
                        }

                        s7DpRequestModules.Add(s7DpRequestModule);
                        AllModules.Add(moduleNo, s7DpRequestModule);
                    }

                    #endregion

                    #region S7PnRequest

                    // 如果模块类型是S7类型的
                    else if (moduleType == ModuleType.mtS7PNRequest)
                    {
                        // 创建一个新的S7TCP Module对象
                        var s7PnRequestModule = new S7PnRequestModule(moduleNo, moduleType);

                        s7PnRequestModule.ModuleName = nodeModule.GetValueByName("Name");
                        s7PnRequestModule.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        // 获取所有的Analog数据
                        var nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (var nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = s7PnRequestModule.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                                dr["S7 Symbol"] = nodeSignal.GetValueByName("S7Symbol");
                                dr["S7 Operand"] = nodeSignal.GetValueByName("S7Operand");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                                dr["S7 符号"] = nodeSignal.GetValueByName("S7Symbol");
                                dr["S7 操作符"] = nodeSignal.GetValueByName("S7Operand");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }

                            s7PnRequestModule.AnalogDataTable.Rows.Add(dr);
                            AnalogCount++;
                        }

                        // 获取所有的Digital数据
                        var nodeDigital = nodeModule.GetNodeByName("Digital");
                        foreach (var nodeSignal in nodeDigital.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = s7PnRequestModule.DigitalDataTable.NewRow();
                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["S7 Symbol"] = nodeSignal.GetValueByName("S7Symbol");
                                dr["S7 Operand"] = nodeSignal.GetValueByName("S7Operand");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["S7 符号"] = nodeSignal.GetValueByName("S7Symbol");
                                dr["S7 操作符"] = nodeSignal.GetValueByName("S7Operand");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }

                            s7PnRequestModule.DigitalDataTable.Rows.Add(dr);
                            DigitalCount++;
                        }

                        s7PnRequestModules.Add(s7PnRequestModule);
                        AllModules.Add(moduleNo, s7PnRequestModule);
                    }

                    #endregion

                    #region ARTI3

                    // 如果模块是ARTI3类型的
                    else if (moduleType == ModuleType.mtArti3)
                    {
                        var arti3Module = new ARTI3Module(moduleNo, ModuleType.mtArti3);

                        arti3Module.ModuleName = nodeModule.GetValueByName("Name");
                        arti3Module.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        arti3Module.IpAddress = nodeModule.GetValueByName("DeviceNode");
                        // 获取所有的Analog数据
                        var nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (var nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = arti3Module.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                                dr["ARTI3 Symbol"] = nodeSignal.GetValueByName("ARTI3Symbol");
                                //dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                                dr["ARTI3 符号"] = nodeSignal.GetValueByName("ARTI3Symbol");
                                //dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }

                            arti3Module.AnalogDataTable.Rows.Add(dr);
                            AnalogCount++;
                        }

                        // 获取所有的Digital数据
                        var nodeDigital = nodeModule.GetNodeByName("Digital");
                        foreach (var nodeSignal in nodeDigital.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = arti3Module.DigitalDataTable.NewRow();
                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["ARTI3 Symbol"] = nodeSignal.GetValueByName("ARTI3Symbol");
                                //dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["ARTI3 符号"] = nodeSignal.GetValueByName("ARTI3Symbol");
                                //dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }

                            arti3Module.DigitalDataTable.Rows.Add(dr);
                            DigitalCount++;
                        }

                        arti3Modules.Add(arti3Module);
                        AllModules.Add(moduleNo, arti3Module);
                    }

                    #endregion

                    #region MELSEC

                    // 如果模块是MELSEC类型的
                    else if (moduleType == ModuleType.mtMelsecQ)
                    {
                        var no = FauDeviceManager.getAvailableFAUNo();
                        var name = "FAU_" + no;
                        var type = "MELSEC";
                        var ip = nodeModule.GetValueByName("GatewayIP");
                        var fau = new FAUDevice(no, name, type, ip);
                        FauDeviceManager.addFAU(fau);

                        var mcModule = new MELSECModule(moduleNo, ModuleType.mtMelsecQ, fau);

                        mcModule.ModuleName = nodeModule.GetValueByName("Name");
                        mcModule.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        // 获取所有的Analog数据
                        var nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (var nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = mcModule.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                                dr["Address Notation"] = nodeSignal.GetValueByName("ComponentType") +
                                                         nodeSignal.GetValueByName("Address");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                                dr["软元件地址符"] = nodeSignal.GetValueByName("ComponentType") +
                                               nodeSignal.GetValueByName("Address");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }

                            mcModule.AnalogDataTable.Rows.Add(dr);
                            AnalogCount++;
                        }

                        // 获取所有的Digital数据
                        var nodeDigital = nodeModule.GetNodeByName("Digital");
                        foreach (var nodeSignal in nodeDigital.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = mcModule.DigitalDataTable.NewRow();
                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Address Notation"] = nodeSignal.GetValueByName("ComponentType") +
                                                         nodeSignal.GetValueByName("Address");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["软元件地址符"] = nodeSignal.GetValueByName("ComponentType") +
                                               nodeSignal.GetValueByName("Address");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }

                            mcModule.DigitalDataTable.Rows.Add(dr);
                            DigitalCount++;
                        }

                        melsecModules.Add(mcModule);
                        AllModules.Add(moduleNo, mcModule);
                    }

                    #endregion

                    #region Video Capture

                    else if (moduleType == ModuleType.mtVideoCapture)
                    {
                        var videoCaptureModule = new VideoCaptureModule(moduleNo, moduleType);

                        videoCaptureModule.ModuleName = nodeModule.GetValueByName("Name");
                        videoCaptureModule.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        // 获取所有的Analog数据
                        var nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (var nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = videoCaptureModule.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                                dr["IPAddress"] = nodeSignal.GetValueByName("IPAddress");
                                dr["CameraName"] = nodeSignal.GetValueByName("CameraName");
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                                dr["IPAddress"] = nodeSignal.GetValueByName("IPAddress");
                                dr["CameraName"] = nodeSignal.GetValueByName("CameraName");
                            }

                            videoCaptureModule.AnalogDataTable.Rows.Add(dr);
                            AnalogCount++;
                        }

                        videoCaptureModules.Add(videoCaptureModule);
                        AllModules.Add(moduleNo, videoCaptureModule);
                    }

                    #endregion

                    #region CPNET

                    else if (moduleType == ModuleType.mtCP3550)
                    {
                        var fauNumber = Convert.ToUInt32(nodeModule.GetValueByName("FAUNumber"));
                        var cpNetModule = new CPNetModule(moduleNo, moduleType, FauDeviceManager.getFAU(fauNumber));

                        cpNetModule.ModuleName = nodeModule.GetValueByName("Name");
                        cpNetModule.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        // 获取所有的Analog数据
                        var nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (var nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = cpNetModule.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                                dr["Operand Notation"] = nodeSignal.GetValueByName("OperandNotation");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                                dr["操作符标志"] = nodeSignal.GetValueByName("OperandNotation");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }

                            cpNetModule.AnalogDataTable.Rows.Add(dr);
                            AnalogCount++;
                        }

                        // 获取所有的Digital数据
                        var nodeDigital = nodeModule.GetNodeByName("Digital");
                        foreach (var nodeSignal in nodeDigital.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = cpNetModule.DigitalDataTable.NewRow();
                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Operand Notation"] = nodeSignal.GetValueByName("OperandNotation");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["操作符标志"] = nodeSignal.GetValueByName("OperandNotation");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }

                            cpNetModule.DigitalDataTable.Rows.Add(dr);
                            DigitalCount++;
                        }

                        cpNetModules.Add(cpNetModule);
                        AllModules.Add(moduleNo, cpNetModule);
                    }

                    #endregion

                    #region TCNET

                    else if (moduleType == ModuleType.mtToshibaV3000 || moduleType == ModuleType.mtToshibaNV)
                    {
                        var fauNumber = Convert.ToUInt32(nodeModule.GetValueByName("FAUNumber"));
                        var tcNetModule = new TCNetModule(moduleNo, moduleType, FauDeviceManager.getFAU(fauNumber));

                        tcNetModule.ModuleName = nodeModule.GetValueByName("Name");
                        tcNetModule.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        // 获取所有的Analog数据
                        var nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (var nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = tcNetModule.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                                dr["Operand Notation"] = nodeSignal.GetValueByName("OperandNotation");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                                dr["操作符标志"] = nodeSignal.GetValueByName("OperandNotation");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }

                            tcNetModule.AnalogDataTable.Rows.Add(dr);
                            AnalogCount++;
                        }

                        // 获取所有的Digital数据
                        var nodeDigital = nodeModule.GetNodeByName("Digital");
                        foreach (var nodeSignal in nodeDigital.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = tcNetModule.DigitalDataTable.NewRow();
                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Operand Notation"] = nodeSignal.GetValueByName("OperandNotation");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["操作符标志"] = nodeSignal.GetValueByName("OperandNotation");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }

                            tcNetModule.DigitalDataTable.Rows.Add(dr);
                            DigitalCount++;
                        }

                        tcNetModules.Add(tcNetModule);
                        AllModules.Add(moduleNo, tcNetModule);
                    }

                    #endregion

                    #region Nisdas

                    else if (moduleType == ModuleType.mtNisdas)
                    {
                        var fauNumber = Convert.ToUInt32(nodeModule.GetValueByName("FAUNumber"));
                        var nisdasModule = new NisdasModule(moduleNo, moduleType, FauDeviceManager.getFAU(fauNumber));

                        nisdasModule.ModuleName = nodeModule.GetValueByName("Name");
                        nisdasModule.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        // 获取所有的Analog数据
                        var nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (var nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = nisdasModule.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                                dr["Variable"] = nodeSignal.GetValueByName("Variable");
                                dr[" Address"] = nodeSignal.GetValueByName("Address");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                                dr["变量名"] = nodeSignal.GetValueByName("Variable");
                                dr[" 地址"] = nodeSignal.GetValueByName("Address");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }

                            nisdasModule.AnalogDataTable.Rows.Add(dr);
                            AnalogCount++;
                        }

                        // 获取所有的Digital数据
                        var nodeDigital = nodeModule.GetNodeByName("Digital");
                        foreach (var nodeSignal in nodeDigital.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = nisdasModule.DigitalDataTable.NewRow();
                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Variable"] = nodeSignal.GetValueByName("Variable");
                                dr[" Address"] = nodeSignal.GetValueByName("Address");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["变量名"] = nodeSignal.GetValueByName("Variable");
                                dr[" 地址"] = nodeSignal.GetValueByName("Address");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }

                            nisdasModule.DigitalDataTable.Rows.Add(dr);
                            DigitalCount++;
                        }

                        nisdasModules.Add(nisdasModule);
                        AllModules.Add(moduleNo, nisdasModule);
                    }

                    #endregion

                    #region Sme

                    else if (moduleType == ModuleType.mtSme)
                    {
                        var fauNumber = Convert.ToUInt32(nodeModule.GetValueByName("FAUNumber"));
                        var smeModule = new SmeModule(moduleNo, moduleType, FauDeviceManager.getFAU(fauNumber));

                        smeModule.ModuleName = nodeModule.GetValueByName("Name");
                        smeModule.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        // 获取所有的Analog数据
                        var nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (var nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = smeModule.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                                dr["Variable"] = nodeSignal.GetValueByName("Variable");
                                dr[" Address"] = nodeSignal.GetValueByName("Address");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                                dr["变量名"] = nodeSignal.GetValueByName("Variable");
                                dr[" 地址"] = nodeSignal.GetValueByName("Address");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }

                            smeModule.AnalogDataTable.Rows.Add(dr);
                            AnalogCount++;
                        }

                        // 获取所有的Digital数据
                        var nodeDigital = nodeModule.GetNodeByName("Digital");
                        foreach (var nodeSignal in nodeDigital.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = smeModule.DigitalDataTable.NewRow();
                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Variable"] = nodeSignal.GetValueByName("Variable");
                                dr[" Address"] = nodeSignal.GetValueByName("Address");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["变量名"] = nodeSignal.GetValueByName("Variable");
                                dr[" 地址"] = nodeSignal.GetValueByName("Address");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }

                            smeModule.DigitalDataTable.Rows.Add(dr);
                            DigitalCount++;
                        }

                        sseModules.Add(smeModule);
                        AllModules.Add(moduleNo, smeModule);
                    }

                    #endregion

                    #region USIGMA

                    else if (moduleType == ModuleType.mtHitachiR700 || moduleType == ModuleType.mtHitachiR900)
                    {
                        var fauNumber = Convert.ToUInt32(nodeModule.GetValueByName("FAUNumber"));
                        var usigmaModule = new USigmaModule(moduleNo, moduleType, FauDeviceManager.getFAU(fauNumber));

                        usigmaModule.ModuleName = nodeModule.GetValueByName("Name");
                        usigmaModule.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        // 获取所有的Analog数据
                        var nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (var nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = usigmaModule.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                                dr["Operand Notation"] = nodeSignal.GetValueByName("OperandNotation");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                                dr["操作符标志"] = nodeSignal.GetValueByName("OperandNotation");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }

                            usigmaModule.AnalogDataTable.Rows.Add(dr);
                            AnalogCount++;
                        }

                        // 获取所有的Digital数据
                        var nodeDigital = nodeModule.GetNodeByName("Digital");
                        foreach (var nodeSignal in nodeDigital.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = usigmaModule.DigitalDataTable.NewRow();
                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");

                                //  N0000|11 => N00000|B  
                                var OperandNotation = nodeSignal.GetValueByName("OperandNotation");
                                var strDec = OperandNotation.Substring(OperandNotation.IndexOf("|") + 1);
                                var strHead = OperandNotation.Substring(0, OperandNotation.IndexOf("|") + 1);
                                var value = Convert.ToInt32(strDec);
                                var hexOutput = string.Format("{0:X}", value);
                                //string formatValue = string.Format("{0:d2}", value);
                                var OperandNotationDec = strHead + hexOutput;

                                dr["Operand Notation"] = OperandNotationDec;
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");

                                //  N0000|11 => N00000|B  
                                var OperandNotation = nodeSignal.GetValueByName("OperandNotation");
                                var strDec = OperandNotation.Substring(OperandNotation.IndexOf("|") + 1);
                                var strHead = OperandNotation.Substring(0, OperandNotation.IndexOf("|") + 1);
                                var value = Convert.ToInt32(strDec);
                                var hexOutput = string.Format("{0:X}", value);
                                //string formatValue = string.Format("{0:d2}", value);
                                var OperandNotationDec = strHead + hexOutput;

                                dr["操作符标志"] = OperandNotationDec;
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }

                            usigmaModule.DigitalDataTable.Rows.Add(dr);
                            DigitalCount++;
                        }

                        uSigmaModules.Add(usigmaModule);
                        AllModules.Add(moduleNo, usigmaModule);
                    }

                    #endregion

                    #region PLAYBACK

                    else if (moduleType == ModuleType.mtPlayback)
                    {
                        // 创建一个新的UDP Module对象
                        var playbackModule = new PlaybackModule(moduleNo, moduleType);

                        playbackModule.ModuleName = nodeModule.GetValueByName("Name");
                        playbackModule.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        // 获取所有的Analog数据
                        var nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (var nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = playbackModule.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }

                            playbackModule.AnalogDataTable.Rows.Add(dr);
                            AnalogCount++;
                        }

                        // 获取所有的Digital数据
                        var nodeDigital = nodeModule.GetNodeByName("Digital");
                        foreach (var nodeSignal in nodeDigital.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = playbackModule.DigitalDataTable.NewRow();
                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }

                            playbackModule.DigitalDataTable.Rows.Add(dr);
                            DigitalCount++;
                        }

                        playbackModules.Add(playbackModule);
                        AllModules.Add(moduleNo, playbackModule);
                    }

                    #endregion

                    #region FDAA

                    else if (moduleType == ModuleType.mtFDAA)
                    {
                        var PMSNumber = Convert.ToUInt32(nodeModule.GetValueByName("PMSNumber"));
                        var pmsService = FDAAPMSServiceManager.getPMSService(PMSNumber);
                        var fdaaModule = new FDAAModule(moduleNo, moduleType, pmsService);
                        pmsService.addModule(moduleNo);

                        fdaaModule.ModuleName = nodeModule.GetValueByName("Name");
                        fdaaModule.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        // 获取所有的Analog数据
                        var nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (var nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = fdaaModule.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                                dr["Operand Notation"] = nodeSignal.GetValueByName("OperandNotation");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                                dr["操作符标志"] = nodeSignal.GetValueByName("OperandNotation");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }

                            fdaaModule.AnalogDataTable.Rows.Add(dr);
                            AnalogCount++;
                        }

                        // 获取所有的Digital数据
                        var nodeDigital = nodeModule.GetNodeByName("Digital");
                        foreach (var nodeSignal in nodeDigital.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = fdaaModule.DigitalDataTable.NewRow();
                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Operand Notation"] = nodeSignal.GetValueByName("OperandNotation");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["操作符标志"] = nodeSignal.GetValueByName("OperandNotation");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }

                            fdaaModule.DigitalDataTable.Rows.Add(dr);
                            DigitalCount++;
                        }

                        fdaaModules.Add(fdaaModule);
                        AllModules.Add(moduleNo, fdaaModule);
                    }

                    #endregion

                    #region VIRTUAL

                    else if (moduleType == ModuleType.mtVirtualforLua)
                    {
                        var virtualModule = new VirtualModule(moduleNo, moduleType);

                        virtualModule.ModuleName = nodeModule.GetValueByName("Name");
                        virtualModule.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        // 获取所有的Analog数据
                        var nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (var nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = virtualModule.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                                dr["Path"] = nodeSignal.GetValueByName("Path");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                                dr["脚本文件"] = nodeSignal.GetValueByName("Path");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }

                            virtualModule.AnalogDataTable.Rows.Add(dr);
                            AnalogCount++;
                        }

                        // 获取所有的Digital数据
                        var nodeDigital = nodeModule.GetNodeByName("Digital");
                        foreach (var nodeSignal in nodeDigital.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            var dr = virtualModule.DigitalDataTable.NewRow();
                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Path"] = nodeSignal.GetValueByName("Path");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["脚本文件"] = nodeSignal.GetValueByName("Path");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }

                            virtualModule.DigitalDataTable.Rows.Add(dr);
                            DigitalCount++;
                        }

                        virtualModules.Add(virtualModule);
                        AllModules.Add(moduleNo, virtualModule);
                    }

                    #endregion

                    #region VIRTUALTS

                    else if (moduleType == ModuleType.mtVirtualforLuaTS)
                    {
                        var virtualTSModule = new TechnostringModule(moduleNo, moduleType);
                        virtualTSModule.ModuleName = nodeModule.GetValueByName("Name");
                        virtualTSModule.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        // 获取所有的section数据
                        var nodeSections = nodeModule.GetNodeByName("Sections");
                        foreach (var nodeSection in nodeSections.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSection.GetValueByName("Active")))) continue;
                            var dr = virtualTSModule.SectionDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSection.GetValueByName("No");
                                dr["Name"] = nodeSection.GetValueByName("Name");
                                //dr["Path"] = nodeSection.GetValueByName("Path");
                                //dr["Data Type"] =
                                //Helper.SigDataTypeNumber2String(nodeSection.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSection.GetValueByName("No");
                                dr["信号名称"] = nodeSection.GetValueByName("Name");
                                //dr["脚本文件"] = nodeSection.GetValueByName("Path");
                                //dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSection.GetValueByName("DataType"));
                            }

                            virtualTSModule.SectionDataTable.Rows.Add(dr);
                            TecnostringCount++;
                        }

                        technostringModules.Add(virtualTSModule);
                        AllModules.Add(moduleNo, virtualTSModule);
                    }

                    #endregion
                }

                // 读取Technostring信息
                if (parser.ParsedNode.NodeList.Count > 4)
                {
                    var nodeTechnostringGeneral = parser.ParsedNode.NodeList[4];
                    var nodeTechnostrings = nodeTechnostringGeneral.GetNodeByName("TechnoStrings");

                    var dataSourceType = string.Empty;
                    foreach (var nodeTechnostring in nodeTechnostrings.NodeList)
                    {
                        if (!Convert.ToBoolean(Convert.ToInt32(nodeTechnostring.GetValueByName("Active")))) continue;
                        dataSourceType = nodeTechnostring.GetValueByName("DataSourceType");
                        if (dataSourceType == "301")
                        {
                            moduleNo = Convert.ToUInt32(nodeTechnostring.GetValueByName("ModuleNo"));
                            var opcTechnostring = new TechnostringModule(moduleNo, ModuleType.mtTsOPC);
                            opcTechnostring.Enabled =
                                Convert.ToBoolean(Convert.ToInt32(nodeTechnostring.GetValueByName("Active")));
                            opcTechnostring.ModuleName = nodeTechnostring.GetValueByName("Name");

                            var nodeSections = nodeTechnostring.GetNodeByName("Sections");
                            foreach (var nodeSection in nodeSections.NodeList)
                            {
                                var dr = opcTechnostring.SectionDataTable.NewRow();

                                if (ServerConfig.getInstance().Language)
                                {
                                    dr["Address"] = nodeSection.GetValueByName("No");
                                    dr["Name"] = nodeSection.GetValueByName("Name");
                                }
                                else
                                {
                                    dr["地址"] = nodeSection.GetValueByName("No");
                                    dr["信号名称"] = nodeSection.GetValueByName("Name");
                                }

                                opcTechnostring.SectionDataTable.Rows.Add(dr);
                                TecnostringCount++;
                            }

                            technostringModules.Add(opcTechnostring);
                            AllModules.Add(moduleNo, opcTechnostring);
                        }

                        if (dataSourceType == "300")
                        {
                            moduleNo = Convert.ToUInt32(nodeTechnostring.GetValueByName("ModuleNo"));
                            var FauTechnostring = new TechnostringModule(moduleNo, ModuleType.mtTsFAU);
                            FauTechnostring.Enabled =
                                Convert.ToBoolean(Convert.ToInt32(nodeTechnostring.GetValueByName("Active")));
                            FauTechnostring.ModuleName = nodeTechnostring.GetValueByName("Name");

                            var nodeSections = nodeTechnostring.GetNodeByName("Sections");
                            foreach (var nodeSection in nodeSections.NodeList)
                            {
                                var dr = FauTechnostring.SectionDataTable.NewRow();

                                if (ServerConfig.getInstance().Language)
                                {
                                    dr["Address"] = nodeSection.GetValueByName("No");
                                    dr["Name"] = nodeSection.GetValueByName("Name");
                                }
                                else
                                {
                                    dr["地址"] = nodeSection.GetValueByName("No");
                                    dr["信号名称"] = nodeSection.GetValueByName("Name");
                                }

                                FauTechnostring.SectionDataTable.Rows.Add(dr);
                                TecnostringCount++;
                                technostringModules.Add(FauTechnostring);
                                AllModules.Add(moduleNo, FauTechnostring);
                            }
                        }

                        if (dataSourceType == "302")
                        {
                            moduleNo = Convert.ToUInt32(nodeTechnostring.GetValueByName("ModuleNo"));
                            var UdpTechnostring = new TechnostringModule(moduleNo, ModuleType.mtTsUDP);
                            UdpTechnostring.Enabled =
                                Convert.ToBoolean(Convert.ToInt32(nodeTechnostring.GetValueByName("Active")));
                            UdpTechnostring.ModuleName = nodeTechnostring.GetValueByName("Name");

                            var nodeSections = nodeTechnostring.GetNodeByName("Sections");
                            foreach (var nodeSection in nodeSections.NodeList)
                            {
                                var dr = UdpTechnostring.SectionDataTable.NewRow();

                                if (ServerConfig.getInstance().Language)
                                {
                                    dr["Address"] = nodeSection.GetValueByName("No");
                                    dr["Name"] = nodeSection.GetValueByName("Name");
                                }
                                else
                                {
                                    dr["地址"] = nodeSection.GetValueByName("No");
                                    dr["信号名称"] = nodeSection.GetValueByName("Name");
                                }

                                UdpTechnostring.SectionDataTable.Rows.Add(dr);
                                TecnostringCount++;
                                technostringModules.Add(UdpTechnostring);
                                AllModules.Add(moduleNo, UdpTechnostring);
                            }
                        }
                    }
                }

                AllModules = AllModules.OrderBy(i => i.Key).ToDictionary(i => i.Key, i => i.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show("LoadIoConfig: " + ex.Message);
            }
        }

        public string GetSignalName(int moduleNo, int signalNo)
        {
            var signalName = "??";
            var address = moduleNo + ":" + signalNo;
            if (moduleNo == -1) return signalName;

            var umo = Convert.ToUInt32(moduleNo);

            // AllModules[umo].
            if (AllModules.Keys.Contains(umo))
            {
                if (ServerConfig.getInstance().Language)
                {
                    foreach (DataRow row in AllModules[umo].AnalogDataTable.Rows)
                        if (row["Address"].ToString() == address)
                            return row["Address"] + " " + row["Name"];
                }
                else
                {
                    foreach (DataRow row in AllModules[umo].AnalogDataTable.Rows)
                        if (row["地址"].ToString() == address)
                            return row["地址"] + " " + row["信号名称"];
                }
            }

            return signalName;
        }
    }
}