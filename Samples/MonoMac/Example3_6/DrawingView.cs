
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing.Drawing2D;

using Foundation;
using AppKit;
using CoreGraphics;
using CoreText;
using CoreGraphics;
using System.Drawing;

namespace Example3_6
{
	public partial class DrawingView : AppKit.NSView, Form
	{

		private DataCollection dc;
		private ChartStyle cs;
		private Legend lg;


		#region Constructors
		
		// Called when created from unmanaged code
		public DrawingView (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public DrawingView (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		
		// Shared initialization code
		void Initialize ()
		{
			this.AutoresizingMask = NSViewResizingMask.HeightSizable | NSViewResizingMask.WidthSizable;
			BackColor = Color.Wheat;
			// Set Form1 size:
			//			this.Width = 350;
			//			this.Height = 300;
			dc = new DataCollection();
			cs = new ChartStyle(this);
			lg = new Legend();
			lg.IsLegendVisible = true;
			lg.LegendPosition = Legend.LegendPositionEnum.NorthWest;
			cs.IsY2Axis = true;
			cs.IsXGrid = false;
			cs.IsYGrid = false;
			cs.TickFont = new Font("Arial", 7, FontStyle.Regular);
			cs.TitleFont = new Font("Arial", 10, FontStyle.Regular);
			cs.XLimMin = 0f;
			cs.XLimMax = 30f;
			cs.YLimMin = -20f;
			cs.YLimMax = 20f;
			cs.XTick = 5.0f;
			cs.YTick = 5f;
			cs.Y2LimMin = 100f;
			cs.Y2LimMax = 700f;
			cs.Y2Tick = 100f;
			cs.XLabel = "X Axis";
			cs.YLabel = "Y Axis";
			cs.Y2Label = "Y2 Axis";
			cs.Title = "With Y2 Axis";
		}
		
		private void AddData(Graphics g)
		{
			dc.DataSeriesList.Clear();
			// Add data points to ds1:
			DataSeries ds1 = new DataSeries();
			ds1.LineStyle.LineColor = Color.Red;
			ds1.LineStyle.Thickness = 2f;
			ds1.LineStyle.Pattern = DashStyle.Dash;
			ds1.SeriesName = "x1*cos(x1)";
			for (int i = 0; i < 20; i++)
			{
				float x1 = 1.0f * i;
				float y1 = x1 * (float)Math.Cos(x1);
				ds1.AddPoint(new PointF(x1, y1));
			}
			dc.Add(ds1);
			// Add data points to ds2:
			DataSeries ds2 = new DataSeries();
			ds2.LineStyle.LineColor = Color.Blue;
			ds2.LineStyle.Thickness = 2f;
			ds2.SeriesName = "100 + 20*x2";
			ds2.IsY2Data = true;
			for (int i = 5; i < 30; i++)
			{
				float x2 = 1.0f * i;
				float y2 = 100.0f + 20.0f*x2;
				ds2.AddPoint(new PointF(x2, y2));
			}
			dc.Add(ds2);
		}

		public DrawingView (CGRect rect) : base (rect)
		{
			Initialize();
		}
		
#endregion
		#region Form interface
		public Rectangle ClientRectangle 
		{
			get {
				return new Rectangle((int)Bounds.X,
				                     (int)Bounds.Y,
				                     (int)Bounds.Width,
				                     (int)Bounds.Height);
			}
		}

		Color backColor = Color.White;
		public Color BackColor 
		{
			get {
				return backColor;
			}
			
			set {
				backColor = value;
			}
		}

		Font font;
		public Font Font
		{
			get {
				if (font == null)
					font = new Font("Arial",12);
				return font;
			}
			set 
			{
				font = value;
			}
		}
		#endregion

		public override void DrawRect (CGRect dirtyRect)
		{

			var g = Graphics.FromCurrentContext();

			g.Clear(backColor);

			cs.ChartArea = this.ClientRectangle;
			AddData(g);
			SetPlotArea(g);
			cs.AddChartStyle(g);
			dc.AddLines(g, cs);
			lg.AddLegend(g, dc, cs);
			g.Dispose();
		}
		
		private void SetPlotArea(Graphics g)
		{
			// Set PlotArea:
			float xOffset = cs.ChartArea.Width / 30.0f;
			float yOffset = cs.ChartArea.Height / 30.0f;
			SizeF labelFontSize = g.MeasureString("A", cs.LabelFont);
			SizeF titleFontSize = g.MeasureString("A", cs.TitleFont);
			if (cs.Title.ToUpper() == "NO TITLE")
			{
				titleFontSize.Width = 8f;
				titleFontSize.Height = 8f;
			}
			float xSpacing = xOffset / 3.0f;
			float ySpacing = yOffset / 3.0f;
			SizeF tickFontSize = g.MeasureString("A", cs.TickFont);
			float tickSpacing = 2f;
			SizeF yTickSize = g.MeasureString(cs.YLimMin.ToString(), cs.TickFont);
			for (float yTick = cs.YLimMin; yTick <= cs.YLimMax; yTick += cs.YTick)
			{
				SizeF tempSize = g.MeasureString(yTick.ToString(), cs.TickFont);
				if (yTickSize.Width < tempSize.Width)
				{
					yTickSize = tempSize;
				}
			}
			float leftMargin = xOffset + labelFontSize.Width +
				xSpacing + yTickSize.Width + tickSpacing;
			float rightMargin = xOffset;
			float topMargin = yOffset + titleFontSize.Height + ySpacing;
			float bottomMargin = yOffset + labelFontSize.Height +
				ySpacing + tickSpacing + tickFontSize.Height;
			
			if (!cs.IsY2Axis)
			{
				// Define the plot area with one Y axis:
				int plotX = cs.ChartArea.X + (int)leftMargin;
				int plotY = cs.ChartArea.Y + (int)topMargin;
				int plotWidth = cs.ChartArea.Width - (int)leftMargin - (int)rightMargin;
				int plotHeight = cs.ChartArea.Height - (int)topMargin - (int)bottomMargin;
				cs.PlotArea = new Rectangle(plotX, plotY, plotWidth, plotHeight);
			}
			else
			{
				// Define the plot area with Y and Y2 axes:
				SizeF y2TickSize = g.MeasureString(cs.Y2LimMin.ToString(), cs.TickFont);
				for (float y2Tick = cs.Y2LimMin; y2Tick <= cs.Y2LimMax; y2Tick += cs.Y2Tick)
				{
					SizeF tempSize2 = g.MeasureString(y2Tick.ToString(), cs.TickFont);
					if (y2TickSize.Width < tempSize2.Width)
					{
						y2TickSize = tempSize2;
					}
				}
				
				rightMargin = xOffset + labelFontSize.Width +
					xSpacing + y2TickSize.Width + tickSpacing;
				int plotX = cs.ChartArea.X + (int)leftMargin;
				int plotY = cs.ChartArea.Y + (int)topMargin;
				int plotWidth = cs.ChartArea.Width - (int)leftMargin - (int)rightMargin;
				int plotHeight = cs.ChartArea.Height - (int)topMargin - (int)bottomMargin;
				cs.PlotArea = new Rectangle(plotX, plotY, plotWidth, plotHeight);
			}
		}

		//		public override bool IsFlipped {
		//			get {
		//				//return base.IsFlipped;
		//				return false;
		//			}
		//		}

	}
}

public interface Form
{
	Rectangle ClientRectangle { get; }
	
	Color BackColor { get; set; } 
	
	Font Font { get; set; }
}
