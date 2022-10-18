using System;

namespace Baosight.FDAA.PackageDiagnosis.Model.Attributes
{
    public class DetailAttribute : Attribute
    {
        public DetailAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}