using System;
using CoreGraphics;
using System.Collections;
using System.Drawing;

namespace Example3_7
{
    public class SubChart
    {
        private int rows = 1;
        private int cols = 1;
        private int margin = 0;
        private Rectangle totalChartArea;
        private Color totalChartBackColor;
        private Color totalChartBorderColor;
        private Form form1;

        public SubChart(Form fm1)
        {
            form1 = fm1;
            TotalChartArea = form1.ClientRectangle;
            totalChartBackColor = fm1.BackColor;
            totalChartBorderColor = fm1.BackColor;
        }

        public int Rows
        {
            get { return rows; }
            set { rows = value; }
        }

        public int Cols
        {
            get { return cols; }
            set { cols = value; }
        }

        public int Margin
        {
            get { return margin; }
            set { margin = value; }
        }

        public Rectangle TotalChartArea
        {
            get { return totalChartArea; }
            set { totalChartArea = value; }
        }

        public Color TotalChartBackColor
        {
            get { return totalChartBackColor; }
            set { totalChartBackColor = value; }
        }

        public Color TotalChartBorderColor
        {
            get { return totalChartBorderColor; }
            set { totalChartBorderColor = value; }
        }

        public Rectangle[,] SetSubChart(Graphics g)
        {
            Rectangle[,] subRectangle = new Rectangle[Rows, Cols];
            int subWidth = (TotalChartArea.Width - 2 * Margin) / Cols;
            int subHeight = (TotalChartArea.Height - 2*Margin) / Rows;
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    int x = TotalChartArea.X + Margin + j * subWidth;
                    int y = TotalChartArea.Y + Margin + i * subHeight;
                    subRectangle[i, j] = new Rectangle(x, y, subWidth, subHeight);
                }
            }
            // Draw total chart area:
            Pen aPen = new Pen(TotalChartBorderColor, 1f);
            SolidBrush aBrush = new SolidBrush(TotalChartBackColor);
            g.FillRectangle(aBrush, TotalChartArea);
            g.DrawRectangle(aPen, TotalChartArea);
            return subRectangle;
        }
    }
}
