using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using Baosight.FDAA.PackageDiagnosis.DAL;
using Baosight.FDAA.PackageDiagnosis.Model.Constant;
using Baosight.FDAA.PackageDiagnosis.Model.Entities;
using Baosight.FDAA.PackageDiagnosis.Model.Enums;
using Baosight.FDAA.PackageDiagnosis.Model.Modules;

namespace Baosight.FDAA.PackageDiagnosis.BLL.Packages
{
    public class Arti3Package : BasePackage, IInstallation, ILicense
    {
        #region Field

        private bool isPassed;

        #endregion

        #region Event

        public override event EventHandler<StatusEventArgs> StatusEvent;

        #endregion

        #region Properties

        public ObservableCollection<ARTI3Module> Arti3Modules { get; set; }

        /// <summary>
        ///     当前功能模块名称
        /// </summary>
        public override string PackageName
        {
            get { return Constants.ARTI3_PACKAGE_NAME; }
        }

        /// <summary>
        ///     当前功能模块是否配置
        /// </summary>
        public override bool IsConfigured
        {
            get
            {
                var signalNumber = Arti3Modules.Sum(module =>
                    module.AnalogDataTable.Rows.Count + module.DigitalDataTable.Rows.Count);

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
            var installedIsPassed =
                DiagnosticHelper.CheckStep(codeInfos, CheckInstallation);

            //  如果服务不存在则中止检查
            PmsServer.CreateInstance().InitPmsService();
            if (PmsServer.CreateInstance().PmsService == null)
            {
                if (installedIsPassed)
                {
                    DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.OK, out isPassed);
                    return codeInfos;
                }

                DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.NotPassed, out isPassed);
                return codeInfos;
            }

            //  步骤一：检查是否授权
            var licensedIsPassed =
                DiagnosticHelper.CheckStep(codeInfos, CheckLicense);

            //  未授权则中止检查
            if (!(installedIsPassed && licensedIsPassed))
            {
                DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.NotPassed, out isPassed);
                return codeInfos;
            }

            #endregion

            #region Step 2

            //  步骤二：检查IP地址是否可以Ping通
            var pingIpStepIsPassed = DiagnosticHelper.CheckStep(codeInfos, CheckIsPingable);

            //  步骤二如果不通过则退出检查
            if (!pingIpStepIsPassed)
            {
                DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.NotPassed, out isPassed);
                return codeInfos;
            }

            #endregion

            #region Step 3

            //  步骤三：检查是否某个模块全部信号接收不到数据
            var receivedStepIsPassed = DiagnosticHelper.CheckStep(codeInfos, CheckDataReceived);

            //  步骤三：检查是否某个模块部分信号接收不到数据
            var partialReceivedStepIsPassed = DiagnosticHelper.CheckStep(codeInfos, CheckPartialDataReceived);

            //  步骤三如果不通过则退出检查
            if (!(receivedStepIsPassed && partialReceivedStepIsPassed))
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
            var installationCodeInfo = DiagnosticHelper.CheckInstallation(Constants.ARTI3_DRIVER_NAME,
                Code.ARTI3Interface_Installed_Info, Code.ARTI3Interface_NotInstalled_Error);
            IsInstalled = installationCodeInfo.Id == 0;
            return installationCodeInfo;
        }

        /// <summary>
        ///     检查授权
        /// </summary>
        /// <returns>诊断码</returns>
        public CodeInfo CheckLicense()
        {
            var licenseCodeInfo = DiagnosticHelper.CheckLicense(Constants.ARTI3_DRIVER_NAME,
                Code.ARTI3Interface_Licensed_Info, Code.ARTI3Interface_NotLicensed_Error);
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
                DiagnosticHelper.CheckReceivedData(Code.ARTI3Interface_NotReceivedData_Error, Arti3Modules);
            return receivedCodeInfo;
        }

        /// <summary>
        ///     检查模块内部分信号接受数据
        /// </summary>
        /// <returns>诊断码</returns>
        private CodeInfo CheckPartialDataReceived()
        {
            var partialReceivedCodeInfo = DiagnosticHelper.CheckPartialReceivedData(
                Code.ARTI3Interface_PartialNotReceivedData_Error,
                Arti3Modules);
            return partialReceivedCodeInfo;
        }

        /// <summary>
        ///     检查某个设备IP地址是否可以Ping通
        /// </summary>
        /// <returns>结果码</returns>
        private CodeInfo CheckIsPingable()
        {
            //  是否存在Ip Ping不通的设备 false 不存在 true存在
            var result = false;

            //  ping不通的设备IP地址
            var notPingablePmsNames = GetNotPingableIps(ref result);

            if (result)
                return DiagnosticHelper.MakeCodeInfo(Code.ARTI3Interface_DeviceIpNotPingable_Error,
                    string.Join(",", notPingablePmsNames));

            return new CodeInfo();
        }

        /// <summary>
        ///     获取所有PING不通的设备
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private IEnumerable<string> GetNotPingableIps(ref bool result)
        {
            //  ping不通的IP地址集合
            var notPingableNames = new List<string>();

            foreach (var keyValuePair in GetHasSignalIpAddress())
            {
                //  当前设备不是IP地址则跳过
                IPAddress address;
                var tryParse = IPAddress.TryParse(keyValuePair.Value, out address);
                if (!tryParse) continue;

                //  当前IP是否可以ping通
                var pingable = NetworkHelper.PingHost(keyValuePair.Value);
                if (!pingable)
                {
                    result = true;
                    notPingableNames.Add("[" + keyValuePair.Value + "]");
                }
            }

            return notPingableNames;
        }

        /// <summary>
        ///     获取有配置信号的ip地址集合
        /// </summary>
        /// <returns>Ip集合 key模块号 value ipAddress</returns>
        private Dictionary<uint, string> GetHasSignalIpAddress()
        {
            return (from arti3Module in Arti3Modules
                    let activeSignals = arti3Module.AnalogDataTable.Rows.Count + arti3Module.DigitalDataTable.Rows.Count
                    where activeSignals != 0
                    select arti3Module)
                .ToDictionary(arti3Module => arti3Module.ModuleNo, arti3Module => arti3Module.IpAddress);
        }

        #endregion
    }
}