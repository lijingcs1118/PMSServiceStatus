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
    public class VirtualPackage : BasePackage, IInstallation, ILicense
    {
        #region Field

        private bool isPassed;

        #endregion

        #region Event

        public override event EventHandler<StatusEventArgs> StatusEvent;

        #endregion

        #region Properties

        public ObservableCollection<VirtualModule> VirtualModules { get; set; }

        /// <summary>
        ///     当前功能模块名称
        /// </summary>
        public override string PackageName
        {
            get { return Constants.VIRTUAL_PACKAGE_NAME; }
        }

        /// <summary>
        ///     当前功能模块是否配置
        /// </summary>
        public override bool IsConfigured
        {
            get
            {
                var signalNumber = VirtualModules.Sum(module =>
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

            //  步骤二：检查是否某个模块部分信号接收不到数据
            var partialReceivedStepIsPassed = DiagnosticHelper.CheckStep(codeInfos, CheckPartialDataReceived);

            //  步骤二如果不通过则退出检查
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
        ///     检查安装
        /// </summary>
        /// <returns>诊断码</returns>
        public CodeInfo CheckInstallation()
        {
            var installationCodeInfo = DiagnosticHelper.CheckInstallation(Constants.VIRTUAL_DRIVER_NAME,
                Code.VirtualInterface_Installed_Info, Code.VirtualInterface_NotInstalled_Error);
            IsInstalled = installationCodeInfo.Id == 0;
            return installationCodeInfo;
        }

        /// <summary>
        ///     检查授权
        /// </summary>
        /// <returns>诊断码</returns>
        public CodeInfo CheckLicense()
        {
            var licenseCodeInfo = DiagnosticHelper.CheckLicense(Constants.VIRTUAL_DRIVER_NAME,
                Code.VirtualInterface_Licensed_Info, Code.VirtualInterface_NotLicensed_Error);
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
                Code.VirtualInterface_PartialNotReceivedData_Error,
                VirtualModules);
            return partialReceivedCodeInfo;
        }

        #endregion
    }
}