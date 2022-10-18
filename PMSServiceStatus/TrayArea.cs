using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PMSServiceStatus
{
    //该类用于刷新托盘窗口(包括显示区域和溢出区域)，达到清除残留图标的目的。
    //原理：在程序每次开启前，向托盘窗口发送鼠标滑过消息WM_MOUSEMOVE，使其刷新，在此过程中，无效的托盘图标即会消失。
    public class TrayArea
    {
        //只读字段，用于获取当前系统语言信息(是中文系统，还是英文系统)
        private static readonly System.Globalization.CultureInfo _CultureInfo = System.Globalization.CultureInfo.InstalledUICulture;

        //定义struct，用于存放托盘区域的位置信息
        private struct Rect
        {
            public int left, top, right, bottom;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string className, string windowTitle);
        [DllImport("user32.dll")]
        private static extern bool GetClientRect(IntPtr handle, out Rect rect);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr handle, uint message, int wParam, int lParam);

        /// <summary>
        /// 刷新托盘区域。
        /// </summary>
        /// <returns>Task</returns>
        public static Task RefreshAsync() //异步形式函数
        {
            return Task.Run(() => Refresh());
        }

        /// <summary>
        /// 刷新托盘区域。
        /// </summary>
        public static void Refresh()
        {
            //一层一层的父窗口的Handle
            IntPtr trayWndHandle = FindWindow("Shell_TrayWnd", null);
            IntPtr trayNotifyWndHandle = FindWindowEx(trayWndHandle, IntPtr.Zero, "TrayNotifyWnd", null);
            IntPtr sysPagerHandle = FindWindowEx(trayNotifyWndHandle, IntPtr.Zero, "SysPager", null);

            //刷新用户提示通知区域
            string windowTile_UserPromotedNotificationArea = _CultureInfo.Name == "zh-CN" ? "用户提示通知区域" : _CultureInfo.Name.StartsWith("en") ? "User Promoted Notification Area" : null; //窗口标题，目前仅支持简体中文和英文
            IntPtr windowHandle_UserPromotedNotificationArea = FindWindowEx(sysPagerHandle, IntPtr.Zero, "ToolbarWindow32", windowTile_UserPromotedNotificationArea); //目标窗口Handle
            Refresh(windowHandle_UserPromotedNotificationArea); //刷新

            //刷新溢出通知区域
            IntPtr notifyIconOverflowWindowHandle = FindWindow("NotifyIconOverflowWindow", null); //父窗口Handle
            string windowTile_OverflowNotificationArea = _CultureInfo.Name == "zh-CN" ? "溢出通知区域" : _CultureInfo.Name.StartsWith("en") ? "Overflow Notification Area" : null; //窗口标题，目前仅支持简体中文和英文
            IntPtr windowHandle_OverflowNotificationArea = FindWindowEx(notifyIconOverflowWindowHandle, IntPtr.Zero, "ToolbarWindow32", windowTile_OverflowNotificationArea); //目标窗口Handle
            Refresh(windowHandle_OverflowNotificationArea); //刷新
        }

        //刷新托盘区域中的所有图标(通过模拟鼠标滑过的方式)
        private static void Refresh(IntPtr windowHandle)
        {
            //Mouse Over消息常量
            const uint WM_MOUSEMOVE = 0x0200;
            Rect rect;
            //获取托盘区域(位置信息放在Rect变量中)
            GetClientRect(windowHandle, out rect);
            //通过循环，从左到右，从上到下，(模拟鼠标)滑过托盘区域中的每个图标
            for (int x = 0; x < rect.right; x += 5) //图标之间的水平间距为5
            {
                for (int y = 0; y < rect.bottom; y += 5) //图标之间的垂直间距为5
                {
                    SendMessage(windowHandle, WM_MOUSEMOVE, 0, (y << 16) + x); //发送鼠标滑过(Mouse Over)消息
                }
            }
        }
    }
}
