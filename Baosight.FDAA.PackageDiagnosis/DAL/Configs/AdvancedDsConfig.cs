using System;
using Baosight.FDAA.PackageDiagnosis.BLL;
using Baosight.FDAA.PackageDiagnosis.Model.DsEntity;
using Baosight.FDAA.Utility;

namespace Baosight.FDAA.PackageDiagnosis.DAL.Configs
{
    public class AdvancedDsConfig
    {
        public DSObject AdvancedDs { get; private set; }

        public void LoadAdvancedDsConfig()
        {
            // 创建XML文件读取对象
            var parser = new XmlParser();

            // 加载CurrentAdvancedDataStorageConfig.ds配置文件
            parser.LoadConfig(FdaaHelper.CreateInstance().AdvancedDsConfigName);

            var nodeAdvancedDataStorages = new MyNode();
            nodeAdvancedDataStorages = parser.ParsedNode.NodeList[0];

            // 遍历DataStorage节点
            foreach (var nodeDataStorage in nodeAdvancedDataStorages.NodeList)
            {
                // 创建一个新的ds对象
                AdvancedDs = new DSObject();

                // 读取General节点
                var nodeGeneral = new MyNode();
                nodeGeneral = nodeDataStorage.GetNodeByName("General");
                AdvancedDs.Id = nodeGeneral.GetValueByName("DataStorageId");
                AdvancedDs.DataStorageName = nodeGeneral.GetValueByName("DataStorageName");
                var yes = Convert.ToInt32(nodeGeneral.GetValueByName("Active"));
                AdvancedDs.Active = Convert.ToBoolean(yes);
                AdvancedDs.FlushDataToDiskPeriod = Convert.ToInt32(nodeGeneral.GetValueByName("FlushDataToDiskPeriod"));

                // 读取Files节点
                var nodeFiles = new MyNode();
                nodeFiles = nodeDataStorage.GetNodeByName("Files");
                AdvancedDs.MaxRecordingTime = Convert.ToInt32(nodeFiles.GetValueByName("MaxRecordingTime"));
                AdvancedDs.BaseFileName = nodeFiles.GetValueByName("BaseFileName");
                AdvancedDs.MaximumFileNumber = Convert.ToInt32(nodeFiles.GetValueByName("MaximumFileNumber"));
                AdvancedDs.NextFileNumber = Convert.ToInt32(nodeFiles.GetValueByName("NextFileNumber"));
                // 向前兼容
                var temp = nodeFiles.GetValueByName("AddTechnoString");
                yes = temp != string.Empty ? Convert.ToInt32(temp) : 0;
                AdvancedDs.AddTechnoString = Convert.ToBoolean(yes);
                yes = Convert.ToInt32(nodeFiles.GetValueByName("AddBaseFileName"));
                AdvancedDs.AddBaseFileName = Convert.ToBoolean(yes);
                yes = Convert.ToInt32(nodeFiles.GetValueByName("AddFileNumber"));
                AdvancedDs.AddFileNumber = Convert.ToBoolean(yes);
                yes = Convert.ToInt32(nodeFiles.GetValueByName("AddDateTime"));
                AdvancedDs.AddDateTime = Convert.ToBoolean(yes);
                AdvancedDs.BaseDirectory = nodeFiles.GetValueByName("BaseDirectory");
                AdvancedDs.SubDirectory =
                    (SUB_DIRECTORY_TYPE) Convert.ToInt32(nodeFiles.GetValueByName("SubDirectory"));
                yes = Convert.ToInt32(nodeFiles.GetValueByName("RestartFileNumberWhenSubdirectoryChanges"));
                AdvancedDs.RestartFileNumberWhenSubdirectoryChanges = Convert.ToBoolean(yes);
                AdvancedDs.FileFormat = nodeFiles.GetValueByName("FileFormat");
                AdvancedDs.StarttimeStrategy = Convert.ToInt32(nodeFiles.GetValueByName("StarttimeStrategy"));
                AdvancedDs.TechnoStringSignal = nodeFiles.GetValueByName("TechnoStringSignal");

                // 读取CleanupStrategy节点
                var nodeCleanupStrategy = new MyNode();
                nodeCleanupStrategy = nodeDataStorage.GetNodeByName("CleanupStrategy");
                // active or not
                yes = Convert.ToInt32(nodeCleanupStrategy.GetValueByName("Active"));
                AdvancedDs.IsCleanupStrategyActive = Convert.ToBoolean(yes);
                AdvancedDs.KeepMinimalSpace = Convert.ToInt32(nodeCleanupStrategy.GetValueByName("KeepMinimalSpace"));

                // 读取Signals节点
                var nodeSignals = new MyNode();
                nodeSignals = nodeDataStorage.GetNodeByName("Signals");
                for (var i = 0; i < nodeSignals.NodeList.Count; i++)
                {
                    var nodeSignal = nodeSignals.NodeList[i];
                    AdvancedDs.Signals.Add(nodeSignal.InnerText);
                }

                // 读取StartTrigger节点
                var nodeStartTrigger = new MyNode();
                nodeStartTrigger = nodeDataStorage.GetNodeByName("StartTrigger");
                yes = Convert.ToInt32(nodeStartTrigger.GetValueByName("Active"));
                AdvancedDs.StartTriggerActive = Convert.ToBoolean(yes);
                AdvancedDs.PreTriggerTime = Convert.ToInt32(nodeStartTrigger.GetValueByName("PreTriggerTime"));
                AdvancedDs.StartTriggerSignal = nodeStartTrigger.GetValueByName("TriggerSignal");
                AdvancedDs.StartTriggerType =
                    (TRIGGER_TYPE) Convert.ToInt32(nodeStartTrigger.GetValueByName("TriggerType"));
                AdvancedDs.StartTriggerLevel = Convert.ToInt32(nodeStartTrigger.GetValueByName("TriggerLevel"));
                AdvancedDs.StartTirggerAgainMode =
                    (START_TRIGGER_AGAIN_MODE) Convert.ToInt32(
                        nodeStartTrigger.GetValueByName("StartTriggerAgainMode"));
                AdvancedDs.MaxOverlappedFiles = Convert.ToInt32(nodeStartTrigger.GetValueByName("MaxOverlappedFiles"));
                // 读取StopTrigger节点
                var nodeStopTrigger = new MyNode();
                nodeStopTrigger = nodeDataStorage.GetNodeByName("StopTrigger");
                yes = Convert.ToInt32(nodeStopTrigger.GetValueByName("Active"));
                AdvancedDs.StopTriggerActive = Convert.ToBoolean(yes);
                AdvancedDs.PostTriggerTime = Convert.ToInt32(nodeStopTrigger.GetValueByName("PostTriggerTime"));
                AdvancedDs.StopTriggerSignal = nodeStopTrigger.GetValueByName("TriggerSignal");
                AdvancedDs.StopTriggerType =
                    (TRIGGER_TYPE) Convert.ToInt32(nodeStopTrigger.GetValueByName("TriggerType"));
                AdvancedDs.StopTriggerLevel = Convert.ToInt32(nodeStopTrigger.GetValueByName("TriggerLevel"));
                // 读取PostProcess 节点
                var nodePostProcess = new MyNode();
                nodePostProcess = nodeDataStorage.GetNodeByName("PostProcess");
                yes = Convert.ToInt32(nodePostProcess.GetValueByName("RunningGuardian"));
                AdvancedDs.RunningGuarian = Convert.ToBoolean(yes);
                AdvancedDs.CommandLine = nodePostProcess.GetValueByName("CommandLine");
            }
        }
    }
}