using System.Linq;
using Baosight.FDAA.PackageDiagnosis.Model.Entities;
using Baosight.FDAA.PackageDiagnosis.Model.Modules;

namespace Baosight.FDAA.PackageDiagnosis.BLL.Packages
{
    public class S7DpRequestPackage<T> : BoardCardWithDataReceivedPackage<T> where T : BaseModule
    {
        /// <summary>
        ///     检查模块内部分信号接受数据
        /// </summary>
        /// <returns>诊断码</returns>
        protected override CodeInfo CheckPartialDataReceived()
        {
            var modules = Modules.Where(m =>
                !m.ModuleName.ToUpper().Contains("FM458") && !m.ModuleName.ToUpper().Contains("TDC"));

            var partialReceivedCodeInfo = DiagnosticHelper.CheckPartialReceivedData(
                CodeList.Find(code => code.ToString().Contains("PartialNotReceivedData_Error")),
                modules);

            return partialReceivedCodeInfo;
        }
    }
}