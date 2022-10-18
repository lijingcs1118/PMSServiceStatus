using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Baosight.FDAA.PackageDiagnosis.DAL;
using Baosight.FDAA.PackageDiagnosis.DAL.Configs;
using Baosight.FDAA.PackageDiagnosis.Model.Constant;
using Baosight.FDAA.PackageDiagnosis.Model.Entities;
using Baosight.FDAA.PackageDiagnosis.Model.Enums;

namespace Baosight.FDAA.PackageDiagnosis.BLL.Packages
{
    public class HdDataStoragePackage : BasePackage
    {
        #region Constructor

        public HdDataStoragePackage()
        {
            //  判断HD是否配置了信号
            if (ProgramModule.Container.Resolve<HdDsConfig>().HdObject.HDServers.Count != 0)
            {
                var count = ProgramModule.Container.Resolve<HdDsConfig>().HdObject.HDServers.FirstOrDefault().Value
                    .Schemas.Values
                    .Where(schema => schema.Enabled).Sum(schema => schema.Signals.Count);

                if (count > 0) isConfigured = true;
            }
        }

        #endregion

        #region Event

        public override event EventHandler<StatusEventArgs> StatusEvent;

        #endregion

        #region Field

        private readonly HdManager hdManager = new HdManager();
        private readonly bool isConfigured;
        private bool isPassed;

        #endregion

        #region Properties

        /// <summary>
        ///     当前功能模块名称
        /// </summary>
        public override string PackageName
        {
            get { return Constants.HD_DATASTORAGE_PACKAGE_NAME; }
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
        ///     诊断当前功能模块
        /// </summary>
        /// <returns>结果码集合</returns>
        public override List<CodeInfo> Diagnose()
        {
            DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.Diagnosing, out isPassed);

            var codeInfos = new List<CodeInfo>();

            #region Step 1

            //  步骤一：检查是否安装VC
            var vcStepIsPassed = DiagnosticHelper.CheckStep(codeInfos, CheckVcInstallation);

            //  步骤一：检查HD Data Storage是否激活
            var activeStepIsPassed = DiagnosticHelper.CheckStep(codeInfos, CheckActivity);

            //  步骤一不通过则退出检查
            if (!(vcStepIsPassed && activeStepIsPassed))
            {
                DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.NotPassed, out isPassed);
                return codeInfos;
            }

            if (!isConfigured)
            {
                DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.OK, out isPassed);
                return codeInfos;
            }

            #endregion

            #region Step 2

            //  步骤二：检查缓存目录所在磁盘逻辑驱动器是否存在
            var driverIsExistStepIsPassed = DiagnosticHelper.CheckStep(codeInfos, CheckDriverIsExist);

            //  步骤二不通过则退出检查
            if (!driverIsExistStepIsPassed)
            {
                DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.NotPassed, out isPassed);
                return codeInfos;
            }

            #endregion

            #region Step 3

            //  步骤三：检查缓存目录所在磁盘逻辑驱动器是否足够 把MB转换成G
            var spaceIsPassed = DiagnosticHelper.CheckStep(codeInfos, CheckCacheSpace);

            //  如果服务不存在则中止检查
            PmsServer.CreateInstance().InitPmsService();
            if (PmsServer.CreateInstance().PmsService == null)
            {
                if (spaceIsPassed)
                {
                    DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.OK, out isPassed);
                    return codeInfos;
                }

                DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.NotPassed, out isPassed);
                return codeInfos;
            }

            #endregion

            #region Step 4

            //  步骤四：检查HD服务是否可以连接
            var pingIsPassed = DiagnosticHelper.CheckStep(codeInfos, CheckHdConnection);

            //  步骤四不通过则退出检查
            if (!pingIsPassed)
            {
                hdManager.DisConnectHd();
                DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.NotPassed, out isPassed);
                return codeInfos;
            }

            #endregion

            #region Step 5

            //  步骤五：检查HD数据访问是否异常
            var tagReadablePassed = DiagnosticHelper.CheckStep(codeInfos, CheckTestTagReadable);

            //  步骤五不通过则退出检查
            if (!tagReadablePassed)
            {
                hdManager.DisConnectHd();
                DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.NotPassed, out isPassed);
                return codeInfos;
            }

            #endregion

            #region Step 6

            //  步骤六：检查HD数据写入时间是否戳偏差过大
            var timeStampIntervalPassed = DiagnosticHelper.CheckStep(codeInfos, CheckTimeStampInterval);

            //  步骤六不通过则退出检查
            if (!timeStampIntervalPassed)
            {
                hdManager.DisConnectHd();
                DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.NotPassed, out isPassed);
                return codeInfos;
            }

            #endregion

            hdManager.DisConnectHd();
            DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.OK, out isPassed);
            return codeInfos;
        }

        /// <summary>
        ///     检查VC运行环境
        /// </summary>
        /// <returns>诊断码</returns>
        private CodeInfo CheckVcInstallation()
        {
            var vcInstallationCodeInfo = !MachineHelper.CheckVcIsInstalled()
                ? DiagnosticHelper.MakeCodeInfo(Code.HD_VcNotInstalled_Error)
                : new CodeInfo();
            return vcInstallationCodeInfo;
        }

        /// <summary>
        ///     检查HD激活选项
        /// </summary>
        /// <returns>诊断码</returns>
        private CodeInfo CheckActivity()
        {
            var activityCodeInfo = !ProgramModule.Container.Resolve<HdDsConfig>().HdObject.Active
                ? DiagnosticHelper.MakeCodeInfo(Code.HD_NotActive_Error)
                : new CodeInfo();
            return activityCodeInfo;
        }

        /// <summary>
        ///     检查配置的缓存目录所在磁盘是否存在
        /// </summary>
        /// <returns>诊断码</returns>
        private CodeInfo CheckDriverIsExist()
        {
            var driverIsExistCodeInfo = DiagnosticHelper.CheckDiscIsExist(
                ProgramModule.Container.Resolve<HdDsConfig>().HdObject.HDServers.FirstOrDefault().Value.BufferLocation,
                Code.HD_CacheDiscNotExist_Error);
            return driverIsExistCodeInfo;
        }

        /// <summary>
        ///     检查配置的缓存目录所在磁盘是否存在
        /// </summary>
        /// <returns>诊断码</returns>
        private CodeInfo CheckCacheSpace()
        {
            var cacheDiscOutOfSpaceCodeInfo = DiagnosticHelper.CheckFreeSpace(
                ProgramModule.Container.Resolve<HdDsConfig>().HdObject.HDServers.FirstOrDefault().Value.BufferLocation,
                ProgramModule.Container.Resolve<HdDsConfig>().HdObject.HDServers.FirstOrDefault().Value.BufferSize,
                Code.HD_CacheDiscOutOfSpace_Error);
            return cacheDiscOutOfSpaceCodeInfo;
        }

        /// <summary>
        ///     检查HD Server是否可以连接
        /// </summary>
        /// <returns>诊断码</returns>
        private CodeInfo CheckHdConnection()
        {
            string error;
            var isConnectToHd = hdManager.ConnectToHd(out error);

            var codeInfo = isConnectToHd
                ? new CodeInfo()
                : DiagnosticHelper.MakeCodeInfo(Code.HD_SererConnectionFailed_Error, error);
            return codeInfo;
        }

        /// <summary>
        ///     检查HD数据访问是否异常
        /// </summary>
        /// <returns>诊断码</returns>
        private CodeInfo CheckTestTagReadable()
        {
            string error;
            var isReadable = hdManager.QueryTestTagSnapShot(out error);

            var codeInfo = isReadable
                ? new CodeInfo()
                : DiagnosticHelper.MakeCodeInfo(Code.HD_DataAccess_Error, error);
            return codeInfo;
        }

        /// <summary>
        ///     检查HD写入时间戳与本地时间间隔是否达标
        /// </summary>
        /// <returns>诊断码</returns>
        private CodeInfo CheckTimeStampInterval()
        {
            string error;
            var isMeet = hdManager.CheckTimeStampIntervalIsMeet(out error);

            var codeInfo = isMeet
                ? new CodeInfo()
                : DiagnosticHelper.MakeCodeInfo(Code.HD_TimestampDeviationTooLarge_Error, error);
            return codeInfo;
        }

        #endregion
    }
}