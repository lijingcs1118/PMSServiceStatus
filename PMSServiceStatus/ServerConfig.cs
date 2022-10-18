using Baosight.FDAA.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PMSServiceStatus
{
    class ServerConfig
    {
        public static readonly ServerConfig serverConfig = new ServerConfig();
        public static ServerConfig getInstance()
        {
            return serverConfig;
        }

        private bool autoStartServerOnStartup;
        public bool AutoStartServerOnStartup
        {
            get { return autoStartServerOnStartup; }
            set { autoStartServerOnStartup = value; }
        }

        private bool startAcquisitionOnStartup;
        public bool StartAcquisitionOnStartup
        {
            get { return startAcquisitionOnStartup; }
            set { startAcquisitionOnStartup = value; }
        }

        private int serverPortNr;
        public int ServerPortNr
        {
            get { return serverPortNr; }
            set { serverPortNr = value; }
        }

        private string ioConfigBackupPath;
        public string IoConfigBackupPath
        {
            get { return ioConfigBackupPath; }
            set { ioConfigBackupPath = value; }
        }

        private string logPath;
        public string LogPath
        {
            get { return logPath; }
            set { logPath = value; }
        }

        private int logLevel;
        public int LogLevel
        {
            get { return logLevel; }
            set { logLevel = value; }
        }

        private bool language;
        public bool Language
        {
            get { return language; }
            set { language = value; }
        }

        /// <summary>
        /// 读取ServerConfig配置文件
        /// </summary>
        /// <param name="fileName">ServerConfig路径</param>
        public void LoadServerConfig(string fileName)
        {
            try
            {
                // 创建xml解析对象
                XmlParser parser = new XmlParser();
                parser.LoadConfig(fileName);

                //  读取ServerConfig属性
                ServerConfig.getInstance().AutoStartServerOnStartup = Convert.ToBoolean(Convert.ToInt32(parser.ParsedNode.GetValueByName("AutoStartServerOnStartup")));
                ServerConfig.getInstance().StartAcquisitionOnStartup = Convert.ToBoolean(Convert.ToInt32(parser.ParsedNode.GetValueByName("StartAcquisitionOnStartup")));
                ServerConfig.getInstance().ServerPortNr = Convert.ToInt32(parser.ParsedNode.GetValueByName("ServerPortNr"));
                ServerConfig.getInstance().IoConfigBackupPath = parser.ParsedNode.GetValueByName("IoConfigBackupPath");
                ServerConfig.getInstance().LogPath = parser.ParsedNode.GetValueByName("LogPath");
                ServerConfig.getInstance().LogLevel = Convert.ToInt32(parser.ParsedNode.GetValueByName("LogLevel"));
                ServerConfig.getInstance().Language = Convert.ToBoolean(Convert.ToInt32(parser.ParsedNode.GetValueByName("Language")));
            }
            catch (Exception ex)
            {
                // 如果读取文件有异常，则提示
                MessageBox.Show(ex.Message, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                throw;
            }
        }

        /// <summary>
        /// 保存ServerConfig配置文件
        /// </summary>
        /// <param name="fileName">ServerConfig路径</param>
        public void SaveServerConfig(string fileName)
        {
            // 创建xml解析对象
            XmlParser parser = new XmlParser();
            parser.RootNodeName = "ServerConfiguration";

            MyNode nodeAutoStartServerOnStartup = new MyNode();
            nodeAutoStartServerOnStartup.NodeName = "AutoStartServerOnStartup";
            nodeAutoStartServerOnStartup.InnerText = Convert.ToInt32(ServerConfig.getInstance().AutoStartServerOnStartup).ToString();
            parser.ParsedNode.NodeList.Add(nodeAutoStartServerOnStartup);

            MyNode nodeStartAcquisitionOnStartup = new MyNode();
            nodeStartAcquisitionOnStartup.NodeName = "StartAcquisitionOnStartup";
            nodeStartAcquisitionOnStartup.InnerText = Convert.ToInt32(ServerConfig.getInstance().StartAcquisitionOnStartup).ToString();
            parser.ParsedNode.NodeList.Add(nodeStartAcquisitionOnStartup);

            MyNode nodeServerPortNr = new MyNode();
            nodeServerPortNr.NodeName = "ServerPortNr";
            nodeServerPortNr.InnerText = ServerConfig.getInstance().ServerPortNr.ToString();
            parser.ParsedNode.NodeList.Add(nodeServerPortNr);

            MyNode nodeIoConfigBackupPath = new MyNode();
            nodeIoConfigBackupPath.NodeName = "IoConfigBackupPath";
            nodeIoConfigBackupPath.InnerText = ServerConfig.getInstance().IoConfigBackupPath.ToString();
            parser.ParsedNode.NodeList.Add(nodeIoConfigBackupPath);

            MyNode nodeLogPath = new MyNode();
            nodeLogPath.NodeName = "LogPath";
            nodeLogPath.InnerText = ServerConfig.getInstance().LogPath.ToString();
            parser.ParsedNode.NodeList.Add(nodeLogPath);

            MyNode nodeLogLevel = new MyNode();
            nodeLogLevel.NodeName = "LogLevel";
            nodeLogLevel.InnerText = ServerConfig.getInstance().LogLevel.ToString();
            parser.ParsedNode.NodeList.Add(nodeLogLevel);

            MyNode nodeLanguage = new MyNode();
            nodeLanguage.NodeName = "Language";
            nodeLanguage.InnerText = Convert.ToInt32(ServerConfig.getInstance().Language).ToString();
            parser.ParsedNode.NodeList.Add(nodeLanguage);

            // 转换成xml文件并存盘
            parser.ConvertToXml();
            parser.WriteToFile(fileName);
        }
    }
}
