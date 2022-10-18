using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSServiceStatus.Modules
{
    class TechnostringModule:BaseModule
    {
        public TechnostringModule(UInt32 moduleNo, PMSServiceStatus.IOConfig.MODULE_TYPE moduleType)
        {
            base.ModuleNo = moduleNo;
            base.ModuleName = "Technostring" + moduleNo.ToString();
            base.MdType = moduleType;

            if (ServerConfig.getInstance().Language)
            {
                // 为Grid添加列
                SectionDataTable.Columns.Add("Address");
                SectionDataTable.Columns.Add("Name");
                SectionDataTable.Columns.Add("Time");
                SectionDataTable.Columns.Add("Actual Value");
            }
            else
            {
                // 为Grid添加列
                SectionDataTable.Columns.Add("地址");
                SectionDataTable.Columns.Add("信号名称");
                SectionDataTable.Columns.Add("时间");
                SectionDataTable.Columns.Add("实际值");
            }
        }
    }
}
