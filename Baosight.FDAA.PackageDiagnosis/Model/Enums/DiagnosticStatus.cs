using Baosight.FDAA.PackageDiagnosis.Model.Attributes;

namespace Baosight.FDAA.PackageDiagnosis.Model.Enums
{
    public enum DiagnosticStatus
    {
        [Chinese("未开始")] [English("Not started")]
        NotStarted,

        [Chinese("通过")] [English("OK")] OK,

        [Chinese("未通过")] [English("Not passed")]
        NotPassed,

        [Chinese("正在检查")] [English("In progress")]
        Diagnosing
    }
}