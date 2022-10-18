using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PMSServiceStatus
{
    public class Helper
    {
        // Signal data type
        enum SIGNAL_DATA_TYPE
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
        };

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
            SIGNAL_DATA_TYPE dt = (SIGNAL_DATA_TYPE)Convert.ToInt32(numberDataType);

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
    }
}
