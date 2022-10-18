using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Baosight.FDAA.PackageDiagnosis.BLL
{
    public class NetworkHelper
    {
        /// <summary>
        ///     检查指定IP是否可以PING通
        /// </summary>
        /// <param name="nameOrAddress">Ip地址</param>
        /// <returns>可以Ping通返回True 不可以返回False</returns>
        public static bool PingHost(string nameOrAddress)
        {
            var pingable = false;
            Ping pinger = null;

            try
            {
                //var sw = new Stopwatch();
                //sw.Start();

                pinger = new Ping();
                var reply = pinger.Send(nameOrAddress, 1000);
                pingable = reply.Status == IPStatus.Success;

                //sw.Stop();
                //Console.WriteLine(sw.Elapsed.TotalMilliseconds);
            }
            catch (PingException)
            {
            }
            finally
            {
                if (pinger != null) pinger.Dispose();
            }

            return pingable;
        }

        public static bool CheckPortEnable(string strIpAddress, int port)
        {
            var ipAddress = strIpAddress;
            var portNum = port;
            var ip = IPAddress.Parse(ipAddress);
            var point = new IPEndPoint(ip, portNum);

            bool portEnable;
            try
            {
                using (var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    sock.Connect(point);
                    sock.Close();

                    portEnable = true;
                }
            }
            catch (SocketException e)
            {
                portEnable = false;
            }

            return portEnable;
        }
    }
}