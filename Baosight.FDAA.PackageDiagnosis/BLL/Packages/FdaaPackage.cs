using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Baosight.FDAA.PackageDiagnosis.DAL;
using Baosight.FDAA.PackageDiagnosis.Model.Constant;
using Baosight.FDAA.PackageDiagnosis.Model.Entities;
using Baosight.FDAA.PackageDiagnosis.Model.Enums;
using Baosight.FDAA.PackageDiagnosis.Model.Modules;

namespace Baosight.FDAA.PackageDiagnosis.BLL.Packages
{
    public class FdaaPackage : BasePackage, IInstallation, ILicense
    {
        #region Field

        private bool isPassed;

        #endregion

        #region Event

        public override event EventHandler<StatusEventArgs> StatusEvent;

        #endregion

        #region Properties

        public Dictionary<uint, FDAAPMSService> PmsServiceCollection { get; set; }

        public ObservableCollection<FDAAModule> FdaaModules { get; set; }

        /// <summary>
        ///     当前功能模块名称
        /// </summary>
        public override string PackageName
        {
            get { return Constants.FDAA_PACKAGE_NAME; }
        }

        /// <summary>
        ///     当前功能模块是否配置
        /// </summary>
        public override bool IsConfigured
        {
            get
            {
                var signalNumber = FdaaModules.Sum(fdaaModule =>
                    fdaaModule.AnalogDataTable.Rows.Count + fdaaModule.DigitalDataTable.Rows.Count);

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

            //  步骤二：检查FDAA PMSService服务IP地址是否可以Ping通
            var pingIpStepIsPassed = DiagnosticHelper.CheckStep(codeInfos, CheckIsPingable);

            //  步骤二：检查FDAA PMSService服务IP地址端口号是否可以连接
            var portStepIsPassed = DiagnosticHelper.CheckStep(codeInfos, CheckPort);

            //  步骤二如果不通过则退出检查
            if (!(pingIpStepIsPassed && portStepIsPassed))
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
            var installationCodeInfo = DiagnosticHelper.CheckInstallation(Constants.FDAA_DRIVER_NAME,
                Code.FDAAInterface_Installed_Info, Code.FDAAInterface_NotInstalled_Error);
            IsInstalled = installationCodeInfo.Id == 0;
            return installationCodeInfo;
        }

        /// <summary>
        ///     检查授权
        /// </summary>
        /// <returns>诊断码</returns>
        public CodeInfo CheckLicense()
        {
            var licenseCodeInfo = DiagnosticHelper.CheckLicense(Constants.FDAA_DRIVER_NAME,
                Code.FDAAInterface_Licensed_Info, Code.FDAAInterface_NotLicensed_Error);
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
                DiagnosticHelper.CheckReceivedData(Code.FDAAInterface_NotReceivedData_Error, FdaaModules);
            return receivedCodeInfo;
        }

        /// <summary>
        ///     检查模块内部分信号接受数据
        /// </summary>
        /// <returns>诊断码</returns>
        private CodeInfo CheckPartialDataReceived()
        {
            var partialReceivedCodeInfo = DiagnosticHelper.CheckPartialReceivedData(
                Code.FDAAInterface_PartialNotReceivedData_Error,
                FdaaModules);
            return partialReceivedCodeInfo;
        }

        /// <summary>
        ///     检查FDAA PMSService服务IP地址是否可以Ping通
        /// </summary>
        /// <returns>结果码</returns>
        private CodeInfo CheckIsPingable()
        {
            //  服务集合是否存在Ip Ping不通的服务 false 不存在 true存在
            var result = false;

            //  ping不同的pms服务地址集合
            var notPingablePmsNames = GetNotPingablePmsNames(ref result);

            if (result)
                return DiagnosticHelper.MakeCodeInfo(Code.FDAAInterface_PmsServiceIpNotPingable_Error,
                    string.Join(",", notPingablePmsNames));

            return new CodeInfo();
        }

        /// <summary>
        ///     检查FDAA PMSService服务端口是否可以连接
        /// </summary>
        /// <returns>结果码</returns>
        private CodeInfo CheckPort()
        {
            //  服务集合是否存在Port端口连接不上的情况 false 不存在 true存在
            var result = false;
            //  ping不同的pms服务地址集合
            var notConnectablePorts = new List<string>();
            foreach (var pmService in PmsServiceCollection)
            {
                //  当前pms服务端口是否可以连接
                var portIsConnectable = NetworkHelper.CheckPortEnable(pmService.Value.Ip, pmService.Value.Port);
                if (portIsConnectable) continue;
                result = true;
                notConnectablePorts.Add("[" + pmService.Value.No + " " + pmService.Value.Name + ": " +
                                        pmService.Value.Ip + " " + pmService.Value.Port + "]");
            }

            if (result)
                return DiagnosticHelper.MakeCodeInfo(Code.FDAAInterface_PmsServicePortNotConnectable_Error,
                    string.Join(",", notConnectablePorts));
            return new CodeInfo();
        }

        /// <summary>
        ///     获取所有PING不通的PMS名字集合 名字格式PMS NO + PMS NAME + PMS Ip
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private IEnumerable<string> GetNotPingablePmsNames(ref bool result)
        {
            //  ping不通的FAU设备地址集合
            var notPingableNames = new List<string>();

            foreach (var pmService in GetHasSignalPmsServiceCollection())
            {
                //  当前pms服务IP是否可以ping通
                var pingable = NetworkHelper.PingHost(pmService.Value.Ip);
                if (!pingable)
                {
                    result = true;
                    notPingableNames.Add("[" + pmService.Value.Ip + "]");
                }
            }

            return notPingableNames;
        }

        /// <summary>
        ///     获取有配置信号的pms服务集合
        /// </summary>
        /// <returns>pmsService集合 key模块号 value FDAAPMSService</returns>
        private Dictionary<uint, FDAAPMSService> GetHasSignalPmsServiceCollection()
        {
            var hasSignalPmsServiceCollection = new Dictionary<uint, FDAAPMSService>();
            foreach (var fdaaModule in FdaaModules)
            {
                var activeSignals = fdaaModule.AnalogDataTable.Rows.Count + fdaaModule.DigitalDataTable.Rows.Count;
                if (activeSignals == 0) continue;
                hasSignalPmsServiceCollection.Add(fdaaModule.PmsService.No,
                    PmsServiceCollection[fdaaModule.PmsService.No]);
            }

            return hasSignalPmsServiceCollection;
        }

        #endregion
    }
}