using System;
using System.Collections.Generic;
using Baosight.FDAA.PackageDiagnosis.Model.Entities;
using Baosight.FDAA.PackageDiagnosis.Model.Enums;

namespace Baosight.FDAA.PackageDiagnosis.BLL.Packages
{
    public class DataStorageBasePackage : BasePackage
    {
        #region Field

        private bool isPassed;

        #endregion

        #region Event

        public override event EventHandler<StatusEventArgs> StatusEvent;

        #endregion

        #region Properties

        /// <summary>
        ///     错误集合
        /// </summary>
        public List<Code> CodeList { get; set; }

        /// <summary>
        ///     缓存所在路径
        /// </summary>
        public string BufferLocation { get; set; }

        /// <summary>
        ///     设置的缓存空间 单位M
        /// </summary>
        public int BufferSize { get; set; }

        /// <summary>
        ///     功能模块名称
        /// </summary>
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
            get { return true; }
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

            //  步骤一：检查缓存目录所在磁盘逻辑驱动器是否存在
            var driverIsExistStepIsPassed = DiagnosticHelper.CheckStep(codeInfos, CheckDriverIsExist);

            //  步骤一不通过则退出检查
            if (!driverIsExistStepIsPassed)
            {
                DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.NotPassed, out isPassed);
                return codeInfos;
            }

            #endregion

            #region Step 2

            //  步骤二：检查缓存目录所在磁盘逻辑驱动器是否足够 把MB转换成G
            var spaceIsPassed = DiagnosticHelper.CheckStep(codeInfos, CheckCacheSpace);

            //  步骤二不通过则退出检查
            if (!spaceIsPassed)
            {
                DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.NotPassed, out isPassed);
                return codeInfos;
            }

            #endregion

            DiagnosticHelper.NotifyStatus(StatusEvent, DiagnosticStatus.OK, out isPassed);
            return codeInfos;
        }

        /// <summary>
        ///     检查配置的缓存目录所在磁盘是否存在
        /// </summary>
        /// <returns>诊断码</returns>
        private CodeInfo CheckDriverIsExist()
        {
            var driverIsExistCodeInfo = DiagnosticHelper.CheckDiscIsExist(
                BufferLocation,
                CodeList.Find(code => code.ToString().Contains("CacheDiscNotExist_Error")));
            return driverIsExistCodeInfo;
        }

        /// <summary>
        ///     检查配置的缓存目录剩余空间是否足够
        /// </summary>
        /// <returns>诊断码</returns>
        private CodeInfo CheckCacheSpace()
        {
            var cacheDiscOutOfSpaceCodeInfo = DiagnosticHelper.CheckFreeSpace(
                BufferLocation,
                BufferSize,
                CodeList.Find(code => code.ToString().Contains("CacheDiscOutOfSpace_Error")));
            return cacheDiscOutOfSpaceCodeInfo;
        }

        #endregion
    }
}