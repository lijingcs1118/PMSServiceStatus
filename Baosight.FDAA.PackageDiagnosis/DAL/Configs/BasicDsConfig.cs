using System;
using Baosight.FDAA.PackageDiagnosis.BLL;
using Baosight.FDAA.PackageDiagnosis.Model.DsEntity;
using Baosight.FDAA.Utility;

namespace Baosight.FDAA.PackageDiagnosis.DAL.Configs
{
    public class BasicDsConfig
    {
        public DSObject Ds { get; private set; }

        public void LoadBasicDSConfig()
        {
            // 创建XML文件读取对象
            var parser = new XmlParser();

            // 加载指定的xml文件
            parser.LoadConfig(FdaaHelper.CreateInstance().BasicDsConfigName);

            // 遍历DataStorage节点
            foreach (var nodeDataStorage in parser.ParsedNode.NodeList)
            {
                // 创建一个新的ds对象
                Ds = new DSObject();

                // 读取General节点
                var nodeGeneral = new MyNode();
                nodeGeneral = nodeDataStorage.GetNodeByName("General");
                Ds.Id = nodeGeneral.GetValueByName("DataStorageId");
                Ds.DataStorageName = nodeGeneral.GetValueByName("DataStorageName");
                var yes = Convert.ToInt32(nodeGeneral.GetValueByName("Active"));
                Ds.Active = Convert.ToBoolean(yes);
                Ds.FlushDataToDiskPeriod = Convert.ToInt32(nodeGeneral.GetValueByName("FlushDataToDiskPeriod"));

                // 读取Files节点
                var nodeFiles = new MyNode();
                nodeFiles = nodeDataStorage.GetNodeByName("Files");
                Ds.MaxRecordingTime = Convert.ToInt32(nodeFiles.GetValueByName("MaxRecordingTime"));
                Ds.BaseFileName = nodeFiles.GetValueByName("BaseFileName");
                Ds.MaximumFileNumber = Convert.ToInt32(nodeFiles.GetValueByName("MaximumFileNumber"));
                Ds.NextFileNumber = Convert.ToInt32(nodeFiles.GetValueByName("NextFileNumber"));
                // 向前兼容
                var temp = nodeFiles.GetValueByName("AddTechnoString");
                yes = temp != string.Empty ? Convert.ToInt32(temp) : 0;
                Ds.AddTechnoString = Convert.ToBoolean(yes);
                yes = Convert.ToInt32(nodeFiles.GetValueByName("AddBaseFileName"));
                Ds.AddBaseFileName = Convert.ToBoolean(yes);
                yes = Convert.ToInt32(nodeFiles.GetValueByName("AddFileNumber"));
                Ds.AddFileNumber = Convert.ToBoolean(yes);
                yes = Convert.ToInt32(nodeFiles.GetValueByName("AddDateTime"));
                Ds.AddDateTime = Convert.ToBoolean(yes);
                Ds.BaseDirectory = nodeFiles.GetValueByName("BaseDirectory");
                Ds.SubDirectory = (SUB_DIRECTORY_TYPE) Convert.ToInt32(nodeFiles.GetValueByName("SubDirectory"));
                yes = Convert.ToInt32(nodeFiles.GetValueByName("RestartFileNumberWhenSubdirectoryChanges"));
                Ds.RestartFileNumberWhenSubdirectoryChanges = Convert.ToBoolean(yes);
                Ds.FileFormat = nodeFiles.GetValueByName("FileFormat");
                Ds.StarttimeStrategy = Convert.ToInt32(nodeFiles.GetValueByName("StarttimeStrategy"));

                // 读取CleanupStrategy节点
                var nodeCleanupStrategy = new MyNode();
                nodeCleanupStrategy = nodeDataStorage.GetNodeByName("CleanupStrategy");
                Ds.KeepMinimalSpace = nodeCleanupStrategy.HasChildNode
                    ? Convert.ToInt32(nodeCleanupStrategy.GetValueByName("KeepMinimalSpace"))
                    : 500;

                if (nodeCleanupStrategy.NodeList.Count >= 2)
                {
                    temp = nodeCleanupStrategy.GetValueByName("DeleteVideoFilesFirst");
                    yes = Convert.ToInt32(temp);
                    Ds.DeleteVideoFiles = Convert.ToBoolean(yes);
                }
                else
                {
                    Ds.DeleteVideoFiles = false;
                }
            }
        }
    }
}