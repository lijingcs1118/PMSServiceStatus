using System;
using System.Reflection;
using MakarovDev.ExpandCollapsePanel;

namespace PMSServiceStatus
{
    public static class DoubleBufferFlowLayoutPanel
    {
        /// <summary>
        /// 双缓冲，解决闪烁问题
        /// </summary>
        /// <param name="flowLayoutPanel"></param>
        /// <param name="flag"></param>
        public static void DoubleBufferedFlowLayoutPanel(this AdvancedFlowLayoutPanel flowLayoutPanel, bool flag)
        {
            Type lvType = flowLayoutPanel.GetType();
            PropertyInfo pi = lvType.GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(flowLayoutPanel, flag, null);
        }
    }
}