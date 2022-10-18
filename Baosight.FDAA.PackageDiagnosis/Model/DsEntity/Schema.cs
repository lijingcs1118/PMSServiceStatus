using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Baosight.FDAA.PackageDiagnosis.Model.DsEntity
{
    public class Schema
    {
        private int flushCycle;
        private string replaceSpaceWith;
        private List<string> signals = new List<string>();
        private int timebase;

        public Schema()
        {
            timebase = 1000;
        }

        public Schema(int No)
        {
            this.No = No;
            Name = "Scheme" + this.No;
            Enabled = true;
            NumberofAnalogSignals = 0;
            NumberofDigitalSignals = 0;
            flushCycle = 1;
            FilterMode = "无";
            timebase = 1000;
            Prefix = Name;
            AddPrefixToTagName = true;
            AddSignalName = false;
            replaceSpaceWith = "_";
            Type = "Archive";
            SyncTagDescription = true;
            SyncroDelete = false;
            AnalogDataType = "float64";
        }

        [DisplayName("0、编号")]
        [Description("存储方案识别号，HD内唯一。")]
        [Category("常规")]
        [ReadOnly(true)]
        public int No { get; set; }

        [DisplayName("1、名称")]
        [Description("存储方案名称，默认Schema+编号。")]
        [Category("常规")]
        [ReadOnly(false)]
        public string Name { get; set; }


        [DisplayName("2、是否启用")]
        [DefaultValue(true)]
        [Description("启用或禁用当前存储方案。\n注意：一旦存储模块被禁用，数据采集服务将忽略处理该存储方案内的所有信号。")]
        [Category("常规")]
        [ReadOnly(false)]
        public bool Enabled { get; set; }

        [DisplayName("3、模拟量信号数量")]
        [Description("当前存储方案已经启用的模拟量信号个数。\n注意：只读属性，由系统根据信号组态情况自动计算得出。")]
        [DefaultValue(0)]
        [Category("常规")]
        [ReadOnly(true)]
        public int NumberofAnalogSignals { get; set; }

        [DisplayName("4、开关量信号数量")]
        [Description("当前存储方案已经启用的开关量信号个数。\n注意：只读属性，由系统根据信号组态情况自动计算得出。")]
        [DefaultValue(0)]
        [Category("常规")]
        [ReadOnly(true)]
        public int NumberofDigitalSignals { get; set; }

        [DisplayName("5、写入周期")]
        [DefaultValue(1)]
        [Description("当前存储方案向磁盘缓冲数据间隔。可设置范围：100-10000，单位：毫秒")]
        [Category("常规")]
        [ReadOnly(false)]
        public int FlushCycle
        {
            get { return flushCycle; }
            set
            {
                if (value >= 1 && value <= 10 && value * 1000 >= timebase && value * 1000 % timebase == 0)
                    flushCycle = value;
                else
                    return;
            }
        }

        [DisplayName("0、时基")]
        [DefaultValue(1000)]
        [Description("当前存储方案重采样后数据的颗粒度。可设置范围：10-10000，单位：毫秒 \n注意：")]
        [Category("策略")]
        [ReadOnly(false)]
        public int Timebase
        {
            get { return timebase; }
            set
            {
                if (value >= 10 && value <= 10000 && value <= flushCycle * 1000 && flushCycle * 1000 % value == 0)
                    timebase = value;
                else
                    return;
            }
        }

        [DisplayName("1、过滤模式")]
        [DefaultValue("无")]
        [Description("向HD转存采样值时，当前存储方案使用哪种数据计算方式：无（最后一个值）、最大值、最小值、平均值。")]
        [Category("策略")]
        [ReadOnly(false)]
        public string FilterMode { get; set; }

        [DisplayName("0、Tag点前缀")]
        [DefaultValue(true)]
        [Description("在HD中创建Tag点时，Tag点前缀会作为TAG点名的一部分，以保证Tag点名的唯一性，默认值等同于schema名称。")]
        [Category("Tag")]
        [ReadOnly(false)]
        public string Prefix { get; set; }

        [DisplayName("0、添加Tag点前缀Tag名")]
        [DefaultValue(true)]
        [Description("是否添加Tag点前缀Tag名")]
        [Category("Tag")]
        [ReadOnly(false)]
        public bool AddPrefixToTagName { get; set; }

        [DisplayName("0、添加信号名称")]
        [DefaultValue(false)]
        [Description("")]
        [Category("Tag")]
        [ReadOnly(false)]
        public bool AddSignalName { get; set; }

        [DisplayName("0、空格替换符")]
        [DefaultValue("_")]
        [Description("")]
        [Category("Tag")]
        [ReadOnly(false)]
        public string ReplaceSpaceWith
        {
            get { return replaceSpaceWith; }
            set
            {
                //不允许包含空格，可以包含中文、数字和英文字母，仅支持一些特殊英文字符：:()@%#+=[]_.-
                var r = new Regex(@"^[\u4e00-\u9fa50-9a-zA-Z()%#+=[\]_.-]*$");
                var match = r.IsMatch(value);
                if (match == false || value.Length > 1)
                    return;
                replaceSpaceWith = value;
            }
        }

        [DisplayName("1、Tag点类型")]
        [DefaultValue("Archive")]
        [Description("以何种形式存放Tag点（刷新/归档）。")]
        [Category("Tag")]
        [ReadOnly(false)]
        public string Type { get; set; }

        [DisplayName("1、模拟量数据类型")]
        [DefaultValue("float64")]
        [Description("创建Tag时，模拟量信号的数据类型，float32占用较少存储资源，float64精度更高。")]
        [Category("Tag")]
        [ReadOnly(false)]
        public string AnalogDataType { get; set; }

        [DisplayName("2、同步信号描述")]
        [DefaultValue(true)]
        [Description("FDAA中修改了信号名称时，HD是否同步更新Tag点描述")]
        [Category("Tag")]
        [ReadOnly(false)]
        public bool SyncTagDescription { get; set; }

        [Browsable(false)]
        [DisplayName("3、同步删除")]
        [DefaultValue(false)]
        [Description("FDAA中删除信号时，HD是否同步删除对应的Tag点")]
        [Category("Tag")]
        [ReadOnly(false)]
        public bool SyncroDelete { get; set; }

        [Browsable(false)]
        public List<string> Signals
        {
            get { return signals; }
            set { signals = value; }
        }
    }
}