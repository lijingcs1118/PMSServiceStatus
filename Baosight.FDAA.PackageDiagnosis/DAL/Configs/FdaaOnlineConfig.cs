using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Baosight.FDAA.PackageDiagnosis.BLL;

namespace Baosight.FDAA.PackageDiagnosis.DAL.Configs
{
    public class FdaaOnlineConfig
    {
        private readonly ClientConfigProperty _clientConfig;

        private readonly LayoutConfig _layoutConfig;

        public FdaaOnlineConfig()
        {
            _clientConfig = new ClientConfigProperty();
            _layoutConfig = new LayoutConfig();
        }

        public bool LoadConfig(out string error)
        {
            if (!File.Exists(FdaaHelper.CreateInstance().FdaaOnlineConfigName))
            {
                error = FdaaHelper.CreateInstance().FdaaOnlineConfigName + "不存在";
                return false;
            }

            error = string.Empty;    
            var fs = new FileStream(FdaaHelper.CreateInstance().FdaaOnlineConfigName, FileMode.Open);
            try
            {
                LoadConfig(fs);
                fs.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                fs.Close();
            }
        }

        private void LoadConfig(Stream stream)
        {
            var xmlIn = new XmlTextReader(stream);

            //去除空格
            xmlIn.WhitespaceHandling = WhitespaceHandling.None;

            xmlIn.MoveToContent();

            while (xmlIn.Read())
            {
                if (xmlIn.Name == "ClientPart" && xmlIn.NodeType == XmlNodeType.Element)
                    //是ClientPart部分，调用ClientConfig的ParseXml方法
                    _clientConfig.ParseFromXml(xmlIn);
                if (xmlIn.Name == "Layouts" && xmlIn.NodeType == XmlNodeType.Element) _layoutConfig.ParseFromXml(xmlIn);
            }

            xmlIn.Close();
        }
    }

    public class ClientConfigProperty
    {
        //Settings
        private string _autoConn = "0";

        private string _axisFont = "宋体,12";
        private string _backgroundColor = "Silver";
        private string _displayStyle = "1";
        private string _graphColor = "White";

        private string _gridlinesColor = "Silver";

        //ConnectString 
        private string _ipAddr = "127.0.0.1";
        private string _layoutToolBar = "1";
        private string _legendFont = "宋体,12";
        private string _lengthRange = "180";
        private string _markerColor = "Red";
        private string _orientation = "0";
        private string _paperMethod = "1";
        private string _penWidth = "4";
        private string _port = "8089";
        private string _recorderType = "Scroll";
        private string _scaleMax = "100";

        private string _scaleMin = "0";

        //YAxisScalingMode
        private string _scaleMode = "0";
        private string _serverToolBar = "1";

        private string _showCurrentValue = "1";

        //RecorderCommonProperty
        private string _showGridLines = "1";

        private string _signalColor0 = "Blue";
        private string _signalColor1 = "Red";
        private string _signalColor10 = "64, 0, 128";
        private string _signalColor11 = "Olive";
        private string _signalColor12 = "Yellow";
        private string _signalColor13 = "Brown";
        private string _signalColor14 = "111,233,48";
        private string _signalColor15 = "200,240,30";
        private string _signalColor2 = "Green";
        private string _signalColor3 = "255, 128, 64";
        private string _signalColor4 = "255, 128, 192";
        private string _signalColor5 = "130, 0, 255";
        private string _signalColor6 = "255, 0, 128";
        private string _signalColor7 = "Black";
        private string _signalColor8 = "255, 128, 128";
        private string _signalColor9 = "64, 128, 128";
        private string _statusBar = "1";

        private string[] _strsignalColorList;
        private string _timeInterval = "200";
        private string _timeRange = "180";
        private string _xAxisColor = "Black";

        private string _yAxisColor = "Black";

        //---------------------------------------------------------------------

        public bool Language { get; set; }

        /// <summary>
        ///     ClientConfig中的IpAddress属性
        /// </summary>
        public string IpAddress
        {
            get { return _ipAddr; }
            set { _ipAddr = value; }
        }

        /// <summary>
        ///     设置端口号
        /// </summary>
        public string Port
        {
            get { return _port; }
            set { _port = value; }
        }

        /// <summary>
        ///     是否显示网格线
        /// </summary>
        public string ShowGridLines
        {
            get { return _showGridLines; }
            set { _showGridLines = value; }
        }

        /// <summary>
        ///     显示当前值
        /// </summary>
        public string ShowCurrentValue
        {
            get { return _showCurrentValue; }
            set { _showCurrentValue = value; }
        }

        /// <summary>
        ///     显示走纸方式 1.TimeBase 2.LengthBase
        /// </summary>
        public string PaperMethod
        {
            get { return _paperMethod; }
            set { _paperMethod = value; }
        }

        /// <summary>
        ///     走纸方向 0.L->R 1.R->L 2.T->B 3.B->T
        /// </summary>
        public string Orientation
        {
            get { return _orientation; }
            set { _orientation = value; }
        }

        /// <summary>
        ///     recorder类型：Scroll,CRT
        /// </summary>
        public string RecorderType
        {
            get { return _recorderType; }
            set { _recorderType = value; }
        }

        /// <summary>
        ///     X轴时间区间
        /// </summary>
        public string TimeRange
        {
            get { return _timeRange; }
            set { _timeRange = value; }
        }

        /// <summary>
        ///     刷新时间
        /// </summary>
        public string TimeInterval
        {
            get { return _timeInterval; }
            set { _timeInterval = value; }
        }

        /// <summary>
        ///     长度区间
        /// </summary>
        public string LengthRange
        {
            get { return _lengthRange; }
            set { _lengthRange = value; }
        }

        /// <summary>
        ///     画笔宽度
        /// </summary>
        public string PenWidth
        {
            get { return _penWidth; }
            set { _penWidth = value; }
        }

        /// <summary>
        ///     背景色
        /// </summary>
        public string BackGroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; }
        }

        /// <summary>
        ///     X轴颜色
        /// </summary>
        public string XAxisColor
        {
            get { return _xAxisColor; }
            set { _xAxisColor = value; }
        }

        /// <summary>
        ///     Y轴颜色
        /// </summary>
        public string YAxisColor
        {
            get { return _yAxisColor; }
            set { _yAxisColor = value; }
        }

        /// <summary>
        ///     作图区域颜色
        /// </summary>
        public string GraphColor
        {
            get { return _graphColor; }
            set { _graphColor = value; }
        }

        /// <summary>
        ///     网格线的颜色
        /// </summary>
        public string GridlinesColor
        {
            get { return _gridlinesColor; }
            set { _gridlinesColor = value; }
        }

        /// <summary>
        ///     游标的颜色
        /// </summary>
        public string MarkerColor
        {
            get { return _markerColor; }
            set { _markerColor = value; }
        }

        /// <summary>
        ///     获取信号配色列表
        /// </summary>
        public string[] SignalColorList
        {
            get
            {
                string[] singnalColors =
                {
                    _signalColor0,
                    _signalColor1,
                    _signalColor2,
                    _signalColor3,
                    _signalColor4,
                    _signalColor5,
                    _signalColor6,
                    _signalColor7,
                    _signalColor8,
                    _signalColor9,
                    _signalColor10,
                    _signalColor11,
                    _signalColor12,
                    _signalColor13,
                    _signalColor14,
                    _signalColor15
                };
                return singnalColors;
            }
            set
            {
                _strsignalColorList = value;

                _signalColor0 = _strsignalColorList[0];
                _signalColor1 = _strsignalColorList[1];
                _signalColor2 = _strsignalColorList[2];
                _signalColor3 = _strsignalColorList[3];
                _signalColor4 = _strsignalColorList[4];
                _signalColor5 = _strsignalColorList[5];
                _signalColor6 = _strsignalColorList[6];
                _signalColor7 = _strsignalColorList[7];
                _signalColor8 = _strsignalColorList[8];
                _signalColor9 = _strsignalColorList[9];
                _signalColor10 = _strsignalColorList[10];
                _signalColor11 = _strsignalColorList[11];
                _signalColor12 = _strsignalColorList[12];
                _signalColor13 = _strsignalColorList[13];
                _signalColor14 = _strsignalColorList[14];
                _signalColor15 = _strsignalColorList[15];
            }
        }

        public string AxisFont
        {
            get { return _axisFont; }
            set { _axisFont = value; }
        }

        public string LegendFont
        {
            get { return _legendFont; }
            set { _legendFont = value; }
        }

        /// <summary>
        ///     刻度模式
        /// </summary>
        public string ScaleMode
        {
            get { return _scaleMode; }
            set { _scaleMode = value; }
        }

        /// <summary>
        ///     最小标尺
        /// </summary>
        public string ScaleMin
        {
            get { return _scaleMin; }
            set { _scaleMin = value; }
        }

        /// <summary>
        ///     最大标尺
        /// </summary>
        public string ScaleMax
        {
            get { return _scaleMax; }
            set { _scaleMax = value; }
        }

        /// <summary>
        ///     自动重连
        /// </summary>
        public string AutoConnection
        {
            get { return _autoConn; }
            set { _autoConn = value; }
        }

        /// <summary>
        ///     是否显示Server工具条
        /// </summary>
        public string ServerToolBar
        {
            get { return _serverToolBar; }
            set { _serverToolBar = value; }
        }

        /// <summary>
        ///     Layout工具条
        /// </summary>
        public string LayoutToolBar
        {
            get { return _layoutToolBar; }
            set { _layoutToolBar = value; }
        }

        /// <summary>
        ///     是否显示StatusBar
        /// </summary>
        public string StatusBar
        {
            get { return _statusBar; }
            set { _statusBar = value; }
        }

        /// <summary>
        ///     显示的样式
        /// </summary>
        public string DisplayStyle
        {
            get { return _displayStyle; }
            set { _displayStyle = value; }
        }

        //---------------------------------------------------------------------
        /// <summary>
        ///     从XmlTextReader中解析出属性
        /// </summary>
        //---------------------------------------------------------------------
        /*  Copyright:  Shanghai Baosight Software Co., Ltd.
         *  返回值： void	 	
         *  参数列表：	参数类型    					描述
         *    xmlIn  :    [IN/OUT] XmlTextReader	     
         *  版本历史		
         *       1.0    2010-08-04  Menggang    First Creation  
         **********************************************************************/
        public void ParseFromXml(XmlTextReader xmlIn)
        {
            xmlIn.MoveToContent();
            if (xmlIn.Name != "ClientPart")
                throw new ArgumentException("ClientConfig Root Element has to be 'ClientPart'");

            xmlIn.Read(); //<ConnectString>
            xmlIn.Read(); //<Ip>
            IpAddress = xmlIn.ReadElementString("IPAddress");
            Port = xmlIn.ReadElementString("Port");
            xmlIn.Read(); //</RecorderCommonProperty>
            xmlIn.Read(); //<ShowGridLines>
            ShowGridLines = xmlIn.ReadElementString("ShowGridLines");
            ShowCurrentValue = xmlIn.ReadElementString("ShowCurrentValue");
            PaperMethod = xmlIn.ReadElementString("PaperMethod");
            Orientation = xmlIn.ReadElementString("Orientation");
            RecorderType = xmlIn.ReadElementString("RecorderType");
            TimeRange = xmlIn.ReadElementString("TimeRange");
            TimeInterval = xmlIn.ReadElementString("TimeInterval");
            LengthRange = xmlIn.ReadElementString("LengthRange");
            PenWidth = xmlIn.ReadElementString("PenWidth");
            BackGroundColor = xmlIn.ReadElementString("BackgroundColor");
            XAxisColor = xmlIn.ReadElementString("XAxisColor");
            YAxisColor = xmlIn.ReadElementString("YAxisColor");
            GraphColor = xmlIn.ReadElementString("GraphColor");
            GridlinesColor = xmlIn.ReadElementString("GridLinesColor");
            MarkerColor = xmlIn.ReadElementString("MarkerColor");
            xmlIn.Read(); //<SignalColorList>
            var signalColors = new string[16];
            signalColors[0] = xmlIn.ReadElementString("SignalColor_0");
            signalColors[1] = xmlIn.ReadElementString("SignalColor_1");
            signalColors[2] = xmlIn.ReadElementString("SignalColor_2");
            signalColors[3] = xmlIn.ReadElementString("SignalColor_3");
            signalColors[4] = xmlIn.ReadElementString("SignalColor_4");
            signalColors[5] = xmlIn.ReadElementString("SignalColor_5");
            signalColors[6] = xmlIn.ReadElementString("SignalColor_6");
            signalColors[7] = xmlIn.ReadElementString("SignalColor_7");
            signalColors[8] = xmlIn.ReadElementString("SignalColor_8");
            signalColors[9] = xmlIn.ReadElementString("SignalColor_9");
            signalColors[10] = xmlIn.ReadElementString("SignalColor_10");
            signalColors[11] = xmlIn.ReadElementString("SignalColor_11");
            signalColors[12] = xmlIn.ReadElementString("SignalColor_12");
            signalColors[13] = xmlIn.ReadElementString("SignalColor_13");
            signalColors[14] = xmlIn.ReadElementString("SignalColor_14");
            signalColors[15] = xmlIn.ReadElementString("SignalColor_15");
            SignalColorList = signalColors;
            xmlIn.Read(); //</SignalColorList>
            AxisFont = xmlIn.ReadElementString("AxisFont");
            LegendFont = xmlIn.ReadElementString("LegendFont");
            xmlIn.Read(); //</RecorderCommonProperty>
            xmlIn.Read(); //<YAxisScaleMode>
            ScaleMode = xmlIn.ReadElementString("ScaleMode");
            ScaleMin = xmlIn.ReadElementString("ScaleMin");
            ScaleMax = xmlIn.ReadElementString("ScaleMax");
            xmlIn.Read(); //</YAxisScaleMode>
            xmlIn.Read(); //<Settings>
            AutoConnection = xmlIn.ReadElementString("AutoConn");
            ServerToolBar = xmlIn.ReadElementString("ServerToolBar");
            LayoutToolBar = xmlIn.ReadElementString("LayoutToolBar");
            StatusBar = xmlIn.ReadElementString("StatusBar");
            DisplayStyle = xmlIn.ReadElementString("DisplayStyle");
            // 语言
            var yes = Convert.ToInt32(xmlIn.ReadElementString("Language"));
            Language = Convert.ToBoolean(yes);

            xmlIn.Read(); //</Settings>
        }

        //---------------------------------------------------------------------
        /// <summary>
        ///     将参数写到流当中
        /// </summary>
        //---------------------------------------------------------------------
        /*  Copyright:  Shanghai Baosight Software Co., Ltd.
         *  返回值： void	 	
         *  参数列表：	参数类型    					描述
         *    stream  :    [IN/OUT] Stream	    
         *    encoding  :    [IN/OUT] Encoding	    
         *  版本历史		
         *       1.0    2010-08-05  Menggang    First Creation  
         **********************************************************************/
        public void WriteConfigToStream(XmlTextWriter writer, Stream stream, Encoding encoding)
        {
            //XmlTextWriter writer = new XmlTextWriter(stream, encoding);
            //// Use indenting for readability 
            //writer.Formatting = Formatting.Indented;
            //开始写元素
            //writer.WriteStartDocument();
            writer.WriteStartElement("ClientPart"); //<ClientPart>
            writer.WriteStartElement("ConnectString"); //<ConnectString>
            writer.WriteElementString("IPAddress", _ipAddr); //<IPAddress>
            writer.WriteElementString("Port", _port); //<Port>
            writer.WriteEndElement(); //</ConnectString>
            writer.WriteStartElement("RecorderCommonProperty"); //<RecorderCommonProperty>
            writer.WriteElementString("ShowGridLines", _showGridLines); //<ShowGridLines>
            writer.WriteElementString("ShowCurrentValue", _showCurrentValue); //<ShowCurrentValue>
            writer.WriteElementString("PaperMethod", _paperMethod); //<PaperMethod>
            writer.WriteElementString("Orientation", _orientation); //<Orientation>
            writer.WriteElementString("RecorderType", _recorderType); //<RecorderType>
            writer.WriteElementString("TimeRange", _timeRange); //<TimeRange>
            writer.WriteElementString("TimeInterval", _timeInterval); //<TimeInterval>
            writer.WriteElementString("LengthRange", _lengthRange); //<LengthRange>
            writer.WriteElementString("PenWidth", _penWidth); //<PenWidth>
            writer.WriteElementString("BackgroundColor", _backgroundColor); //<BackgroundColor>
            writer.WriteElementString("XAxisColor", _xAxisColor); //<XAxisColor> 
            writer.WriteElementString("YAxisColor", _yAxisColor); //<YAxisColor>
            writer.WriteElementString("GraphColor", _graphColor); //<GraphColor>
            writer.WriteElementString("GridLinesColor", _gridlinesColor); //<GridLinesColor>
            writer.WriteElementString("MarkerColor", _markerColor); //<MarkerColor>
            writer.WriteStartElement("SignalColorList"); //<SignalColorList>
            writer.WriteElementString("SignalColor_0", _signalColor0); //<SignalColor_0>
            writer.WriteElementString("SignalColor_1", _signalColor1); //<SignalColor_1>
            writer.WriteElementString("SignalColor_2", _signalColor2); //<SignalColor_2>
            writer.WriteElementString("SignalColor_3", _signalColor3); //<SignalColor_3>
            writer.WriteElementString("SignalColor_4", _signalColor4); //<SignalColor_4>
            writer.WriteElementString("SignalColor_5", _signalColor5); //<SignalColor_5>
            writer.WriteElementString("SignalColor_6", _signalColor6); //<SignalColor_6>
            writer.WriteElementString("SignalColor_7", _signalColor7);
            writer.WriteElementString("SignalColor_8", _signalColor8);
            writer.WriteElementString("SignalColor_9", _signalColor9);
            writer.WriteElementString("SignalColor_10", _signalColor10);
            writer.WriteElementString("SignalColor_11", _signalColor11);
            writer.WriteElementString("SignalColor_12", _signalColor12);
            writer.WriteElementString("SignalColor_13", _signalColor13);
            writer.WriteElementString("SignalColor_14", _signalColor14);
            writer.WriteElementString("SignalColor_15", _signalColor15);
            writer.WriteEndElement(); //</SignalColorList>
            writer.WriteElementString("AxisFont", _axisFont); //</AxisFont>
            writer.WriteElementString("LegendFont", _legendFont); //</LegendFont>
            writer.WriteEndElement(); //</RecorderCommonProperty>
            writer.WriteStartElement("YAxisScaleMode"); //<YAxisScaleMode>
            writer.WriteElementString("ScaleMode", _scaleMode); //<ScaleMode>
            writer.WriteElementString("ScaleMin", _scaleMin); //<ScaleMin>
            writer.WriteElementString("ScaleMax", _scaleMax); //<ScaleMax>
            writer.WriteEndElement(); //</YAxisScaleMode>
            writer.WriteStartElement("Settings");
            writer.WriteElementString("AutoConn", _autoConn);
            writer.WriteElementString("ServerToolBar", _serverToolBar);
            writer.WriteElementString("LayoutToolBar", _layoutToolBar);
            writer.WriteElementString("StatusBar", _statusBar);
            writer.WriteElementString("DisplayStyle", _displayStyle);
            writer.WriteElementString("Language", Convert.ToInt32(Language).ToString());
            writer.WriteEndElement();
            writer.WriteEndElement(); //</ClientPart>
            //writer.Close();
        }
    }

    public class LayoutConfig
    {
        //Store Layout Object
        private readonly List<LayoutItem> _layoutList = new List<LayoutItem>();

        //Store Layout Names
        private readonly List<string> _namesList = new List<string>();

        //上一个Layout

        //当前选中的Layout

        /// <summary>
        ///     构造函数
        /// </summary>
        /// ;
        public LayoutConfig()
        {
            _namesList.Clear();
            _layoutList.Clear();
        }

        /// <summary>
        ///     选中的索引
        /// </summary>
        public int SelectedIndex { get; set; }

        /// <summary>
        ///     上一个选中的的索引
        /// </summary>
        public int PreviousIndex { get; set; }

        /// <summary>
        ///     从XMLTextReader中读取出相应的节点
        /// </summary>
        /// <param name="xmlIn"></param>
        public void ParseFromXml(XmlTextReader xmlIn)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xmlIn.ReadOuterXml());

            var xe = doc.DocumentElement;
            var _rootNodeName = xe.Name; //<Layouts>

            var nodeList = doc.SelectSingleNode(_rootNodeName).ChildNodes;

            //获取Layout的个数
            var count = nodeList.Count - 2; //除去头尾，中间即是Layout的个数

            for (var i = 1; i <= count; i++)
            {
                var node = nodeList[i];
                var nnlist = node.ChildNodes;

                var item = new LayoutItem();
                item.DockingConfig = nnlist[1];
                item.TabbedGroups = nnlist[2];

                //New Modify,2010-09-09
                _namesList.Add(nnlist[0].InnerText);
                _layoutList.Add(item);
            }

            var layoutIndex = nodeList[count + 1];
            SelectedIndex = Convert.ToInt32(layoutIndex.InnerText);
        }

        /// <summary>
        ///     获取特定节点的流
        /// </summary>
        /// <param name="layoutName">Layout的名</param>
        /// <param name="fixNode">特定的节点</param>
        /// <returns></returns>
        public Stream GetFixedStream(int index, string fixNode)
        {
            var _doc = new XmlDocument();

            //写声明
            var xmlDecl = _doc.CreateXmlDeclaration("1.0", "utf-8", string.Empty);
            //增加一个根节点
            var _rootElement = _doc.DocumentElement;
            //将声明写在根节点之前
            _doc.InsertBefore(xmlDecl, _rootElement);
            //创建根节点
            XmlNode node;
            if (fixNode == "TabbedGroups")
                node = _layoutList[index].TabbedGroups;
            else
                node = _layoutList[index].DockingConfig;

            var aNode = _doc.ImportNode(node, true);
            _doc.AppendChild(aNode);

            var sm = new MemoryStream();
            _doc.Save(sm);
            sm.Seek(0, SeekOrigin.Begin);
            return sm;
        }

        /// <summary>
        ///     将配置文件写到流当中
        /// </summary>
        /// <param name="stream">XmlTextWriter的句柄</param>
        /// <param name="encoding">文件的编码</param>
        public void WriteConfigToStream(string layoutName, XmlTextWriter writer, Encoding encoding)
        {
            var _doc = new XmlDocument();

            //增加Layouts节点
            XmlNode _layouts = _doc.CreateElement("Layouts");
            _doc.AppendChild(_layouts);

            XmlNode _layoutCount = _doc.CreateElement("LayoutCount");
            _layoutCount.InnerText = "1";
            _layouts.AppendChild(_layoutCount);

            for (var i = 0; i < _namesList.Count; i++)
            {
                //Layout节点
                XmlNode _layout = _doc.CreateElement("Layout");

                //增加LayoutName节点
                XmlNode _layoutName = _doc.CreateElement("LayoutName");
                _layoutName.InnerText = _namesList[i];
                _layout.AppendChild(_layoutName);

                //增加TabbedGroups节点
                var node1 = _layoutList[i].DockingConfig;
                if (node1 != null)
                {
                    //XmlNode node1 = _layoutDic[key].DockingConfig;
                    var aNode = _doc.ImportNode(node1, true);
                    _layout.AppendChild(aNode);
                }

                //增加Layout节点
                //XmlNode node2 = _layoutDic[key].TabbedGroups;
                var node2 = _layoutList[i].TabbedGroups;
                if (node2 != null)
                {
                    var bNode = _doc.ImportNode(node2, true);
                    _layout.AppendChild(bNode);
                }

                _layouts.AppendChild(_layout);
            }

            XmlNode _selectLayout = _doc.CreateElement("SelectedLayout");
            _selectLayout.InnerText = SelectedIndex.ToString();

            _layouts.AppendChild(_selectLayout);
            _doc.WriteContentTo(writer);
        }

        /// <summary>
        ///     保存到特定的节点 Author: Menggang
        /// </summary>
        /// <param name="layoutName">Layout的名称</param>
        /// <param name="xmlIn">写入的XML文件</param>
        /// <param name="encoding">文件编码格式</param>
        public void SaveDockingConfig(int index, string layoutName, XmlTextReader xmlIn, Encoding encoding)
        {
            //如果是空配置文件,增加一个默认的
            if (_namesList.Count == 0)
            {
                _namesList.Add("Default");
                _layoutList.Add(new LayoutItem());
            }

            xmlIn.WhitespaceHandling = WhitespaceHandling.None;
            xmlIn.MoveToContent();

            var _doc = new XmlDocument();
            _doc.LoadXml(xmlIn.ReadOuterXml());

            var xe = _doc.DocumentElement;
            var _rootNodeName = xe.Name; //<Layouts>

            //8/13号更改，如果是更改LayoutName
            if (!_namesList.Contains(layoutName))
                ModifyLayoutName(Convert.ToInt32(SelectedIndex), layoutName);

            //更新相应的节点
            var item = new LayoutItem();
            item.DockingConfig = xe;
            item.TabbedGroups = _layoutList[Convert.ToInt32(index)].TabbedGroups;

            _layoutList[Convert.ToInt32(index)] = item;
        }

        /// <summary>
        ///     保存TabbedGroups配置文件 Author: menggang
        /// </summary>
        /// <param name="layoutName">Layout名称</param>
        /// <param name="xmlIn">写入的Xml文件</param>
        /// <param name="encoding">文件编码</param>
        public void SaveTabConfig(int index, string layoutName, XmlTextReader xmlIn, Encoding encoding)
        {
            xmlIn.WhitespaceHandling = WhitespaceHandling.None;
            xmlIn.MoveToContent();

            var _doc = new XmlDocument();
            _doc.LoadXml(xmlIn.ReadOuterXml());

            var xe = _doc.DocumentElement;

            //8/13号更改，如果是更改LayoutName
            if (!_namesList.Contains(layoutName))
                ModifyLayoutName(index, layoutName);

            //更新相应的节点
            var item = new LayoutItem();
            item.DockingConfig = _layoutList[index].DockingConfig;
            item.TabbedGroups = xe;
            _layoutList[index] = item;
        }

        /// <summary>
        ///     更新当前的Layout名称
        /// </summary>
        /// <param name="index"></param>
        /// <param name="newName"></param>
        public void ModifyLayoutName(int index, string newName)
        {
            _namesList[index] = newName;
        }

        /// <summary>
        ///     新增一个Layout
        /// </summary>
        /// <param name="layoutName"></param>
        public void AddNewLayout(int index, string layoutName)
        {
            //复制一份当前的Layout
            var item = new LayoutItem();
            if (index != -1) item = _layoutList[index];
            //增加一份新的Layout
            _layoutList.Add(item);
            _namesList.Add(layoutName);
        }

        /// <summary>
        ///     新增一个Layout
        /// </summary>
        /// <param name="layoutName"></param>
        public void AddNewLayout(string layoutName)
        {
            _layoutList.Add(new LayoutItem());
            _namesList.Add(layoutName);
        }

        /// <summary>
        ///     移除当前的Layout
        /// </summary>
        /// <param name="index"></param>
        public void RemoveLayout(int index)
        {
            _layoutList.RemoveAt(index);
            _namesList.RemoveAt(index);
        }

        /// <summary>
        ///     获取所有Layout的名
        /// </summary>
        /// <returns></returns>
        public string[] GetLayoutNames()
        {
            //return _namesDic.Values.ToArray();
            return _namesList.ToArray();
        }

        /// <summary>
        ///     清除当前的配置
        /// </summary>
        public void ClearCurrentConfig()
        {
            _namesList.Clear();
            _layoutList.Clear();
        }
    }

    /// <summary>
    ///     与Layout相关的选项
    /// </summary>
    public struct LayoutItem
    {
        public XmlNode DockingConfig { get; set; }

        public XmlNode TabbedGroups { get; set; }
    }
}