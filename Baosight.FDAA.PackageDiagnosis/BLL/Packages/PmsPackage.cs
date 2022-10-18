using System;
using System.Collections.Generic;
using Baosight.FDAA.PackageDiagnosis.DAL;
using Baosight.FDAA.PackageDiagnosis.Model.Constant;
using Baosight.FDAA.PackageDiagnosis.Model.Entities;
using Baosight.FDAA.PackageDiagnosis.Model.Enums;

namespace Baosight.FDAA.PackageDiagnosis.BLL.Packages
{
    public class PmsPackage : BasePackage, ILicense
    {
        #region Constructor

        /// <summary>
        ///     构造函数
        /// </summary>
        public PmsPackage()
        {
            //  不用配置，默认参数设置为已配置
            isConfigured = true;
            packageName = Constants.PMS_PACKAGE_NAME;
        }

        #endregion

        #region Event

        public override event EventHandler<StatusEventArgs> StatusEvent;

        #endregion

        #region Field

        private readonly bool isConfigured;
        private readonly string packageName;
        private bool isPassed;

        #endregion

        #region Properties

        public override string PackageName
        {
            get { return packageName; }
        }

        public override bool IsConfigured
        {
            get { return isConfigured; }
        }

        public override bool IsPassed
        {
            get { return isPassed; }
        }

        /// <summary>
        ///     当前功能模块是否授权
        /// </summary>
        public bool IsLicensed { get; private set; }

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

            //  步骤一：检查.Net环境
            var doNetStepIsPassed = DiagnosticHelper.CheckStep(codeInfos, CheckDoNetInstallation);

            //  步骤一：检查是否授权
            var licenseStepIsPassed =
                DiagnosticHelper.CheckStep(codeInfos, CheckLicense);

            //  如第一步检查失败退出检查
            if (!(doNetStepIsPassed && licenseStepIsPassed))
            {
                DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.NotPassed, out isPassed);
                return codeInfos;
            }

            //  如果服务不存在则中止以下检查
            PmsServer.CreateInstance().InitPmsService();
            if (PmsServer.CreateInstance().PmsService == null)
            {
                DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.OK, out isPassed);
                return codeInfos;
            }

            #endregion

            #region Step 2

            //  步骤二：检查服务状态是否异常
            var isAbnormalStepIsPassed = DiagnosticHelper.CheckStep(codeInfos, CheckServiceIsAbnormal);

            //  步骤二：检查Guardian是否运行
            var guardianStepIsPassed = DiagnosticHelper.CheckStep(codeInfos, CheckGuardianRunningStatus);

            //  步骤二如果不通过则退出检查
            if (!(isAbnormalStepIsPassed && guardianStepIsPassed))
            {
                DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.NotPassed, out isPassed);
                return codeInfos;
            }

            #endregion

            DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.OK, out isPassed);

            return codeInfos;
        }

        /// <summary>
        ///     检查.Net 4.7.2安装
        /// </summary>
        /// <returns>诊断码</returns>
        private CodeInfo CheckDoNetInstallation()
        {
            var doNetCodeInfo = MachineHelper.CheckDotNetVersionIsGreaterThan472() == false
                ? DiagnosticHelper.MakeCodeInfo(Code.PMSService_DoNetVersionLow_Error)
                : new CodeInfo();
            return doNetCodeInfo;
        }

        /// <summary>
        ///     检查授权
        /// </summary>
        /// <returns>诊断码</returns>
        public CodeInfo CheckLicense()
        {
            var licenseCodeInfo = FdaaHelper.CreateInstance().GetLicensedSignalCount() == 32
                ? DiagnosticHelper.MakeCodeInfo(Code.PMSService_NotLicensed_Error)
                : new CodeInfo();

            IsLicensed = licenseCodeInfo.Id == 0;
            return licenseCodeInfo;
        }

        /// <summary>
        ///     检查Guardian运行
        /// </summary>
        /// <returns>诊断码</returns>
        private CodeInfo CheckGuardianRunningStatus()
        {
            var guardianIsRunningCodeInfo = !PmsServer.CheckServiceIfExist(Constants.GUARDIAN_PROCESS_NAME)
                ? DiagnosticHelper.MakeCodeInfo(Code.PMSService_Guardian_NotRunning_Error)
                : new CodeInfo();
            return guardianIsRunningCodeInfo;
        }

        /// <summary>
        ///     检查Pms服务状态
        /// </summary>
        /// <returns>结果码</returns>
        private CodeInfo CheckServiceIsAbnormal()
        {
            // TODO 异常未检查
            if (PmsServer.CreateInstance().ServerStatus != ServiceStatus.Running) return new CodeInfo();
            if (PmsServer.CreateInstance().PmsService == null)
                return DiagnosticHelper.MakeCodeInfo(Code.PMSService_StatusAbnormal_Error);

            try
            {
                long lInterrupts;
                PmsServer.CreateInstance().PmsService.getSoftInterrupts(out lInterrupts);
            }
            catch (Exception)
            {
                return DiagnosticHelper.MakeCodeInfo(Code.PMSService_StatusAbnormal_Error);
            }

            return new CodeInfo();
        }

        #endregion
    }
}