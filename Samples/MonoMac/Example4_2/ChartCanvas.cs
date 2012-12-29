
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing.Drawing2D;

using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.CoreGraphics;
using MonoMac.CoreText;
using System.Drawing;

namespace Example4_2
{
	public partial class ChartCanvas : MonoMac.AppKit.NSView
	{

		private ChartStyle cs;
		private DataCollection dc;
		private DataSeries ds;
		private ColorMap cm;
		
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

			// Subscribing to a paint eventhandler to drawingPanel: 
			PlotPanel.Paint +=
				new PaintEventHandler(PlotPanelPaint);

			// Set Form1 size:
			//			this.Width = 350;
			//			this.Height = 300;
			// Subscribing to a paint eventhandler to drawingPanel: 
			cs = new ChartStyle(this);
			dc = new DataCollection();
			cm = new ColorMap(100, 180);
			// Specify chart style parameters:
			cs.Title = "Bar Chart";
			cs.XLimMin = 0f;
			cs.XLimMax = 5f;
			cs.YLimMin = 0f;
			cs.YLimMax = 10f;
			cs.XTick = 1f;
			cs.YTick = 2f;
			cs.BarType = ChartStyle.BarTypeEnum.Vertical;
		}
		
		private void AddData(Graphics g)
		{
			float x, y;
			// Add first data series:
			dc.DataSeriesList.Clear();
			ds = new DataSeries();
			ds.BarStyle.BorderColor = Color.Red;
			//ds.IsSingleColorMap = true;
			ds.IsColorMap = true;
			ds.CMap = cm.Jet();
			for (int i = 0; i < 5; i++)
			{
				x = i + 1;
				y = 2.0f * x;
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

			var g = new Graphics();

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

