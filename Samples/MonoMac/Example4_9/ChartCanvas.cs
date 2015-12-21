
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

namespace Example4_9
{
	public partial class ChartCanvas : AppKit.NSView
	{

		private ChartStyle cs;
		private DataCollection dc;

		public PlotPanel PlotPanel;

		#region Constructors
		
		// Called when created from unmanaged code
		public ChartCanvas (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public ChartCanvas (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		
		// Shared initialization code
		void Initialize ()
		{
			this.AutoresizingMask = NSViewResizingMask.HeightSizable | NSViewResizingMask.WidthSizable;
			BackColor = Color.Wheat;
			PlotPanel = new PlotPanel (Frame);
			
			this.AddSubview (PlotPanel);

			// Subscribing to a paint eventhandler to drawingPanel: 
			PlotPanel.Paint +=
				new PaintEventHandler (PlotPanelPaint);
			
			cs = new ChartStyle(this);
			dc = new DataCollection(this);
			// Specify chart style parameters:
			cs.Title = "Chart of GE Stock";
			cs.XTickOffset = 1;
			cs.XLimMin = -1f;
			cs.XLimMax = 20f;
			cs.YLimMin = 32f;
			cs.YLimMax = 36f;
			cs.XTick = 2f;
			cs.YTick = 0.5f;
			dc.StockChartType = DataCollection.StockChartTypeEnum.Candle;
		}
		
		private void AddData()
		{
			dc.DataSeriesList.Clear();
			TextFileReader tfr = new TextFileReader();
			DataSeries ds = new DataSeries();
			
			// Add GE stock data from a text data file:
			ds = new DataSeries();
			var mainBundle = NSBundle.MainBundle;
			var filePath = mainBundle.PathForResource("GE","txt");
			ds.DataString = tfr.ReadTextFile(filePath);
			ds.LineStyle.LineColor = Color.DarkBlue;
			dc.Add(ds);         
		}
		
		public ChartCanvas (CGRect rect) : base (rect)
		{
			Initialize ();
		}
		
#endregion
		#region Form interface
		public Rectangle ClientRectangle {
			get {
				return new Rectangle ((int)Bounds.X,
				                     (int)Bounds.Y,
				                     (int)Bounds.Width,
				                     (int)Bounds.Height);
			}
		}

		Color backColor = Color.White;

		public Color BackColor {
			get {
				return backColor;
			}
			
			set {
				backColor = value;
			}
		}

		Font font;

		public Font Font {
			get {
				if (font == null)
					font = new Font ("Arial", 12);
				return font;
			}
			set {
				font = value;
			}
		}
		#endregion

		public override void DrawRect (CGRect dirtyRect)
		{

			var g = Graphics.FromCurrentContext();

			g.Clear (backColor);

			cs.ChartArea = this.ClientRectangle;
			cs.SetChartArea(g);


		}
		
		private void PlotPanelPaint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			AddData();
			cs.PlotPanelStyle(g);
			dc.AddStockChart(g, cs);
		}
		


		// Here we make sure we are flipped so our subview PlotPanel size and location
		// are calculated correctly.  If not the positions are calculated on the 0,0 in 
		// the lower left corner instead of upper left.
		public override bool IsFlipped {
			get {
				//return base.IsFlipped;
				return true;
			}
		}

	}
}

