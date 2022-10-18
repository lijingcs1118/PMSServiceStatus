using System;

namespace Baosight.FDAA.PackageDiagnosis.Model.Attributes
{
    public class ChineseAttribute : Attribute
    {
        public ChineseAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}