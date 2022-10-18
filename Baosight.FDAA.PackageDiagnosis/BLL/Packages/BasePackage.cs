using System;
using System.Collections.Generic;
using Baosight.FDAA.PackageDiagnosis.Model.Entities;

namespace Baosight.FDAA.PackageDiagnosis.BLL.Packages
{
    public abstract class BasePackage : IPackage
    {
        public abstract string PackageName { get; }

        public abstract bool IsConfigured { get; }

        public abstract bool IsPassed { get; }

        public abstract List<CodeInfo> Diagnose();

        public abstract event EventHandler<StatusEventArgs> StatusEvent;
    }
}