using System;
using System.Management;
using Microsoft.Win32;

namespace Baosight.FDAA.PackageDiagnosis.BLL
{
    public class MachineHelper
    {
        /// <summary>
        ///     检查.Net环境是否大于4.7.2 是返回True 否返回False
        /// </summary>
        /// <returns></returns>
        public static bool CheckDotNetVersionIsGreaterThan472()
        {
            const string ndpV4Full = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";
            using (var ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)
                .OpenSubKey(ndpV4Full))
            {
                if (ndpKey != null && ndpKey.GetValue("Release") != null)
                    return CheckFor45PlusVersion((int) ndpKey.GetValue("Release"));
            }

            return false;
        }

        /// <summary>
        ///     检查.Net2.0是否安装 是返回True 否返回False
        /// </summary>
        /// <returns></returns>
        public static bool CheckDotNet20IsInstalled()
        {
            const string ndpV4Full = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v2.0.50727\";
            using (var ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)
                .OpenSubKey(ndpV4Full))
            {
                if (ndpKey != null && Equals(ndpKey.GetValue("Install"), 1))
                    return true;
            }

            return false;
        }

        private static bool CheckFor45PlusVersion(int releaseKey)
        {
            if (releaseKey >= 528040)
                return true;
            if (releaseKey >= 461808)
                return true;
            if (releaseKey >= 461308)
                return false;
            if (releaseKey >= 460798)
                return false;
            if (releaseKey >= 394802)
                return false;
            if (releaseKey >= 394254)
                return false;
            if (releaseKey >= 393295)
                return false;
            if (releaseKey >= 379893)
                return false;
            if (releaseKey >= 378675)
                return false;
            if (releaseKey >= 378389)
                return false;
            // This code should never execute. A non-null release key should mean
            // that 4.5 or later is installed.
            return false;
        }

        /// <summary>
        ///     检查机器VC++ 2010版本是否安装
        /// </summary>
        /// <returns>True安装  false未安装</returns>
        public static bool CheckVcIsInstalled()
        {
            //  64位系统
            if (Environment.Is64BitOperatingSystem)
            {
                const string sub10Key = @"SOFTWARE\Wow6432Node\Microsoft\VisualStudio\10.0\VC\VCRedist\x86";
                using (var ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
                    .OpenSubKey(sub10Key))
                {
                    if (ndpKey != null && Equals(ndpKey.GetValue("installed"), 1))
                        return true;
                }
            }
            else
            {
                const string sub10Key = @"SOFTWARE\Microsoft\VisualStudio\10.0\VC\VCRedist\x86";
                using (var ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)
                    .OpenSubKey(sub10Key))
                {
                    if (ndpKey != null && Equals(ndpKey.GetValue("installed"), 1))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     检查指定板卡驱动是否安装
        /// </summary>
        /// <param name="strValue">驱动名称</param>
        /// <returns>true为安装 false未安装</returns>
        public static bool CheckCardDriverIsInstalled(string strValue)
        {
            var query = @"SELECT * FROM Win32_PnPEntity";

            var moSearch = new ManagementObjectSearcher(query);
            var moCollection = moSearch.Get();

            foreach (var o in moCollection)
            {
                var mo = (ManagementObject) o;
                foreach (var item in mo.Properties)
                    if (item.Value != null && item.Value.ToString().Contains(strValue))
                        return true;
            }

            return false;
        }
    }
}