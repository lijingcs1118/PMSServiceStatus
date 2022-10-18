using System.Collections.Generic;
using Baosight.FDAA.PackageDiagnosis.Model.Entities;

namespace Baosight.FDAA.PackageDiagnosis.BLL.Packages
{
    public interface IPackage
    {
        List<CodeInfo> Diagnose();
    }
}