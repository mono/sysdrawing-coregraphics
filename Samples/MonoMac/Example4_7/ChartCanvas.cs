
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

namespace Example4_7
{
	public partial class ChartCanvas : MonoMac.AppKit.NSView
	{

		private DataCollection dc;
		private DataSeries ds;
		private ChartStyle cs;
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
			dc = new DataCollection();
			cs = new ChartStyle(this);
			cs.XLimMin = 0f;
			cs.XLimMax = 10f;
			cs.YLimMin = 0f;
			cs.YLimMax = 10f;
			cs.XTick = 2.0f;
			cs.YTick = 2.0f;
			cs.XLabel = "This is X axis";
			cs.YLabel = "This is Y axis";
			cs.Title = "Area Plot";
			cs.IsXGrid = true;
			cs.IsYGrid = true;
		}
		
		private void AddData()
		{
			dc.DataSeriesList.Clear();
			// Add Sine data:
			ds = new DataSeries();
			for (int i = 0; i < 21; i++)
			{
				ds.AddPoint(new PointF(0.5f * i,
				                       2.0f + (float)Math.Sin(0.5f * i)));
			}
			dc.Add(ds);
			
			// Add Cosine data:
			ds = new DataSeries();
			for (int i = 0; i < 21; i++)
			{
				ds.AddPoint(new PointF(0.5f * i,
				                       2.0f + (float)Math.Cos(0.5f * i)));
			}
			dc.Add(ds);
			
			// Add another Sine data:
			ds = new DataSeries();
			for (int i = 0; i < 21; i++)
			{
				ds.AddPoint(new PointF(0.5f * i,
				                       3.0f + (float)Math.Sin(0.5f * i)));
			}
			dc.Add(ds);
			
			
			cm = new ColorMap(dc.DataSeriesList.Count, 150);
			dc.CMap = cm.Summer();
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

		public override void DrawRect (RectangleF dirtyRect)
		{

			var g = Graphics.FromCurrentContext();

			g.Clear(backColor);

			cs.ChartArea = this.ClientRectangle;
			cs.SetChartArea(g);
		}
		
		private void PlotPanelPaint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			AddData();
			cs.PlotPanelStyle(g);
			dc.AddAreas(g, cs, dc.DataSeriesList.Count, ds.PointList.Count);
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

