using System;

namespace PMSServiceStatus.Modules
{
    public class UDPMulticastModule:BaseModule
    {
        public UDPMulticastModule(UInt32 moduleNo, IOConfig.MODULE_TYPE moduleType)
        {
            ModuleNo = moduleNo;

            if(ServerConfig.getInstance().Language)
            {
                // 添加analog table的列
                AnalogDataTable.Columns.Add("Address");
                AnalogDataTable.Columns.Add("Name");
                AnalogDataTable.Columns.Add("Unit");
                AnalogDataTable.Columns.Add("Gain");
                AnalogDataTable.Columns.Add("Offset");
                AnalogDataTable.Columns.Add("DataType");
                AnalogDataTable.Columns.Add("Actual Value");

                // 添加digital table的列
                DigitalDataTable.Columns.Add("Address");
                DigitalDataTable.Columns.Add("Name");
                DigitalDataTable.Columns.Add("Offset", Type.GetType("System.String"));
                DigitalDataTable.Columns.Add("BitNo");
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
                AnalogDataTable.Columns.Add("偏移地址");
                AnalogDataTable.Columns.Add("数据类型");
                AnalogDataTable.Columns.Add("实际值");

                // 添加digital table的列
                DigitalDataTable.Columns.Add("地址");
                DigitalDataTable.Columns.Add("信号名称");
                DigitalDataTable.Columns.Add("偏移地址", Type.GetType("System.String"));
                DigitalDataTable.Columns.Add("比特位");
                DigitalDataTable.Columns.Add("数据类型");
                DigitalDataTable.Columns.Add("实际值");
            }
            // Module Type
            MdType = moduleType;
        }
    }
}