using System;

namespace Baosight.FDAA.PackageDiagnosis.Model.Attributes
{
    public class EnglishAttribute : Attribute
    {
        public EnglishAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}