namespace Baosight.FDAA.PackageDiagnosis.Model.Entities
{
    public struct S7Status
    {
        public ushort ModuleNo;
        public uint ExecuteTimeMin;
        public uint ExecuteTimeMax;
        public uint ExecuteTimeAve;
        public uint ExecuteTimeActual;
        public int StatusCode;
        public ushort ExecuteTimes;
    }
}