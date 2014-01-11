using System;
using System.Collections.Generic;
using System.Collections;
using System.DrawingNative;
using System.DrawingNative.Drawing2D;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using Rectangle = System.Drawing.Rectangle;
using SizeF = System.Drawing.SizeF;
using PointF = System.Drawing.PointF;
using RectangleF = System.Drawing.RectangleF;
namespace Example4_6
{
    public class ChartStyle
    {
        private ChartCanvas form1;
        private int offset;

        public ChartStyle(ChartCanvas fm1)
        {
            form1 = fm1;
        }

        public int Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        public Rectangle SetPieArea()
        {
            Offset = form1.PlotPanel.Width / 10;
            int height = form1.PlotPanel.Height - 2 * Offset;
            int width = height;
            Rectangle rect = new Rectangle(Offset, Offset, width, height);
            return rect;
        }
    }
}

