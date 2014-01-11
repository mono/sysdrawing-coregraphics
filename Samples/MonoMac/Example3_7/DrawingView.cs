
using System;
using System.Collections.Generic;
using System.Linq;
using System.DrawingNative.Drawing2D;

using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.CoreGraphics;
using MonoMac.CoreText;
using System.DrawingNative;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using Rectangle = System.Drawing.Rectangle;
using SizeF = System.Drawing.SizeF;
using PointF = System.Drawing.PointF;
using RectangleF = System.Drawing.RectangleF;

namespace Example3_7
{
	public partial class DrawingView : MonoMac.AppKit.NSView, Form
	{

		private SubChart sc;
		private DataCollection dc1;
		private DataCollection dc2;
		private DataCollection dc3;
		private DataCollection dc4;
		private ChartStyle cs1;
		private ChartStyle cs2;
		private ChartStyle cs3;
		private ChartStyle cs4;
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
			// Sub Chart parameters:
			sc = new SubChart(this);
			sc.TotalChartBackColor = Color.White;
			sc.Margin = 20;
			sc.Rows = 2;
			sc.Cols = 2;
			
			// Sub-chart 1 (0, 0):
			dc1 = new DataCollection();
			cs1 = new ChartStyle(this);
			cs1.TickFont = new Font("Arial", 7, FontStyle.Regular);
			cs1.TitleFont = new Font("Arial", 10, FontStyle.Regular);
			cs1.XLimMin = 0f;
			cs1.XLimMax = 7f;
			cs1.YLimMin = -1.5f;
			cs1.YLimMax = 1.5f;
			cs1.XTick = 1.0f;
			cs1.YTick = 0.5f;
			cs1.Title = "Sin(x)";
			
			// Sub-chart 2 (0, 1):
			dc2 = new DataCollection();
			cs2 = new ChartStyle(this);
			cs2.TickFont = new Font("Arial", 7, FontStyle.Regular);
			cs2.TitleFont = new Font("Arial", 10, FontStyle.Regular);
			cs2.XLimMin = 0f;
			cs2.XLimMax = 7f;
			cs2.YLimMin = -1.5f;
			cs2.YLimMax = 1.5f;
			cs2.XTick = 1.0f;
			cs2.YTick = 0.5f;
			cs2.Title = "Cos(x)";
			
			// Sub-chart 3 (1, 0):
			dc3 = new DataCollection();
			cs3 = new ChartStyle(this);
			cs3.TickFont = new Font("Arial", 7, FontStyle.Regular);
			cs3.TitleFont = new Font("Arial", 10, FontStyle.Regular);
			cs3.XLimMin = 0f;
			cs3.XLimMax = 7f;
			cs3.YLimMin = -0.5f;
			cs3.YLimMax = 1.5f;
			cs3.XTick = 1.0f;
			cs3.YTick = 0.5f;
			cs3.Title = "Sin(x)^2";
			
			// Sub-chart 4 (1, 1):
			dc4 = new DataCollection();
			cs4 = new ChartStyle(this);
			cs4.IsY2Axis = true;
			cs4.IsXGrid = false;
			cs4.IsYGrid = false;
			cs4.TickFont = new Font("Arial", 7, FontStyle.Regular);
			cs4.TitleFont = new Font("Arial", 10, FontStyle.Regular);
			cs4.XLimMin = 0f;
			cs4.XLimMax = 30f;
			cs4.YLimMin = -20f;
			cs4.YLimMax = 20f;
			cs4.XTick = 5.0f;
			cs4.YTick = 5f;
			cs4.Y2LimMin = 100f;
			cs4.Y2LimMax = 700f;
			cs4.Y2Tick = 100f;
			cs4.XLabel = "X Axis";
			cs4.YLabel = "Y Axis";
			cs4.Y2Label = "Y2 Axis";
			cs4.Title = "With Y2 Axis";
			lg = new Legend();
			lg.IsLegendVisible = true;
			lg.LegendPosition = Legend.LegendPositionEnum.SouthEast;
		}
		
		private void AddData(Graphics g)
		{
			float x = 0f;
			float y = 0f;
			
			// Add Sin(x) data to sub-chart 1:
			dc1.DataSeriesList.Clear();
			DataSeries ds = new DataSeries();
			ds.LineStyle.LineColor = Color.Red;
			ds.LineStyle.Thickness = 2f;
			ds.LineStyle.Pattern = DashStyle.Dash;
			for (int i = 0; i < 50; i++)
			{
				x = i / 5.0f;
				y = (float)Math.Sin(x);
				ds.AddPoint(new PointF(x, y));
			}
			dc1.Add(ds);
			
			// Add Cos(x) data sub-chart 2:
			dc2.DataSeriesList.Clear();
			ds = new DataSeries();
			ds.LineStyle.LineColor = Color.Blue;
			ds.LineStyle.Thickness = 1f;
			ds.SymbolStyle.SymbolType = SymbolStyle.SymbolTypeEnum.OpenDiamond;
			for (int i = 0; i < 50; i++)
			{
				x = i / 5.0f;
				y = (float)Math.Cos(x);
				ds.AddPoint(new PointF(x, y));
			}
			dc2.Add(ds);
			
			// Add Sin(x)^2 data to sub-chart 3:
			dc3.DataSeriesList.Clear();
			ds = new DataSeries();
			ds.LineStyle.IsVisible = false;
			ds.SymbolStyle.SymbolType = SymbolStyle.SymbolTypeEnum.Dot;
			ds.SymbolStyle.FillColor = Color.Yellow;
			ds.SymbolStyle.BorderColor = Color.DarkCyan;
			for (int i = 0; i < 50; i++)
			{
				x = i / 5.0f;
				y = (float)Math.Sin(x);
				ds.AddPoint(new PointF(x, y * y));
			}
			dc3.Add(ds);
			
			// Add y1 and y2 data to sub-chart 4:
			dc4.DataSeriesList.Clear();
			// Add y1 data:
			ds = new DataSeries();
			ds.LineStyle.LineColor = Color.Red;
			ds.LineStyle.Thickness = 2f;
			ds.LineStyle.Pattern = DashStyle.Dash;
			ds.SeriesName = "x1*cos(x1)";
			for (int i = 0; i < 20; i++)
			{
				float x1 = 1.0f * i;
				float y1 = x1 * (float)Math.Cos(x1);
				ds.AddPoint(new PointF(x1, y1));
			}
			dc4.Add(ds);
			// Add y2 data:
			ds = new DataSeries();
			ds.LineStyle.LineColor = Color.Blue;
			ds.LineStyle.Thickness = 2f;
			ds.SeriesName = "100 + 20*x2";
			ds.IsY2Data = true;
			for (int i = 5; i < 30; i++)
			{
				float x2 = 1.0f * i;
				float y2 = 100.0f + 20.0f * x2;
				ds.AddPoint(new PointF(x2, y2));
			}
			dc4.Add(ds);
		}

		public DrawingView (RectangleF rect) : base (rect)
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

		public override void DrawRect (System.Drawing.RectangleF dirtyRect)
		{

			var g = Graphics.FromCurrentContext();

			g.Clear(backColor);

			// Re-define TotalChartArea for resiz-redraw:
			sc.TotalChartArea = this.ClientRectangle;
			
			// Add data for all sub-charts:
			AddData(g);
			
			// Create sub-chart layout:
			Rectangle[,] subchart = sc.SetSubChart(g);
			
			// Create sub-chart 1:
			cs1.ChartArea = subchart[0, 0];
			cs1.AddChartStyle(g);
			dc1.AddLines(g, cs1);
			
			// Create sub-chart 2:
			cs2.ChartArea = subchart[0, 1];
			cs2.AddChartStyle(g);
			dc2.AddLines(g, cs2);
			
			// Create sub-chart 3:
			cs3.ChartArea = subchart[1, 0];
			cs3.AddChartStyle(g);
			dc3.AddLines(g, cs3);
			
			// Create sub-chart 4:
			cs4.ChartArea = subchart[1, 1];
			cs4.AddChartStyle(g);
			dc4.AddLines(g, cs4);
			lg.AddLegend(g, dc4, cs4);
			g.Dispose();
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