using System.Collections.Generic;

namespace Baosight.FDAA.PackageDiagnosis.Model.Modules
{
    public class FAUDeviceManager
    {
        private Dictionary<uint, FAUDevice> _fauCollection = new Dictionary<uint, FAUDevice>();

        public Dictionary<uint, FAUDevice> FauCollection
        {
            get { return _fauCollection; }
            set { _fauCollection = value; }
        }

        public void initFAUs()
        {
            _fauCollection.Clear();
        }

        public void addFAU(FAUDevice fau)
        {
            if (!_fauCollection.ContainsKey(fau.No))
                _fauCollection.Add(fau.No, fau);
        }

        public FAUDevice addFAU(uint no, string name, string fauType, string ip)
        {
            var fau = new FAUDevice(no, name, fauType, ip);
            _fauCollection.Add(fau.No, fau);
            return fau;
        }

        public FAUDevice addFAU(string fauType)
        {
            var fau = new FAUDevice(getAvailableFAUNo(), string.Empty, fauType, null);
            _fauCollection.Add(fau.No, fau);
            return fau;
        }

        public void removeFAU(uint no)
        {
            if (_fauCollection.ContainsKey(no))
                _fauCollection.Remove(no);
        }

        public FAUDevice getFAU(uint no)
        {
            if (_fauCollection.ContainsKey(no))
                return _fauCollection[no];
            return null;
        }

        public uint getAvailableFAUNo()
        {
            var finded = false;
            uint fauNo = 0;
            while (!finded)
                if (_fauCollection.ContainsKey(fauNo))
                    fauNo++;
                else
                    finded = true;

            return fauNo;
        }
    }
}