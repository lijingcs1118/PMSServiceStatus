using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSServiceStatus.Modules
{
    public class BaseModule
    {
        private UInt32 _moduleNo = 0;
        public System.UInt32 ModuleNo
        {
            get { return _moduleNo; }
            set
            {
                _moduleNo = value;
            }
        }

        private string _moduleName = "Module Name";
        public string ModuleName
        {
            get { return _moduleName; }
            set { _moduleName = value; }
        }

        private bool _enabled = true;
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        private UInt32 _timeBase = 10;
        public System.UInt32 TimeBase
        {
            get { return _timeBase; }
            // 验证是否为基准时钟的整数倍
            set
            {
                _timeBase = value;
            }
        }

        PMSServiceStatus.IOConfig.MODULE_TYPE _mdType;
        public PMSServiceStatus.IOConfig.MODULE_TYPE MdType
        {
            get { return _mdType; }
            set { _mdType = value; }
        }

        /// <summary>
        /// 为AnalogGrid提供的数据源
        /// </summary>
        DataTable _analogDataTable = new DataTable("AnalogSignals");
        public System.Data.DataTable AnalogDataTable
        {
            get { return _analogDataTable; }
            set { _analogDataTable = value; }
        }

        /// <summary>
        /// 为DigitalGrid提供的数据源
        /// </summary>
        DataTable _digitalDataTable = new DataTable("DigitalSignals");
        public System.Data.DataTable DigitalDataTable
        {
            get { return _digitalDataTable; }
            set { _digitalDataTable = value; }
        }

        DataTable _sectionDataTable = new DataTable("Sections");
        public DataTable SectionDataTable
        {
            get { return _sectionDataTable; }
            set { _sectionDataTable = value; }
        }
    }
}
