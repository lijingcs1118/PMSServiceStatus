﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PMSServiceStatus
{
    static class Program
    {
        /// <summary>
        ///         /// <summary>
        /// 该函数设置由不同线程产生的窗口的显示状态。
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="cmdShow">指定窗口如何显示。查看允许值列表，请查阅ShowWlndow函数的说明部分。</param>
        /// <returns>如果函数原来可见，返回值为非零；如果函数原来被隐藏，返回值为零。</returns>
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        /// <summary>
        /// 该函数将创建指定窗口的线程设置到前台，并且激活该窗口。键盘输入转向该窗口，并为用户改各种可视的记号。系统给创建前台窗口的线程分配的权限稍高于其他线程。
        /// </summary>
        /// <param name="hWnd">将被激活并被调入前台的窗口句柄。</param>
        /// <returns>如果窗口设入了前台，返回值为非零；如果窗口未被设入前台，返回值为零。</returns>
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("User32.dll")]
        public static extern IntPtr FindWindow(string ipClassName, string ipWindowName);


        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            TrayArea.RefreshAsync();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Process instance = RunningInstance();

            IntPtr hand = FindWindow(null, "PMS 服务状态: 已连接");
            if (hand == IntPtr.Zero)
                hand = FindWindow(null, "PMS Service Status : Connected");
            if (hand == IntPtr.Zero)
                hand = FindWindow(null, "PMS 服务状态 : 已断开");
            if (hand == IntPtr.Zero)
                hand = FindWindow(null, "PMS Service Status : Disconnected");

            if (hand != IntPtr.Zero)
            {
                ShowWindowAsync(hand, 4);//显示
                SetForegroundWindow(instance.MainWindowHandle);
                SetForegroundWindow(instance.MainWindowHandle); 
            }
            else
            {
                Application.Run(new MainFrm());
            }
        }

        /// <summary>
        /// 获取正在运行的实例，没有运行的实例返回null;
        /// </summary>
        public static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            foreach (Process process in processes)
            {
                if (process.Id != current.Id)
                {
                    if (Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == current.MainModule.FileName)
                    {
                        return process;
                    }
                }
            }
            return null;
        }
    }
}
