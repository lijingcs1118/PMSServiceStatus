using System.Collections.Generic;

namespace Baosight.FDAA.PackageDiagnosis.Model.Modules
{
    public class FDAAPMSService
    {
        // 回放器中包含的模块号
        private List<uint> _moduleNo = new List<uint>();

        private int port;

        private int reconnectionPeriod;

        public FDAAPMSService()
        {
            ReconnectionPeriod = 3;
        }

        public uint No { get; set; }

        public string Ip { get; set; } // 需要校验，不能重复

        public int Port
        {
            get { return port; }
            set
            {
                if (value >= 1024 && value <= 65535) port = value;
            }
        } // 默认8089

        public string Name { get; set; } // 显示在接口树中

        public string prefix { get; set; } // 前缀字符，如果启用，添加模块和映射信号时，在头部自动添加前缀字符

        public bool usePrefixName { get; set; } // 是否启用前缀字符，默认不启用

        public int ReconnectionPeriod
        {
            get { return reconnectionPeriod; }
            set
            {
                if (value >= 1 && value <= 10) reconnectionPeriod = value;
            }
        } // 服务端开后，重连间隔，默认3秒

        public List<uint> ModuleNoCollection
        {
            get { return _moduleNo; }
            set { _moduleNo = value; }
        }

        public void addModule(uint moduleNo)
        {
            _moduleNo.Add(moduleNo);
        }

        public void removeModule(uint moduleNo)
        {
            _moduleNo.Remove(moduleNo);
        }

        public void clearModules()
        {
            _moduleNo.Clear();
        }
    }
}