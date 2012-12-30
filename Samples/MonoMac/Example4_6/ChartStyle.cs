using System;
using System.Collections.Generic;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;

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

