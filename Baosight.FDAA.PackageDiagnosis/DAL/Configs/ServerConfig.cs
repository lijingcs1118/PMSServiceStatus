using System;
using System.Windows.Forms;
using Baosight.FDAA.Utility;

namespace Baosight.FDAA.PackageDiagnosis.DAL.Configs
{
    internal class ServerConfig
    {
        public static readonly ServerConfig serverConfig = new ServerConfig();

        public bool AutoStartServerOnStartup { get; set; }

        public bool StartAcquisitionOnStartup { get; set; }

        public int ServerPortNr { get; set; }

        public string IoConfigBackupPath { get; set; }

        public string LogPath { get; set; }

        public int LogLevel { get; set; }

        public bool Language { get; set; }

        public static ServerConfig getInstance()
        {
            return serverConfig;
        }

        /// <summary>
        ///     读取ServerConfig配置文件
        /// </summary>
        /// <param name="fileName">ServerConfig路径</param>
        public void LoadServerConfig(string fileName)
        {
            try
            {
                // 创建xml解析对象
                var parser = new XmlParser();
                parser.LoadConfig(fileName);

                //  读取ServerConfig属性
                getInstance().AutoStartServerOnStartup =
                    Convert.ToBoolean(Convert.ToInt32(parser.ParsedNode.GetValueByName("AutoStartServerOnStartup")));
                getInstance().StartAcquisitionOnStartup =
                    Convert.ToBoolean(Convert.ToInt32(parser.ParsedNode.GetValueByName("StartAcquisitionOnStartup")));
                getInstance().ServerPortNr = Convert.ToInt32(parser.ParsedNode.GetValueByName("ServerPortNr"));
                getInstance().IoConfigBackupPath = parser.ParsedNode.GetValueByName("IoConfigBackupPath");
                getInstance().LogPath = parser.ParsedNode.GetValueByName("LogPath");
                getInstance().LogLevel = Convert.ToInt32(parser.ParsedNode.GetValueByName("LogLevel"));
                getInstance().Language =
                    Convert.ToBoolean(Convert.ToInt32(parser.ParsedNode.GetValueByName("Language")));
            }
            catch (Exception ex)
            {
                // 如果读取文件有异常，则提示
                MessageBox.Show(ex.Message, "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                throw;
            }
        }

        /// <summary>
        ///     保存ServerConfig配置文件
        /// </summary>
        /// <param name="fileName">ServerConfig路径</param>
        public void SaveServerConfig(string fileName)
        {
            // 创建xml解析对象
            var parser = new XmlParser();
            parser.RootNodeName = "ServerConfiguration";

            var nodeAutoStartServerOnStartup = new MyNode();
            nodeAutoStartServerOnStartup.NodeName = "AutoStartServerOnStartup";
            nodeAutoStartServerOnStartup.InnerText = Convert.ToInt32(getInstance().AutoStartServerOnStartup).ToString();
            parser.ParsedNode.NodeList.Add(nodeAutoStartServerOnStartup);

            var nodeStartAcquisitionOnStartup = new MyNode();
            nodeStartAcquisitionOnStartup.NodeName = "StartAcquisitionOnStartup";
            nodeStartAcquisitionOnStartup.InnerText =
                Convert.ToInt32(getInstance().StartAcquisitionOnStartup).ToString();
            parser.ParsedNode.NodeList.Add(nodeStartAcquisitionOnStartup);

            var nodeServerPortNr = new MyNode();
            nodeServerPortNr.NodeName = "ServerPortNr";
            nodeServerPortNr.InnerText = getInstance().ServerPortNr.ToString();
            parser.ParsedNode.NodeList.Add(nodeServerPortNr);

            var nodeIoConfigBackupPath = new MyNode();
            nodeIoConfigBackupPath.NodeName = "IoConfigBackupPath";
            nodeIoConfigBackupPath.InnerText = getInstance().IoConfigBackupPath;
            parser.ParsedNode.NodeList.Add(nodeIoConfigBackupPath);

            var nodeLogPath = new MyNode();
            nodeLogPath.NodeName = "LogPath";
            nodeLogPath.InnerText = getInstance().LogPath;
            parser.ParsedNode.NodeList.Add(nodeLogPath);

            var nodeLogLevel = new MyNode();
            nodeLogLevel.NodeName = "LogLevel";
            nodeLogLevel.InnerText = getInstance().LogLevel.ToString();
            parser.ParsedNode.NodeList.Add(nodeLogLevel);

            var nodeLanguage = new MyNode();
            nodeLanguage.NodeName = "Language";
            nodeLanguage.InnerText = Convert.ToInt32(getInstance().Language).ToString();
            parser.ParsedNode.NodeList.Add(nodeLanguage);

            // 转换成xml文件并存盘
            parser.ConvertToXml();
            parser.WriteToFile(fileName);
        }
    }
}