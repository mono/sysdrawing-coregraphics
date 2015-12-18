
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

namespace Example3_2
{
	public partial class DrawingView : AppKit.NSView, Form
	{

		private DataCollection dc;
		private ChartStyle cs;

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
			dc = new DataCollection();
			cs = new ChartStyle(this);
			cs.XLimMin = 0f;
			cs.XLimMax = 6f;
			cs.YLimMin = -1.1f;
			cs.YLimMax = 1.1f;

		}

		public DrawingView (CGRect rect) : base (rect)
		{
			Initialize();
		}
		
#endregion

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


		public override void DrawRect (CGRect dirtyRect)
		{

			var g = Graphics.FromCurrentContext();

			g.Clear(backColor);

			cs.ChartArea = this.ClientRectangle;
			AddData();
			SetPlotArea(g);
			cs.AddChartStyle(g);
			dc.AddLines(g, cs);
			g.Dispose();
		}
		
		public void AddData()
		{
			dc.DataSeriesList.Clear();
			// Add Sine data with 20 data point:
			DataSeries ds1 = new DataSeries();
			ds1.LineStyle.LineColor = Color.Red;
			ds1.LineStyle.Thickness = 2f;
			ds1.LineStyle.Pattern = DashStyle.Dash;
			for (int i = 0; i < 20; i++)
			{
				ds1.AddPoint(new PointF(i / 5.0f, (float)Math.Sin(i / 5.0f)));
			}
			dc.Add(ds1);
			
			// Add Cosine data with 40 data point:
			DataSeries ds2 = new DataSeries();
			ds2.LineStyle.LineColor = Color.Blue;
			ds2.LineStyle.Thickness = 1f;
			ds2.LineStyle.Pattern = DashStyle.Solid;
			for (int i = 0; i < 40; i++)
			{
				ds2.AddPoint(new PointF(i / 5.0f, (float)Math.Cos(i / 5.0f)));
			}
			dc.Add(ds2);
		}
		
		private void SetPlotArea(Graphics g)
		{
			// Set PlotArea:
			int xOffset = cs.ChartArea.Width / 10;
			int yOffset = cs.ChartArea.Height / 10;
			// Define the plot area:
			int plotX = cs.ChartArea.X + xOffset;
			int plotY = cs.ChartArea.Y + yOffset;
			int plotWidth = cs.ChartArea.Width - 2 * xOffset;
			int plotHeight = cs.ChartArea.Height - 2 * yOffset;
			cs.PlotArea = new Rectangle(plotX, plotY, plotWidth, plotHeight);
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
}
