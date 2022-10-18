using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Baosight.FDAA.PackageDiagnosis.DAL;
using Baosight.FDAA.PackageDiagnosis.Model.Constant;
using Baosight.FDAA.PackageDiagnosis.Model.Entities;
using Baosight.FDAA.PackageDiagnosis.Model.Enums;
using Baosight.FDAA.PackageDiagnosis.Model.Modules;

namespace Baosight.FDAA.PackageDiagnosis.BLL.Packages
{
    public class UdpPackage : BasePackage, IInstallation, ILicense
    {
        #region Event

        public override event EventHandler<StatusEventArgs> StatusEvent;

        #endregion

        #region Field

        private bool isPassed;

        private readonly UdpProcessor udpProcessor = new UdpProcessor();

        #endregion

        #region Properties

        public ObservableCollection<UDPUnicastModule> UdpUnicastModules { get; set; }

        public ObservableCollection<UDPMulticastModule> UdpMulticastModules { get; set; }

        /// <summary>
        ///     功能模块名称
        /// </summary>
        public override string PackageName
        {
            get { return Constants.UDP_PACKAGE_NAME; }
        }

        /// <summary>
        ///     当前功能模块是否配置
        /// </summary>
        public override bool IsConfigured
        {
            get
            {
                var signalNumber = UdpUnicastModules.Sum(udpUnicastModule =>
                                       udpUnicastModule.AnalogDataTable.Rows.Count +
                                       udpUnicastModule.DigitalDataTable.Rows.Count) +
                                    UdpMulticastModules.Sum(udpMulticastModule =>
                                       udpMulticastModule.AnalogDataTable.Rows.Count +
                                       udpMulticastModule.DigitalDataTable.Rows.Count);

                return signalNumber != 0;
            }
        }

        /// <summary>
        ///     当前功能模块诊断结果是否通过
        /// </summary>
        public override bool IsPassed
        {
            get { return isPassed; }
        }

        /// <summary>
        ///     当前功能模块是否授权
        /// </summary>
        public bool IsLicensed { get; private set; }

        /// <summary>
        ///     当前功能模块是否安装
        /// </summary>
        public bool IsInstalled { get; private set; }

        #endregion

        #region Method

        /// <summary>
        ///     诊断当前功能模块
        /// </summary>
        /// <returns>结果码集合</returns>
        public override List<CodeInfo> Diagnose()
        {
            DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.Diagnosing, out isPassed);

            var codeInfos = new List<CodeInfo>();

            #region Step 1

            //  步骤一：检查是否安装
            var installStepIsPassed =
                DiagnosticHelper.CheckStep(codeInfos, CheckInstallation);

            //  如果服务不存在则中止检查
            PmsServer.CreateInstance().InitPmsService();
            if (PmsServer.CreateInstance().PmsService == null)
            {
                if (installStepIsPassed)
                {
                    DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.OK, out isPassed);
                    return codeInfos;
                }

                DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.NotPassed, out isPassed);
                return codeInfos;
            }

            //  步骤一：检查是否授权
            var licenseStepIsPass =
                DiagnosticHelper.CheckStep(codeInfos, CheckLicense);

            //  步骤一不通过则中止检查
            if (!(licenseStepIsPass && installStepIsPassed))
            {
                DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.NotPassed, out isPassed);
                return codeInfos;
            }

            #endregion

            #region Step 2

            //  步骤二：检查所有模块的UDP端口是否绑定成功
            var portBindStepIsPassed = DiagnosticHelper.CheckStep(codeInfos, CheckBindPort);
            //  步骤二如果不通过则退出检查
            if (!portBindStepIsPassed)
            {
                DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.NotPassed, out isPassed);
                return codeInfos;
            }

            #endregion

            #region Step 3

            //  步骤三：检查所有模块的UDP端口是否可以接受数据
            var portStepIsPassed = DiagnosticHelper.CheckStep(codeInfos, CheckPort);
            //  步骤三如果不通过则退出检查
            if (!portStepIsPassed)
            {
                DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.NotPassed, out isPassed);
                return codeInfos;
            }

            #endregion

            #region Step 4

            //  步骤四：检查所有模块的UDP端口是否可以接受数据
            var sourceAddressStepIsPassed = DiagnosticHelper.CheckStep(codeInfos, CheckMulticastSourceAddress);
            //  步骤四如果不通过则退出检查
            if (!sourceAddressStepIsPassed)
            {
                DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.NotPassed, out isPassed);
                return codeInfos;
            }

            #endregion

            #region Step 5

            //  步骤五：检查是否模块接收的报文是否匹配
            var telegramStepIsPassed = DiagnosticHelper.CheckStep(codeInfos, CheckTelegram);
            //  步骤五如果不通过则退出检查
            if (!telegramStepIsPassed)
            {
                DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.NotPassed, out isPassed);
                return codeInfos;
            }

            #endregion

            #region Step 6

            //  步骤六：检查是否某个模块全部信号接收不到数据
            var receivedStepIsPassed = DiagnosticHelper.CheckStep(codeInfos, CheckDataReceived);

            //  步骤六如果不通过则退出检查
            if (!receivedStepIsPassed)
            {
                DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.NotPassed, out isPassed);
                return codeInfos;
            }

            #endregion

            DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.OK, out isPassed);

            return codeInfos;
        }


        /// <summary>
        ///     检查安装
        /// </summary>
        /// <returns>诊断码</returns>
        public CodeInfo CheckInstallation()
        {
            var installationCodeInfo = DiagnosticHelper.CheckInstallation(Constants.UDP_DRIVER_NAME,
                Code.UDPInterface_Installed_Info, Code.UDPInterface_NotInstalled_Error);
            IsInstalled = installationCodeInfo.Id == 0;
            return installationCodeInfo;
        }

        /// <summary>
        ///     检查授权
        /// </summary>
        /// <returns>诊断码</returns>
        public CodeInfo CheckLicense()
        {
            var licenseCodeInfo = DiagnosticHelper.CheckLicense(Constants.UDP_DRIVER_NAME,
                Code.UDPInterface_Licensed_Info, Code.UDPInterface_NotLicensed_Error);
            IsLicensed = licenseCodeInfo.Id == 0;
            return licenseCodeInfo;
        }

        /// <summary>
        ///     检查模块内全部信号接受数据
        /// </summary>
        /// <returns>诊断码</returns>
        private CodeInfo CheckDataReceived()
        {
            var receivedCodeInfo =
                DiagnosticHelper.CheckReceivedData(Code.UDPInterface_NotReceivedData_Error, UdpUnicastModules);
            return receivedCodeInfo;
        }

        /// <summary>
        ///     检查所有模块的UDP端口是否可以绑定
        /// </summary>
        /// <returns>结果码</returns>
        private CodeInfo CheckBindPort()
        {
            //  集合是否存在绑定失败的端口 false 不存在 true存在
            var result = false;
            //  获取所有端口绑定失败的模块，端口号Dictionary
            var notBindableModulePort = GetBindFailureModulePort(UdpUnicastModules, ref result);
            if (result)
                return DiagnosticHelper.MakeCodeInfo(Code.UDPInterface_PortBindFailure_Error,
                    string.Join(",", notBindableModulePort));

            return new CodeInfo();
        }

        /// <summary>
        ///     获取绑定失败的Dictionary端口集合 名字格式key moduleNo value port
        /// </summary>
        /// <param name="modules">UDP模块集合</param>
        /// <param name="result">模块集合是否存在绑定失败的端口 false 不存在 true存在</param>
        /// <returns></returns>
        private Dictionary<uint, int> GetBindFailureModulePort(ObservableCollection<UDPUnicastModule> modules,
            ref bool result)
        {
            var modulePort = new Dictionary<uint, int>();
            var tasks = new List<Task>();

            foreach (var port in modules.Select(m => m.Port).Distinct())
                //  当前端口是否可以接收数据
                tasks.Add(Task.Run(() => udpProcessor.CheckPortBindable(port)));

            Task.WaitAll();

            foreach (var task in tasks)
            {
                var portIsBindable = ((Task<Tuple<int, bool>>) task).Result;
                if (portIsBindable.Item2) continue;
                //  存在绑定失败的端口
                result = true;
                //  获取绑定失败端口下的所有模块
                var notBindableModules = modules.Where(m => m.Port == portIsBindable.Item1);
                //  所有绑定失败的[模块,端口号]组织好
                foreach (var module in notBindableModules) modulePort.Add(module.ModuleNo, module.Port);
            }

            return modulePort;
        }

        /// <summary>
        ///     检查所有单播模块的UDP端口是否可以接受数据
        /// </summary>
        /// <returns>结果码</returns>
        private CodeInfo CheckPort()
        {
            //  服务集合是否存在接受不到数据的模块 false 不存在 true存在
            var result = false;

            //  获取所有接受不到数据的模块，端口号Dictionary
            var notReceivedDataModulePort = GetNotReceivedDataModulePort(UdpUnicastModules, ref result);

            if (result)
                return DiagnosticHelper.MakeCodeInfo(Code.UDPInterface_PortNotReceivedData_Error,
                    string.Join(",", notReceivedDataModulePort));

            return new CodeInfo();
        }

        /// <summary>
        ///     检查所有多播模块的源地址，目标端口是否可以接受数据
        /// </summary>
        /// <returns>结果码</returns>
        private CodeInfo CheckMulticastSourceAddress()
        {
            //  服务集合是否存在接受不到数据的多播模块 false 不存在 true存在
            var result = false;

            //  获取所有接受不到数据的模块，端口号Dictionary
            var notReceivedDataModulePort = GetNotReceivedDataModuleSourceAddressPort(UdpMulticastModules, ref result);

            if (result)
                return DiagnosticHelper.MakeCodeInfo(Code.UDPInterface_SourceAddressDestinationPortNotReceivedData_Error,
                    string.Join(",", notReceivedDataModulePort));

            return new CodeInfo();
        }

        /// <summary>
        ///     获取接收不到数据的Dictionary端口集合 名字格式key moduleNo value port
        /// </summary>
        /// <param name="modules">UDP模块集合</param>
        /// <param name="result">服务集合是否存接收不到数据的端口号 false 不存在 true存在</param>
        /// <returns></returns>
        private Dictionary<uint, int> GetNotReceivedDataModulePort(ObservableCollection<UDPUnicastModule> modules,
            ref bool result)
        {
            var modulePort = new Dictionary<uint, int>();
            var tasks = new List<Task>();

            foreach (var port in modules.Select(m => m.Port).Distinct())
                //  当前端口是否可以接收数据
                tasks.Add(Task.Run(() => udpProcessor.CheckIsReceiveData(port)));

            Task.WaitAll();

            foreach (var task in tasks)
            {
                var portIsReceived = ((Task<Tuple<int, bool>>) task).Result;
                if (portIsReceived.Item2) continue;
                //  存在接受不到数据的端口
                result = true;
                //  获取接受不到数据端口下的所有模块
                var notReceivedModules = modules.Where(m => m.Port == portIsReceived.Item1);
                //  所有接受不到数据的[模块,端口号]组织好
                foreach (var module in notReceivedModules) modulePort.Add(module.ModuleNo, module.Port);
            }

            return modulePort;
        }

        /// <summary>
        ///     获取接收不到数据的Dictionary端口集合 名字格式key moduleNo value item1 sourceAddress item2 MulticastPort
        /// </summary>
        /// <param name="modules">UDP多播模块集合</param>
        /// <param name="result">服务集合是否存接收不到数据的端口号 false 不存在 true存在</param>
        /// <returns></returns>
        private Dictionary<uint, Tuple<string,int>> GetNotReceivedDataModuleSourceAddressPort(ObservableCollection<UDPMulticastModule> modules,
            ref bool result)
        {
            var notReceivedDataModuleSourceAddressPort = new Dictionary<uint, Tuple<string, int>>();
            var tasks = new List<Task>();

            //  获取不重复的源地址，端口号集合
            var addressPorts = modules.GroupBy(m => new
            {
                m.MulticastAddress, m.MulticastPort
            }).Select(g => new {g.Key.MulticastAddress, g.Key.MulticastPort});

            foreach (var addressPort in addressPorts)
                //  当前端口是否可以接收数据
                tasks.Add(Task.Run(() => udpProcessor.CheckIsReceiveData(addressPort.MulticastAddress,addressPort.MulticastPort)));

            Task.WaitAll();

            foreach (var task in tasks)
            {
                var portIsReceived = ((Task<Tuple<Tuple<string, int>, bool>>)task).Result;
                if (portIsReceived.Item2) continue;
                //  存在接受不到数据的端口
                result = true;
                //  获取接受不到数据端口下的所有模块
                var notReceivedModules = modules.Where(m =>
                    m.MulticastAddress == portIsReceived.Item1.Item1 && m.MulticastPort == portIsReceived.Item1.Item2);

                //  所有接受不到数据的[模块,源地址、端口号]组织好
                foreach (var module in notReceivedModules)
                    notReceivedDataModuleSourceAddressPort.Add(module.ModuleNo,
                        new Tuple<string, int>(module.MulticastAddress, module.MulticastPort));
            }

            return notReceivedDataModuleSourceAddressPort;
        }

        /// <summary>
        ///     检查UDP报文信息是否
        /// </summary>
        /// <returns>结果码</returns>
        private CodeInfo CheckTelegram()
        {
            //  服务集合是否存在报文不匹配的模块 false 不存在 true存在
            var result = false;

            //  获取所有不匹配的模块号 错误信息
            var mismatchModuleNoError = GetMismatchModuleNoErrors(UdpUnicastModules, ref result);

            if (result)
                return DiagnosticHelper.MakeCodeInfo(Code.UDPInterface_MessageMismatch_Error,
                    string.Join(",", mismatchModuleNoError));

            return new CodeInfo();
        }

        /// <summary>
        ///     获取报文不匹配不匹配的key:模块号 value:错误信息
        /// </summary>
        /// <param name="modules">UDP模块集合</param>
        /// <param name="result">是否存在 存在不匹配的报文True 不存在False</param>
        /// <returns>key:模块号 value:错误信息</returns>
        public Dictionary<uint, string> GetMismatchModuleNoErrors(ObservableCollection<UDPUnicastModule> modules,
            ref bool result)
        {
            var moduleErrors = new Dictionary<uint, string>();

            //  获取key:端口号 value:报文集合
            var portTelegrams = GetPortTelegrams(modules);

            foreach (var module in modules)
            {
                var port = module.Port;
                var portTelegram = portTelegrams[port];

                //  是否存在网络字节序错误
                var netWorkByteOrder = CheckNetworkByteOrder(module, portTelegram);

                if (netWorkByteOrder)
                {
                    moduleErrors.Add(module.ModuleNo, "网络字节序设置错误");
                    result = true;
                    continue;
                }

                var telegram = GetOneMatchedTelegram(module, portTelegram);

                //  如果找到证明报文号一致
                if (telegram == null)
                {
                    moduleErrors.Add(module.ModuleNo,
                        string.Format("{0}端口未接收到报文号为{1}的报文", module.Port, module.ModuleIndex));
                    result = true;
                    continue;
                }

                var telegramLengthMatch =
                    telegram.ActualLength == telegram.Length && telegram.Length == module.TelegramLength;
                if (telegramLengthMatch) continue;
                //  如果不存在
                if (moduleErrors.ContainsKey(module.ModuleNo)) continue;
                moduleErrors.Add(module.ModuleNo,
                    string.Format("实际报文长度{0}，报文携带长度{1}，预期报文长度{2}", telegram.ActualLength, telegram.Length,
                        module.TelegramLength));
                result = true;
            }

            return moduleErrors;
        }

        /// <summary>
        ///     检查模块内是否存在大小端错误
        ///     如果从大端小端集合中没有找到对应报文则返回没有大小端错误
        ///     IO设置小端从小端集合中找到对应报文则返回没有网络字节序错误
        ///     IO设置小端从大端集合中找到对应报文则返回有网络字节序错误
        ///     IO设置大端从大端集合中找到对应报文则返回没有网络字节序错误
        ///     IO设置大端从小端集合中找到对应报文则返回有网络字节序错误
        ///     IO设置AutoDetect一直返回没有网络字节序错误
        /// </summary>
        /// <param name="module"></param>
        /// <param name="portTelegram"></param>
        /// <returns></returns>
        private bool CheckNetworkByteOrder(UDPUnicastModule module, List<Tuple<Telegram, Telegram>> portTelegram)
        {
            Telegram telegram;

            switch (module.Nbo)
            {
                //  从报文中取一条报文号一致的报文
                case "Little Endian":
                {
                    var telegramLittle
                        = portTelegram.FirstOrDefault(t => t.Item1.ModuleIndex == module.ModuleIndex);
                    telegram = telegramLittle == null ? null : telegramLittle.Item1;
                    //  如果找到了一致的报文头，则返回没有网络字节序错误
                    if (telegram != null) return false;
                    //  没找到则检查大端是否匹配
                    var telegramBig
                        = portTelegram.FirstOrDefault(t => t.Item2.ModuleIndex == module.ModuleIndex);
                    telegram = telegramBig == null ? null : telegramBig.Item2;
                    //  本来应该找到小端，但是从大端找到，返回有网络字节序错误
                    return telegram != null;
                }
                case "Big Endian":
                {
                    var telegramLittle
                        = portTelegram.FirstOrDefault(t => t.Item2.ModuleIndex == module.ModuleIndex);
                    telegram = telegramLittle == null ? null : telegramLittle.Item2;
                    //  如果找到了一致的报文头，则返回没有网络字节序错误
                    if (telegram != null) return false;
                    //  没找到则检查大端是否匹配
                    var telegramBig
                        = portTelegram.FirstOrDefault(t => t.Item1.ModuleIndex == module.ModuleIndex);
                    telegram = telegramBig == null ? null : telegramBig.Item1;
                    //  本来应该找到大端，但是从小端找到，返回有网络字节序错误
                    return telegram != null;
                }
                default:
                {
                    //  IO设置AutoDetect一直返回没有网络字节序错误

                    var telegramLittle
                        = portTelegram.FirstOrDefault(t => t.Item1.ModuleIndex == module.ModuleIndex);
                    telegram = telegramLittle == null ? null : telegramLittle.Item1;
                    //  如果小端找到了一致的报文头，设置此模块大小端为小端
                    if (telegram != null)
                    {
                        module.Nbo = "Little Endian";
                        return false;
                    }

                    //  没找到则检查大端是否匹配
                    var telegramBig
                        = portTelegram.FirstOrDefault(t => t.Item2.ModuleIndex == module.ModuleIndex);
                    telegram = telegramBig == null ? null : telegramBig.Item2;
                    //  如果大端找到了一致的报文头，设置此模块大小端为大端
                    if (telegram != null)
                    {
                        module.Nbo = "Big Endian";
                        return false;
                    }

                    return false;
                }
            }
        }

        /// <summary>
        ///     从解析出的小端大端集合中选出一条与当前模块的报文号一致的报文
        /// </summary>
        /// <param name="module">单个UDP模块</param>
        /// <param name="portTelegram">大端小端报文对集合</param>
        /// <returns></returns>
        private Telegram GetOneMatchedTelegram(UDPUnicastModule module, IEnumerable<Tuple<Telegram, Telegram>> portTelegram)
        {
            Telegram telegram;

            if (module.Nbo == "Little Endian")
                //  从报文中取一条报文号一致的报文
            {
                var telegramTuple
                    = portTelegram.FirstOrDefault(t => t.Item1.ModuleIndex == module.ModuleIndex);
                telegram = telegramTuple == null ? null : telegramTuple.Item1;
            }
            else if (module.Nbo == "Big Endian")
            {
                var telegramTuple
                    = portTelegram.FirstOrDefault(t => t.Item2.ModuleIndex == module.ModuleIndex);
                telegram = telegramTuple == null ? null : telegramTuple.Item2;
            }
            else
            {
                if (BitConverter.IsLittleEndian)
                {
                    var telegramTuple
                        = portTelegram.FirstOrDefault(t => t.Item1.ModuleIndex == module.ModuleIndex);
                    telegram = telegramTuple == null ? null : telegramTuple.Item1;
                }
                else
                {
                    var telegramTuple
                        = portTelegram.FirstOrDefault(t => t.Item2.ModuleIndex == module.ModuleIndex);
                    telegram = telegramTuple == null ? null : telegramTuple.Item2;
                }
            }

            return telegram;
        }

        /// <summary>
        ///     获取某个端口号接收数据的集合
        /// </summary>
        /// <param name="modules">模块集合</param>
        /// <returns>key:端口号 value:报文集合</returns>
        private Dictionary<int, List<Tuple<Telegram, Telegram>>> GetPortTelegrams(IEnumerable<UDPUnicastModule> modules)
        {
            //  key 端口号，Value解析出的报文集合
            var portTelegrams = new Dictionary<int, List<Tuple<Telegram, Telegram>>>();
            var tasks = new List<Task>();
            foreach (var port in modules.Select(m => m.Port).Distinct())
                //  当前端口是否可以接收数据
                tasks.Add(Task.Run(() => udpProcessor.ReceiveData(port)));

            Task.WaitAll(tasks.ToArray());
            foreach (var task in tasks)
            {
                var result = ((Task<Tuple<int, List<Tuple<Telegram, Telegram>>>>) task).Result;
                portTelegrams.Add(result.Item1, result.Item2);
            }

            return portTelegrams;
        }

        #endregion
    }
}