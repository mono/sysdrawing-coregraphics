using System.Drawing;
using System.Drawing.Drawing2D;
using CoreGraphics;

namespace MTExample4_2 {
	public class ChartStyle {
		ChartCanvas form1;
		Rectangle chartArea;
		Color chartBackColor;
		Color chartBorderColor;
		Color plotBackColor = Color.White;
		Color plotBorderColor = Color.Black;
		DashStyle gridPattern = DashStyle.Solid;
		Color gridColor = Color.LightGray;
		float gridLineThickness = 1f;
		bool isXGrid = false;
		bool isYGrid = false;
		string xLabel = "X Axis";
		string yLabel = "Y Axis";
		string sTitle = "Title";
		Font labelFont = new Font ("Arial", 10f, FontStyle.Regular);
		Color labelFontColor = Color.Black;
		Font titleFont = new Font ("Arial", 12f, FontStyle.Regular);
		Color titleFontColor = Color.Black;
		float xLimMin = 0f;
		float xLimMax = 10f;
		float yLimMin = 0f;
		float yLimMax = 10f;
		float xTick = 1f;
		float yTick = 2f;
		Font tickFont;
		Color tickFontColor = Color.Black;
		float xtickOffset = 0f;
		float ytickOffset = 0f;
		BarTypeEnum barType = BarTypeEnum.Vertical;

		public ChartStyle (ChartCanvas fm1)
		{
			form1 = fm1;
			chartArea = (Rectangle)form1.ClientRectangle;
			chartBackColor = fm1.BackColor;
			chartBorderColor = fm1.BackColor;
			tickFont = form1.Font;
		}

		public BarTypeEnum BarType {
			get {
				return barType;
			}
			set {
				barType = value;
			}
		}

		public enum BarTypeEnum {
			Vertical = 0,
			Horizontal = 1,
			VerticalStack = 2,
			HorizontalStack = 3,
			VerticalOverlay = 4,
			HorizontalOverlay = 5
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

		public float XTickOffset {
			get {
				return xtickOffset;
			}
			set {
				xtickOffset = value;
			}
		}

		public float YTickOffset {
			get {
				return ytickOffset;
			}
			set {
				ytickOffset = value;
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

		public Color PlotBorderColor
		{
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

		virtual public Color GridColor {
			get {
				return gridColor;
			}
			set {
				gridColor = value;
			}
		}

		public void PlotPanelStyle(Graphics g)
		{
			var aPen = new Pen (ChartBorderColor, 1f);
			var aBrush = new SolidBrush (ChartBackColor);

			// Create vertical gridlines:
			float fX, fY, xm, ym;

			aPen = new Pen(GridColor, 1f);
			aPen.DashStyle = GridPattern;
			xm = XLimMin + XTickOffset;
			if (BarType == BarTypeEnum.Vertical || BarType == BarTypeEnum.VerticalOverlay || BarType == BarTypeEnum.VerticalStack)
				xm = XTickOffset + XLimMin + XTick / 2;

			// Create vertical gridelines:
			if (IsYGrid == true) {
				for (fX = xm; fX < XLimMax; fX += XTick) {
					g.DrawLine (aPen, Point2D (new CGPoint (fX, YLimMin)), Point2D (new CGPoint (fX, YLimMax)));
				}
			}
			// Create the x-axis tick marks:
			for (fX = xm; fX < XLimMax; fX += XTick) {
				Point yAxisPoint = Point2D (new CGPoint(fX, YLimMin));
				g.DrawLine (Pens.Black, yAxisPoint, new PointF (yAxisPoint.X, yAxisPoint.Y - 8f));
			}

			// Create horizontal gridlines:
			aPen = new Pen(GridColor, 1f);
			aPen.DashStyle = GridPattern;
			ym = YLimMin + YTickOffset;
			if (BarType == BarTypeEnum.Horizontal || BarType == BarTypeEnum.HorizontalOverlay || BarType == BarTypeEnum.HorizontalStack) {
				ym = YTickOffset + YLimMin + YTick / 2;
			}

			if (IsXGrid == true) {
				for (fY = ym; fY < YLimMax; fY += YTick) {
					g.DrawLine(aPen, Point2D(new CGPoint(XLimMin, fY)),
					           Point2D(new CGPoint(XLimMax, fY)));
				}
			}

			// Create the y-axis tick marks:
			for (fY = ym; fY < YLimMax; fY += YTick) {
				Point xAxisPoint = Point2D (new CGPoint(XLimMin, fY));
				g.DrawLine (Pens.Black, xAxisPoint, new PointF (xAxisPoint.X + 5f, xAxisPoint.Y));
			}
			aPen.Dispose ();
			aBrush.Dispose ();
		}

		public void SetChartArea (Graphics g)
		{
			SetPlotPanel (g);
			// Draw chart area:
			var aPen = new Pen (ChartBorderColor, 1f);
			var aBrush = new SolidBrush (ChartBackColor);
			CGSize tickFontSize = g.MeasureString("A", TickFont);
			g.FillRectangle (aBrush, ChartArea);
			g.DrawRectangle (aPen, ChartArea);

			// Create the x-axis tick labels:
			aBrush = new SolidBrush(TickFontColor);
			float xm = XLimMin + XTickOffset;
			float xticklabel = 0f;
			if (BarType == BarTypeEnum.Vertical || BarType == BarTypeEnum.VerticalOverlay || BarType == BarTypeEnum.VerticalStack) {
				xm = XTickOffset + XLimMin + XTick / 2;
				xticklabel = XTick / 2;
			}

			for (float fX = xm; fX <= XLimMax; fX += XTick) {
				var yAxisPoint = Point2D (new CGPoint (fX, YLimMin));
				var sFormat = new StringFormat {
					Alignment = StringAlignment.Center
				};
				g.DrawString ((fX + xticklabel).ToString (), TickFont, aBrush, new PointF (form1.PlotPanel.Left + yAxisPoint.X, form1.PlotPanel.Top + yAxisPoint.Y + 4f), sFormat);
			}

			// Create the y-axis tick labels:
			float ym = YLimMin + YTickOffset;
			float yticklabel = 0f;
			if (BarType == BarTypeEnum.Horizontal || BarType == BarTypeEnum.HorizontalOverlay || BarType == BarTypeEnum.HorizontalStack) {
				ym = YTickOffset + YLimMin + YTick / 2;
				yticklabel = YTick / 2;
			}

			for (float fY = ym; fY <= YLimMax; fY += YTick) {
				Point xAxisPoint = Point2D (new CGPoint(XLimMin, fY));
				var sFormat = new StringFormat {
					Alignment = StringAlignment.Far
				};
				g.DrawString ((fY + yticklabel).ToString (), TickFont, aBrush,
				              new PointF (form1.PlotPanel.Left + xAxisPoint.X - 3f, (float)(form1.PlotPanel.Top + xAxisPoint.Y - tickFontSize.Height / 2)), sFormat);
			}

			AddLabels (g);
		}

		void SetPlotPanel (Graphics g)
		{
			// Set form1.PlotPanel:
			float xOffset = ChartArea.Width / 30f;
			float yOffset = ChartArea.Height / 30f;
			CGSize labelFontSize = g.MeasureString ("A", LabelFont);
			CGSize titleFontSize = g.MeasureString ("A", TitleFont);
			if (Title.ToUpper() == "NO TITLE") {
				titleFontSize.Width = 8f;
				titleFontSize.Height = 8f;
			}

			float xSpacing = xOffset / 3f;
			float ySpacing = yOffset / 3f;

			CGSize tickFontSize = g.MeasureString ("A", TickFont);
			float tickSpacing = 2f;
			CGSize yTickSize = g.MeasureString(YLimMin.ToString(), TickFont);
			for (float yTick = YLimMin + YTickOffset; yTick <= YLimMax; yTick += YTick) {
				CGSize tempSize = g.MeasureString(yTick.ToString(), TickFont);
				if (yTickSize.Width < tempSize.Width)
					yTickSize = tempSize;
			}

			var leftMargin = (float)(xOffset + labelFontSize.Width + xSpacing + yTickSize.Width + tickSpacing);
			float rightMargin = xOffset;
			var topMargin = (float)(yOffset + titleFontSize.Height + ySpacing);
			var bottomMargin = (float)(yOffset + labelFontSize.Height + ySpacing + tickSpacing + tickFontSize.Height);

			// Define the plot panel size:
			form1.PlotPanel.Left = ChartArea.X + (int)leftMargin;
			form1.PlotPanel.Top = ChartArea.Y + (int)topMargin;
			form1.PlotPanel.Width = ChartArea.Width - (int)leftMargin - 2 * (int)rightMargin;
			form1.PlotPanel.Height = ChartArea.Height - (int)topMargin - (int)bottomMargin;
			form1.PlotPanel.BackColor = plotBackColor;
		}

		void AddLabels (Graphics g)
		{
			float xOffset = ChartArea.Width / 30f;
			float yOffset = ChartArea.Height / 30f;
			CGSize labelFontSize = g.MeasureString ("A", LabelFont);
			CGSize titleFontSize = g.MeasureString ("A", TitleFont);

			// Add horizontal axis label:
			var aBrush = new SolidBrush (LabelFontColor);
			CGSize stringSize = g.MeasureString (XLabel, LabelFont);
			g.DrawString (XLabel, LabelFont, aBrush, new PointF (form1.PlotPanel.Left + form1.PlotPanel.Width / 2 - (int)stringSize.Width / 2, ChartArea.Bottom - (int)yOffset - (int)labelFontSize.Height));

			// Add y-axis label:
			var sFormat = new StringFormat ();
			sFormat.Alignment = StringAlignment.Center;
			stringSize = g.MeasureString (YLabel, LabelFont);
			// Save the state of the current Graphics object
			GraphicsState gState = g.Save ();
			g.TranslateTransform (ChartArea.X + xOffset, (float)(ChartArea.Y +
			                                                     yOffset + titleFontSize.Height +
			                                                     yOffset / 3 + form1.PlotPanel.Height / 2));
			g.RotateTransform (-90);
			g.DrawString (YLabel, LabelFont, aBrush, 0, 0, sFormat);
			// Restore it:
			g.Restore (gState);

			// Add title:
			aBrush = new SolidBrush (TitleFontColor);
			stringSize = g.MeasureString (Title, TitleFont);
			if (Title.ToUpper () != "NO TITLE")
				g.DrawString (Title, TitleFont, aBrush, new PointF (form1.PlotPanel.Left + form1.PlotPanel.Width / 2 -
				                                                    (int)stringSize.Width / 2, ChartArea.Top + (int)yOffset));
			aBrush.Dispose ();
		}

		public Point Point2D (CGPoint pt)
		{
			var aPoint = new Point ();
			aPoint.X = (int)((pt.X - XLimMin) * form1.PlotPanel.Width / (XLimMax - XLimMin));
			aPoint.Y = (int)(form1.PlotPanel.Height - (pt.Y - YLimMin) * form1.PlotPanel.Height / (YLimMax - YLimMin));
			return aPoint;
		}
	}
}

