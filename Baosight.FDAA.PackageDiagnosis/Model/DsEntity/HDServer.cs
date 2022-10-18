using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Baosight.FDAA.PackageDiagnosis.Model.DsEntity
{
    public class HDServer
    {
        private int bufferSize;
        private int connectionTimeOut;
        private int connectionTimePeriod;
        private int port;
        private Dictionary<int, Schema> schemas = new Dictionary<int, Schema>();

        public HDServer()
        {
        }

        public HDServer(int hdNo, string IP)
        {
            No = hdNo;
            Name = "HD Server" + hdNo;
            this.IP = IP;
            port = 5673;
            User = "admin";
            Password = "admin";
            connectionTimePeriod = 10;
            ConnectionTimeOut = 3;
            //bufferLocation = DSObjectCollection.getInstance().DataStorageCollection.ElementAt(0).Value.BaseDirectory + @"\HDCache";
            bufferSize = 10240;
        }

        public int No { get; set; }

        public string Name { get; set; }

        public string IP { get; set; }

        public int Port
        {
            get { return port; }
            set
            {
                if (value >= 1024 && value <= 65535)
                    port = value;
                else
                    return;
            }
        }

        public string User { get; set; }

        public string Password { get; set; }

        public int ConnectionTimePeriod
        {
            get { return connectionTimePeriod; }
            set
            {
                if (value >= 5 && value <= 30)
                    connectionTimePeriod = value;
                else
                    return;
            }
        }

        public int ConnectionTimeOut
        {
            get { return connectionTimeOut; }
            set
            {
                if (value >= 1 && value <= 5)
                    connectionTimeOut = value;
                else
                    return;
            }
        }

        public string BufferLocation { get; set; }

        public int BufferSize
        {
            get { return bufferSize; }
            set
            {
                if (value >= 1024 && value <= 102400)
                    bufferSize = value;
            }
        }

        public int TotalsignalCount { get; set; }

        public int TotalTagCount { get; set; }

        public Dictionary<int, Schema> Schemas
        {
            get { return schemas; }
            set { schemas = value; }
        }

        /// <summary>
        ///     生成随机IP
        /// </summary>
        /// <returns></returns>
        public static string GenerateIpAddress()
        {
            var sj = new Random();
            var s = "";
            for (var i = 0; i <= 3; i++)
            {
                var q = sj.Next(0, 255).ToString();
                if (i < 3)
                    s += q + ".";
                else
                    s += q;
            }

            var zz = Regex.IsMatch(s,
                "^((25[0-5]|2[0-4]\\d|((1\\d{2})|([1-9]?\\d)))\\.){3}(25[0-5]|2[0-4]\\d|((1\\d{2})|([1-9]?\\d)))$");
            if (zz)
                return s;
            return "";
        }

        #region PropertyConverter

        /// <summary>
        ///     属性栏密码黑化显示
        /// </summary>
        public class PasswordStringConverter : StringConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return base.CanConvertFrom(context, sourceType);
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if (destinationType.GetType() == typeof(string))
                    return true;

                return base.CanConvertTo(context, destinationType);
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                Type destinationType)
            {
                if (value.GetType() == typeof(string))
                {
                    int stringSize;
                    var retVal = "*";
                    var randomString = new Random();


                    if (value != null)
                        stringSize = ((string) value).Length;
                    else
                        stringSize = randomString.Next(10);

                    for (var i = 0; i < stringSize; i++)
                        retVal += "*";

                    return retVal;
                }

                return base.ConvertTo(context, culture, value, destinationType);
            }

            public override bool GetPropertiesSupported(ITypeDescriptorContext context)
            {
                return false;
            }

            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                var standardValues = new string[1];
                int stringSize;
                var retVal = "*";
                var randomString = new Random();


                stringSize = randomString.Next(10);

                for (var i = 0; i < stringSize; i++)
                    retVal += "*";

                standardValues[0] = retVal;

                return new StandardValuesCollection(standardValues);
            }
        }

        /// <summary>
        ///     使用正则表达式验证IP地址
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsValidIP(string ip)
        {
            if (Regex.IsMatch(ip, "[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}"))
            {
                var ips = ip.Split('.');
                if (ips.Length == 4 || ips.Length == 6)
                {
                    if (int.Parse(ips[0]) < 256 && (int.Parse(ips[1]) < 256) & (int.Parse(ips[2]) < 256) &
                        (int.Parse(ips[3]) < 256))
                        return true;
                    return false;
                }

                return false;
            }

            return false;
        }

        #endregion
    }
}