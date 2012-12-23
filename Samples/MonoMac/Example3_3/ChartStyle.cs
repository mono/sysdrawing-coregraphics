using System;
using System.Collections.Generic;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;

namespace Example3_3


{
    public class ChartStyle
    {
        private Form form1;
        private Rectangle chartArea;
        private Rectangle plotArea;
        private Color chartBackColor;
        private Color chartBorderColor;
        private Color plotBackColor = Color.White;
        private Color plotBorderColor = Color.Black;
        private float xLimMin = 0f;
        private float xLimMax = 10f;
        private float yLimMin = 0f;
        private float yLimMax = 10f;
        private DashStyle gridPattern = DashStyle.Solid;
        private Color gridColor = Color.LightGray;
        private float gridLineThickness = 1.0f;
        private bool isXGrid = true;
        private bool isYGrid = true;
        private string xLabel = "X Axis";
        private string yLabel = "Y Axis";
        private string sTitle = "Title";
        private Font labelFont = new Font("Arial", 10, FontStyle.Regular);
        private Color labelFontColor = Color.Black;
        private Font titleFont = new Font("Arial", 12, FontStyle.Regular);
        private Color titleFontColor = Color.Black;
        private float xTick = 1f;
        private float yTick = 1f;
        private Font tickFont;
        private Color tickFontColor = Color.Black;

        public ChartStyle(Form fm1)
        {
            form1 = fm1;
            chartArea = form1.ClientRectangle;
            chartBackColor = fm1.BackColor;
            chartBorderColor = fm1.BackColor;
            PlotArea = chartArea;
            tickFont = form1.Font;
        }

        public Font TickFont
        {
            get { return tickFont; }
            set { tickFont = value; }
        }

        public Color TickFontColor
        {
            get { return tickFontColor; }
            set { tickFontColor = value; }
        }

        public Color ChartBackColor
        {
            get { return chartBackColor; }
            set { chartBackColor = value; }
        }

        public Color ChartBorderColor
        {
            get { return chartBorderColor; }
            set { chartBorderColor = value; }
        }

        public Color PlotBackColor
        {
            get { return plotBackColor; }
            set { plotBackColor = value; }
        }

        public Color PlotBorderColor
        {
            get { return plotBorderColor; }
            set { plotBorderColor = value; }
        }
        
        public Rectangle ChartArea
        {
            get { return chartArea; }
            set { chartArea = value; }
        }

        public Rectangle PlotArea
        {
            get { return plotArea; }
            set { plotArea = value; }
        }

        public bool IsXGrid
        {
            get { return isXGrid; }
            set { isXGrid = value; }
        }
        public bool IsYGrid
        {
            get { return isYGrid; }
            set { isYGrid = value; }
        }
        public string Title
        {
            get { return sTitle; }
            set { sTitle = value; }
        }
        public string XLabel
        {
            get { return xLabel; }
            set { xLabel = value; }
        }
        public string YLabel
        {
            get { return yLabel; }
            set { yLabel = value; }
        }
        public Font LabelFont
        {
            get { return labelFont; }
            set { labelFont = value; }
        }
        public Color LabelFontColor
        {
            get { return labelFontColor; }
            set { labelFontColor = value; }
        }
        public Font TitleFont
        {
            get { return titleFont; }
            set { titleFont = value; }
        }
        public Color TitleFontColor
        {
            get { return titleFontColor; }
            set { titleFontColor = value; }
        }
        public float XLimMax
        {
            get { return xLimMax; }
            set { xLimMax = value; }
        }
        public float XLimMin
        {
            get { return xLimMin; }
            set { xLimMin = value; }
        }
        public float YLimMax
        {
            get { return yLimMax; }
            set { yLimMax = value; }
        }
        public float YLimMin
        {
            get { return yLimMin; }
            set { yLimMin = value; }
        }
        public float XTick
        {
            get { return xTick; }
            set { xTick = value; }
        }
        public float YTick
        {
            get { return yTick; }
            set { yTick = value; }
        }
        virtual public DashStyle GridPattern
        {
            get { return gridPattern; }
            set { gridPattern = value; }
        }
        public float GridThickness
        {
            get { return gridLineThickness; }
            set { gridLineThickness = value; }
        }
        virtual public Color GridColor
        {
            get { return gridColor; }
            set { gridColor = value; }
        }

        public void AddChartStyle(Graphics g)
        {
            // Draw ChartArea and PlotArea:
            Pen aPen = new Pen(ChartBorderColor, 1f);
            SolidBrush aBrush = new SolidBrush(ChartBackColor);
            g.FillRectangle(aBrush, ChartArea);
            g.DrawRectangle(aPen, ChartArea);
            aPen = new Pen(PlotBorderColor, 1f);
            aBrush = new SolidBrush(PlotBackColor);
            g.FillRectangle(aBrush, PlotArea);
            g.DrawRectangle(aPen, PlotArea);

            SizeF tickFontSize = g.MeasureString("A", TickFont);
            // Create vertical gridlines:
            float fX, fY;
            if (IsYGrid == true)
            {
                aPen = new Pen(GridColor, 1f);
                aPen.DashStyle = GridPattern;
                for (fX = XLimMin + XTick; fX < XLimMax; fX += XTick)
                {
                    g.DrawLine(aPen, Point2D(new PointF(fX, YLimMin)),
                        Point2D(new PointF(fX, YLimMax)));
                }
            }

            // Create horizontal gridlines:
            if (IsXGrid == true)
            {
                aPen = new Pen(GridColor, 1f);
                aPen.DashStyle = GridPattern;
                for (fY = YLimMin + YTick; fY < YLimMax; fY += YTick)
                {
                    g.DrawLine(aPen, Point2D(new PointF(XLimMin, fY)),
                        Point2D(new PointF(XLimMax, fY)));
                }
            }

            // Create the x-axis tick marks:
            aBrush = new SolidBrush(TickFontColor);
            for (fX = XLimMin; fX <= XLimMax; fX += XTick)
            {
                PointF yAxisPoint = Point2D(new PointF(fX, YLimMin));
                g.DrawLine(Pens.Black, yAxisPoint, new PointF(yAxisPoint.X,
                                   yAxisPoint.Y - 5f));
                StringFormat sFormat = new StringFormat();
                sFormat.Alignment = StringAlignment.Far;
                SizeF sizeXTick = g.MeasureString(fX.ToString(), TickFont);
                g.DrawString(fX.ToString(), TickFont, aBrush,
                    new PointF(yAxisPoint.X + sizeXTick.Width / 2,
                    yAxisPoint.Y + 4f), sFormat);
            }

            // Create the y-axis tick marks:
            for (fY = YLimMin; fY <= YLimMax; fY += YTick)
            {
                PointF xAxisPoint = Point2D(new PointF(XLimMin, fY));
                g.DrawLine(Pens.Black, xAxisPoint, 
                    new PointF(xAxisPoint.X + 5f, xAxisPoint.Y));
                StringFormat sFormat = new StringFormat();
                sFormat.Alignment = StringAlignment.Far;
                g.DrawString(fY.ToString(), TickFont, aBrush,
                    new PointF(xAxisPoint.X - 3f,
                    xAxisPoint.Y - tickFontSize.Height / 2), sFormat);
            }
            aPen.Dispose();
            aBrush.Dispose();
            AddLabels(g);
        }

        private void AddLabels(Graphics g)
        {
            float xOffset = chartArea.Width / 30.0f;
            float yOffset = chartArea.Height / 30.0f;
            SizeF labelFontSize = g.MeasureString("A", LabelFont);
            SizeF titleFontSize = g.MeasureString("A", TitleFont);

            // Add horizontal axis label:
            SolidBrush aBrush = new SolidBrush(LabelFontColor);
            SizeF stringSize = g.MeasureString(XLabel, LabelFont);
            g.DrawString(XLabel, LabelFont, aBrush,
                new Point(PlotArea.Left + PlotArea.Width / 2 -
                (int)stringSize.Width / 2, ChartArea.Bottom - 
                (int)yOffset - (int)labelFontSize.Height));

            // Add y-axis label:
            StringFormat sFormat = new StringFormat();
            sFormat.Alignment = StringAlignment.Center;
            stringSize = g.MeasureString(YLabel, LabelFont);
            // Save the state of the current Graphics object
            GraphicsState gState = g.Save();
            g.TranslateTransform(xOffset, yOffset + titleFontSize.Height
                + yOffset / 3 + PlotArea.Height / 2);
            g.RotateTransform(-90);
            g.DrawString(YLabel, LabelFont, aBrush, 0, 0, sFormat);
            // Restore it:
            g.Restore(gState);

            // Add title:
            aBrush = new SolidBrush(TitleFontColor);
            stringSize = g.MeasureString(Title, TitleFont);
            if (Title.ToUpper() != "NO TITLE")
            {
                g.DrawString(Title, TitleFont, aBrush,
                    new Point(PlotArea.Left + PlotArea.Width / 2 -
                    (int)stringSize.Width / 2, ChartArea.Top + (int)yOffset));
            }
            aBrush.Dispose();
        }

        public PointF Point2D(PointF pt)
        {
            PointF aPoint = new PointF();
            if (pt.X < XLimMin || pt.X > XLimMax ||
                pt.Y < YLimMin || pt.Y > YLimMax)
            {
                pt.X = Single.NaN;
                pt.Y = Single.NaN;
            }
            aPoint.X = PlotArea.X + (pt.X - XLimMin) *
                PlotArea.Width / (XLimMax - XLimMin);
            aPoint.Y = PlotArea.Bottom - (pt.Y - YLimMin) *
                PlotArea.Height / (YLimMax - YLimMin);
            return aPoint;
        }
    }
}

