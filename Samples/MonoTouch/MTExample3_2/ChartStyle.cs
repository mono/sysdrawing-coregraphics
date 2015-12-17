using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using UIKit;

namespace MTExample3_2
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

		public ChartStyle (Form fm1)
		{
			form1 = fm1;
			chartArea = form1.ClientRectangle;
			chartBackColor = fm1.BackColor;
			chartBorderColor = fm1.BackColor;
			PlotArea = chartArea;
		}

		public Color ChartBackColor {
			get { return chartBackColor; }
			set { chartBackColor = value; }
		}

		public Color ChartBorderColor {
			get { return chartBorderColor; }
			set { chartBorderColor = value; }
		}

		public Color PlotBackColor {
			get { return plotBackColor; }
			set { plotBackColor = value; }
		}

		public Color PlotBorderColor {
			get { return plotBorderColor; }
			set { plotBorderColor = value; }
		}

		public Rectangle ChartArea {
			get { return chartArea; }
			set { chartArea = value; }
		}

		public Rectangle PlotArea {
			get { return plotArea; }
			set { plotArea = value; }
		}

		public float XLimMax {
			get { return xLimMax; }
			set { xLimMax = value; }
		}

		public float XLimMin {
			get { return xLimMin; }
			set { xLimMin = value; }
		}

		public float YLimMax {
			get { return yLimMax; }
			set { yLimMax = value; }
		}

		public float YLimMin {
			get { return yLimMin; }
			set { yLimMin = value; }
		}

		public void AddChartStyle (Graphics g)
		{
			// Draw ChartArea and PlotArea:
			Pen aPen = new Pen (ChartBorderColor, 1f);
			SolidBrush aBrush = new SolidBrush (ChartBackColor);
			g.FillRectangle (aBrush, ChartArea);
			g.DrawRectangle (aPen, ChartArea);
			aPen = new Pen (PlotBorderColor, 1f);
			aBrush = new SolidBrush (PlotBackColor);
			g.FillRectangle (aBrush, PlotArea);
			g.DrawRectangle (aPen, PlotArea);
			aPen.Dispose ();
			aBrush.Dispose ();
		}

		public PointF Point2D (PointF pt)
		{
			PointF aPoint = new PointF ();
			if (pt.X < XLimMin || pt.X > XLimMax ||
				pt.Y < YLimMin || pt.Y > YLimMax) {
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

