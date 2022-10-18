using Baosight.FDAA.PackageDiagnosis.Model.Entities;

namespace Baosight.FDAA.PackageDiagnosis.BLL.Packages
{
    public interface IInstallation
    {
        bool IsInstalled { get; }
        CodeInfo CheckInstallation();
    }
}