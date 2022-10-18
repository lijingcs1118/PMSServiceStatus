using System.Data;
using Baosight.FDAA.PackageDiagnosis.DAL.Configs;
using Baosight.FDAA.PackageDiagnosis.Model.Enums;

namespace Baosight.FDAA.PackageDiagnosis.Model.Modules
{
    public class BaseModule
    {
        /// <summary>
        ///     为AnalogGrid提供的数据源
        /// </summary>
        private DataTable _analogDataTable = new DataTable("AnalogSignals");

        /// <summary>
        ///     为DigitalGrid提供的数据源
        /// </summary>
        private DataTable _digitalDataTable = new DataTable("DigitalSignals");

        private bool _enabled = true;

        private string _moduleName = "Module Name";

        private DataTable _sectionDataTable = new DataTable("Sections");

        private uint _timeBase = 10;

        public BaseModule()
        {
            ModuleNo = 0;
        }

        public uint ModuleNo { get; set; }

        public string ModuleName
        {
            get { return _moduleName; }
            set { _moduleName = value; }
        }

        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public uint TimeBase
        {
            get { return _timeBase; }
            // 验证是否为基准时钟的整数倍
            set { _timeBase = value; }
        }

        public ModuleType MdType { get; set; }

        public DataTable AnalogDataTable
        {
            get { return _analogDataTable; }
            set { _analogDataTable = value; }
        }

        public DataTable DigitalDataTable
        {
            get { return _digitalDataTable; }
            set { _digitalDataTable = value; }
        }

        public DataTable SectionDataTable
        {
            get { return _sectionDataTable; }
            set { _sectionDataTable = value; }
        }
    }
}