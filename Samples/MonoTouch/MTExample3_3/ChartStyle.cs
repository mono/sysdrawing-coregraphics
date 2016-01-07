using System.Drawing;
using System.Drawing.Drawing2D;

using CoreGraphics;

namespace MTExample3_3  {
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
		DashStyle gridPattern = DashStyle.Solid;
		Color gridColor = Color.LightGray;
		float gridLineThickness = 1.0f;
		bool isXGrid = true;
		bool isYGrid = true;
		string xLabel = "X Axis";
		string yLabel = "Y Axis";
		string sTitle = "Title";
		Font labelFont = new Font("Arial", 10, FontStyle.Regular);
		Color labelFontColor = Color.Black;
		Font titleFont = new Font("Arial", 12, FontStyle.Regular);
		Color titleFontColor = Color.Black;
		float xTick = 1f;
		float yTick = 1f;
		Font tickFont;
		Color tickFontColor = Color.Black;

		public ChartStyle (Form fm1)
		{
			form1 = fm1;
			chartArea = form1.ClientRectangle;
			chartBackColor = fm1.BackColor;
			chartBorderColor = fm1.BackColor;
			PlotArea = chartArea;
			tickFont = form1.Font;
		}

		public Font TickFont {
			get {
				return tickFont;
			}
			set {
				tickFont = value;
			}
		}

		public Color TickFontColor {
			get {
				return tickFontColor;
			}
			set {
				tickFontColor = value;
			}
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

		public bool IsXGrid {
			get {
				return isXGrid;
			}
			set {
				isXGrid = value;
			}
		}

		public bool IsYGrid {
			get {
				return isYGrid;
			}
			set {
				isYGrid = value;
			}
		}

		public string Title {
			get {
				return sTitle;
			}
			set {
				sTitle = value;
			}
		}

		public string XLabel {
			get {
				return xLabel;
			}
			set {
				xLabel = value;
			}
		}

		public string YLabel {
			get {
				return yLabel;
			}
			set {
				yLabel = value;
			}
		}

		public Font LabelFont {
			get {
				return labelFont;
			}
			set {
				labelFont = value;
			}
		}

		public Color LabelFontColor {
			get {
				return labelFontColor;
			}
			set {
				labelFontColor = value;
			}
		}

		public Font TitleFont {
			get {
				return titleFont;
			}
			set {
				titleFont = value;
			}
		}

		public Color TitleFontColor {
			get {
				return titleFontColor;
			}
			set {
				titleFontColor = value;
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

		public float XTick {
			get {
				return xTick;
			}
			set {
				xTick = value;
			}
		}

		public float YTick {
			get {
				return yTick;
			}
			set {
				yTick = value;
			}
		}

		virtual public DashStyle GridPattern {
			get {
				return gridPattern;
			}
			set {
				gridPattern = value;
			}
		}

		public float GridThickness {
			get {
				return gridLineThickness;
			}
			set {
				gridLineThickness = value;
			}
		}

		virtual public Color GridColor
		{
			get {
				return gridColor;
			}
			set {
				gridColor = value;
			}
		}

		public void AddChartStyle(Graphics g)
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

			CGSize tickFontSize = g.MeasureString ("A", TickFont);
			// Create vertical gridlines:
			float fX, fY;
			if (IsYGrid == true) {
				aPen = new Pen(GridColor, 1f);
				aPen.DashStyle = GridPattern;
				for (fX = XLimMin + XTick; fX < XLimMax; fX += XTick)
					g.DrawLine(aPen, Point2D (new CGPoint(fX, YLimMin)), Point2D (new CGPoint(fX, YLimMax)));
			}

			// Create horizontal gridlines:
			if (IsXGrid == true) {
				aPen = new Pen(GridColor, 1f);
				aPen.DashStyle = GridPattern;
				for (fY = YLimMin + YTick; fY < YLimMax; fY += YTick) {
					g.DrawLine(aPen, Point2D (new CGPoint(XLimMin, fY)), Point2D (new CGPoint(XLimMax, fY)));
				}
			}

			// Create the x-axis tick marks:
			aBrush = new SolidBrush(TickFontColor);
			for (fX = XLimMin; fX <= XLimMax; fX += XTick) {
				PointF yAxisPoint = Point2D (new CGPoint(fX, YLimMin));
				g.DrawLine (Pens.Black, yAxisPoint, new PointF (yAxisPoint.X, yAxisPoint.Y - 5f));
				var sFormat = new StringFormat ();
				sFormat.Alignment = StringAlignment.Far;
				CGSize sizeXTick = g.MeasureString (fX.ToString (), TickFont);
				g.DrawString (fX.ToString (), TickFont, aBrush, new PointF ((float)(yAxisPoint.X + sizeXTick.Width / 2), yAxisPoint.Y + 4f), sFormat);
			}

			// Create the y-axis tick marks:
			for (fY = YLimMin; fY <= YLimMax; fY += YTick) {
				PointF xAxisPoint = Point2D (new CGPoint(XLimMin, fY));
				g.DrawLine (Pens.Black, xAxisPoint, new PointF (xAxisPoint.X + 5f, xAxisPoint.Y));
				var sFormat = new StringFormat ();
				sFormat.Alignment = StringAlignment.Far;
				g.DrawString (fY.ToString (), TickFont, aBrush, new PointF (xAxisPoint.X - 3f, (float)(xAxisPoint.Y - tickFontSize.Height / 2)), sFormat);
			}

			aPen.Dispose ();
			aBrush.Dispose ();
			AddLabels (g);
		}

		void AddLabels (Graphics g)
		{
			float xOffset = chartArea.Width / 30.0f;
			float yOffset = chartArea.Height / 30.0f;
			CGSize labelFontSize = g.MeasureString ("A", LabelFont);
			CGSize titleFontSize = g.MeasureString ("A", TitleFont);

			// Add horizontal axis label:
			var aBrush = new SolidBrush (LabelFontColor);
			CGSize stringSize = g.MeasureString(XLabel, LabelFont);
			g.DrawString (XLabel, LabelFont, aBrush, new PointF(
				PlotArea.Left + PlotArea.Width / 2 - (int)stringSize.Width / 2,
				ChartArea.Bottom - (int)yOffset - (int)labelFontSize.Height)
			);

			// Add y-axis label:
			var sFormat = new StringFormat ();
			sFormat.Alignment = StringAlignment.Center;
			stringSize = g.MeasureString (YLabel, LabelFont);
			// Save the state of the current Graphics object
			GraphicsState gState = g.Save ();
			g.TranslateTransform (xOffset, (float)(yOffset + titleFontSize.Height + yOffset / 3 + PlotArea.Height / 2));
			g.RotateTransform (-90);
			g.DrawString (YLabel, LabelFont, aBrush, 0, 0, sFormat);
			// Restore it:
			g.Restore(gState);

			// Add title:
			aBrush = new SolidBrush(TitleFontColor);
			stringSize = g.MeasureString(Title, TitleFont);
			if (Title.ToUpper() != "NO TITLE")
				g.DrawString (Title, TitleFont, aBrush, new PointF (PlotArea.Left + PlotArea.Width / 2 - (int)stringSize.Width / 2, ChartArea.Top + (int)yOffset));
			aBrush.Dispose();
		}

		public PointF Point2D (CGPoint pt)
		{
			var aPoint = new PointF ();
			if (pt.X < XLimMin || pt.X > XLimMax || pt.Y < YLimMin || pt.Y > YLimMax) {
				pt.X = float.NaN;
				pt.Y = float.NaN;
			}

			aPoint.X = (float)(PlotArea.X + (pt.X - XLimMin) * PlotArea.Width / (XLimMax - XLimMin));
			aPoint.Y = (float)(PlotArea.Bottom - (pt.Y - YLimMin) * PlotArea.Height / (YLimMax - YLimMin));
			return aPoint;
		}
	}
}

