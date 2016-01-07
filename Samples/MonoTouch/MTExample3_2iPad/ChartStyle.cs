using System.Drawing;
using CoreGraphics;

namespace MTExample3_2 {
	public class ChartStyle {
		Form form1;
		Rectangle chartArea;
		Rectangle plotArea;
		Color chartBackColor;
		Color chartBorderColor;
		Color plotBackColor = Color.White;
		Color plotBorderColor = Color.Black;
		float xLimMin = 0f;
		float xLimMax = 10f;
		float yLimMin = 0f;
		float yLimMax = 10f;

		public ChartStyle (Form fm1)
		{
			form1 = fm1;
			chartArea = form1.ClientRectangle;
			chartBackColor = fm1.BackColor;
			chartBorderColor = fm1.BackColor;
			PlotArea = chartArea;
		}

		public Color ChartBackColor {
			get {
				return chartBackColor;
			}
			set {
				chartBackColor = value;
			}
		}

		public Color ChartBorderColor {
			get {
				return chartBorderColor;
			}
			set {
				chartBorderColor = value;
			}
		}

		public Color PlotBackColor {
			get {
				return plotBackColor;
			}
			set {
				plotBackColor = value;
			}
		}

		public Color PlotBorderColor {
			get {
				return plotBorderColor;
			}
			set {
				plotBorderColor = value;
			}
		}

		public Rectangle ChartArea {
			get {
				return chartArea;
			}
			set {
				chartArea = value;
			}
		}

		public Rectangle PlotArea {
			get {
				return plotArea;
			}
			set {
				plotArea = value;
			}
		}

		public float XLimMax {
			get {
				return xLimMax;
			}
			set {
				xLimMax = value;
			}
		}

		public float XLimMin {
			get {
				return xLimMin;
			}
			set {
				xLimMin = value;
			}
		}

		public float YLimMax {
			get {
				return yLimMax;
			}
			set {
				yLimMax = value;
			}
		}

		public float YLimMin {
			get {
				return yLimMin;
			}
			set {
				yLimMin = value;
			}
		}

		public void AddChartStyle (Graphics g)
		{
			// Draw ChartArea and PlotArea:
			var aPen = new Pen (ChartBorderColor, 1f);
			var aBrush = new SolidBrush (ChartBackColor);
			g.FillRectangle (aBrush, ChartArea);
			g.DrawRectangle (aPen, ChartArea);
			aPen = new Pen (PlotBorderColor, 1f);
			aBrush = new SolidBrush (PlotBackColor);
			g.FillRectangle (aBrush, PlotArea);
			g.DrawRectangle (aPen, PlotArea);
			aPen.Dispose ();
			aBrush.Dispose ();
		}

		public PointF Point2D (CGPoint pt)
		{
			var aPoint = new PointF ();
			if (pt.X < XLimMin || pt.X > XLimMax ||
				pt.Y < YLimMin || pt.Y > YLimMax) {
				pt.X = float.NaN;
				pt.Y = float.NaN;
			}
			aPoint.X = (float)(PlotArea.X + (pt.X - XLimMin) * PlotArea.Width / (XLimMax - XLimMin));
			aPoint.Y = (float)(PlotArea.Bottom - (pt.Y - YLimMin) * PlotArea.Height / (YLimMax - YLimMin));
			return aPoint;
		}
	}
}

