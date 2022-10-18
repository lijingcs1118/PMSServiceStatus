using Baosight.FDAA.Utility;
using PMSServiceStatus.Modules;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PMSServiceStatus
{
    public class IOConfig
    {
        public static readonly IOConfig ioConfig = new IOConfig();
        public static IOConfig getInstance()
                            {
            return ioConfig;
        }

        // Module type definition
        public enum MODULE_TYPE
        {
            mtUDPInteger = 1,
            mtUDPReal = 2,
            mtUDPUserDefined = 3,
            mtUDPMulticast = 4,
            mtOPCClient = 21,
            mtDPLiteInteger = 41,
            mtDPLiteReal = 42,
            mtDPLiteDoubleInteger = 43,
            mtRFM5565 = 61,
            mtS7TCP300 = 81,
            mtS7TCP400DB = 82,
            mtS7TCP400NoneDB = 83,
            mtS7TCP = 84,
            mtS7DPRequest = 91,
            mtS7PNRequest = 92,
            mtArti3 = 96,
            mtMelsecQ = 101,
            mtCP3550 = 110,
            mtToshibaV3000 = 120,
            mtToshibaNV = 121,
            mtHitachiR700 = 130,
            mtHitachiR900 = 131,
            mtNisdas = 140,
            mtFDAA = 150,
            mtVirtualforLua = 160,
            mtVirtualforLuaTS = 161,
            mtPlayback = 241,
            mtVideoCapture = 255,
            mtTsFAU = 300,
            mtTsOPC = 301,
            mtTsUDP = 302,
            mtSme = 500
        };

        private UInt32 softTimebase;
        public UInt32 SoftTimebase
        {
            get { return softTimebase; }
            set { softTimebase = value; }
        }

        int lostFrameModuleNo;
        public int LostFrameModuleNo
        {
            get { return lostFrameModuleNo; }
            set { lostFrameModuleNo = value; }
        }

        int lostFrameSignalNo;
        public int LostFrameSignalNo
        {
            get { return lostFrameSignalNo; }
            set { lostFrameSignalNo = value; }
        }

        private int maxValue;
        public int MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; }
        }

        private int minValue;
        public int MinValue
        {
            get { return minValue; }
            set { minValue = value; }
        }

        private int incrementValue;
        public int IncrementValue
        {
            get { return incrementValue; }
            set { incrementValue = value; }
        }

        public Dictionary<uint, BaseModule> AllModules = new Dictionary<uint, BaseModule>();

        private int analogCount = 0;
        public int AnalogCount
        {
            get { return analogCount; }
            set { analogCount = value; }
        }

        private int digitalCount = 0;
        public int DigitalCount
        {
            get { return digitalCount; }
            set { digitalCount = value; }
        }

        private int tecnostringCount = 0;
        public int TecnostringCount
        {
            get { return tecnostringCount; }
            set { tecnostringCount = value; }
        }

        private bool enableModuleMapping = false;
        public bool EnableModuleMapping
        {
            get { return enableModuleMapping; }
            set { enableModuleMapping = value; }
        }

        //  连接资源是否被占用
        public static int usingResource = 0;

        public void LoadIOConfig(string fileName)
        {
            analogCount = 0;
            digitalCount = 0;
            tecnostringCount = 0;
            IOConfig.getInstance().AllModules.Clear();

            // 创建XML文件读取对象
            XmlParser parser = new XmlParser();

            try
            {
                // 加载指定的xml文件
                parser.LoadConfig(fileName);
            }
            catch (Exception ex)
            {
                // 如果读取文件有异常，则提示
                MessageBox.Show(ex.Message, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                throw;
            }

            // 读取General信息
            MyNode nodeGeneral = parser.ParsedNode.NodeList[0];
            softTimebase = Convert.ToUInt32(nodeGeneral.GetValueByName("SoftTimebase"));
            // 获取UDP工作模式
            if (nodeGeneral.NodeList.Count > 16)
            {
                enableModuleMapping = Convert.ToBoolean(Convert.ToInt32(nodeGeneral.GetValueByName("EnableModuleMapping")));
            }

            // 读取FrameLostCounter信息
            if (nodeGeneral.NodeList.Count > 1)
            {
                MyNode nodeFrameLostCounter = nodeGeneral.NodeList[1];
                string sigAddress = nodeFrameLostCounter.GetValueByName("SignalAddress");
                // 从信号地址中截取module号和signal号
                int len = sigAddress.Length;
                int spaceIndex = sigAddress.IndexOf(":");
                if (spaceIndex != -1)
                {
                    string temp = sigAddress.Substring(0, spaceIndex);
                    IOConfig.getInstance().LostFrameModuleNo = Convert.ToInt32(temp);

                    temp = sigAddress.Substring(spaceIndex + 1, len - spaceIndex - 1);
                    IOConfig.getInstance().LostFrameSignalNo = Convert.ToInt32(temp);
                }
                IOConfig.getInstance().MaxValue = Convert.ToInt32(nodeFrameLostCounter.GetValueByName("MaxValue"));
                IOConfig.getInstance().MinValue = Convert.ToInt32(nodeFrameLostCounter.GetValueByName("MinValue"));
                IOConfig.getInstance().IncrementValue = Convert.ToInt32(nodeFrameLostCounter.GetValueByName("IncrementValue"));
            }

            MyNode nodeModules = parser.ParsedNode.NodeList[1];
            UInt32 moduleNo;
            bool enabled;
            MODULE_TYPE moduleType;
            try
            {
                foreach (MyNode nodeModule in nodeModules.NodeList)
                {
                    // 获取ModuleNo和模块类型
                    moduleNo = Convert.ToUInt32(nodeModule.GetValueByName("ModuleNo"));
                    moduleType = (MODULE_TYPE)Convert.ToUInt32(nodeModule.GetValueByName("ModuleType"));
                    enabled = Convert.ToBoolean(Convert.ToInt32(nodeModule.GetValueByName("Enabled")));

                    if (enabled == false) continue;

                    #region UDP
                    // 如果模块是UDP类型的
                    if (moduleType == MODULE_TYPE.mtUDPInteger || moduleType == MODULE_TYPE.mtUDPReal
                         || moduleType == MODULE_TYPE.mtUDPUserDefined)
                    {
                        // 创建一个新的UDP Module对象
                        UDPModule udpModule = new UDPModule(moduleNo, moduleType);

                        udpModule.ModuleName = nodeModule.GetValueByName("Name");
                        udpModule.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        // 获取所有的Analog数据
                        MyNode nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (MyNode nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            DataRow dr = udpModule.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                                dr["Offset"] = nodeSignal.GetValueByName("Offset");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                                dr["偏移地址"] = nodeSignal.GetValueByName("Offset");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            udpModule.AnalogDataTable.Rows.Add(dr);
                            analogCount++;
                        }

                        // 获取所有的Digital数据
                        MyNode nodeDigital = nodeModule.GetNodeByName("Digital");
                        foreach (MyNode nodeSignal in nodeDigital.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            DataRow dr = udpModule.DigitalDataTable.NewRow();
                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Offset"] = nodeSignal.GetValueByName("Offset");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["偏移地址"] = nodeSignal.GetValueByName("Offset");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            udpModule.DigitalDataTable.Rows.Add(dr);
                            digitalCount++;
                        }
                        AllModules.Add(moduleNo, udpModule);
                    }
                    #endregion
                    #region UDP multicast
                    // 如果模块是UDP类型的
                    if (moduleType == MODULE_TYPE.mtUDPMulticast)
                    {
                        // 创建一个新的UDP Module对象
                        UDPMulticastModule udpMulticastModule = new UDPMulticastModule(moduleNo, moduleType);

                        udpMulticastModule.ModuleName = nodeModule.GetValueByName("Name");
                        udpMulticastModule.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        // 获取所有的Analog数据
                        MyNode nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (MyNode nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            DataRow dr = udpMulticastModule.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                                dr["Offset"] = nodeSignal.GetValueByName("Offset");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                                dr["偏移地址"] = nodeSignal.GetValueByName("Offset");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            udpMulticastModule.AnalogDataTable.Rows.Add(dr);
                            analogCount++;
                        }

                        // 获取所有的Digital数据
                        MyNode nodeDigital = nodeModule.GetNodeByName("Digital");
                        foreach (MyNode nodeSignal in nodeDigital.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            DataRow dr = udpMulticastModule.DigitalDataTable.NewRow();
                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Offset"] = nodeSignal.GetValueByName("Offset");
                                dr["BitNo"] = nodeSignal.GetValueByName("BitNo");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["偏移地址"] = nodeSignal.GetValueByName("Offset");
                                dr["比特位"] = nodeSignal.GetValueByName("BitNo");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            udpMulticastModule.DigitalDataTable.Rows.Add(dr);
                            digitalCount++;
                        }
                        AllModules.Add(moduleNo, udpMulticastModule);
                    }
                    #endregion
                    #region DP
                    // 如果模块类型是Profibus DP类型的
                    else if (moduleType == MODULE_TYPE.mtDPLiteInteger || moduleType == MODULE_TYPE.mtDPLiteReal
                             || moduleType == MODULE_TYPE.mtDPLiteDoubleInteger)
                    {
                        // 创建一个新的DP Module对象
                        DPLiteModule dpLiteModule = new DPLiteModule(moduleNo, moduleType);

                        dpLiteModule.ModuleName = nodeModule.GetValueByName("Name");
                        dpLiteModule.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        // 获取所有的Analog数据
                        MyNode nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (MyNode nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            DataRow dr = dpLiteModule.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                                dr["Offset"] = nodeSignal.GetValueByName("Offset");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                                dr["偏移地址"] = nodeSignal.GetValueByName("Offset");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            dpLiteModule.AnalogDataTable.Rows.Add(dr);
                            analogCount++;
                        }

                        // 获取所有的Digital数据
                        MyNode nodeDigital = nodeModule.GetNodeByName("Digital");
                        foreach (MyNode nodeSignal in nodeDigital.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            DataRow dr = dpLiteModule.DigitalDataTable.NewRow();
                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Offset"] = nodeSignal.GetValueByName("Offset");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["偏移地址"] = nodeSignal.GetValueByName("Offset");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            dpLiteModule.DigitalDataTable.Rows.Add(dr);
                            digitalCount++;
                        }
                        AllModules.Add(moduleNo, dpLiteModule);
                    }
                    #endregion
                    #region mtRFM5565
                    // 如果模块类型是RFM 5565类型的
                    else if (moduleType == MODULE_TYPE.mtRFM5565)
                    {
                        // 创建一个新的RFM5565 Module对象
                        RFM5565Module rfm5566Module = new RFM5565Module(moduleNo, moduleType);

                        rfm5566Module.ModuleName = nodeModule.GetValueByName("Name");
                        rfm5566Module.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        // 获取所有的Analog数据
                        MyNode nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (MyNode nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            DataRow dr = rfm5566Module.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                                dr["Rfm Address"] = String.Format("0x{0:X8}", Convert.ToInt32(nodeSignal.GetValueByName("Address")));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                                dr["反射内存网地址"] = String.Format("0x{0:X8}", Convert.ToInt32(nodeSignal.GetValueByName("Address")));
                            }
                            rfm5566Module.AnalogDataTable.Rows.Add(dr);
                            analogCount++;
                        }

                        // 获取所有的Digital数据
                        MyNode nodeDigital = nodeModule.GetNodeByName("Digital");
                        foreach (MyNode nodeSignal in nodeDigital.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            DataRow dr = rfm5566Module.DigitalDataTable.NewRow();
                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Rfm Address"] = String.Format("0x{0:X8}", Convert.ToInt32(nodeSignal.GetValueByName("Address")));
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                                dr["Bit No"] = nodeSignal.GetValueByName("BitNo");
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["反射内存网地址"] = String.Format("0x{0:X8}", Convert.ToInt32(nodeSignal.GetValueByName("Address")));
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                                dr["比特位"] = nodeSignal.GetValueByName("BitNo");
                            }
                            rfm5566Module.DigitalDataTable.Rows.Add(dr);
                            digitalCount++;
                        }
                        AllModules.Add(moduleNo, rfm5566Module);
                    }
                    #endregion
                    #region OPC
                    // 如果模块类型是OPC Client类型的
                    else if (moduleType == MODULE_TYPE.mtOPCClient)
                    {
                        // 创建一个新的OPCClient Module对象
                        OPCClientModule opcClientModule = new OPCClientModule(moduleNo, moduleType);

                        opcClientModule.ModuleName = nodeModule.GetValueByName("Name");
                        opcClientModule.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        // 获取所有的Analog数据
                        MyNode nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (MyNode nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            DataRow dr = opcClientModule.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                                dr["Item ID"] = nodeSignal.GetValueByName("ItemID");
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                                dr["数据项识别号"] = nodeSignal.GetValueByName("ItemID");
                            }
                            opcClientModule.AnalogDataTable.Rows.Add(dr);
                            analogCount++;
                        }

                        // 获取所有的Digital数据
                        MyNode nodeDigital = nodeModule.GetNodeByName("Digital");
                        foreach (MyNode nodeSignal in nodeDigital.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            DataRow dr = opcClientModule.DigitalDataTable.NewRow();
                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Item ID"] = nodeSignal.GetValueByName("ItemID");

                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["数据项识别号"] = nodeSignal.GetValueByName("ItemID");
                            }
                            opcClientModule.DigitalDataTable.Rows.Add(dr);
                            digitalCount++;
                        }
                        AllModules.Add(moduleNo, opcClientModule);
                    }
                    #endregion
                    #region S7
                    // 如果模块类型是S7类型的
                    else if ((moduleType == MODULE_TYPE.mtS7TCP300) || (moduleType == MODULE_TYPE.mtS7TCP400DB)
                            || (moduleType == MODULE_TYPE.mtS7TCP400NoneDB) || (moduleType == MODULE_TYPE.mtS7TCP) || moduleType == MODULE_TYPE.mtS7DPRequest || moduleType == MODULE_TYPE.mtS7PNRequest)
                    {
                        // 创建一个新的S7TCP Module对象
                        S7Module s7TCPModule = new S7Module(moduleNo, moduleType);

                        s7TCPModule.ModuleName = nodeModule.GetValueByName("Name");
                        s7TCPModule.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        // 获取所有的Analog数据
                        MyNode nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (MyNode nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            DataRow dr = s7TCPModule.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                                dr["S7 Symbol"] = nodeSignal.GetValueByName("S7Symbol");
                                dr["S7 Operand"] = nodeSignal.GetValueByName("S7Operand");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                                dr["S7 符号"] = nodeSignal.GetValueByName("S7Symbol");
                                dr["S7 操作符"] = nodeSignal.GetValueByName("S7Operand");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            s7TCPModule.AnalogDataTable.Rows.Add(dr);
                            analogCount++;
                        }

                        // 获取所有的Digital数据
                        MyNode nodeDigital = nodeModule.GetNodeByName("Digital");
                        foreach (MyNode nodeSignal in nodeDigital.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            DataRow dr = s7TCPModule.DigitalDataTable.NewRow();
                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["S7 Symbol"] = nodeSignal.GetValueByName("S7Symbol");
                                dr["S7 Operand"] = nodeSignal.GetValueByName("S7Operand");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));

                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["S7 符号"] = nodeSignal.GetValueByName("S7Symbol");
                                dr["S7 操作符"] = nodeSignal.GetValueByName("S7Operand");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            s7TCPModule.DigitalDataTable.Rows.Add(dr);
                            digitalCount++;
                        }
                        AllModules.Add(moduleNo, s7TCPModule);
                    }
                    #endregion
                    #region ARTI3
                    // 如果模块是ARTI3类型的
                    else if (moduleType == MODULE_TYPE.mtArti3)
                    {
                        ARTI3Module arti3Module = new ARTI3Module(moduleNo, MODULE_TYPE.mtArti3);

                        arti3Module.ModuleName = nodeModule.GetValueByName("Name");
                        arti3Module.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        // 获取所有的Analog数据
                        MyNode nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (MyNode nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            DataRow dr = arti3Module.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                                dr["ARTI3 Symbol"] = nodeSignal.GetValueByName("ARTI3Symbol");
                                //dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                                dr["ARTI3 符号"] = nodeSignal.GetValueByName("ARTI3Symbol");
                                //dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            arti3Module.AnalogDataTable.Rows.Add(dr);
                            analogCount++;
                        }

                        // 获取所有的Digital数据
                        MyNode nodeDigital = nodeModule.GetNodeByName("Digital");
                        foreach (MyNode nodeSignal in nodeDigital.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            DataRow dr = arti3Module.DigitalDataTable.NewRow();
                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["ARTI3 Symbol"] = nodeSignal.GetValueByName("ARTI3Symbol");
                                //dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["ARTI3 符号"] = nodeSignal.GetValueByName("ARTI3Symbol");
                                //dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            arti3Module.DigitalDataTable.Rows.Add(dr);
                            digitalCount++;
                        }
                        AllModules.Add(moduleNo, arti3Module);
                    }
                    #endregion
                    #region MELSEC
                    // 如果模块是MELSEC类型的
                    else if (moduleType == MODULE_TYPE.mtMelsecQ)
                    {
                        MELSECModule mcModule = new MELSECModule(moduleNo, MODULE_TYPE.mtMelsecQ);

                        mcModule.ModuleName = nodeModule.GetValueByName("Name");
                        mcModule.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        // 获取所有的Analog数据
                        MyNode nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (MyNode nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            DataRow dr = mcModule.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                                dr["Address Notation"] = nodeSignal.GetValueByName("ComponentType") + nodeSignal.GetValueByName("Address");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                                dr["软元件地址符"] = nodeSignal.GetValueByName("ComponentType") + nodeSignal.GetValueByName("Address");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            mcModule.AnalogDataTable.Rows.Add(dr);
                            analogCount++;
                        }

                        // 获取所有的Digital数据
                        MyNode nodeDigital = nodeModule.GetNodeByName("Digital");
                        foreach (MyNode nodeSignal in nodeDigital.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            DataRow dr = mcModule.DigitalDataTable.NewRow();
                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Address Notation"] = nodeSignal.GetValueByName("ComponentType") + nodeSignal.GetValueByName("Address");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["软元件地址符"] = nodeSignal.GetValueByName("ComponentType") + nodeSignal.GetValueByName("Address");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            mcModule.DigitalDataTable.Rows.Add(dr);
                            digitalCount++;
                        }
                        AllModules.Add(moduleNo, mcModule);
                    }
                    #endregion
                    #region VC
                    else if (moduleType == MODULE_TYPE.mtVideoCapture)
                    {
                        VideoCaptureModule videoCaptureModule = new VideoCaptureModule(moduleNo, moduleType);

                        videoCaptureModule.ModuleName = nodeModule.GetValueByName("Name");
                        videoCaptureModule.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        // 获取所有的Analog数据
                        MyNode nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (MyNode nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            DataRow dr = videoCaptureModule.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                            }
                            videoCaptureModule.AnalogDataTable.Rows.Add(dr);
                            analogCount++;
                        }
                        AllModules.Add(moduleNo, videoCaptureModule);
                    }
                    #endregion
                    #region CPNET
                    else if (moduleType == MODULE_TYPE.mtCP3550)
                    {
                        CPNetModule cpNetModule = new CPNetModule(moduleNo, moduleType);

                        cpNetModule.ModuleName = nodeModule.GetValueByName("Name");
                        cpNetModule.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        // 获取所有的Analog数据
                        MyNode nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (MyNode nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            DataRow dr = cpNetModule.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                                dr["Operand Notation"] = nodeSignal.GetValueByName("OperandNotation");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                                dr["操作符标志"] = nodeSignal.GetValueByName("OperandNotation");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            cpNetModule.AnalogDataTable.Rows.Add(dr);
                            analogCount++;
                        }

                        // 获取所有的Digital数据
                        MyNode nodeDigital = nodeModule.GetNodeByName("Digital");
                        foreach (MyNode nodeSignal in nodeDigital.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            DataRow dr = cpNetModule.DigitalDataTable.NewRow();
                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Operand Notation"] = nodeSignal.GetValueByName("OperandNotation");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["操作符标志"] = nodeSignal.GetValueByName("OperandNotation");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            cpNetModule.DigitalDataTable.Rows.Add(dr);
                            digitalCount++;
                        }
                        AllModules.Add(moduleNo, cpNetModule);
                    }
                    #endregion
                    #region TCNET
                    else if ((moduleType == MODULE_TYPE.mtToshibaV3000) || (moduleType == MODULE_TYPE.mtToshibaNV))
                    {
                        TCNetModule tcNetModule = new TCNetModule(moduleNo, moduleType);

                        tcNetModule.ModuleName = nodeModule.GetValueByName("Name");
                        tcNetModule.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        // 获取所有的Analog数据
                        MyNode nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (MyNode nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            DataRow dr = tcNetModule.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                                dr["Operand Notation"] = nodeSignal.GetValueByName("OperandNotation");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                                dr["操作符标志"] = nodeSignal.GetValueByName("OperandNotation");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            tcNetModule.AnalogDataTable.Rows.Add(dr);
                            analogCount++;
                        }

                        // 获取所有的Digital数据
                        MyNode nodeDigital = nodeModule.GetNodeByName("Digital");
                        foreach (MyNode nodeSignal in nodeDigital.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            DataRow dr = tcNetModule.DigitalDataTable.NewRow();
                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Operand Notation"] = nodeSignal.GetValueByName("OperandNotation");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["操作符标志"] = nodeSignal.GetValueByName("OperandNotation");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            tcNetModule.DigitalDataTable.Rows.Add(dr);
                            digitalCount++;
                        }
                        AllModules.Add(moduleNo, tcNetModule);
                    }
                    #endregion
                    #region Nisdas
                    else if (moduleType == MODULE_TYPE.mtNisdas)
                    {
                        NisdasModule nisdasModule = new NisdasModule(moduleNo, moduleType);

                        nisdasModule.ModuleName = nodeModule.GetValueByName("Name");
                        nisdasModule.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        // 获取所有的Analog数据
                        MyNode nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (MyNode nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            DataRow dr = nisdasModule.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                                dr["Variable"] = nodeSignal.GetValueByName("Variable");
                                dr[" Address"] = nodeSignal.GetValueByName("Address");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                                dr["变量名"] = nodeSignal.GetValueByName("Variable");
                                dr[" 地址"] = nodeSignal.GetValueByName("Address");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            nisdasModule.AnalogDataTable.Rows.Add(dr);
                            analogCount++;
                        }

                        // 获取所有的Digital数据
                        MyNode nodeDigital = nodeModule.GetNodeByName("Digital");
                        foreach (MyNode nodeSignal in nodeDigital.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            DataRow dr = nisdasModule.DigitalDataTable.NewRow();
                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Variable"] = nodeSignal.GetValueByName("Variable");
                                dr[" Address"] = nodeSignal.GetValueByName("Address");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["变量名"] = nodeSignal.GetValueByName("Variable");
                                dr[" 地址"] = nodeSignal.GetValueByName("Address");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            nisdasModule.DigitalDataTable.Rows.Add(dr);
                            digitalCount++;
                        }
                        AllModules.Add(moduleNo, nisdasModule);
                    }
                    #endregion
                    #region Sme
                    else if (moduleType == MODULE_TYPE.mtSme)
                    {
                        SmeModule smeModule = new SmeModule(moduleNo, moduleType);

                        smeModule.ModuleName = nodeModule.GetValueByName("Name");
                        smeModule.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        // 获取所有的Analog数据
                        MyNode nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (MyNode nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            DataRow dr = smeModule.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                                dr["Variable"] = nodeSignal.GetValueByName("Variable");
                                dr[" Address"] = nodeSignal.GetValueByName("Address");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                                dr["变量名"] = nodeSignal.GetValueByName("Variable");
                                dr[" 地址"] = nodeSignal.GetValueByName("Address");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            smeModule.AnalogDataTable.Rows.Add(dr);
                            analogCount++;
                        }

                        // 获取所有的Digital数据
                        MyNode nodeDigital = nodeModule.GetNodeByName("Digital");
                        foreach (MyNode nodeSignal in nodeDigital.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            DataRow dr = smeModule.DigitalDataTable.NewRow();
                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Variable"] = nodeSignal.GetValueByName("Variable");
                                dr[" Address"] = nodeSignal.GetValueByName("Address");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["变量名"] = nodeSignal.GetValueByName("Variable");
                                dr[" 地址"] = nodeSignal.GetValueByName("Address");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            smeModule.DigitalDataTable.Rows.Add(dr);
                            digitalCount++;
                        }
                        AllModules.Add(moduleNo, smeModule);
                    }
                    #endregion
                    #region USIGMA
                    else if ((moduleType == MODULE_TYPE.mtHitachiR700) || (moduleType == MODULE_TYPE.mtHitachiR900))
                    {
                        USigmaModule usigmaModule = new USigmaModule(moduleNo, moduleType);

                        usigmaModule.ModuleName = nodeModule.GetValueByName("Name");
                        usigmaModule.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        // 获取所有的Analog数据
                        MyNode nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (MyNode nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            DataRow dr = usigmaModule.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                                dr["Operand Notation"] = nodeSignal.GetValueByName("OperandNotation");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                                dr["操作符标志"] = nodeSignal.GetValueByName("OperandNotation");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            usigmaModule.AnalogDataTable.Rows.Add(dr);
                            analogCount++;
                        }

                        // 获取所有的Digital数据
                        MyNode nodeDigital = nodeModule.GetNodeByName("Digital");
                        foreach (MyNode nodeSignal in nodeDigital.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            DataRow dr = usigmaModule.DigitalDataTable.NewRow();
                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");

                                //  N0000|11 => N00000|B  
                                string OperandNotation = nodeSignal.GetValueByName("OperandNotation");
                                string strDec = OperandNotation.Substring(OperandNotation.IndexOf("|") + 1);
                                string strHead = OperandNotation.Substring(0, OperandNotation.IndexOf("|") + 1);
                                int value = Convert.ToInt32(strDec);
                                string hexOutput = String.Format("{0:X}", value);
                                //string formatValue = string.Format("{0:d2}", value);
                                string OperandNotationDec = strHead + hexOutput;

                                dr["Operand Notation"] = OperandNotationDec;
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");

                                //  N0000|11 => N00000|B  
                                string OperandNotation = nodeSignal.GetValueByName("OperandNotation");
                                string strDec = OperandNotation.Substring(OperandNotation.IndexOf("|") + 1);
                                string strHead = OperandNotation.Substring(0, OperandNotation.IndexOf("|") + 1);
                                int value = Convert.ToInt32(strDec);
                                string hexOutput = String.Format("{0:X}", value);
                                //string formatValue = string.Format("{0:d2}", value);
                                string OperandNotationDec = strHead + hexOutput;

                                dr["操作符标志"] = OperandNotationDec;
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            usigmaModule.DigitalDataTable.Rows.Add(dr);
                            digitalCount++;
                        }
                        AllModules.Add(moduleNo, usigmaModule);
                    }
                    #endregion
                    #region PLAYBACK
                    else if (moduleType == MODULE_TYPE.mtPlayback)
                    {
                        // 创建一个新的UDP Module对象
                        PlaybackModule playbackModule = new PlaybackModule(moduleNo, moduleType);

                        playbackModule.ModuleName = nodeModule.GetValueByName("Name");
                        playbackModule.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        // 获取所有的Analog数据
                        MyNode nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (MyNode nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            DataRow dr = playbackModule.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            playbackModule.AnalogDataTable.Rows.Add(dr);
                            analogCount++;
                        }

                        // 获取所有的Digital数据
                        MyNode nodeDigital = nodeModule.GetNodeByName("Digital");
                        foreach (MyNode nodeSignal in nodeDigital.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            DataRow dr = playbackModule.DigitalDataTable.NewRow();
                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            playbackModule.DigitalDataTable.Rows.Add(dr);
                            digitalCount++;
                        }
                        AllModules.Add(moduleNo, playbackModule);
                    }
                    #endregion
                    #region FDAA
                    else if (moduleType == MODULE_TYPE.mtFDAA)
                    {
                        FDAAModule fdaaModule = new FDAAModule(moduleNo, moduleType);

                        fdaaModule.ModuleName = nodeModule.GetValueByName("Name");
                        fdaaModule.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        // 获取所有的Analog数据
                        MyNode nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (MyNode nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            DataRow dr = fdaaModule.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                                dr["Operand Notation"] = nodeSignal.GetValueByName("OperandNotation");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                                dr["操作符标志"] = nodeSignal.GetValueByName("OperandNotation");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            fdaaModule.AnalogDataTable.Rows.Add(dr);
                            analogCount++;
                        }

                        // 获取所有的Digital数据
                        MyNode nodeDigital = nodeModule.GetNodeByName("Digital");
                        foreach (MyNode nodeSignal in nodeDigital.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            DataRow dr = fdaaModule.DigitalDataTable.NewRow();
                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Operand Notation"] = nodeSignal.GetValueByName("OperandNotation");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["操作符标志"] = nodeSignal.GetValueByName("OperandNotation");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            fdaaModule.DigitalDataTable.Rows.Add(dr);
                            digitalCount++;
                        }
                        AllModules.Add(moduleNo, fdaaModule);
                    }
                    #endregion
                    #region VIRTUAL
                    else if (moduleType == MODULE_TYPE.mtVirtualforLua)
                    {
                        VirtualModule virtualModule = new VirtualModule(moduleNo, moduleType);

                        virtualModule.ModuleName = nodeModule.GetValueByName("Name");
                        virtualModule.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        // 获取所有的Analog数据
                        MyNode nodeAnalog = nodeModule.GetNodeByName("Analog");
                        foreach (MyNode nodeSignal in nodeAnalog.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            DataRow dr = virtualModule.AnalogDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Unit"] = nodeSignal.GetValueByName("Unit");
                                dr["Gain"] = nodeSignal.GetValueByName("Gain");
                                dr["Path"] = nodeSignal.GetValueByName("Path");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["单位"] = nodeSignal.GetValueByName("Unit");
                                dr["增益"] = nodeSignal.GetValueByName("Gain");
                                dr["脚本文件"] = nodeSignal.GetValueByName("Path");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            virtualModule.AnalogDataTable.Rows.Add(dr);
                            analogCount++;
                        }

                        // 获取所有的Digital数据
                        MyNode nodeDigital = nodeModule.GetNodeByName("Digital");
                        foreach (MyNode nodeSignal in nodeDigital.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSignal.GetValueByName("Active")))) continue;
                            DataRow dr = virtualModule.DigitalDataTable.NewRow();
                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSignal.GetValueByName("No");
                                dr["Name"] = nodeSignal.GetValueByName("Name");
                                dr["Path"] = nodeSignal.GetValueByName("Path");
                                dr["DataType"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSignal.GetValueByName("No");
                                dr["信号名称"] = nodeSignal.GetValueByName("Name");
                                dr["脚本文件"] = nodeSignal.GetValueByName("Path");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSignal.GetValueByName("DataType"));
                            }
                            virtualModule.DigitalDataTable.Rows.Add(dr);
                            digitalCount++;
                        }
                        AllModules.Add(moduleNo, virtualModule);
                    }
                    #endregion
                    #region VIRTUALTS
                    else if (moduleType == MODULE_TYPE.mtVirtualforLuaTS)
                    {
                        VirtualTSModule virtualTSModule = new VirtualTSModule(moduleNo, moduleType);
                        virtualTSModule.ModuleName = nodeModule.GetValueByName("Name");
                        virtualTSModule.TimeBase = Convert.ToUInt32(nodeModule.GetValueByName("Timebase"));
                        // 获取所有的section数据
                        MyNode nodeSections = nodeModule.GetNodeByName("Sections");
                        foreach (MyNode nodeSection in nodeSections.NodeList)
                        {
                            if (!Convert.ToBoolean(Convert.ToInt32(nodeSection.GetValueByName("Active")))) continue;
                            DataRow dr = virtualTSModule.SectionDataTable.NewRow();

                            if (ServerConfig.getInstance().Language)
                            {
                                dr["Address"] = nodeSection.GetValueByName("No");
                                dr["Name"] = nodeSection.GetValueByName("Name");
                                dr["Path"] = nodeSection.GetValueByName("Path");
                                dr["Data Type"] = Helper.SigDataTypeNumber2String(nodeSection.GetValueByName("DataType"));
                            }
                            else
                            {
                                dr["地址"] = nodeSection.GetValueByName("No");
                                dr["信号名称"] = nodeSection.GetValueByName("Name");
                                dr["脚本文件"] = nodeSection.GetValueByName("Path");
                                dr["数据类型"] = Helper.SigDataTypeNumber2String(nodeSection.GetValueByName("DataType"));
                            }

                            virtualTSModule.SectionDataTable.Rows.Add(dr);
                            tecnostringCount++;
                        }
                        AllModules.Add(moduleNo, virtualTSModule);
                    }
                    #endregion
                }

                // 读取Technostring信息
                if (parser.ParsedNode.NodeList.Count > 4)
                {
                    MyNode nodeTechnostringGeneral = parser.ParsedNode.NodeList[4];
                    MyNode nodeTechnostrings = nodeTechnostringGeneral.GetNodeByName("TechnoStrings");

                    string dataSourceType = string.Empty;
                    foreach (MyNode nodeTechnostring in nodeTechnostrings.NodeList)
                    {
                        if (!Convert.ToBoolean(Convert.ToInt32(nodeTechnostring.GetValueByName("Active")))) continue;
                        dataSourceType = nodeTechnostring.GetValueByName("DataSourceType");
                        if (dataSourceType == "301")
                        {
                            moduleNo = Convert.ToUInt32(nodeTechnostring.GetValueByName("ModuleNo"));
                            TechnostringModule opcTechnostring = new TechnostringModule(moduleNo, MODULE_TYPE.mtTsOPC);
                            opcTechnostring.Enabled = Convert.ToBoolean(Convert.ToInt32(nodeTechnostring.GetValueByName("Active")));
                            opcTechnostring.ModuleName = nodeTechnostring.GetValueByName("Name");

                            MyNode nodeSections = nodeTechnostring.GetNodeByName("Sections");
                            foreach (MyNode nodeSection in nodeSections.NodeList)
                            {
                                DataRow dr = opcTechnostring.SectionDataTable.NewRow();

                                if (ServerConfig.getInstance().Language)
                                {
                                    dr["Address"] = nodeSection.GetValueByName("No");
                                    dr["Name"] = nodeSection.GetValueByName("Name");
                                }
                                else
                                {
                                    dr["地址"] = nodeSection.GetValueByName("No");
                                    dr["信号名称"] = nodeSection.GetValueByName("Name");
                                }
                                opcTechnostring.SectionDataTable.Rows.Add(dr);
                                tecnostringCount++;
                            }
                            AllModules.Add(moduleNo, opcTechnostring);
                        }
                        if (dataSourceType == "300")
                        {
                            moduleNo = Convert.ToUInt32(nodeTechnostring.GetValueByName("ModuleNo"));
                            TechnostringModule FauTechnostring = new TechnostringModule(moduleNo, MODULE_TYPE.mtTsFAU);
                            FauTechnostring.Enabled = Convert.ToBoolean(Convert.ToInt32(nodeTechnostring.GetValueByName("Active")));
                            FauTechnostring.ModuleName = nodeTechnostring.GetValueByName("Name");

                            MyNode nodeSections = nodeTechnostring.GetNodeByName("Sections");
                            foreach (MyNode nodeSection in nodeSections.NodeList)
                            {
                                DataRow dr = FauTechnostring.SectionDataTable.NewRow();

                                if (ServerConfig.getInstance().Language)
                                {
                                    dr["Address"] = nodeSection.GetValueByName("No");
                                    dr["Name"] = nodeSection.GetValueByName("Name");
                                }
                                else
                                {
                                    dr["地址"] = nodeSection.GetValueByName("No");
                                    dr["信号名称"] = nodeSection.GetValueByName("Name");
                                }
                                FauTechnostring.SectionDataTable.Rows.Add(dr);
                                tecnostringCount++;
                                AllModules.Add(moduleNo, FauTechnostring);
                            }
                        }
                        if (dataSourceType == "302")
                        {
                            moduleNo = Convert.ToUInt32(nodeTechnostring.GetValueByName("ModuleNo"));
                            TechnostringModule UdpTechnostring = new TechnostringModule(moduleNo, MODULE_TYPE.mtTsUDP);
                            UdpTechnostring.Enabled = Convert.ToBoolean(Convert.ToInt32(nodeTechnostring.GetValueByName("Active")));
                            UdpTechnostring.ModuleName = nodeTechnostring.GetValueByName("Name");

                            MyNode nodeSections = nodeTechnostring.GetNodeByName("Sections");
                            foreach (MyNode nodeSection in nodeSections.NodeList)
                            {
                                DataRow dr = UdpTechnostring.SectionDataTable.NewRow();

                                if (ServerConfig.getInstance().Language)
                                {
                                    dr["Address"] = nodeSection.GetValueByName("No");
                                    dr["Name"] = nodeSection.GetValueByName("Name");
                                }
                                else
                                {
                                    dr["地址"] = nodeSection.GetValueByName("No");
                                    dr["信号名称"] = nodeSection.GetValueByName("Name");
                                }
                                UdpTechnostring.SectionDataTable.Rows.Add(dr);
                                tecnostringCount++;
                                AllModules.Add(moduleNo, UdpTechnostring);
                            }
                        }
                    }
                }
                AllModules = AllModules.OrderBy(i => i.Key).ToDictionary(i => i.Key, i => i.Value);  
            }
            catch (Exception)
            {
                
            }
        }

        public string GetSignalName(int moduleNo, int signalNo)
        {
            string signalName = "??";
            string address = moduleNo + ":" + signalNo;
            if (moduleNo == -1) return signalName;

            UInt32 umo = Convert.ToUInt32(moduleNo);

           // AllModules[umo].
            if (AllModules.Keys.Contains(umo))
            {
                if(ServerConfig.getInstance().Language)
                {
                    foreach (DataRow row in AllModules[umo].AnalogDataTable.Rows)
                    {
                        if (row["Address"].ToString() == address)
                            return row["Address"].ToString() + " "+ row["Name"].ToString();
                    }                    
                }
                else
                {
                    foreach (DataRow row in AllModules[umo].AnalogDataTable.Rows)
                    {
                        if (row["地址"].ToString() == address)
                            return row["地址"].ToString() + " " + row["信号名称"].ToString();
                    }   
                }
            }
            return signalName;
        }
    }
}
