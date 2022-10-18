using System.Collections.Generic;

namespace Baosight.FDAA.PackageDiagnosis.Model.DsEntity
{
    public class HDObject : DSObject
    {
        public Dictionary<int, HDServer> hDServers = new Dictionary<int, HDServer>();

        public HDObject()
        {
        }

        public HDObject(string id, DATA_STORAGE_TYPE dsType)
            : base(id, dsType)
        {
        }

        public Dictionary<int, HDServer> HDServers
        {
            get { return hDServers; }
            set { hDServers = value; }
        }
    }
}