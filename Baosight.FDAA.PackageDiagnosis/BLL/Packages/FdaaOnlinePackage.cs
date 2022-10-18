using System;
using System.Collections.Generic;
using System.IO;
using Baosight.FDAA.PackageDiagnosis.DAL.Configs;
using Baosight.FDAA.PackageDiagnosis.Model.Constant;
using Baosight.FDAA.PackageDiagnosis.Model.Entities;
using Baosight.FDAA.PackageDiagnosis.Model.Enums;

namespace Baosight.FDAA.PackageDiagnosis.BLL.Packages
{
    public class FdaaOnlinePackage : BasePackage, IInstallation
    {
        #region Constructor

        /// <summary>
        ///     构造函数
        /// </summary>
        public FdaaOnlinePackage()
        {
            //  不用配置，默认参数设置为已配置
            isConfigured = true;
        }

        #endregion

        #region Event

        public override event EventHandler<StatusEventArgs> StatusEvent;

        #endregion

        #region Field

        private readonly FdaaOnlineConfig fdaaOnlineConfig = new FdaaOnlineConfig();
        private readonly bool isConfigured;
        private bool isPassed;
        private int signalNumber;

        #endregion

        #region Properties

        /// <summary>
        ///     功能模块名称
        /// </summary>
        public override string PackageName
        {
            get { return Constants.FDAAONLINE_PACKAGE_NAME; }
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

        /// <summary>
        ///     当前功能模块是否安装
        /// </summary>
        public bool IsInstalled { get; private set; }

        #endregion

        #region Method

        public override List<CodeInfo> Diagnose()
        {
            DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.Diagnosing, out isPassed);

            var codeInfos = new List<CodeInfo>();

            #region Step 1

            //  步骤一：检查FDAAONLINE是否安装
            var installStepIsPassed =
                DiagnosticHelper.CheckStep(codeInfos, CheckInstallation);

            //  如第一步检查失败退出检查
            if (!installStepIsPassed)
            {
                DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.OK, out isPassed);
                return codeInfos;
            }

            #endregion

            #region Step 2

            //  步骤二：检查是否配置文件损坏
            var configStepIsPassed = DiagnosticHelper.CheckStep(codeInfos, CheckConfig);

            //  如第二步检查失败退出检查
            if (!configStepIsPassed)
            {
                DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.NotPassed, out isPassed);
                return codeInfos;
            }

            #endregion

            DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.OK, out isPassed);

            return codeInfos;
        }

        /// <summary>
        ///     检查FDAA ONLINE是否安装
        /// </summary>
        /// <returns>诊断码</returns>
        public CodeInfo CheckInstallation()
        {
            var installationCodeInfo = Directory.Exists(FdaaHelper.CreateInstance().ClientPath) == false
                ? DiagnosticHelper.MakeCodeInfo(Code.FDAAOnline_NotInstalled_Error)
                : new CodeInfo();
            IsInstalled = installationCodeInfo.Id == 0;
            return installationCodeInfo;
        }

        /// <summary>
        ///     检查配置文件
        /// </summary>
        /// <returns>诊断码</returns>
        private CodeInfo CheckConfig()
        {
            string error;
            var result = fdaaOnlineConfig.LoadConfig(out error);
            var configCodeInfo = !result
                ? DiagnosticHelper.MakeCodeInfo(Code.FDAAOnline_ConfigCorrupted_Error, error)
                : new CodeInfo();
            return configCodeInfo;
        }

        #endregion
    }
}