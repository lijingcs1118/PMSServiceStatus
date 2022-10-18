using System;
using System.Text.RegularExpressions;
using Baosight.FDAA.PackageDiagnosis.Model.Enums;

namespace Baosight.FDAA.PackageDiagnosis.DAL.Configs
{
    public class Helper
    {
        public static string ConvertLogNumberToString(int logNumber)
        {
            switch (logNumber)
            {
                case 60000:
                    return "Off";
                case 50000:
                    return "Fatal";
                case 40000:
                    return "Error";
                case 30000:
                    return "Warn";
                case 20000:
                    return "Info";
                case 10000:
                    return "Debug";
                default:
                    return "Fatal";
            }
        }

        public static int ConvertStrLogNumberToInt(string strlogNumber)
        {
            switch (strlogNumber)
            {
                case "Off":
                    return 60000;
                case "Fatal":
                    return 50000;
                case "Error":
                    return 40000;
                case "Warn":
                    return 30000;
                case "Info":
                    return 20000;
                case "Debug":
                    return 10000;
                default:
                    return 50000;
            }
        }

        public static string SigDataTypeNumber2String(string numberDataType)
        {
            string stringDataType;
            var dt = (SIGNAL_DATA_TYPE) Convert.ToInt32(numberDataType);

            switch (dt)
            {
                case SIGNAL_DATA_TYPE.sdtBOOL:
                    stringDataType = "BOOL";
                    break;
                case SIGNAL_DATA_TYPE.sdtBYTE:
                    stringDataType = "BYTE";
                    break;
                case SIGNAL_DATA_TYPE.sdtWORD:
                    stringDataType = "WORD";
                    break;
                case SIGNAL_DATA_TYPE.sdtDWORD:
                    stringDataType = "DWORD";
                    break;
                case SIGNAL_DATA_TYPE.sdtINT:
                    stringDataType = "INT";
                    break;
                case SIGNAL_DATA_TYPE.sdtDINT:
                    stringDataType = "DINT";
                    break;
                case SIGNAL_DATA_TYPE.sdtREAL:
                    stringDataType = "REAL";
                    break;
                case SIGNAL_DATA_TYPE.sdtSINT:
                    stringDataType = "SINT";
                    break;
                case SIGNAL_DATA_TYPE.sdtUSINT:
                    stringDataType = "USINT";
                    break;
                case SIGNAL_DATA_TYPE.sdtUINT:
                    stringDataType = "UINT";
                    break;
                case SIGNAL_DATA_TYPE.sdtUDINT:
                    stringDataType = "UDINT";
                    break;
                case SIGNAL_DATA_TYPE.sdtLREAL:
                    stringDataType = "LREAL";
                    break;
                case SIGNAL_DATA_TYPE.sdtSTRING:
                    stringDataType = "STRING";
                    break;
                default:
                    stringDataType = "INT";
                    break;
            }

            return stringDataType;
        }

        public static bool IsNumeric(string value)
        {
            value = value.Replace(":", "").Replace(".", "").Replace("-", "");
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }

        public static string FilterNumberToString(string numberFilter)
        {
            string stringFilter;

            switch (numberFilter)
            {
                case "0":
                    stringFilter = "无";
                    break;
                case "1":
                    stringFilter = "最大值";
                    break;
                case "2":
                    stringFilter = "最小值";
                    break;
                case "3":
                    stringFilter = "平均值";
                    break;
                default:
                    stringFilter = "无";
                    break;
            }

            return stringFilter;
        }

        public static string TypeNumberToString(string numberType)
        {
            string stringType;

            switch (numberType)
            {
                case "0":
                    stringType = "Archive";
                    break;
                case "1":
                    stringType = "Refresh";
                    break;
                default:
                    stringType = "Archive";
                    break;
            }

            return stringType;
        }

        public static string AnalogDataTypeNumberToString(string numberAnalogDataType)
        {
            string stringAnalogDataType;

            switch (numberAnalogDataType)
            {
                case "0":
                    stringAnalogDataType = "float32";
                    break;
                case "1":
                    stringAnalogDataType = "float64";
                    break;
                default:
                    stringAnalogDataType = "float32";
                    break;
            }

            return stringAnalogDataType;
        }


        /// <summary>
        ///     根据秒数得到datetime
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static DateTime GetDateTimeBySeconds(long seconds)
        {
            //return DateTime.Parse(DateTime.Now.ToString("1970-01-01 00:00:00")).AddSeconds(seconds);
            var dt = new DateTime(1970, 1, 1);
            dt = dt.AddSeconds(seconds);
            return dt.AddHours(8);
        }

        public static string NboNumber2String(string numberNBO)
        {
            string stringNBO;
            var nb = (NetwordByteOrder) Convert.ToInt32(numberNBO);

            switch (nb)
            {
                case NetwordByteOrder.AutoDetected:
                    stringNBO = "Auto Detected";
                    break;
                case NetwordByteOrder.LittleEndian:
                    stringNBO = "Little Endian";
                    break;
                case NetwordByteOrder.BigEndian:
                    stringNBO = "Big Endian";
                    break;
                default:
                    stringNBO = "Little Endian";
                    break;
            }

            return stringNBO;
        }

        // Signal data type
        private enum SIGNAL_DATA_TYPE
        {
            sdtBOOL = 0,
            sdtBYTE = 1,
            sdtWORD = 2,
            sdtDWORD = 3,
            sdtINT = 4,
            sdtDINT = 5,
            sdtREAL = 6,
            sdtSINT = 7,
            sdtUSINT = 8,
            sdtUINT = 9,
            sdtUDINT = 10,
            sdtLREAL = 11,
            sdtSTRING = 20
        }
    }
}