using Baosight.FDAA.PackageDiagnosis.Model.Attributes;

namespace Baosight.FDAA.PackageDiagnosis.Model.Enums
{
    public enum Code : uint
    {
        //  注意多个建议举措用&分开 影响前台显示！！！

        #region pmsService package

        [Detail(".Net版本过低")]
        [RecommendedMeasures("安装.Net4.7.2及以上版本,安装程序NDP472-KB4054530-x86-x64-AllOS-ENU.exe位于CD包DotNetFx目录下")]
        PMSService_DoNetVersionLow_Error = 01001,

        [Detail("pmsService 未授权")] 
        [RecommendedMeasures("联系供应商")]
        PMSService_NotLicensed_Error = 01002,

        [Detail("服务状态异常")]
        [RecommendedMeasures("点击常规诊断页面初始化采集坏境按钮 或 以管理员身份运行FDAA安装目录Server文件夹下的ServiceInstall.bat文件")]
        PMSService_StatusAbnormal_Error = 01003,

        [Detail("Guardian 未运行")]
        [RecommendedMeasures("以管理员身份运行FDAA安装目录Server文件夹下的PMSServiceGuardian.exe程序")]
        PMSService_Guardian_NotRunning_Error = 01004,

        [Detail("pmsService 已授权")] 
        [RecommendedMeasures("")]
        PMSService_Licensed_Info = 01101,

        #endregion

        #region udp package

        [Detail("UDP Interface 未安装")]
        [RecommendedMeasures("以<修改>方式运行FDAA安装程序，安装UDP采集接口至当前系统")]
        UDPInterface_NotInstalled_Error = 02001,

        [Detail("UDP Interface 未授权")] 
        [RecommendedMeasures("安装授权")]
        UDPInterface_NotLicensed_Error = 02002,

        [Detail("UDP Interface 端口接收不到数据")]
        [RecommendedMeasures("1.检查pmsService.exe和PMSServiceStatus.exe是否被windows防火墙或第三方安全软件阻挡&2.检查物理网络是否畅通&3.数据源是否将数据发送到FDAA主机的对应端口")]
        UDPInterface_PortNotReceivedData_Error = 02003,

        [Detail("UDP Interface 报文校验失败")] 
        [RecommendedMeasures("1.检查报文头携带的报文号是否正确&2.检查报文长度是否一致&3.网络字节序是否配置正确")]
        UDPInterface_MessageMismatch_Error = 02004,

        [Detail("UDP Interface 模块所有信号采集不到数据")] 
        [RecommendedMeasures("重新启动PMS采集服务")]
        UDPInterface_NotReceivedData_Error = 02005,

        [Detail("UDP Interface 端口绑定失败")]
        [RecommendedMeasures("在IO配置中更换接收端口")]
        UDPInterface_PortBindFailure_Error = 02006,

        [Detail("UDP Interface 多播源地址，目标端口接收不到数据")]
        [RecommendedMeasures("1.检查pmsService.exe和PMSServiceStatus.exe是否被windows防火墙或第三方安全软件阻挡&2.检查物理网络是否畅通&3.数据源是否将数据发送到FDAA主机的对应端口")]
        UDPInterface_SourceAddressDestinationPortNotReceivedData_Error = 02007,

        [Detail("UDP Interface 已安装")] [RecommendedMeasures("")]
        UDPInterface_Installed_Info = 02100,

        [Detail("UDP Interface 已授权")] [RecommendedMeasures("")]
        UDPInterface_Licensed_Info = 02101,

        #endregion

        #region arti3 package

        [Detail("ARTI3 Interface 未安装")]
        [RecommendedMeasures("以<修改>方式运行FDAA安装程序，安装ARTI3采集接口至当前系统")]
        ARTI3Interface_NotInstalled_Error = 03001,

        [Detail("ARTI3 Interface 未授权")]
        [RecommendedMeasures("安装授权")]
        ARTI3Interface_NotLicensed_Error = 03002,

        [Detail("ARTI3 Interface 设备无法访问")] 
        [RecommendedMeasures("1.确认设备IP地址是否配置正确&2.检查网络连接是否畅通")]
        ARTI3Interface_DeviceIpNotPingable_Error = 03003,

        [Detail("ARTI3 Interface 模块所有信号采集不到数据")]
        [RecommendedMeasures("1.检查PLC工作状态&2.重新启动PMS采集服务")]
        ARTI3Interface_NotReceivedData_Error = 03004,

        [Detail("ARTI3 Interface 模块部分信号采集不到数据")]
        [RecommendedMeasures("检查采集信号在PLC中是否存在")]
        ARTI3Interface_PartialNotReceivedData_Error = 03005,

        [Detail("ARTI3 Interface 已安装")] [RecommendedMeasures("")]
        ARTI3Interface_Installed_Info = 03100,

        [Detail("ARTI3 Interface 已授权")] [RecommendedMeasures("")]
        ARTI3Interface_Licensed_Info = 03101,

        #endregion

        #region opc package

        [Detail("OPC Interface 未安装")]
        [RecommendedMeasures("以<修改>方式运行FDAA安装程序，安装OPC采集接口至当前系统")]
        OPCInterface_NotInstalled_Error = 04001,

        [Detail(".net 2.0未安装")]
        [RecommendedMeasures("1.Windows7下，执行.Net2.0安装程序dotnetfx.exe，位于CD包DotNetFx目录下&2.Windows10下，控制面板=>程序和功能=>启用或关闭Windows功能=>安装.NET Framework 3.5")]
        OPCInterface_DoNet20NotInstalled_Error = 04002,

        [Detail("OPC Interface 未授权")]
        [RecommendedMeasures("安装授权")]
        OPCInterface_NotLicensed_Error = 04003,

        [Detail("OPC Interface 模块所有信号采集不到数据")]
        [RecommendedMeasures("1.检查与OPC Server所在的主机网络是否畅通&2.使用IO配置工具”连接到选定的OPC服务“功能查询OPC Server状态，返回错误代码参考OPC用户手册第三章疑难排解指南")]
        OPCInterface_NotReceivedData_Error = 04004,

        [Detail("OPC Interface 模块部分信号采集不到数据")]
        [RecommendedMeasures("1.检查异常信号在OPC Server中是否存在&2.检查OPC Server中对应的标记值是否正常&3.检查ItemID是否含有非法字符")]
        OPCInterface_PartialNotReceivedData_Error = 04005,

        [Detail("OPC Interface 已安装")] [RecommendedMeasures("")]
        OPCInterface_Installed_Info = 04100,

        [Detail("OPC Interface 已授权")] [RecommendedMeasures("")]
        OPCInterface_Licensed_Info = 04101,

        #endregion

        #region profibusDPLite package

        [Detail("ProfibusDPLite Interface 未安装")]
        [RecommendedMeasures("以<修改>方式运行FDAA安装程序，安装ProfibusDPLite采集接口至当前系统")]
        ProfibusDPLiteInterface_NotInstalled_Error = 05001,

        [Detail("ProfibusDPLite Interface 板卡或对应驱动程序未安装")]
        [RecommendedMeasures("安装5136-PBMS-PCI板卡和对应驱动程序")]
        ProfibusDPLiteInterface_CardDriver_NotInstalled_Error = 05002,

        [Detail("ProfibusDPLite Interface 未授权")]
        [RecommendedMeasures("安装授权")]
        ProfibusDPLiteInterface_NotLicensed_Error = 05003,

        [Detail("ProfibusDPLite Interface 已安装")] [RecommendedMeasures("")]
        ProfibusDPLiteInterface_Installed_Info = 05100,

        [Detail("ProfibusDPLite Interface 已授权")] [RecommendedMeasures("")]
        ProfibusDPLiteInterface_Licensed_Info = 05101,

        #endregion

        #region reflective memory 5565 package

        [Detail("Reflective Memory 5565 Interface 未安装")]
        [RecommendedMeasures("以<修改>方式运行FDAA安装程序，安装Reflective Memory 5565采集接口至当前系统")]
        RFMInterface_NotInstalled_Error = 06001,

        [Detail("Reflective Memory 5565 Interface 板卡或对应驱动程序未安装")]
        [RecommendedMeasures("安装Rfm2g PCI/PMC 5565 Driver板卡和对应驱动程序")]
        RFMInterface_CardDriver_NotInstalled_Error = 06002,

        [Detail("Reflective Memory 5565 Interface 未授权")]
        [RecommendedMeasures("安装授权")]
        RFMInterface_NotLicensed_Error = 06003,

        [Detail("Reflective Memory 5565 Interface 已安装")] [RecommendedMeasures("")]
        RFMInterface_Installed_Info = 06100,

        [Detail("Reflective Memory 5565 Interface 已授权")] [RecommendedMeasures("")]
        RFMInterface_Licensed_Info = 06101,

        #endregion

        #region s7DpRequest package

        [Detail("S7DPRequest Interface 未安装")]
        [RecommendedMeasures("以<修改>方式运行FDAA安装程序，安装S7DPRequest采集接口至当前系统")]
        S7DPRequestInterface_NotInstalled_Error = 07001,

        [Detail("S7DPRequest Interface 板卡或对应驱动程序未安装")]
        [RecommendedMeasures("安装5136-PBMS-PCI板卡和对应驱动程序")]
        S7DPRequestInterface_CardDriver_NotInstalled_Error = 07002,

        [Detail("S7DPRequest Interface 未授权")]
        [RecommendedMeasures("安装授权")]
        S7DPRequestInterface_NotLicensed_Error = 07003,

        [Detail("S7DPRequest Interface 模块部分信号采集不到数据")]
        [RecommendedMeasures("S7操作符地址在PLC中不存在，请确认采集信号正确性")]
        S7DPRequestInterface_PartialNotReceivedData_Error = 07004,

        [Detail("S7DPRequest Interface 已安装")] [RecommendedMeasures("")]
        S7DPRequestInterface_Installed_Info = 07100,

        [Detail("S7DPRequest Interface 已授权")] [RecommendedMeasures("")]
        S7DPRequestInterface_Licensed_Info = 07101,

        #endregion

        #region s7PnRequest package

        [Detail("S7PNRequest Interface 未安装")]
        [RecommendedMeasures("以<修改>方式运行FDAA安装程序，安装S7PNRequest采集接口至当前系统")]
        S7PNRequestInterface_NotInstalled_Error = 08001,

        [Detail("S7PNRequest Interface 板卡或对应驱动程序未安装")]
        [RecommendedMeasures("安装cifX PCI/PCIe Device板卡和对应驱动程序")]
        S7PNRequestInterface_CardDriver_NotInstalled_Error = 08002,

        [Detail("S7PNRequest Interface 未授权")]
        [RecommendedMeasures("安装授权")]
        S7PNRequestInterface_NotLicensed_Error = 08003,

        [Detail("S7PNRequest Interface 模块部分信号采集不到数据")]
        [RecommendedMeasures("S7操作符地址在PLC中不存在，请确认采集信号正确性")]
        S7PNRequestInterface_PartialNotReceivedData_Error = 08004,

        [Detail("S7PNRequest Interface 已安装")] [RecommendedMeasures("")]
        S7PNRequestInterface_Installed_Info = 08100,

        [Detail("S7PNRequest Interface 已授权")] [RecommendedMeasures("")]
        S7PNRequestInterface_Licensed_Info = 08101,

        #endregion

        #region s7Tcp package

        [Detail("S7TCP Interface 未安装")]
        [RecommendedMeasures("以<修改>方式运行FDAA安装程序，安装S7TCP采集接口至当前系统")]
        S7TCPInterface_NotInstalled_Error = 09001,

        [Detail("S7TCP Interface 未授权")]
        [RecommendedMeasures("安装授权")]
        S7TCPInterface_NotLicensed_Error = 09002,

        [Detail("S7TCP Interface 模块连接失败")]
        [RecommendedMeasures("打开IO配置工具=>执行对应模块的”测试S7 TCP/IP 连接”命令=>检查IP地址、机架号、槽号是否配置正确=>检查物理链路是否畅通=>检查是否为当前S7连接类型预留了足够的连接资源")]
        S7TCPInterface_ModuleConnectionFailed_Error = 09003,

        [Detail("S7TCP Interface 模块数据响应时间未达到预期")]
        [RecommendedMeasures("增大模块采样时基")]
        S7TCPInterface_ResponseTimeNotAsExpected_Error = 09004,

        [Detail("S7TCP Interface 模块数据一致性不达标")] 
        [RecommendedMeasures("将不达标模块中的信号拆解到多个模块中")]
        S7TCPInterface_ConsistencyNotMeet_Error = 09005,

        [Detail("S7TCP Interface 模块部分信号采集不到数据")]
        [RecommendedMeasures("S7操作符地址在PLC中不存在，请确认采集信号正确性")]
        S7TCPInterface_PartialNotReceivedData_Error = 09006,

        [Detail("S7TCP Interface 已安装")] [RecommendedMeasures("")]
        S7TCPInterface_Installed_Info = 09100,

        [Detail("S7TCP Interface 已授权")] [RecommendedMeasures("")]
        S7TCPInterface_Licensed_Info = 09101,

        #endregion

        #region virtual package

        [Detail("Virtual Interface 未安装")]
        [RecommendedMeasures("以<修改>方式运行FDAA安装程序，安装Virtual采集接口至当前系统")]
        VirtualInterface_NotInstalled_Error = 10001,

        [Detail("Virtual Interface 未授权")]
        [RecommendedMeasures("安装授权")]
        VirtualInterface_NotLicensed_Error = 10002,

        [Detail("Virtual Interface 模块部分信号采集不到数据")]
        [RecommendedMeasures("1.检查对应虚拟信号的脚本文件在磁盘上是否存在&2.脚本文件运行时是否有语法错误&3.如果脚本中引用了绑定或未绑定变量，是否使用了数组下标方式访问其值&4.脚本文件是否有一个明确的return 返回值&5.脚本文件的返回值类型是否正确")]
        VirtualInterface_PartialNotReceivedData_Error = 10003,

        [Detail("Virtual Interface 已安装")] [RecommendedMeasures("")]
        VirtualInterface_Installed_Info = 10100,

        [Detail("Virtual Interface 已授权")] [RecommendedMeasures("")]
        VirtualInterface_Licensed_Info = 10101,

        #endregion

        #region melsec package

        [Detail("MELSEC Interface 未安装")]
        [RecommendedMeasures("以<修改>方式运行FDAA安装程序，安装MELSEC采集接口至当前系统")]
        MELSECInterface_NotInstalled_Error = 11001,

        [Detail("MELSEC Interface 未授权")]
        [RecommendedMeasures("安装授权")]
        MELSECInterface_NotLicensed_Error = 11002,

        [Detail("MELSEC Interface FAU设备无法访问")] 
        [RecommendedMeasures("1.检查IP地址是否配置正确&2.检查网络连接是否畅通&3.将FAU设备断电，重新启动")]
        MELSECInterface_FauDeviceIpNotPingable_Error = 11003,

        [Detail("MELSEC Interface 模块所有信号采集不到数据")]
        [RecommendedMeasures("检查FAU固件类型和版本是否正确，其授权状态和运行状态是否正常")]
        MELSECInterface_NotReceivedData_Error = 11004,
        //  TODO 是否要这个逻辑？
        [Detail("MELSEC Interface 模块部分信号采集不到数据")]
        [RecommendedMeasures("")]
        MELSECInterface_PartialNotReceivedData_Error = 11005,

        [Detail("MELSEC Interface 已安装")] [RecommendedMeasures("")]
        MELSECInterface_Installed_Info = 11100,

        [Detail("MELSEC Interface 已授权")] [RecommendedMeasures("")]
        MELSECInterface_Licensed_Info = 11101,

        #endregion

        #region cpNet package

        [Detail("CPNet Interface 未安装")]
        [RecommendedMeasures("以<修改>方式运行FDAA安装程序，安装CPNet采集接口至当前系统")]
        CPNetInterface_NotInstalled_Error = 12001,

        [Detail("CPNet Interface 未授权")]
        [RecommendedMeasures("安装授权")]
        CPNetInterface_NotLicensed_Error = 12002,

        [Detail("CPNet Interface FAU设备无法访问")]
        [RecommendedMeasures("1.检查IP地址是否配置正确&2.检查网络连接是否畅通&3.将FAU设备断电，重新启动")]
        CPNetInterface_FauDeviceIpNotPingable_Error = 12003,

        [Detail("CPNet Interface 模块所有信号采集不到数据")]
        [RecommendedMeasures("检查FAU固件类型和版本是否正确，其授权状态和运行状态是否正常")]
        CPNetInterface_NotReceivedData_Error = 12004,

        [Detail("CPNet Interface 已安装")] [RecommendedMeasures("")]
        CPNetInterface_Installed_Info = 12100,

        [Detail("CPNet Interface 已授权")] [RecommendedMeasures("")]
        CPNetInterface_Licensed_Info = 12101,

        #endregion

        #region tcNet package

        [Detail("TCNet Interface 未安装")]
        [RecommendedMeasures("以<修改>方式运行FDAA安装程序，安装TCNet采集接口至当前系统")]
        TCNetInterface_NotInstalled_Error = 13001,

        [Detail("TCNet Interface 未授权")]
        [RecommendedMeasures("安装授权")]
        TCNetInterface_NotLicensed_Error = 13002,

        [Detail("TCNet Interface FAU设备无法访问")]
        [RecommendedMeasures("1.检查IP地址是否配置正确&2.检查网络连接是否畅通&3.将FAU设备断电，重新启动")]
        TCNetInterface_FauDeviceIpNotPingable_Error = 13003,

        [Detail("TCNet Interface 模块所有信号采集不到数据")]
        [RecommendedMeasures("检查FAU固件类型和版本是否正确，其授权状态和运行状态是否正常")]
        TCNetInterface_NotReceivedData_Error = 13004,

        [Detail("TCNet Interface 已安装")] [RecommendedMeasures("")]
        TCNetInterface_Installed_Info = 13100,

        [Detail("TCNet Interface 已授权")] [RecommendedMeasures("")]
        TCNetInterface_Licensed_Info = 13101,

        #endregion

        #region µΣNetwork package

        [Detail("µΣ-Network Interface 未安装")]
        [RecommendedMeasures("以<修改>方式运行FDAA安装程序，安装µΣ-Network采集接口至当前系统")]
        UsigmaInterface_NotInstalled_Error = 14001,

        [Detail("µΣ-Network Interface 未授权")]
        [RecommendedMeasures("安装授权")]
        UsigmaInterface_NotLicensed_Error = 14002,

        [Detail("µΣNetwork Interface FAU设备无法访问")]
        [RecommendedMeasures("1.检查IP地址是否配置正确&2.检查网络连接是否畅通&3.将FAU设备断电，重新启动")]
        UsigmaInterface_FauDeviceIpNotPingable_Error = 14003,

        [Detail("µΣ-Network Interface 模块所有信号采集不到数据")]
        [RecommendedMeasures("检查FAU固件类型和版本是否正确，其授权状态和运行状态是否正常")]
        UsigmaInterface_NotReceivedData_Error = 14004,

        [Detail("µΣ-Network Interface 已安装")] [RecommendedMeasures("")]
        UsigmaInterface_Installed_Info = 14100,

        [Detail("µΣ-Network Interface 已授权")] [RecommendedMeasures("")]
        UsigmaInterface_Licensed_Info = 14101,

        #endregion

        #region nisdas package

        [Detail("NISDAS Interface 未安装")]
        [RecommendedMeasures("以<修改>方式运行FDAA安装程序，安装NISDAS采集接口至当前系统")]
        NISDASInterface_NotInstalled_Error = 15001,

        [Detail("NISDAS Interface 未授权")]
        [RecommendedMeasures("安装授权")]
        NISDASInterface_NotLicensed_Error = 15002,

        [Detail("NISDAS Interface FAU设备无法访问")]
        [RecommendedMeasures("1.检查IP地址是否配置正确&2.检查网络连接是否畅通&3.将FAU设备断电，重新启动")]
        NISDASInterface_FauDeviceIpNotPingable_Error = 15003,

        [Detail("NISDAS Interface 模块所有信号采集不到数据")]
        [RecommendedMeasures("检查FAU固件类型和版本是否正确，其授权状态和运行状态是否正常")]
        NISDASInterface_NotReceivedData_Error = 15004,

        [Detail("NISDAS Interface 已安装")] [RecommendedMeasures("")]
        NISDASInterface_Installed_Info = 15100,

        [Detail("NISDAS Interface 已授权")] [RecommendedMeasures("")]
        NISDASInterface_Licensed_Info = 15101,

        #endregion

        #region sse package

        [Detail("SSE Interface 未安装")]
        [RecommendedMeasures("以<修改>方式运行FDAA安装程序，安装SSE采集接口至当前系统")]
        SSEInterface_NotInstalled_Error = 16001,

        [Detail("SSE Interface 未授权")]
        [RecommendedMeasures("安装授权")]
        SSEInterface_NotLicensed_Error = 16002,

        [Detail("SSE Interface FAU设备无法访问")]
        [RecommendedMeasures("1.检查IP地址是否配置正确&2.检查网络连接是否畅通&3.将FAU设备断电，重新启动")]
        SSEInterface_FauDeviceIpNotPingable_Error = 16003,

        [Detail("SSE Interface 模块所有信号采集不到数据")]
        [RecommendedMeasures("检查FAU固件类型和版本是否正确，其授权状态和运行状态是否正常")]
        SSEInterface_NotReceivedData_Error = 16004,

        [Detail("SSE Interface 已安装")] [RecommendedMeasures("")]
        SSEInterface_Installed_Info = 16100,

        [Detail("SSE Interface 已授权")] [RecommendedMeasures("")]
        SSEInterface_Licensed_Info = 16101,

        #endregion

        #region videoCapture package

        [Detail("VideoCapture Interface 未安装")]
        [RecommendedMeasures("以<修改>方式运行FDAA安装程序，安装VideoCapture采集接口至当前系统")]
        VideoCaptureInterface_NotInstalled_Error = 17001,

        [Detail("VideoCapture Interface 未授权")]
        [RecommendedMeasures("安装授权")]
        VideoCaptureInterface_NotLicensed_Error = 17002,

        [Detail("VideoCapture Interface IPC设备无法访问")]
        [RecommendedMeasures("1.检查IP地址是否配置正确&2.检查网络连接是否畅通&3.将IPC设备断电，重新启动")]
        VideoCaptureInterface_DeviceIpNotPingable_Error = 17003,

        [Detail("VideoCapture Interface IPC设备无信号")]
        [RecommendedMeasures("重启FDAA和IPC设备")]
        VideoCaptureInterface_PartialNotReceivedData_Error = 17004,

        [Detail("VideoCapture Interface 已安装")] [RecommendedMeasures("")]
        VideoCaptureInterface_Installed_Info = 17100,

        [Detail("VideoCapture Interface 已授权")] [RecommendedMeasures("")]
        VideoCaptureInterface_Licensed_Info = 17101,

        #endregion

        #region technostring package

        [Detail("Technostring Interface 未安装")]
        [RecommendedMeasures("以<修改>方式运行FDAA安装程序，安装Technostring采集接口至当前系统")]
        TechnostringInterface_NotInstalled_Error = 18001,

        [Detail("Technostring Interface 未授权")]
        [RecommendedMeasures("安装授权")]
        TechnostringInterface_NotLicensed_Error = 18002,

        [Detail("Technostring Interface 模块所有信号采集不到数据")]
        [RecommendedMeasures("检查通讯是否正常")]
        TechnostringInterface_NotReceivedData_Error = 18003,

        [Detail("Technostring Interface 模块部分信号采集不到数据")]
        [RecommendedMeasures("检查字符串截取是否正确")]
        TechnostringInterface_PartialNotReceivedData_Error = 18004,

        [Detail("Technostring Interface 已安装")] [RecommendedMeasures("")]
        TechnostringInterface_Installed_Info = 18100,

        [Detail("Technostring Interface 已授权")] [RecommendedMeasures("")]
        TechnostringInterface_Licensed_Info = 18101,

        #endregion

        #region playback package

        [Detail("Playback Interface 未安装")]
        [RecommendedMeasures("以<修改>方式运行FDAA安装程序，安装Playback采集接口至当前系统")]
        PlaybackInterface_NotInstalled_Error = 19001,

        [Detail("Playback Interface 未授权")]
        [RecommendedMeasures("安装授权")]
        PlaybackInterface_NotLicensed_Error = 19002,

        [Detail("Playback Interface 模块所有信号采集不到数据")]
        [RecommendedMeasures("数据文件在磁盘上是否存在")]
        PlaybackInterface_NotReceivedData_Error = 19003,

        [Detail("Playback Interface 已安装")] [RecommendedMeasures("")]
        PlaybackInterface_Installed_Info = 19100,

        [Detail("Playback Interface 已授权")] [RecommendedMeasures("")]
        PlaybackInterface_Licensed_Info = 19101,

        #endregion

        #region fdaa package

        [Detail("FDAA Interface 未安装")]
        [RecommendedMeasures("以<修改>方式运行FDAA安装程序，安装FDAA采集接口至当前系统")]
        FDAAInterface_NotInstalled_Error = 20001,

        [Detail("FDAA Interface 未授权")]
        [RecommendedMeasures("安装授权")]
        FDAAInterface_NotLicensed_Error = 20002,

        [Detail("FDAA Interface PMS采集服务无法访问")]
        [RecommendedMeasures("1.检查IP地址是否配置正确&2.检查网络连接是否畅通")]
        FDAAInterface_PmsServiceIpNotPingable_Error = 20003,

        [Detail("FDAA 服务未就绪")] 
        [RecommendedMeasures("1.检查端口号是否配置正确&2.启动数据源侧FDAA采集服务")]
        FDAAInterface_PmsServicePortNotConnectable_Error = 20004,

        [Detail("FDAA Interface 模块所有信号采集不到数据")]
        [RecommendedMeasures("1.数据源侧FDAA对应模块是否存在并处于启用状态&2.检查数据源侧FDAA本地采集是否正常")]
        FDAAInterface_NotReceivedData_Error = 20005,

        [Detail("FDAA Interface 模块部分信号采集不到数据")]
        [RecommendedMeasures("1.检查异常信号在数据源侧的FDAA中是否存在并启用&2.检查数据源侧FDAA数据信号是否正常采集")]
        FDAAInterface_PartialNotReceivedData_Error = 20006,

        [Detail("FDAA Interface 已安装")] [RecommendedMeasures("")]
        FDAAInterface_Installed_Info = 20100,

        [Detail("FDAA Interface 已授权")] [RecommendedMeasures("")]
        FDAAInterface_Licensed_Info = 20101,

        #endregion

        #region fdaaOnline package

        [Detail("FDAAOnline 加载ClientConfig.xml配置文件失败")]
        [RecommendedMeasures("以<修复>方式运行FDAA安装程序，安装FDAA Online至当前系统")]
        FDAAOnline_ConfigCorrupted_Error = 21001,

        [Detail("FDAAOnline 未安装")] 
        [RecommendedMeasures("")]
        FDAAOnline_NotInstalled_Error = 21002,

        [Detail("FDAAOnline 已安装")] [RecommendedMeasures("")]
        FDAAOnlie_Installed_Info = 21100,

        #endregion

        #region basic data storage package

        [Detail("Basic Data Storage 基础目录所在磁盘逻辑驱动器不存在")]
        [RecommendedMeasures("更换基础目录")]
        BasicDataStorage_CacheDiscNotExist_Error = 22001,

        [Detail("Basic Data Storage 基础目录所在磁盘空间不够")]
        [RecommendedMeasures("1.更换基础目录&2.清理基础目录所在磁盘空间")]
        BasicDataStorage_CacheDiscOutOfSpace_Error = 22002,

        #endregion

        #region advanced data storage package

        [Detail("Advanced Data Storage 基础目录所在磁盘逻辑驱动器不存在")]
        [RecommendedMeasures("更换基础目录")]
        AdvancedDataStorage_CacheDiscNotExist_Error = 23001,

        [Detail("Advanced Data Storage 基础目录所在磁盘空间不够")]
        [RecommendedMeasures("1.更换基础目录&2.清理基础目录所在磁盘空间")]
        AdvancedDataStorage_CacheDiscOutOfSpace_Error = 23002,

        #endregion

        #region hd data storage package

        [Detail("VC++ 运行环境未安装")]
        [RecommendedMeasures("执行vc++Runtime安装程序vc10redist_x86.exe，位于CD包DotNetFx目录下")]
        HD_VcNotInstalled_Error = 24001,

        [Detail("HD Data Storage 未启用")]
        [RecommendedMeasures("打开IOConfig=>更多配置=>数据存储=>选中HD数据存储节点=>勾选启用")]
        HD_NotActive_Error = 24002,

        [Detail("HD Data Storage 缓存目录所在磁盘逻辑驱动器不存在")]
        [RecommendedMeasures("更换缓存目录")]
        HD_CacheDiscNotExist_Error = 24003,

        [Detail("HD Data Storage 缓存目录所在磁盘空间不够")]
        [RecommendedMeasures("1.更换缓存目录&2.清理缓存目录所在磁盘空间")]
        HD_CacheDiscOutOfSpace_Error = 24004,

        [Detail("HD Server 连接失败")]
        [RecommendedMeasures("1.检查HD连接参数，IP地址和端口号是否配置正确&2.参考HD手册中的“常见错误代码表”")]
        HD_SererConnectionFailed_Error = 24005,

        [Detail("HD 数据访问异常")] 
        [RecommendedMeasures("1.PMS采集服务是否在HD中创建了对应的连接测试点&2.HD数据节点状态是否正常")]
        HD_DataAccess_Error = 24006,

        [Detail("HD 数据写入时间戳偏差过大(3秒)")] 
        [RecommendedMeasures("配置FDAA与HD Server的时钟同步，参见HD用户手册FAQ2")]
        HD_TimestampDeviationTooLarge_Error = 24007,

        #endregion

        #region mqtt data storage package

        [Detail("MQTT Data Storage 缓存目录所在磁盘逻辑驱动器不存在")]
        [RecommendedMeasures("更换缓存目录")]
        MQTT_CacheDiscNotExist_Error = 25001,

        [Detail("MQTT Data Storage 缓存目录所在磁盘空间不够")]
        [RecommendedMeasures("1.更换缓存目录&2.清理缓存目录所在磁盘空间")]
        MQTT_CacheDiscOutOfSpace_Error = 25002,

        [Detail("MQTT Broker 连接失败")]
        [RecommendedMeasures("1.检查IP地址是否配置正确&2.检查网络连接是否畅通")]
        MQTT_SererConnectionFailed_Error = 25003,

        [Detail("MQTT 数据访问异常")]
        [RecommendedMeasures("")]
        MQTT_DataAccess_Error = 25004,

        #endregion
    }
}