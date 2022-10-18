using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using Baosight.FDAA.PackageDiagnosis.DAL;
using Baosight.FDAA.PackageDiagnosis.Model.Constant;
using Baosight.FDAA.PackageDiagnosis.Model.Entities;
using Baosight.FDAA.PackageDiagnosis.Model.Enums;
using Baosight.FDAA.PackageDiagnosis.Model.Modules;

namespace Baosight.FDAA.PackageDiagnosis.BLL.Packages
{
    public class VideoCapturePackage : BasePackage, IInstallation, ILicense
    {
        #region Field

        private bool isPassed;

        #endregion

        #region Event

        public override event EventHandler<StatusEventArgs> StatusEvent;

        #endregion

        #region Properties

        internal ObservableCollection<VideoCaptureModule> VideoCaptureModules;

        /// <summary>
        ///     当前功能模块名称
        /// </summary>
        public override string PackageName
        {
            get { return Constants.VIDEOCAPTURE_PACKAGE_NAME; }
        }

        /// <summary>
        ///     当前功能模块是否配置
        /// </summary>
        public override bool IsConfigured
        {
            get
            {
                var signalNumber = VideoCaptureModules.Sum(videoCaptureModule =>
                    videoCaptureModule.AnalogDataTable.Rows.Count + videoCaptureModule.DigitalDataTable.Rows.Count);

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
            var licenseStepIsPassed =
                DiagnosticHelper.CheckStep(codeInfos, CheckLicense);

            //  步骤一未通过则中止检查
            if (!(installStepIsPassed && licenseStepIsPassed))
            {
                DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.NotPassed, out isPassed);
                return codeInfos;
            }

            #endregion

            #region Step 2

            //  步骤二：检查VideoCapture设备 IP地址是否可以Ping通
            var pingIpStepIsPassed = DiagnosticHelper.CheckStep(codeInfos, CheckIsPingable);

            //  步骤二如果不通过则退出检查
            if (!pingIpStepIsPassed)
            {
                DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.NotPassed, out isPassed);
                return codeInfos;
            }

            #endregion

            #region Step 3

            //  步骤三：检查是否某个模块部分信号接收不到数据
            var partialReceivedStepIsPassed = DiagnosticHelper.CheckStep(codeInfos, CheckPartialDataReceived);

            //  步骤三如果不通过则退出检查
            if (!partialReceivedStepIsPassed)
            {
                DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.NotPassed, out isPassed);
                return codeInfos;
            }

            #endregion

            DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.OK, out isPassed);
            return codeInfos;
        }

        /// <summary>
        ///     检查VideoCapture设备IP地址是否可以Ping通
        /// </summary>
        /// <returns>结果码</returns>
        private CodeInfo CheckIsPingable()
        {
            //  是否存在Ip Ping不通的设备 false 不存在 true存在
            var result = false;

            //  ping不通的设备地址集合
            var notPingableNames = GetNotPingableNames(ref result);

            if (result)
                return DiagnosticHelper.MakeCodeInfo(Code.VideoCaptureInterface_DeviceIpNotPingable_Error,
                    string.Join(",", notPingableNames));

            return new CodeInfo();
        }

        /// <summary>
        ///     获取所有PING不通的名字集合
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private IEnumerable<string> GetNotPingableNames(ref bool result)
        {
            //  ping不通的地址集合
            var notPingableNames = new List<string>();

            foreach (var item in GetIpCollection())
            foreach (var ip in item.Value)
            {
                //  当前项IP是否可以ping通
                var pingable = NetworkHelper.PingHost(ip);
                if (!pingable)
                {
                    result = true;
                    notPingableNames.Add(ip);
                }
            }

            return notPingableNames;
        }

        /// <summary>
        ///     获取IP集合 key模块号，value当前模块的ip集合
        /// </summary>
        /// <returns></returns>
        private Dictionary<uint, List<string>> GetIpCollection()
        {
            var dictionary = new Dictionary<uint, List<string>>();
            foreach (var videoCaptureModule in VideoCaptureModules)
            {
                var list = (from DataRow row in videoCaptureModule.AnalogDataTable.Rows
                    select row["IPAddress"].ToString()).ToList();
                dictionary.Add(videoCaptureModule.ModuleNo, list);
            }

            return dictionary;
        }

        /// <summary>
        ///     检查安装
        /// </summary>
        /// <returns>诊断码</returns>
        public CodeInfo CheckInstallation()
        {
            var installationCodeInfo = DiagnosticHelper.CheckInstallation(Constants.VIDEOCAPTURE_DRIVER_NAME,
                Code.VideoCaptureInterface_Installed_Info, Code.VideoCaptureInterface_NotInstalled_Error);
            IsInstalled = installationCodeInfo.Id == 0;
            return installationCodeInfo;
        }

        /// <summary>
        ///     检查授权
        /// </summary>
        /// <returns>诊断码</returns>
        public CodeInfo CheckLicense()
        {
            var licenseCodeInfo = DiagnosticHelper.CheckLicense(Constants.VIDEOCAPTURE_DRIVER_NAME,
                Code.VideoCaptureInterface_Licensed_Info, Code.VideoCaptureInterface_NotLicensed_Error);
            IsLicensed = licenseCodeInfo.Id == 0;
            return licenseCodeInfo;
        }

        /// <summary>
        ///     检查模块内部分信号接受数据
        /// </summary>
        /// <returns>诊断码</returns>
        private CodeInfo CheckPartialDataReceived()
        {
            var partialReceivedCodeInfo = DiagnosticHelper.CheckPartialReceivedData(
                Code.VideoCaptureInterface_PartialNotReceivedData_Error,
                VideoCaptureModules);
            return partialReceivedCodeInfo;
        }

        #endregion
    }
}