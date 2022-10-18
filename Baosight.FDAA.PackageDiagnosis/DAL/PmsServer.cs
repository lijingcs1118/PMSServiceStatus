using System;
using System.Collections.Generic;
using System.Diagnostics;
using Baosight.FDAA.PackageDiagnosis.Model.Entities;
using Baosight.FDAA.PackageDiagnosis.Model.Enums;
using pmsServiceLib;

namespace Baosight.FDAA.PackageDiagnosis.DAL
{
    public class PmsServer
    {
        private static readonly PmsServer Instance = new PmsServer();

        private PmsServer()
        {
            ServerStatus = ServiceStatus.Unknown;
        }

        public ServiceStatus ServerStatus { get; private set; }

        public PMSSupervisor PmsService { get; private set; }

        public static PmsServer CreateInstance()
        {
            return Instance;
        }

        public void InitPmsService()
        {
            if (CheckServiceIfExist("pmsService"))
            {
                if (PmsService == null)
                {
                    PmsService = new PMSSupervisor();
                    PmsService.ServiceStatusNotify += PmsService_ServiceStatusNotify;
                    ServerStatus = ServiceStatus.Running;
                }
            }
            else
            {
                PmsService = null;
                ServerStatus = ServiceStatus.Unknown;
            }
        }

        private void PmsService_ServiceStatusNotify(int bRunningNow)
        {
            if (Convert.ToBoolean(bRunningNow))
            {
                ServerStatus = ServiceStatus.Running;
            }
            else
            {
                ServerStatus = ServiceStatus.Stopped;
                PmsService = null;
            }
        }

        /// <summary>
        ///     检查指定进程进程是否存在
        /// </summary>
        /// <param name="serviceName">进程名称</param>
        /// <returns></returns>
        public static bool CheckServiceIfExist(string serviceName)
        {
            var result = true;
            var proc = Process.GetProcessesByName(serviceName);

            if (proc.Length == 0) result = false;
            return result;
        }

        /// <summary>
        ///     获取所有驱动的授权信息
        /// </summary>
        public List<Driver> GetDriverInfo()
        {
            var drivers = new List<Driver>();
            if (PmsService == null) return drivers;

            //  驱动信息
            var lstDriverInfos = new List<_arrayItem>();
            Array driverInfos = lstDriverInfos.ToArray();

            try
            {
                PmsService.getDriversInfo(out driverInfos);
            }
            catch (Exception e)
            {
                PmsService = null;
                Console.WriteLine(e);
                throw;
            }

            if (driverInfos.GetValue(0) != null)
                for (var i = 0; i < driverInfos.Length; i++)
                {
                    var name = ((_arrayItem) driverInfos.GetValue(i)).param0;
                    var version = ((_arrayItem) driverInfos.GetValue(i)).param1;
                    var license = ((_arrayItem) driverInfos.GetValue(i)).param2;
                    if (name == "DBStorageManager") continue;
                    var driver = new Driver();

                    if (name == "pmsService" && license == "99999")
                        driver.License = "unlimited";
                    else
                        driver.License = license;

                    driver.Name = name;
                    driver.Version = version;
                    drivers.Add(driver);
                }

            return drivers;
        }

        public Dictionary<string, double> GetSignalValues(ushort moduleNo)
        {
            var signalValues = new Dictionary<string, double>();
            if (PmsService == null) return signalValues;

            var lstSignalValueItems = new List<_signalValueItem>();
            Array SignalValueItems = lstSignalValueItems.ToArray();

            try
            {
                PmsService.getSignalValues(moduleNo, out SignalValueItems);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }

            for (var i = 0; i < SignalValueItems.Length; i++)
                signalValues.Add(((_signalValueItem) SignalValueItems.GetValue(i))._no,
                    ((_signalValueItem) SignalValueItems.GetValue(i))._value);

            return signalValues;
        }

        internal Dictionary<string, double> GetStringValues(ushort moduleNo)
        {
            var signalValues = new Dictionary<string, double>();
            if (PmsService == null) return signalValues;


            List<_stringValueItem> lstStringValueItemValueItems = new List<_stringValueItem>();
            Array StringValueItems = lstStringValueItemValueItems.ToArray();

            try
            {
                PmsService.getStringValues(moduleNo, out StringValueItems);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }

            for (int i = 0; i < StringValueItems.Length; i++)
            {
                double value;
                double.TryParse(((_stringValueItem) StringValueItems.GetValue(i))._value, out value);
                signalValues.Add(((_stringValueItem)StringValueItems.GetValue(i))._no, value);
            }

            return signalValues;
        }

        /// <summary>
        ///     获取S7连接具体信息
        /// </summary>
        /// <param name="moduleNo">模块号</param>
        /// <returns>S7连接信息</returns>
        public S7Status GetModuleStatus(ushort moduleNo)
        {
            var s7Status = new S7Status();
            if (PmsService == null) return s7Status;

            uint executeTimeMin;
            uint executeTimeMax;
            uint executeTimeAverage;
            uint executeTimeActual;
            int statusCode;
            ushort executeTimes;

            PmsService.getModuleStatus(moduleNo, out executeTimeMin, out executeTimeMax, out executeTimeAverage,
                out executeTimeActual, out statusCode, out executeTimes);

            s7Status.ExecuteTimeMin = executeTimeMin;
            s7Status.ExecuteTimeMax = executeTimeMax;
            s7Status.ExecuteTimeAve = executeTimeAverage;
            s7Status.ExecuteTimeActual = executeTimeActual;
            s7Status.StatusCode = statusCode;
            s7Status.ExecuteTimes = executeTimes;

            return s7Status;
        }
    }
}