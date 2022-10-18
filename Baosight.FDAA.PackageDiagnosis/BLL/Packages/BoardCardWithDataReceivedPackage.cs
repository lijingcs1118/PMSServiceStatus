using System;
using System.Collections.Generic;
using Baosight.FDAA.PackageDiagnosis.DAL;
using Baosight.FDAA.PackageDiagnosis.Model.Entities;
using Baosight.FDAA.PackageDiagnosis.Model.Enums;
using Baosight.FDAA.PackageDiagnosis.Model.Modules;

namespace Baosight.FDAA.PackageDiagnosis.BLL.Packages
{
    /// <summary>
    ///     S7DPRequest ,S7PNRequest用此类诊断,比GenericPackageWithBoardCard类多了 某个模块个别信号接收不到数据的检查
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BoardCardWithDataReceivedPackage<T> : GenericPackageWithBoardCard<T> where T : BaseModule
    {
        public override event EventHandler<StatusEventArgs> StatusEvent;

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

            #region Step2

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
        ///     检查模块内部分信号接受数据
        /// </summary>
        /// <returns>诊断码</returns>
        protected virtual CodeInfo CheckPartialDataReceived()
        {
            var partialReceivedCodeInfo = DiagnosticHelper.CheckPartialReceivedData(
                CodeList.Find(code => code.ToString().Contains("PartialNotReceivedData_Error")),
                Modules);
            return partialReceivedCodeInfo;
        }
    }
}