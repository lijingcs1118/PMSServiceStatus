namespace Baosight.FDAA.PackageDiagnosis.Model.Modules
{
    public enum FAU_OPER_MODE
    {
        FAU_GET_INFO = 0,
        FAU_DOWNLOAD_ADDRESSBOOK = 1,
        FAU_DOWNLOAD_CURRENTIO = 2,
        FAU_TEST_CONNECTION = 3
    }

    public class FAUDevice
    {
        private readonly int _fastestSamplingRate = 100;
        private readonly uint _no;
        private readonly string _protocol;
        private readonly int _signalCapacity = 1024;
        private readonly string _type = string.Empty;

        // 用于在FAU节点下添加模块时，上下文菜单中的文字
        private string _subtype = string.Empty;
        private string ip = string.Empty;

        public FAUDevice(uint no, string name, string type, string ip)
        {
            // 根据设备类型字符串，解析出各个参数
            if (type.Contains("2048"))
                _signalCapacity = 2048;
            else if (type.Contains("1024"))
                _signalCapacity = 1024;
            else if (type.Contains("512"))
                _signalCapacity = 512;
            else if (type.Contains("4096"))
                _signalCapacity = 4096;
            else if (type.Contains("8192"))
                _signalCapacity = 8192;
            else
                _signalCapacity = 512;

            if (type.Contains("50"))
                _fastestSamplingRate = 50;
            else if (type.Contains("200"))
                _fastestSamplingRate = 200;
            else if (type.Contains("20"))
                _fastestSamplingRate = 20;
            else if (type.Contains("100"))
                _fastestSamplingRate = 100;
            else
                _fastestSamplingRate = 100;

            if (type.Contains("TCNet"))
            {
                _protocol = "TCNet";
            }
            else if (type.Contains("µΣNetwork"))
            {
                _protocol = "µΣNetwork";
            }
            else if (type.Contains("MELSEC"))
            {
                _protocol = "MELSEC";
            }
            else if (type.Contains("CPNet"))
            {
                _protocol = "CPNet";
            }
            else if (type.Contains("NISDAS"))
            {
                _protocol = "NISDAS";
            }
            else if (type.Contains("SSE"))
            {
                // protocol等同于FAU设备的子类型。譬如：FAU-SSE-Trumpf，则protocol为SSE-Trumpf, subtype为Trumpf
                var len = type.Length;
                _protocol = type.Substring(4, len - 4);
                Subtype = type.Substring(8, len - 8);
            }

            _no = no;
            this.ip = ip ?? "192.168.3.127";

            if (name == string.Empty)
                Name = "FAU_" + _no;
            else
                Name = name;
            _type = type;
        }

        public string Subtype
        {
            get { return _subtype; }
            set { _subtype = value; }
        }

        public uint No
        {
            get { return _no; }
            set
            {
                //_no = value; 
            }
        }

        public string Name { get; set; }

        public string Protocol
        {
            get { return _protocol; }
            set
            {
                //_protocol = value; 
            }
        }

        public string Type
        {
            get { return _type; }
            set
            {
                //_type = value; 
            }
        }

        public int SignalCapacity
        {
            get { return _signalCapacity; }
            set
            {
                //_signalCapacity = value; 
            }
        }

        public int FastestSamplingRate
        {
            get { return _fastestSamplingRate; }
            set
            {
                //_fastestSamplingRate = value; 
            }
        }

        public string Ip
        {
            get { return ip; }
            // ip地址取值校验
            set { ip = value; }
        }
    }
}