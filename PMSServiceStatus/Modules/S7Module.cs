using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSServiceStatus.Modules
{
    class S7Module : BaseModule
    {
        public S7Module(UInt32 moduleNo, PMSServiceStatus.IOConfig.MODULE_TYPE moduleType)
        {
            ModuleNo = moduleNo;

            if(ServerConfig.getInstance().Language)
            {
                // 添加analog table的列
                AnalogDataTable.Columns.Add("Address");
                AnalogDataTable.Columns.Add("Name");
                AnalogDataTable.Columns.Add("Unit");
                AnalogDataTable.Columns.Add("Gain");
                AnalogDataTable.Columns.Add("S7 Symbol");
                AnalogDataTable.Columns.Add("S7 Operand");
                AnalogDataTable.Columns.Add("DataType");
                AnalogDataTable.Columns.Add("Actual Value");

                // 添加digital table的列
                DigitalDataTable.Columns.Add("Address");
                DigitalDataTable.Columns.Add("Name");
                DigitalDataTable.Columns.Add("S7 Symbol");
                DigitalDataTable.Columns.Add("S7 Operand");
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
                AnalogDataTable.Columns.Add("S7 符号");
                AnalogDataTable.Columns.Add("S7 操作符");
                AnalogDataTable.Columns.Add("数据类型");
                AnalogDataTable.Columns.Add("实际值");

                // 添加digital table的列
                DigitalDataTable.Columns.Add("地址");
                DigitalDataTable.Columns.Add("信号名称");
                DigitalDataTable.Columns.Add("S7 符号");
                DigitalDataTable.Columns.Add("S7 操作符");
                DigitalDataTable.Columns.Add("数据类型");
                DigitalDataTable.Columns.Add("实际值");
            }
            // Module Type
            MdType = moduleType;
        }
    }
}
