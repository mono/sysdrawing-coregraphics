
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing.Drawing2D;

using Foundation;
using AppKit;
using CoreGraphics;
using CoreText;
using System.Drawing;

namespace Example4_8
{
	public partial class ChartCanvas : MonoMac.AppKit.NSView
	{

		private ChartStyle cs;
		private DataCollection dc;
		private Legend lg;
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
			
			dc = new DataCollection ();
			cs = new ChartStyle (this);
//			cs.RMax = 0.5f;
//			cs.RMin = 0f;
			cs.RMax = 1f;
			cs.RMin = -5f;
			cs.NTicks = 4;
			cs.AngleStep = 30;
			cs.AngleDirection = ChartStyle.AngleDirectionEnum.CounterClockWise;
			lg = new Legend ();
			lg.IsLegendVisible = true;
		}
		
		private void AddData ()
		{
			dc.DataSeriesList.Clear ();
			// Add log-sine data:
			DataSeries ds = new DataSeries ();
			ds.LineStyle.LineColor = Color.Red;
			ds.LineStyle.Thickness = 1f;
			ds.LineStyle.Pattern = DashStyle.Solid;
			ds.SeriesName = "Sin(theta)";
			for (int i = 0; i < 360; i++) {
				float theta = 1.0f * i;
				float r = (float)Math.Log (1.001f + Math.Sin (2 * theta * Math.PI / 180));
				ds.AddPoint (new PointF (theta, r));
			}
			dc.Add (ds);
			
			// Add log-cosine data:
			ds = new DataSeries ();
			ds.LineStyle.LineColor = Color.Green;
			ds.LineStyle.Thickness = 1f;
			ds.LineStyle.Pattern = DashStyle.Solid;
			ds.SeriesName = "Cos(theta)";
			for (int i = 0; i < 360; i++) {
				float theta = 1.0f * i;
				float r = (float)Math.Log (1.001f + Math.Cos (2 * theta * Math.PI / 180));
				ds.AddPoint (new PointF (theta, r));
			}
			dc.Add (ds);
			
			ds = new DataSeries ();
			ds.LineStyle.LineColor = Color.Blue;
			ds.LineStyle.Thickness = 1f;
			ds.LineStyle.Pattern = DashStyle.Dot;
			ds.SeriesName = "(r,theta)";
			for (int i = 0; i < 360; i++) {
				float theta = 1.0f * i;
				float r = (float)Math.Abs (Math.Cos (2 * theta * Math.PI / 180) * 
					Math.Sin (2 * theta * Math.PI / 180));
				ds.AddPoint (new PointF (theta, r));
			}
			dc.Add (ds);
		}
		
		public ChartCanvas (RectangleF rect) : base (rect)
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

		public override void DrawRect (System.Drawing.RectangleF dirtyRect)
		{

			var g = Graphics.FromCurrentContext();

			g.Clear (backColor);

		}
		
		private void PlotPanelPaint (object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			AddData ();
			cs.SetPolarAxes (g);
			dc.AddPolar (g, cs);
			lg.AddLegend (g, dc, cs);
			g.Dispose ();
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

