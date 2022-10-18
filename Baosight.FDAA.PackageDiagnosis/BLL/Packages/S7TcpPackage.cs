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
    public class S7TcpPackage : BasePackage, IInstallation, ILicense
    {
        #region Field

        private bool isPassed;

        #endregion

        #region Event

        public override event EventHandler<StatusEventArgs> StatusEvent;

        #endregion

        #region Properties

        internal ObservableCollection<S7TcpModule> S7TcpModules { get; set; }

        /// <summary>
        ///     当前功能模块名称
        /// </summary>
        public override string PackageName
        {
            get { return Constants.S7TCP_PACKAGE_NAME; }
        }

        /// <summary>
        ///     当前功能模块是否配置
        /// </summary>
        public override bool IsConfigured
        {
            get
            {
                var signalNumber = S7TcpModules.Sum(s7TcpModule =>
                    s7TcpModule.AnalogDataTable.Rows.Count + s7TcpModule.DigitalDataTable.Rows.Count);
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

            //  步骤二：检查是否某个模块连接失败
            var connectStepIsPassed = DiagnosticHelper.CheckStep(codeInfos, CheckModuleConnection);

            //  步骤二：检查某个模块数据响应时间是否达到预期
            var responseStepIsPassed = DiagnosticHelper.CheckStep(codeInfos, CheckResponseTime);

            //  步骤二：检查某个模块数据一致性是否达标
            var consistencyStepIsPassed = DiagnosticHelper.CheckStep(codeInfos, CheckConsistency);

            //  步骤二如果不通过则退出检查
            if (!(connectStepIsPassed && responseStepIsPassed && consistencyStepIsPassed))
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
        ///     检查安装
        /// </summary>
        /// <returns>诊断码</returns>
        public CodeInfo CheckInstallation()
        {
            var installationCodeInfo = DiagnosticHelper.CheckInstallation(Constants.S7TCP_DRIVER_NAME,
                Code.S7TCPInterface_Installed_Info, Code.S7TCPInterface_NotInstalled_Error);
            IsInstalled = installationCodeInfo.Id == 0;
            return installationCodeInfo;
        }

        /// <summary>
        ///     检查授权
        /// </summary>
        /// <returns>诊断码</returns>
        public CodeInfo CheckLicense()
        {
            var licenseCodeInfo = DiagnosticHelper.CheckLicense(Constants.S7TCP_DRIVER_NAME,
                Code.S7TCPInterface_Licensed_Info, Code.S7TCPInterface_NotLicensed_Error);
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
                Code.S7TCPInterface_PartialNotReceivedData_Error,
                S7TcpModules);
            return partialReceivedCodeInfo;
        }

        /// <summary>
        ///     检查S7模块连接状态 s7Status.StatusCode == 0
        /// </summary>
        /// <returns>诊断码</returns>
        private CodeInfo CheckModuleConnection()
        {
            Func<S7Status, uint, bool> func = (status, timebase) => status.StatusCode == 0;
            var codeInfo = GetModuleConnectionCodeInfo(func, Code.S7TCPInterface_ModuleConnectionFailed_Error);
            return codeInfo;
        }

        /// <summary>
        ///     检查某个模块数据响应时间是否达到预期 s7Status.StatusCode == 0 && excuteTimeMax /2 <= Timebase
        /// </summary>
        /// <returns>诊断码</returns>
        private CodeInfo CheckResponseTime()
        {
            Func<S7Status, uint, bool> func = (status, timebase) =>
            {
                if (status.StatusCode == 0)
                    return status.ExecuteTimeMax / 2 <= timebase;
                //  连接失败则不进行此次诊断
                return true;
            };
            var codeInfo = GetModuleConnectionCodeInfo(func, Code.S7TCPInterface_ResponseTimeNotAsExpected_Error);
            return codeInfo;
        }

        /// <summary>
        ///     检查某个模块数据一致性是否达标  ExecuteTimes > 1
        /// </summary>
        /// <returns>诊断码</returns>
        private CodeInfo CheckConsistency()
        {
            Func<S7Status, uint, bool> func = (status, timebase) =>
            {
                if (status.StatusCode == 0)
                    return status.ExecuteTimes <= 1;
                //  连接失败则不进行此次诊断
                return true;
            };
            var codeInfo = GetModuleConnectionCodeInfo(func, Code.S7TCPInterface_ConsistencyNotMeet_Error);
            return codeInfo;
        }

        /// <summary>
        ///     获取特定检查结果码
        /// </summary>
        /// <param name="function">function</param>
        /// <param name="code">诊断码</param>
        /// <returns>诊断结果</returns>
        private CodeInfo GetModuleConnectionCodeInfo(Func<S7Status, uint, bool> function, Code code)
        {
            //  是否存在不合规的S7模块
            var exist = false;

            var notConnectedModuleNumbers = new List<uint>();

            foreach (var s7TcpModule in S7TcpModules)
            {
                var s7Status = PmsServer.CreateInstance().GetModuleStatus(Convert.ToUInt16(s7TcpModule.ModuleNo));
                if (function(s7Status, s7TcpModule.TimeBase)) continue;
                //  存在不合规的模块
                exist = true;
                //  不合规的模块号加入集合
                notConnectedModuleNumbers.Add(s7TcpModule.ModuleNo);
            }

            if (exist)
                return DiagnosticHelper.MakeCodeInfo(code,
                    string.Join(",", notConnectedModuleNumbers));
            return new CodeInfo();
        }

        #endregion
    }
}