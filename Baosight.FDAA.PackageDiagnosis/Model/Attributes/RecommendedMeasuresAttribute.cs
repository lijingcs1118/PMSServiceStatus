using System;

namespace Baosight.FDAA.PackageDiagnosis.Model.Attributes
{
    public class RecommendedMeasuresAttribute : Attribute
    {
        public RecommendedMeasuresAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}