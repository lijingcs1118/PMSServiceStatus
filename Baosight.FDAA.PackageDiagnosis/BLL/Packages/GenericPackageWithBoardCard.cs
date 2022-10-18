using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Baosight.FDAA.PackageDiagnosis.DAL;
using Baosight.FDAA.PackageDiagnosis.Model.Entities;
using Baosight.FDAA.PackageDiagnosis.Model.Enums;
using Baosight.FDAA.PackageDiagnosis.Model.Modules;

namespace Baosight.FDAA.PackageDiagnosis.BLL.Packages
{
    /// <summary>
    ///     ProfibusDPLite ,Reflective Memory 5565用此类诊断
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericPackageWithBoardCard<T> : BasePackage, IInstallation, ILicense where T : BaseModule
    {
        #region Field

        protected bool isPassed;

        #endregion

        #region Event

        public override event EventHandler<StatusEventArgs> StatusEvent;

        #endregion

        #region Properties

        public ObservableCollection<T> Modules { get; set; }

        public List<Code> CodeList { get; set; }

        public string DriverName { get; set; }

        public string Name { get; set; }

        public string BoardCardName { get; set; }


        /// <summary>
        ///     当前功能模块名称
        /// </summary>
        public override string PackageName
        {
            get { return Name; }
        }

        /// <summary>
        ///     当前功能模块是否配置
        /// </summary>
        public override bool IsConfigured
        {
            get
            {
                var signalNumber = Modules.Sum(module =>
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

            #region Step1

            //  步骤一：检查是否安装
            var installStepIsPassed =
                DiagnosticHelper.CheckStep(codeInfos, CheckInstallation);

            //  步骤一：检查板卡驱动是否安装
            var cardStepIsPassed = DiagnosticHelper.CheckStep(codeInfos, CheckBoardCard);

            //  如果服务不存在则中止检查
            PmsServer.CreateInstance().InitPmsService();
            if (PmsServer.CreateInstance().PmsService == null)
            {
                if (installStepIsPassed && cardStepIsPassed)
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
            if (!(installStepIsPassed && cardStepIsPassed && licenseStepIsPassed))
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
            var installationCodeInfo = DiagnosticHelper.CheckInstallation(DriverName,
                CodeList.Find(code => code.ToString().Contains("Installed_Info")),
                CodeList.Find(code => code.ToString().Contains("NotInstalled_Error")));
            IsInstalled = installationCodeInfo.Id == 0;
            return installationCodeInfo;
        }

        /// <summary>
        ///     检查授权
        /// </summary>
        /// <returns>诊断码</returns>
        public CodeInfo CheckLicense()
        {
            var licenseCodeInfo = DiagnosticHelper.CheckLicense(DriverName,
                CodeList.Find(code => code.ToString().Contains("Licensed_Info")),
                CodeList.Find(code => code.ToString().Contains("NotLicensed_Error")));
            IsLicensed = licenseCodeInfo.Id == 0;
            return licenseCodeInfo;
        }

        /// <summary>
        ///     检查板卡
        /// </summary>
        /// <returns></returns>
        protected CodeInfo CheckBoardCard()
        {
            var cardDriverIsInstalledCodeInfo =
                DiagnosticHelper.CheckCardDriverIsInstalled(BoardCardName,
                    CodeList.Find(code => code.ToString().Contains("CardDriver_NotInstalled_Error")));
            return cardDriverIsInstalledCodeInfo;
        }

        #endregion
    }
}