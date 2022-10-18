namespace Baosight.FDAA.PackageDiagnosis.Model.Entities
{
    public class Telegram
    {
        //  报文号
        public int ModuleIndex { get; set; }

        //  报头记录的长度
        public uint Length { get; set; }

        //  报文实际长度
        public uint ActualLength { get; set; }
    }
}