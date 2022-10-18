using System.Linq;
using System.Xml.Linq;
using Baosight.FDAA.PackageDiagnosis.BLL;
using Baosight.FDAA.PackageDiagnosis.Model.DsEntity;

namespace Baosight.FDAA.PackageDiagnosis.DAL.Configs
{
    public class HdDsConfig
    {
        public HDObject HdObject { get; private set; }

        public void LoadHdDsConfig()
        {
            HdObject = new HDObject();
            //  HDServer节点
            XElement xeleHdServerNodes;
            //  兼容老版本
            if (XElement.Load(FdaaHelper.CreateInstance().HdDsConfigName).Element("HDServers") != null)
            {
                xeleHdServerNodes = XElement.Load(FdaaHelper.CreateInstance().HdDsConfigName).Element("HDServers");
                HdObject.Id = (string) XElement.Load(FdaaHelper.CreateInstance().HdDsConfigName).Element("General")
                    .Element("DataStorageId");
                HdObject.DataStorageName = (string) XElement.Load(FdaaHelper.CreateInstance().HdDsConfigName)
                    .Element("General").Element("DataStorageName");
                HdObject.Active = (bool) XElement.Load(FdaaHelper.CreateInstance().HdDsConfigName).Element("General")
                    .Element("Active");
                HdObject.FlushDataToDiskPeriod = (int) XElement.Load(FdaaHelper.CreateInstance().HdDsConfigName)
                    .Element("General").Element("FlushDataToDiskPeriod");
            }
            //  新版本    
            else
            {
                var node = XElement.Load(FdaaHelper.CreateInstance().HdDsConfigName).Element("HDDataStorage");
                xeleHdServerNodes = node.Element("HDServers");
                HdObject.Id = (string) node.Element("General").Element("DataStorageId");
                HdObject.DataStorageName = (string) node.Element("General").Element("DataStorageName");
                HdObject.Active = (bool) node.Element("General").Element("Active");
                HdObject.FlushDataToDiskPeriod = (int) node.Element("General").Element("FlushDataToDiskPeriod");
            }

            HdObject.HDServers = (from hdServer in xeleHdServerNodes.Elements("HD")
                select new HDServer
                {
                    No = (int) hdServer.Element("No"),
                    Name = (string) hdServer.Element("Name"),
                    IP = (string) hdServer.Element("Ip"),
                    Port = (int) hdServer.Element("Port"),
                    User = (string) hdServer.Element("User"),
                    Password = (string) hdServer.Element("Password"),
                    ConnectionTimePeriod = (int) hdServer.Element("ConnectionTimePeriod"),
                    ConnectionTimeOut = (int) hdServer.Element("ConnectionTimeOut"),
                    BufferLocation = hdServer.Element("BufferLocation") != null
                        ? (string) hdServer.Element("BufferLocation")
                        : "D:\\DATA\\HDCache",
                    BufferSize = hdServer.Element("BufferSize") != null ? (int) hdServer.Element("BufferSize") : 10240,
                    Schemas = (from schema in hdServer.Element("StorageSchemes").Elements("Scheme")
                        select new Schema
                        {
                            No = (int) schema.Element("General").Element("No"),
                            Name = (string) schema.Element("General").Element("Name"),
                            Enabled = (bool) schema.Element("General").Element("Enabled"),
                            NumberofAnalogSignals = (int) schema.Element("General").Element("NumberofAnalogSignals"),
                            NumberofDigitalSignals = (int) schema.Element("General").Element("NumberofDigitalSignals"),
                            FlushCycle = (int) schema.Element("General").Element("FlushCycle"),
                            Timebase = (int) schema.Element("Profile").Element("Timebase"),
                            FilterMode =
                                Helper.FilterNumberToString((string) schema.Element("Profile").Element("FilterMode")),
                            Prefix = (string) schema.Element("Tag").Element("Prefix"),
                            AddPrefixToTagName = (bool) schema.Element("Tag").Element("AddPrefixToTagName"),
                            AddSignalName = schema.Element("Tag").Element("AddSignalName") != null
                                ? (bool) schema.Element("Tag").Element("AddSignalName")
                                : false,
                            ReplaceSpaceWith = schema.Element("Tag").Element("ReplaceSpaceWith") != null
                                ? (string) schema.Element("Tag").Element("ReplaceSpaceWith")
                                : "_",
                            Type = Helper.TypeNumberToString((string) schema.Element("Tag").Element("Type")),
                            AnalogDataType =
                                Helper.AnalogDataTypeNumberToString((string) schema.Element("Tag")
                                    .Element("AnalogDataType")),
                            SyncTagDescription = (bool) schema.Element("Tag").Element("SyncTagDescription"),
                            SyncroDelete = (bool) schema.Element("Tag").Element("SyncroDelete"),
                            Signals = schema.Element("Signals").Elements("Signal").Select(signal => signal.Value)
                                .ToList()
                        }).ToDictionary(key => key.No, value => value)
                }).ToDictionary(key => key.No, value => value);
            // Add new hd object to the collection
            HdObject.DsType = DATA_STORAGE_TYPE.dsHD;
        }
    }
}