using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Baosight.FDAA.PackageDiagnosis.DAL;
using Baosight.FDAA.PackageDiagnosis.Model.Constant;
using Baosight.FDAA.PackageDiagnosis.Model.Entities;
using Baosight.FDAA.PackageDiagnosis.Model.Enums;
using Baosight.FDAA.PackageDiagnosis.Model.Modules;

namespace Baosight.FDAA.PackageDiagnosis.BLL.Packages
{
    public class USigmaPackage : BasePackage
    {
        #region Constructor

        public USigmaPackage()
        {
            USigmaModules = new ObservableCollection<USigmaModule>();
            USigmaModules.CollectionChanged += OnListChanged;
        }

        #endregion

        #region Event

        public override event EventHandler<StatusEventArgs> StatusEvent;

        #endregion

        #region Field

        private bool isConfigured;
        private bool isPassed;
        private int signalNumber;
        internal ObservableCollection<USigmaModule> USigmaModules;

        #endregion

        #region Properties

        public Dictionary<uint, FAUDevice> FauCollection { get; set; }

        /// <summary>
        ///     当前功能模块名称
        /// </summary>
        public override string PackageName
        {
            get { return Constants.USIGMA_PACKAGE_NAME; }
        }

        /// <summary>
        ///     当前功能模块是否配置
        /// </summary>
        public override bool IsConfigured
        {
            get { return isConfigured; }
        }

        /// <summary>
        ///     当前功能模块诊断结果是否通过
        /// </summary>
        public override bool IsPassed
        {
            get { return isPassed; }
        }

        #endregion

        #region Method

        /// <summary>
        ///     计算当前功能模块 是否配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var uSigmaModule in USigmaModules)
                signalNumber += uSigmaModule.AnalogDataTable.Rows.Count +
                                uSigmaModule.DigitalDataTable.Rows.Count;

            isConfigured = signalNumber != 0;
        }

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
                DiagnosticHelper.CheckStep(codeInfos, CheckInstallation, Code.UsigmaInterface_Installed_Info);

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
                DiagnosticHelper.CheckStep(codeInfos, CheckLicense, Code.UsigmaInterface_Licensed_Info);

            //  步骤一未通过则中止检查
            if (!(installStepIsPassed && licenseStepIsPassed))
            {
                DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.NotPassed, out isPassed);
                return codeInfos;
            }

            #endregion

            #region Step 2

            //  步骤二：检查µΣNetwork FAU设备 IP地址是否可以Ping通
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

            //  步骤三如果不通过则退出检查
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
        private CodeInfo CheckInstallation()
        {
            var installationCodeInfo = DiagnosticHelper.CheckInstallation(Constants.USIGMA_DRIVER_NAME,
                Code.UsigmaInterface_Installed_Info, Code.UsigmaInterface_NotInstalled_Error);
            return installationCodeInfo;
        }

        /// <summary>
        ///     检查授权
        /// </summary>
        /// <returns>诊断码</returns>
        private CodeInfo CheckLicense()
        {
            var licenseCodeInfo = DiagnosticHelper.CheckLicense(Constants.USIGMA_DRIVER_NAME,
                Code.UsigmaInterface_Licensed_Info, Code.UsigmaInterface_NotLicensed_Error);
            return licenseCodeInfo;
        }

        /// <summary>
        ///     检查模块内全部信号接受数据
        /// </summary>
        /// <returns>诊断码</returns>
        private CodeInfo CheckDataReceived()
        {
            var receivedCodeInfo =
                DiagnosticHelper.CheckReceivedData(Code.UsigmaInterface_NotReceivedData_Error, USigmaModules);
            return receivedCodeInfo;
        }

        /// <summary>
        ///     检查FAU设备 IP地址是否可以Ping通
        /// </summary>
        /// <returns>结果码</returns>
        private CodeInfo CheckIsPingable()
        {
            //  服务集合是否存在Ip Ping不通的服务 false 不存在 true存在
            var result = false;

            //  获取所有PING不通的FAU设备名字集合
            var notPingableNames = DiagnosticHelper.NotPingableFauNames(USigmaModules, ref result);

            if (result)
                return DiagnosticHelper.MakeCodeInfo(Code.UsigmaInterface_FauDeviceIpNotPingable_Error,
                    string.Join(",", notPingableNames));

            return new CodeInfo();
        }

        #endregion
    }
}