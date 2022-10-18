using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PMSServiceStatus
{
    public partial class MyProgressBar : ProgressBar
    {
        public MyProgressBar()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rect = ClientRectangle;
            Graphics g = e.Graphics;

            ProgressBarRenderer.DrawHorizontalBar(g, rect);
            rect.Inflate(-3, -3);
            //// 大于95绿色、90-95之间黄色、小于90红色
            if (Value >= 95)
            {
                var clip = new Rectangle(rect.X, rect.Y, (int)((float)Value / Maximum * rect.Width), rect.Height);
                ProgressBarRenderer.DrawHorizontalChunks(g, clip);
                //e.Graphics.FillRectangle(Brushes.Green, 2, 2, rect.Width* (Value / rect.Width), rect.Height);
            }
            else if (Value >= 90 && Value < 95)
            {
                e.Graphics.FillRectangle(Brushes.Yellow, 2, 2, (int)((float)Value / Maximum * rect.Width), rect.Height);
            }
            else if (Value > 0 && Value < 90)
            {
                e.Graphics.FillRectangle(Brushes.Red, 2, 2, (int)((float)Value / Maximum * rect.Width), rect.Height);
            }
        }
    }
}
