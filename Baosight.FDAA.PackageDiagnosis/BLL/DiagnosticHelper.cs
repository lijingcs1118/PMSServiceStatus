using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Autofac;
using Baosight.FDAA.PackageDiagnosis.DAL;
using Baosight.FDAA.PackageDiagnosis.DAL.Configs;
using Baosight.FDAA.PackageDiagnosis.Model.Attributes;
using Baosight.FDAA.PackageDiagnosis.Model.Entities;
using Baosight.FDAA.PackageDiagnosis.Model.Enums;
using Baosight.FDAA.PackageDiagnosis.Model.Modules;

namespace Baosight.FDAA.PackageDiagnosis.BLL
{
    public class DiagnosticHelper
    {
        /// <summary>
        ///     返回单一错误码不能包含可能出现具体错误信息，封装返回给调用者
        /// </summary>
        /// <param name="code">结果码</param>
        /// <param name="specificInformation">检查当前模块详细信息结果</param>
        /// <returns>结果码具体信息</returns>
        public static CodeInfo MakeCodeInfo(Code code, string specificInformation)
        {
            var codeInfo = new CodeInfo
            {
                Id = code,
                Detail = code.GetAttribute<DetailAttribute>().Name,
                RecommendedMeasures = code.GetAttribute<RecommendedMeasuresAttribute>().Name,
                SpecificInformation = specificInformation
            };

            return codeInfo;
        }

        /// <summary>
        ///     错误码转换成 CodeInfo详细信息
        /// </summary>
        /// <param name="code">结果码</param>
        /// <returns>结果码具体信息</returns>
        public static CodeInfo MakeCodeInfo(Code code)
        {
            var codeInfo = new CodeInfo
            {
                Id = code,
                Detail = code.GetAttribute<DetailAttribute>().Name,
                RecommendedMeasures = code.GetAttribute<RecommendedMeasuresAttribute>().Name
            };

            return codeInfo;
        }

        /// <summary>
        ///     检查是否安装
        /// </summary>
        /// <returns>结果码</returns>
        public static CodeInfo CheckInstallation(string driverName, Code installedInfo, Code notInstalledError)
        {
            //  已安装
            if (FdaaHelper.CreateInstance().GetDriverInfo().Contains(driverName))
                return new CodeInfo();
            //  未安装
            return MakeCodeInfo(notInstalledError);
        }

        /// <summary>
        ///     检查当前功能模块是否授权
        /// </summary>
        /// <returns>结果码</returns>
        public static CodeInfo CheckLicense(string driverName, Code licenseInfo, Code notLicenseError)
        {
            var driver = PmsServer.CreateInstance().GetDriverInfo().FirstOrDefault(d => d.Name == driverName);
            if (driver != null)
                //  TODO
                if (driver.License == "OK")
                    return new CodeInfo();

            return MakeCodeInfo(notLicenseError);
        }

        /// <summary>
        ///     检查当前功能模块 某个模块所有信号接收不到数据
        /// </summary>
        /// <returns>结果码</returns>
        public static CodeInfo CheckReceivedData<T>(Code code, ObservableCollection<T> modules) where T : BaseModule
        {
            //  模块集合是否存在全是-99999的模块 false 不存在 true存在
            var result = false;
            //  全是-99999的模块号集合
            var notReceivedModuleNumbers = new List<uint>();
            foreach (var module in modules)
            {
                Dictionary<string, double> signalValues;
                //  获取所有值类型值
                if (module.MdType != ModuleType.mtVirtualforLuaTS && 
                    module.MdType != ModuleType.mtTsOPC &&
                    module.MdType != ModuleType.mtTsFAU && 
                    module.MdType != ModuleType.mtTsUDP)
                    signalValues = PmsServer.CreateInstance().GetSignalValues(Convert.ToUInt16(module.ModuleNo));
                //  获取所有Technostring类型值
                else
                    signalValues = PmsServer.CreateInstance().GetStringValues(Convert.ToUInt16(module.ModuleNo));
                //  当前模块所有值是否全部等于-99999
                //  如果集合为空，用ALL也会返回True
                if (signalValues.Count != 0)
                {
                    var notReceived = signalValues.All(valuePair => valuePair.Value == -99999);
                    if (notReceived)
                    {
                        result = true;
                        //  把所有值全部等于-99999的模块号加入集合
                        notReceivedModuleNumbers.Add(module.ModuleNo);
                    }
                }
            }

            if (result)
                return MakeCodeInfo(code,
                    string.Join(",", notReceivedModuleNumbers));
            return new CodeInfo();
        }

        /// <summary>
        ///     检查当前功能模块 某个模块个别信号接收不到数据
        /// </summary>
        /// <returns>结果码</returns>
        public static CodeInfo CheckPartialReceivedData<T>(Code code, IEnumerable<T> modules)
            where T : BaseModule
        {
            //  模块集合是否存在部分是-99999的模块 false 不存在 true存在
            var result = false;
            //  信号值为-99999的信号集合
            var notReceivedSignalAddresses = new List<string>();
            foreach (var module in modules)
            {
                //  获取当前模块所有的值
                Dictionary<string, double> signalValues;
                //  获取所有值类型值
                if (module.MdType != ModuleType.mtVirtualforLuaTS &&
                    module.MdType != ModuleType.mtTsOPC &&
                    module.MdType != ModuleType.mtTsFAU &&
                    module.MdType != ModuleType.mtTsUDP)
                    signalValues = PmsServer.CreateInstance().GetSignalValues(Convert.ToUInt16(module.ModuleNo));
                //  获取所有Technostring类型值
                else
                    signalValues = PmsServer.CreateInstance().GetStringValues(Convert.ToUInt16(module.ModuleNo));
                //  当前模块是否存在值为-99999的信号 
                var notReceived = signalValues.Any(valuePair => valuePair.Value == -99999) &&
                                  !signalValues.All(valuePair => valuePair.Value == -99999);

                if (notReceived)
                {
                    result = true;
                    //  把-99999的信号地址加入集合
                    notReceivedSignalAddresses.AddRange(signalValues
                        .Where(valuePair => valuePair.Value == -99999)
                        .ToDictionary(valuePair => valuePair.Key, dictionary => dictionary.Value).Keys);
                }
            }

            if (result)
                return MakeCodeInfo(code,
                    string.Join(",", notReceivedSignalAddresses));
            return new CodeInfo();
        }

        /// <summary>
        ///     检查磁盘是否存在
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="code">结果码></param>
        /// <returns>结果码</returns>
        public static CodeInfo CheckDiscIsExist(string path, Code code)
        {
            //  通过路径获取逻辑磁盘
            var drive = Path.GetPathRoot(path);
            //  检查磁盘是否存在
            var contains = Directory.GetLogicalDrives().Contains(drive.ToUpper());
            return !contains ? MakeCodeInfo(code) : new CodeInfo();
        }

        /// <summary>
        ///     检查磁盘空间是否足够
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="settingValue">设置的缓存空间 单位M</param>
        /// <param name="code">结果码</param>
        /// <returns>结果码</returns>
        public static CodeInfo CheckFreeSpace(string path, int settingValue, Code code)
        {
            //  通过路径获取逻辑磁盘
            var drive = Path.GetPathRoot(path);
            var driveInfo = new DriveInfo(drive);
            //  磁盘可用空间 单位M
            var freeSpace = driveInfo.TotalFreeSpace / 1024 / 1024;
            return freeSpace < settingValue ? MakeCodeInfo(code) : new CodeInfo();
        }

        /// <summary>
        ///     检查IP是否可以ping通
        /// </summary>
        /// <param name="ipAddress">ip地址</param>
        /// <param name="code">结果码</param>
        /// <returns>结果码</returns>
        public static CodeInfo CheckIpPingable(string ipAddress, Code code)
        {
            return NetworkHelper.PingHost(ipAddress) ? new CodeInfo() : MakeCodeInfo(code);
        }

        /// <summary>
        ///     检查板卡驱动是否安装
        /// </summary>
        /// <param name="cardDriverName">板卡驱动名称</param>
        /// <param name="code">结果码</param>
        /// <returns>结果码</returns>
        public static CodeInfo CheckCardDriverIsInstalled(string cardDriverName, Code code)
        {
            return !MachineHelper.CheckCardDriverIsInstalled(cardDriverName)
                ? MakeCodeInfo(code)
                : new CodeInfo();
        }

        /// <summary>
        ///     通知调用方状态发生改变
        /// </summary>
        /// <param name="statusEvent">EventHandler</param>
        /// <param name="diagnosticStatus">当前状态</param>
        /// <param name="isPassed">是否通过检查</param>
        public static void NotifyStatus(EventHandler<StatusEventArgs> statusEvent, DiagnosticStatus diagnosticStatus,
            out bool isPassed)
        {
            isPassed = diagnosticStatus == DiagnosticStatus.OK;
            var handler = statusEvent;
            if (handler != null) handler(null, new StatusEventArgs(diagnosticStatus));
        }

        /// <summary>
        ///     获取所有PING不通的FAU设备名字集合 名字格式FAU NO + FAU NAME + FAU Ip
        /// </summary>
        /// <param name="modules">带FAU的模块集合</param>
        /// <param name="result">服务集合是否存在Ip Ping不通的服务 false 不存在 true存在</param>
        /// <returns>所有PING不通的FAU设备名字集合</returns>
        public static IEnumerable<string> NotPingableFauNames<T>(ObservableCollection<T> modules, ref bool result)
            where T : ModuleWithFau
        {
            //  PING不通的FAU设备地址集合
            var notPingableNames = new List<string>();

            foreach (var fauDevice in HasSignalFauCollection(modules))
            {
                //  当前FAU设备IP是否可以ping通
                var pingable = NetworkHelper.PingHost(fauDevice.Value.Ip);
                if (pingable) continue;
                result = true;
                notPingableNames.Add("[" + fauDevice.Value.Ip +"]"); //fauDevice.Value.No + " " + fauDevice.Value.Name + ": " +
            }

            return notPingableNames;
        }

        /// <summary>
        ///     获取有信号的FAU设备集合
        /// </summary>
        /// <returns>FAU设备集合</returns>
        public static Dictionary<uint, FAUDevice> HasSignalFauCollection<T>(ObservableCollection<T> modules)
            where T : ModuleWithFau
        {
            return (from module in modules
                let activeSignals = module.AnalogDataTable.Rows.Count + module.DigitalDataTable.Rows.Count
                where activeSignals != 0
                select module).ToDictionary(module => module.FauDeviceDevice.No,
                module => ProgramModule.Container.Resolve<IOConfig>().FauDeviceManager
                    .FauCollection[module.FauDeviceDevice.No]);
        }

        /// <summary>
        ///     诊断单步
        /// </summary>
        /// <param name="codeInfos">诊断码集合</param>
        /// <param name="function">诊断逻辑</param>
        /// <returns>此步骤是否通过 True通过 False不通过</returns>
        public static bool CheckStep(ICollection<CodeInfo> codeInfos, Func<CodeInfo> function)
        {
            var ipCodeInfo = function.Invoke();
            codeInfos.Add(ipCodeInfo);
            var stepIsPassed = ipCodeInfo.Id == 0;
            return stepIsPassed;
        }

        /// <summary>
        ///     诊断单步 用于诊断安装和授权信息
        /// </summary>
        /// <param name="codeInfos">诊断码集合</param>
        /// <param name="function">诊断逻辑</param>
        /// <param name="code">正确的诊断码</param>
        /// <returns>此步骤是否通过 True通过 False不通过</returns>
        public static bool CheckStep(ICollection<CodeInfo> codeInfos, Func<CodeInfo> function, Code code)
        {
            var ipCodeInfo = function.Invoke();
            codeInfos.Add(ipCodeInfo);
            var stepIsPassed = ipCodeInfo.Id == code;
            return stepIsPassed;
        }
    }
}