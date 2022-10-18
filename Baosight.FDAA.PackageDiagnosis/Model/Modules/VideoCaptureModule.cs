using Baosight.FDAA.PackageDiagnosis.DAL.Configs;
using Baosight.FDAA.PackageDiagnosis.Model.Enums;

namespace Baosight.FDAA.PackageDiagnosis.Model.Modules
{
    public class VideoCaptureModule : BaseModule
    {
        public VideoCaptureModule(uint moduleNo, ModuleType moduleType)
        {
            ModuleNo = moduleNo;

            if (ServerConfig.getInstance().Language)
            {
                // 添加analog table的列
                AnalogDataTable.Columns.Add("Address");
                AnalogDataTable.Columns.Add("Name");
                AnalogDataTable.Columns.Add("Unit");
                AnalogDataTable.Columns.Add("Gain");
                AnalogDataTable.Columns.Add("Actual Value");
                AnalogDataTable.Columns.Add("Address");
                AnalogDataTable.Columns.Add("IPAddress");
                AnalogDataTable.Columns.Add("CameraName");
            }
            else
            {
                // 添加analog table的列
                AnalogDataTable.Columns.Add("地址");
                AnalogDataTable.Columns.Add("信号名称");
                AnalogDataTable.Columns.Add("单位");
                AnalogDataTable.Columns.Add("增益");
                AnalogDataTable.Columns.Add("实际值");
                AnalogDataTable.Columns.Add("Address");
                AnalogDataTable.Columns.Add("IPAddress");
                AnalogDataTable.Columns.Add("CameraName");
            }

            // Module Type
            MdType = moduleType;
        }
    }
}