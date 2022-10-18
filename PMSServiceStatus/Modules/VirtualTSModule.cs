using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSServiceStatus.Modules
{
    class VirtualTSModule:BaseModule
    {
        public VirtualTSModule(UInt32 moduleNo, PMSServiceStatus.IOConfig.MODULE_TYPE moduleType)
        {
            base.ModuleNo = moduleNo;
            base.MdType = moduleType;

            if (ServerConfig.getInstance().Language)
            {
                // 为Grid添加列
                SectionDataTable.Columns.Add("Address");
                SectionDataTable.Columns.Add("Name");
                SectionDataTable.Columns.Add("Path");
                SectionDataTable.Columns.Add("Data Type");
                SectionDataTable.Columns.Add("Actual Value");
            }
            else
            {
                // 为Grid添加列
                SectionDataTable.Columns.Add("地址");
                SectionDataTable.Columns.Add("信号名称");
                SectionDataTable.Columns.Add("脚本文件");
                SectionDataTable.Columns.Add("数据类型");
                SectionDataTable.Columns.Add("实际值");
            }
        }
    }
}
