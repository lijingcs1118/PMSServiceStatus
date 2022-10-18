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
    ///     MELSEC,CPNET,TECNET,USIGMA,NISDAS,SSE用此类诊断
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericPackageWithFau<T> : BasePackage, IInstallation, ILicense where T : ModuleWithFau
    {
        #region Field

        private bool isPassed;

        #endregion

        #region Event

        public override event EventHandler<StatusEventArgs> StatusEvent;

        #endregion

        #region Properties

        public ObservableCollection<T> GenericWithFauModules { get; set; }

        public List<Code> CodeList { get; set; }

        public string DriverName { get; set; }

        public string Name { get; set; }


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
                var signalNumber = GenericWithFauModules.Sum(genericWithFauModule =>
                    genericWithFauModule.AnalogDataTable.Rows.Count + genericWithFauModule.DigitalDataTable.Rows.Count);

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
        ///     检查模块内全部信号接受数据
        /// </summary>
        /// <returns>诊断码</returns>
        private CodeInfo CheckDataReceived()
        {
            var receivedCodeInfo =
                DiagnosticHelper.CheckReceivedData(
                    CodeList.Find(code => code.ToString().Contains("NotReceivedData_Error")), GenericWithFauModules);

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
            var notPingableNames = DiagnosticHelper.NotPingableFauNames(GenericWithFauModules, ref result);

            if (result)
                return DiagnosticHelper.MakeCodeInfo(
                    CodeList.Find(code => code.ToString().Contains("FauDeviceIpNotPingable_Error")),
                    string.Join(",", notPingableNames));

            return new CodeInfo();
        }

        #endregion
    }
}