using System.Collections.Generic;

namespace Baosight.FDAA.PackageDiagnosis.Model.Modules
{
    public class FDAAPMSServiceManager
    {
        private Dictionary<uint, FDAAPMSService> _PMSServiceCollection = new Dictionary<uint, FDAAPMSService>();

        public Dictionary<uint, FDAAPMSService> PMSServiceCollection
        {
            get { return _PMSServiceCollection; }
            set { _PMSServiceCollection = value; }
        }

        public void addPMSService(FDAAPMSService pms)
        {
            if (!_PMSServiceCollection.ContainsKey(pms.No))
                _PMSServiceCollection.Add(pms.No, pms);
        }

        public FDAAPMSService addPMSService()
        {
            var pmsService = new FDAAPMSService();
            pmsService.Ip = "255.255.255.255";
            pmsService.Name = "PMS Service";
            pmsService.Port = 8089;
            pmsService.prefix = "pms";
            pmsService.usePrefixName = false;
            pmsService.No = getAvailablePMSServiceNo();

            _PMSServiceCollection.Add(pmsService.No, pmsService);

            return pmsService;
        }

        public FDAAPMSService getPMSService(uint no)
        {
            if (_PMSServiceCollection.ContainsKey(no))
                return _PMSServiceCollection[no];
            return null;
        }

        public uint getAvailablePMSServiceNo()
        {
            var finded = false;
            uint no = 0;
            while (!finded)
                if (_PMSServiceCollection.ContainsKey(no))
                    no++;
                else
                    finded = true;

            return no;
        }

        public void initPMSServices()
        {
            _PMSServiceCollection.Clear();
        }
    }
}