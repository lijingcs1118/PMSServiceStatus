using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Baosight.FDAA.PackageDiagnosis.Model.Entities;

namespace Baosight.FDAA.PackageDiagnosis.BLL
{
    public class UdpProcessor
    {
        /// <summary>
        ///     检测本地指定端口是否可以绑定
        /// </summary>
        /// <param name="port">需要检测的本地端口号</param>
        /// <returns>是否可以绑定 True可以 False 不可以</returns>
        public Tuple<int, bool> CheckPortBindable(int port)
        {
            var udpClient = new UdpClient();
            udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
            using (udpClient)
            {
                try
                {
                    udpClient.Client.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), port));
                    return new Tuple<int, bool>(port, true);
                }
                catch (Exception e)
                {
                    return new Tuple<int, bool>(port, false);
                }
            }
        }

        /// <summary>
        ///     检测本地指定端口是否可以接收到报文
        /// </summary>
        /// <param name="port">需要检测的本地端口号</param>
        /// <returns>是否可以接收 True可以 False 不可以</returns>
        public Tuple<int, bool> CheckIsReceiveData(int port)
        {
            var udpClient = new UdpClient();
            udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), port));
            udpClient.Client.ReceiveTimeout = 2000;

            using (udpClient)
            {
                var remoteEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
                try
                {
                    udpClient.Receive(ref remoteEndPoint);
                    return new Tuple<int, bool>(port, true);
                }
                catch (Exception e)
                {
                    return new Tuple<int, bool>(port, false);
                }
            }
        }

        /// <summary>
        ///     检测源地址，目标端口是否可以接收到多播报文
        /// </summary>
        /// <param name="multipleAddress">多播IP地址段</param>
        /// <param name="port">需要检测的本地端口号</param>
        /// <param name="sourceAddress">源地址</param>
        /// <returns>address port 是否可以接收数据 True可以 False 不可以</returns>
        public Tuple<Tuple<string, int,string>, bool> CheckIsReceiveData(string multipleAddress, int port,string sourceAddress)
        {
            var udpClient = new UdpClient();
            udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Parse(sourceAddress), port));
            udpClient.JoinMulticastGroup(IPAddress.Parse(multipleAddress));
            udpClient.Client.ReceiveTimeout = 2000;

            using (udpClient)
            {
                var remoteEndPoint = new IPEndPoint(IPAddress.Parse(multipleAddress), port);
                try
                {
                    udpClient.Receive(ref remoteEndPoint);
                    return new Tuple<Tuple<string, int, string>, bool>(new Tuple<string, int, string>(multipleAddress, port, sourceAddress), true);
                }
                catch (Exception e)
                {
                    return new Tuple<Tuple<string, int, string>, bool>(new Tuple<string, int, string>(multipleAddress, port, sourceAddress), false);
                }
            }
        }

        /// <summary>
        ///     接受指定端口的数据
        /// </summary>
        /// <param name="port">端口号</param>
        /// <returns>item1端口号 item2此端口解析出的小大端报文数据集合</returns>
        public Tuple<int, List<Tuple<Telegram, Telegram>>> ReceiveData(int port)
        {
            var udpClient = new UdpClient();
            udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), port));
            udpClient.Client.ReceiveTimeout = 2000;

            var telegrams = new List<Tuple<Telegram, Telegram>>();

            using (udpClient)
            {
                var sw = new Stopwatch();
                sw.Start();
                while (sw.ElapsedMilliseconds < 2000)
                {
                    var remoteEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
                    try
                    {
                        var receivedResults = udpClient.Receive(ref remoteEndPoint);

                        if (receivedResults.Length < 8) continue;

                        var telegram = ResolveTelegram(receivedResults);

                        telegrams.Add(telegram);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }

            return new Tuple<int, List<Tuple<Telegram, Telegram>>>(port, telegrams);
        }

        /// <summary>
        ///     单播单条报文解析
        /// </summary>
        /// <param name="receivedResults">数据报</param>
        /// <returns>解析数据Item1 小端解析 item2大端解析</returns>
        private static Tuple<Telegram, Telegram> ResolveTelegram(byte[] receivedResults)
        {
            var littleTelegram = new Telegram();
            var bigTelegram = new Telegram();

            //  moduleIndex
            var moduleIndexBytes = new byte[2];
            //  解析小端ModuleIndex
            Buffer.BlockCopy(receivedResults, 0, moduleIndexBytes, 0, moduleIndexBytes.Length);
            littleTelegram.ModuleIndex = BitConverter.ToInt16(moduleIndexBytes, 0);
            //  解析大端ModuleIndex
            bigTelegram.ModuleIndex = BitConverter.ToInt16(moduleIndexBytes.Reverse().ToArray(), 0);

            //  报文长度      
            var lengthBytes = new byte[2];
            //  解析小端报文长度拷贝
            Buffer.BlockCopy(receivedResults, 2, lengthBytes, 0, lengthBytes.Length);
            littleTelegram.Length = BitConverter.ToUInt16(lengthBytes, 0);
            //  解析大端报文长度拷贝
            bigTelegram.Length = BitConverter.ToUInt16(lengthBytes.Reverse().ToArray(), 0);

            littleTelegram.ActualLength = Convert.ToUInt16(receivedResults.Length);
            bigTelegram.ActualLength = Convert.ToUInt16(receivedResults.Length);

            return new Tuple<Telegram, Telegram>(littleTelegram, bigTelegram);
        }
    }
}