using Baosight.FDAA.PackageDiagnosis.Model.Entities;

namespace Baosight.FDAA.PackageDiagnosis.BLL.Packages
{
    public interface ILicense
    {
        bool IsLicensed { get; }
        CodeInfo CheckLicense();
    }
}