using Baosight.FDAA.PackageDiagnosis.Model.Enums;

namespace Baosight.FDAA.PackageDiagnosis.Model.Entities
{
    public class CodeInfo
    {
        public CodeInfo()
        {
            Id = 0;
            Detail = string.Empty;
            SpecificInformation = string.Empty;
            RecommendedMeasures = string.Empty;
        }

        /// <summary>
        ///     结果码
        /// </summary>
        public Code Id { get; set; }

        /// <summary>
        ///     描述
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        ///     详细信息
        /// </summary>
        public string SpecificInformation { get; set; }

        /// <summary>
        ///     建议举措
        /// </summary>
        public string RecommendedMeasures { get; set; }
    }
}