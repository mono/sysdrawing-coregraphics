
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing.Drawing2D;

using Foundation;
using AppKit;
using CoreGraphics;
using CoreText;
using System.Drawing;

namespace Example4_1
{
	public partial class ChartCanvas : MonoMac.AppKit.NSView
	{

		private ChartStyle cs;
		private DataCollection dc;
		private DataSeries ds;
		
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
			PlotPanel = new PlotPanel(Frame);
			
			this.AddSubview(PlotPanel);
			
			// Set Form1 size:
			//			this.Width = 350;
			//			this.Height = 300;
			// Subscribing to a paint eventhandler to drawingPanel: 
			PlotPanel.Paint +=
				new PaintEventHandler(PlotPanelPaint);
			
			cs = new ChartStyle(this);
			dc = new DataCollection();
			// Specify chart style parameters:
			cs.Title = "Bar Chart";
			
			cs.XLimMin = 0f;
			cs.XLimMax = 25f;
			cs.YLimMin = 0f;
			cs.YLimMax = 5f;
			cs.XTick = 5f;
			cs.YTick = 1f;
			
			
			// Vertical
			//cs.BarType = ChartStyle.BarTypeEnum.Vertical;
			
			
			// Horizontal
			//cs.BarType = ChartStyle.BarTypeEnum.HorizontalOverlay;
			cs.BarType = ChartStyle.BarTypeEnum.HorizontalStack;
		}
		
		private void AddData(Graphics g)
		{
			float x, y;
			// Add first data series:
			dc.DataSeriesList.Clear();
			ds = new DataSeries();
			ds.BarStyle.BorderColor = Color.Red;
			ds.BarStyle.FillColor = Color.Green;
			for (int i = 0; i < 5; i++)
			{
				x = i + 1;
				y = 2.0f * x;
				ds.AddPoint(new PointF(x, y));
			}
			dc.Add(ds);
			
			// Add second data series:
			ds = new DataSeries();
			ds.BarStyle.BorderColor = Color.Red;
			ds.BarStyle.FillColor = Color.Yellow;
			for (int i = 0; i < 5; i++)
			{
				x = i + 1;
				y = 1.5f * x;
				ds.AddPoint(new PointF(x, y));
			}
			dc.Add(ds);
			
			// Add third data series:
			ds = new DataSeries();
			ds.BarStyle.BorderColor = Color.Red;
			ds.BarStyle.FillColor = Color.Blue;
			for (int i = 0; i < 5; i++)
			{
				x = i + 1;
				y = 1.0f * x;
				ds.AddPoint(new PointF(x, y));
			}
			dc.Add(ds);
		}

		public ChartCanvas (RectangleF rect) : base (rect)
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

			cs.ChartArea = this.ClientRectangle;
			cs.SetChartArea(g);
		}
		
		private void PlotPanelPaint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			AddData(g);
			cs.PlotPanelStyle(g);
			dc.AddBars(g, cs, dc.DataSeriesList.Count,
			           ds.PointList.Count);
			g.Dispose();
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

