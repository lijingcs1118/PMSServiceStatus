using System;
using Baosight.FDAA.PackageDiagnosis.Model.Enums;

namespace Baosight.FDAA.PackageDiagnosis.Model.Entities
{
    public class StatusEventArgs : EventArgs
    {
        public StatusEventArgs(DiagnosticStatus diagnosticStatus)
        {
            DiagnosticStatus = diagnosticStatus;
        }

        public DiagnosticStatus DiagnosticStatus { get; set; }
    }
}