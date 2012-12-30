using System;
using System.Collections.Generic;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;

//using System.Windows.Forms;

namespace MTExample4_7
{
	public class ChartStyle
	{
		private ChartCanvas form1;
		private Rectangle chartArea;
		private Color chartBackColor;
		private Color chartBorderColor;
		private Color plotBackColor = Color.White;
		private Color plotBorderColor = Color.Black;
		private DashStyle gridPattern = DashStyle.Solid;
		private Color gridColor = Color.LightGray;
		private float gridLineThickness = 1.0f;
		private bool isXGrid = false;
		private bool isYGrid = false;
		private string xLabel = "X Axis";
		private string yLabel = "Y Axis";
		private string sTitle = "Title";
		private Font labelFont = new Font ("Arial", 10, FontStyle.Regular);
		private Color labelFontColor = Color.Black;
		private Font titleFont = new Font ("Arial", 12, FontStyle.Regular);
		private Color titleFontColor = Color.Black;
		private float xLimMin = 0f;
		private float xLimMax = 10f;
		private float yLimMin = 0f;
		private float yLimMax = 10f;
		private float xTick = 1f;
		private float yTick = 2f;
		private Font tickFont;
		private Color tickFontColor = Color.Black;
		private float xtickOffset = 0f;
		private float ytickOffset = 0f;
		private BarTypeEnum barType = BarTypeEnum.Vertical;

		public ChartStyle (ChartCanvas fm1)
		{
			form1 = fm1;
			chartArea = form1.ClientRectangle;
			chartBackColor = fm1.BackColor;
			chartBorderColor = fm1.BackColor;
			tickFont = form1.Font;
		}

		public BarTypeEnum BarType {
			get { return barType; }
			set { barType = value; }
		}

		public enum BarTypeEnum
		{
			Vertical = 0,
			Horizontal = 1,
			VerticalStack = 2,
			HorizontalStack = 3,
			VerticalOverlay = 4,
			HorizontalOverlay = 5
		}

		public Font TickFont {
			get { return tickFont; }
			set { tickFont = value; }
		}

		public Color TickFontColor {
			get { return tickFontColor; }
			set { tickFontColor = value; }
		}

		public Color ChartBackColor {
			get { return chartBackColor; }
			set { chartBackColor = value; }
		}

		public float XTickOffset {
			get { return xtickOffset; }
			set { xtickOffset = value; }
		}

		public float YTickOffset {
			get { return ytickOffset; }
			set { ytickOffset = value; }
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

		public bool IsXGrid {
			get { return isXGrid; }
			set { isXGrid = value; }
		}

		public bool IsYGrid {
			get { return isYGrid; }
			set { isYGrid = value; }
		}

		public string Title {
			get { return sTitle; }
			set { sTitle = value; }
		}

		public string XLabel {
			get { return xLabel; }
			set { xLabel = value; }
		}

		public string YLabel {
			get { return yLabel; }
			set { yLabel = value; }
		}

		public Font LabelFont {
			get { return labelFont; }
			set { labelFont = value; }
		}

		public Color LabelFontColor {
			get { return labelFontColor; }
			set { labelFontColor = value; }
		}

		public Font TitleFont {
			get { return titleFont; }
			set { titleFont = value; }
		}

		public Color TitleFontColor {
			get { return titleFontColor; }
			set { titleFontColor = value; }
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

		public float XTick {
			get { return xTick; }
			set { xTick = value; }
		}

		public float YTick {
			get { return yTick; }
			set { yTick = value; }
		}

		virtual public DashStyle GridPattern {
			get { return gridPattern; }
			set { gridPattern = value; }
		}

		public float GridThickness {
			get { return gridLineThickness; }
			set { gridLineThickness = value; }
		}

		virtual public Color GridColor {
			get { return gridColor; }
			set { gridColor = value; }
		}

		public void PlotPanelStyle (Graphics g)
		{
			Pen aPen = new Pen (ChartBorderColor, 1f);
			SolidBrush aBrush = new SolidBrush (ChartBackColor);
			SizeF tickFontSize = g.MeasureString ("A", TickFont);

			// Create vertical gridlines:
			float fX, fY, xm, ym;

			aPen = new Pen (GridColor, 1f);
			aPen.DashStyle = GridPattern;
			xm = XLimMin + XTickOffset;
			if (BarType == BarTypeEnum.Vertical || 
				BarType == BarTypeEnum.VerticalOverlay ||
				BarType == BarTypeEnum.VerticalStack) {
				xm = XTickOffset + XLimMin + XTick / 2;
			}

			// Create vertical gridelines:
			if (IsYGrid == true) {

				for (fX = xm; fX < XLimMax; fX += XTick) {
					g.DrawLine (aPen, Point2D (new PointF (fX, YLimMin)),
                        Point2D (new PointF (fX, YLimMax)));
				}
			}
			// Create the x-axis tick marks:
			for (fX = xm; fX < XLimMax; fX += XTick) {
				PointF yAxisPoint = Point2D (new PointF (fX, YLimMin));
				g.DrawLine (Pens.Black, yAxisPoint, new PointF (yAxisPoint.X,
                                   yAxisPoint.Y - 8f));
			}

			// Create horizontal gridlines:
			aPen = new Pen (GridColor, 1f);
			aPen.DashStyle = GridPattern;
			ym = YLimMin + YTickOffset;
			if (BarType == BarTypeEnum.Horizontal ||
				BarType == BarTypeEnum.HorizontalOverlay ||
				BarType == BarTypeEnum.HorizontalStack) {
				ym = YTickOffset + YLimMin + YTick / 2;
			}

			if (IsXGrid == true) {
				for (fY = ym; fY < YLimMax; fY += YTick) {
					g.DrawLine (aPen, Point2D (new PointF (XLimMin, fY)),
                        Point2D (new PointF (XLimMax, fY)));
				}
			}

			// Create the y-axis tick marks:
			for (fY = ym; fY < YLimMax; fY += YTick) {
				PointF xAxisPoint = Point2D (new PointF (XLimMin, fY));
				g.DrawLine (Pens.Black, xAxisPoint,
                    new PointF (xAxisPoint.X + 5f, xAxisPoint.Y));
			}
			aPen.Dispose ();
			aBrush.Dispose ();
		}

		public void SetChartArea (Graphics g)
		{
			SetPlotPanel (g);
			// Draw chart area:
			Pen aPen = new Pen (ChartBorderColor, 1f);
			SolidBrush aBrush = new SolidBrush (ChartBackColor);
			SizeF tickFontSize = g.MeasureString ("A", TickFont);
			g.FillRectangle (aBrush, ChartArea);
			g.DrawRectangle (aPen, ChartArea);

			// Create the x-axis tick labels:
			aBrush = new SolidBrush (TickFontColor);
			float xm = XLimMin + XTickOffset;
			float xticklabel = 0f;
			if (BarType == BarTypeEnum.Vertical ||
				BarType == BarTypeEnum.VerticalOverlay ||
				BarType == BarTypeEnum.VerticalStack) {
				xm = XTickOffset + XLimMin + XTick / 2;
				xticklabel = XTick / 2;
                
			}

			for (float fX =  xm; fX <= XLimMax; fX += XTick) {
				PointF yAxisPoint = Point2D (new PointF (fX, YLimMin));
				StringFormat sFormat = new StringFormat ();
				sFormat.Alignment = StringAlignment.Center;
				g.DrawString ((fX + xticklabel).ToString (), TickFont, aBrush,
                    new PointF (form1.PlotPanel.Left + yAxisPoint.X,
                    form1.PlotPanel.Top + yAxisPoint.Y + 4f), sFormat);
			}

			// Create the y-axis tick labels:
			float ym = YLimMin + YTickOffset;
			float yticklabel = 0f;
			if (BarType == BarTypeEnum.Horizontal ||
				BarType == BarTypeEnum.HorizontalOverlay ||
				BarType == BarTypeEnum.HorizontalStack) {
				ym = YTickOffset + YLimMin + YTick / 2;
				yticklabel = YTick / 2;
			}
			for (float fY = ym; fY <= YLimMax; fY += YTick) {
				PointF xAxisPoint = Point2D (new PointF (XLimMin, fY));
				StringFormat sFormat = new StringFormat ();
				sFormat.Alignment = StringAlignment.Far;
				g.DrawString ((fY + yticklabel).ToString (), TickFont, aBrush,
                    new PointF (form1.PlotPanel.Left + xAxisPoint.X - 3f,
                    form1.PlotPanel.Top + xAxisPoint.Y
					- tickFontSize.Height / 2), sFormat);
			}

			AddLabels (g);
		}

		private void SetPlotPanel (Graphics g)
		{
			// Set form1.PlotPanel:
			float xOffset = ChartArea.Width / 30.0f;
			float yOffset = ChartArea.Height / 30.0f;
			SizeF labelFontSize = g.MeasureString ("A", LabelFont);
			SizeF titleFontSize = g.MeasureString ("A", TitleFont);
			if (Title.ToUpper () == "NO TITLE") {
				titleFontSize.Width = 8f;
				titleFontSize.Height = 8f;
			}
			float xSpacing = xOffset / 3.0f;
			float ySpacing = yOffset / 3.0f;
			SizeF tickFontSize = g.MeasureString ("A", TickFont);
			float tickSpacing = 2f;
			SizeF yTickSize = g.MeasureString (YLimMin.ToString (), TickFont);
			for (float yTick = YLimMin + YTickOffset; yTick <= YLimMax; yTick += YTick) {
				SizeF tempSize = g.MeasureString (yTick.ToString (), TickFont);
				if (yTickSize.Width < tempSize.Width) {
					yTickSize = tempSize;
				}
			}
			float leftMargin = xOffset + labelFontSize.Width +
				xSpacing + yTickSize.Width + tickSpacing;
			float rightMargin = xOffset;
			float topMargin = yOffset + titleFontSize.Height + ySpacing;
			float bottomMargin = yOffset + labelFontSize.Height +
				ySpacing + tickSpacing + tickFontSize.Height;

			// Define the plot panel size:
			int[] panelsize = new int[4];
			form1.PlotPanel.Left = ChartArea.X + (int)leftMargin;
			form1.PlotPanel.Top = ChartArea.Y + (int)topMargin;
			form1.PlotPanel.Width = ChartArea.Width - (int)leftMargin - 2 * (int)rightMargin;
			form1.PlotPanel.Height = ChartArea.Height - (int)topMargin - (int)bottomMargin;
			form1.PlotPanel.BackColor = plotBackColor;
		}

		private void AddLabels (Graphics g)
		{
			float xOffset = ChartArea.Width / 30.0f;
			float yOffset = ChartArea.Height / 30.0f;
			SizeF labelFontSize = g.MeasureString ("A", LabelFont);
			SizeF titleFontSize = g.MeasureString ("A", TitleFont);

			// Add horizontal axis label:
			SolidBrush aBrush = new SolidBrush (LabelFontColor);
			SizeF stringSize = g.MeasureString (XLabel, LabelFont);
			g.DrawString (XLabel, LabelFont, aBrush,
                new Point (form1.PlotPanel.Left + form1.PlotPanel.Width / 2 -
				(int)stringSize.Width / 2, ChartArea.Bottom - 
				(int)yOffset - (int)labelFontSize.Height));

			// Add y-axis label:
			StringFormat sFormat = new StringFormat ();
			sFormat.Alignment = StringAlignment.Center;
			stringSize = g.MeasureString (YLabel, LabelFont);
			// Save the state of the current Graphics object
			GraphicsState gState = g.Save ();
			g.TranslateTransform (ChartArea.X + xOffset, ChartArea.Y 
				+ yOffset + titleFontSize.Height
				+ yOffset / 3 + form1.PlotPanel.Height / 2);
			g.RotateTransform (-90);
			g.DrawString (YLabel, LabelFont, aBrush, 0, 0, sFormat);
			// Restore it:
			g.Restore (gState);

			// Add title:
			aBrush = new SolidBrush (TitleFontColor);
			stringSize = g.MeasureString (Title, TitleFont);
			if (Title.ToUpper () != "NO TITLE") {
				g.DrawString (Title, TitleFont, aBrush,
                    new Point (form1.PlotPanel.Left + form1.PlotPanel.Width / 2 -
					(int)stringSize.Width / 2, ChartArea.Top + (int)yOffset));
			}
			aBrush.Dispose ();
		}

		public PointF Point2D (PointF pt)
		{
			PointF aPoint = new PointF ();
			aPoint.X = (pt.X - XLimMin) *
				form1.PlotPanel.Width / (XLimMax - XLimMin);
			aPoint.Y = form1.PlotPanel.Height - (pt.Y - YLimMin) *
				form1.PlotPanel.Height / (YLimMax - YLimMin);
			return aPoint;
		}
	}
}

