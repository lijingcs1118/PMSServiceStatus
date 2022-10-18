using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSServiceStatus.Modules
{
    class RFM5565Module : BaseModule
    {
        public RFM5565Module(UInt32 moduleNo, PMSServiceStatus.IOConfig.MODULE_TYPE moduleType)
        {
            ModuleNo = moduleNo;

            if(ServerConfig.getInstance().Language)
            {
                // 添加analog table的列
                AnalogDataTable.Columns.Add("Address");
                AnalogDataTable.Columns.Add("Name");
                AnalogDataTable.Columns.Add("Unit");
                AnalogDataTable.Columns.Add("Gain");
                AnalogDataTable.Columns.Add("Rfm Address");
                AnalogDataTable.Columns.Add("DataType");
                // Add 'Active' column to the table:
                AnalogDataTable.Columns.Add("Actual Value");

                // 添加digital table的列
                DigitalDataTable.Columns.Add("Address");
                DigitalDataTable.Columns.Add("Name");
                DigitalDataTable.Columns.Add("Rfm Address", System.Type.GetType("System.String"));
                DigitalDataTable.Columns.Add("Bit No");
                DigitalDataTable.Columns.Add("DataType");
                DigitalDataTable.Columns.Add("Actual Value");
            }
            else
            {
                // 添加analog table的列
                AnalogDataTable.Columns.Add("地址");
                AnalogDataTable.Columns.Add("信号名称");
                AnalogDataTable.Columns.Add("单位");
                AnalogDataTable.Columns.Add("增益");
                AnalogDataTable.Columns.Add("反射内存网地址");
                AnalogDataTable.Columns.Add("数据类型");
                // Add 'Active' column to the table:
                AnalogDataTable.Columns.Add("实际值");

                // 添加digital table的列
                DigitalDataTable.Columns.Add("地址");
                DigitalDataTable.Columns.Add("信号名称");
                DigitalDataTable.Columns.Add("反射内存网地址", System.Type.GetType("System.String"));
                DigitalDataTable.Columns.Add("比特位");
                DigitalDataTable.Columns.Add("数据类型");
                DigitalDataTable.Columns.Add("实际值");
            }

            // Module Type和Module Index赋值
            MdType = moduleType;
        }
    }
}
