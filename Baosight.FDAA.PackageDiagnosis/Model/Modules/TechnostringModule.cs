using Baosight.FDAA.PackageDiagnosis.DAL.Configs;
using Baosight.FDAA.PackageDiagnosis.Model.Enums;

namespace Baosight.FDAA.PackageDiagnosis.Model.Modules
{
    public class TechnostringModule : BaseModule
    {
        public TechnostringModule(uint moduleNo, ModuleType moduleType)
        {
            ModuleNo = moduleNo;
            ModuleName = "Technostring" + moduleNo;
            MdType = moduleType;

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