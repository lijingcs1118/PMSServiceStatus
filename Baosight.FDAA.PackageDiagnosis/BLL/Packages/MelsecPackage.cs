using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Baosight.FDAA.PackageDiagnosis.Model.Entities;
using Baosight.FDAA.PackageDiagnosis.Model.Modules;

namespace Baosight.FDAA.PackageDiagnosis.BLL.Packages
{
    public class MelsecPackage : BasePackage
    {
        private bool isConfigured;
        private bool isPassed;

        public ObservableCollection<MELSECModule> MelsecModules;
        private string packageName;

        public override string PackageName
        {
            get { return packageName; }
        }

        public override bool IsConfigured
        {
            get { return isConfigured; }
        }

        public override bool IsPassed
        {
            get { return isPassed; }
        }

        public override List<CodeInfo> Diagnose()
        {
            throw new NotImplementedException();
        }

        public override event EventHandler<StatusEventArgs> StatusEvent;
    }
}