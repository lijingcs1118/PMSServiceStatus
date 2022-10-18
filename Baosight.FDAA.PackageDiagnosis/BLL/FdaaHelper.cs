using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Baosight.FDAA.PackageDiagnosis.BLL
{
    public class FdaaHelper
    {
        private static readonly FdaaHelper SingletonInstance = new FdaaHelper();

        private FdaaHelper()
        {
            CheckFdaaIsInstalled();
        }

        public string IoConfigName { get; private set; }

        public string BasicDsConfigName { get; private set; }

        public string AdvancedDsConfigName { get; private set; }

        public string HdDsConfigName { get; private set; }

        public string MqttDsConfigName { get; private set; }

        public string DriversPath { get; private set; }

        public string FdaaOnlineConfigName { get; private set; }

        public string ClientPath { get; private set; }

        public string DiagnosePath { get; private set; }

        [DllImport("FDAALicence.dll", EntryPoint = "getAllowSignalCountCLR")]
        private static extern int getAllowSignalCountCLR();

        [DllImport("FDAALicence.dll", EntryPoint = "deletePtr")]
        private static extern void deletePtr();

        public static FdaaHelper CreateInstance()
        {
            return SingletonInstance;
        }

        public List<string> GetDriverInfo()
        {
            var installedDrivers = new List<string>();
            var files = new DirectoryInfo(DriversPath).GetFiles();
            foreach (var file in files)
                if (Path.GetExtension(file.Name) == ".dll")
                    installedDrivers.Add(Path.GetFileNameWithoutExtension(file.Name));
            return installedDrivers;
        }

        public bool CheckFdaaIsInstalled()
        {
            string installLocation;

            //获取32位
            var localMachine32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
            var uninstallNode32 =
                localMachine32.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall", false);

            //获取64位
            var localMachine64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            var uninstallNode64 = localMachine64.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall");

            foreach (var subKeyName in uninstallNode32.GetSubKeyNames())
            {
                var subKey = uninstallNode32.OpenSubKey(subKeyName);
                var OdisplayName = subKey.GetValue("DisplayName");
                if (OdisplayName != null)
                    if (OdisplayName.ToString().Contains("FDAA"))
                    {
                        var iVersion = Convert.ToInt32(subKey.GetValue("DisplayVersion").ToString().Substring(0, 1));
                        if (iVersion > 1)
                        {
                            subKey.GetValue("DisplayName").ToString();
                            subKey.GetValue("DisplayVersion").ToString();
                            installLocation = subKey.GetValue("InstallLocation").ToString();

                            IoConfigName = Path.Combine(installLocation, @"Server\config\CurrentIoConfig.io");
                            BasicDsConfigName = Path.Combine(installLocation,
                                @"Server\config\CurrentDataStorageConfig.ds");
                            AdvancedDsConfigName = Path.Combine(installLocation,
                                @"Server\config\CurrentAdvancedDataStorageConfig.ds");
                            HdDsConfigName = Path.Combine(installLocation,
                                @"Server\config\CurrentHDDataStorageConfig.ds");
                            MqttDsConfigName = Path.Combine(installLocation,
                                @"Server\config\CurrentHDDataStorageConfig.ds");
                            FdaaOnlineConfigName = Path.Combine(installLocation,
                                @"Client\ClientConfig.xml");
                            DriversPath = Path.Combine(installLocation, @"Server\drivers");
                            ClientPath = Path.Combine(installLocation, @"Client");
                            DiagnosePath = Path.Combine(installLocation, @"Server\diagnostic");
                            return true;
                        }
                    }
            }

            foreach (var subKeyName in uninstallNode64.GetSubKeyNames())
            {
                var subKey = uninstallNode64.OpenSubKey(subKeyName);
                var OdisplayName = subKey.GetValue("DisplayName");
                if (OdisplayName != null)
                    if (OdisplayName.ToString().Contains("FDAA"))
                    {
                        var iVersion = Convert.ToInt32(subKey.GetValue("DisplayVersion").ToString().Substring(0, 1));
                        if (iVersion > 1)
                        {
                            subKey.GetValue("DisplayName").ToString();
                            subKey.GetValue("DisplayVersion").ToString();
                            installLocation = subKey.GetValue("InstallLocation").ToString();

                            IoConfigName = Path.Combine(installLocation, @"\Server\config\CurrentIoConfig.io");
                            BasicDsConfigName = Path.Combine(installLocation,
                                @"Server\config\CurrentDataStorageConfig.ds");
                            AdvancedDsConfigName = Path.Combine(installLocation,
                                @"Server\config\CurrentAdvancedDataStorageConfig.ds");
                            HdDsConfigName = Path.Combine(installLocation,
                                @"Server\config\CurrentHDDataStorageConfig.ds");
                            MqttDsConfigName = Path.Combine(installLocation,
                                @"Server\config\CurrentHDDataStorageConfig.ds");
                            FdaaOnlineConfigName = Path.Combine(installLocation,
                                @"Client\ClientConfig.xml");
                            DriversPath = Path.Combine(installLocation, @"\Server\drivers");
                            ClientPath = Path.Combine(installLocation, @"Client");
                            DiagnosePath = Path.Combine(installLocation, @"Server\diagnostic");
                            return true;
                        }
                    }
            }

            return false;
        }

        public int GetLicensedSignalCount()
        {
            var _licencedSignalCount = -1;
            try
            {
                _licencedSignalCount = getAllowSignalCountCLR();
                deletePtr();
            }
            catch (Exception ex)
            {
            }

            return _licencedSignalCount;
        }
    }
}