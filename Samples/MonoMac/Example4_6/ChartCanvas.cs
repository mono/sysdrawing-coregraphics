
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing.Drawing2D;

using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.CoreGraphics;
using MonoMac.CoreText;
using System.Drawing;

namespace Example4_6
{
	public partial class ChartCanvas : MonoMac.AppKit.NSView
	{

		private ChartStyle cs;
		private DataSeries ds;
		private ColorMap cm;
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
			PlotPanel = new PlotPanel(Frame);
			
			this.AddSubview(PlotPanel);

			// Subscribing to a paint eventhandler to drawingPanel: 
			PlotPanel.Paint +=
				new PaintEventHandler(PlotPanelPaint);
			
			cs = new ChartStyle(this);
			ds = new DataSeries();
			lg = new Legend(this);
			lg.IsLegendVisible = true;
		}
		
		private void AddData()
		{
			// Create a standard pie chart:
//			float[] data = new float[5] { 30, 35, 15, 10, 8 };
//			string[] labels = new string[5] { "Soc. Sec. Tax", "Income Tax", 
//				"Borrowing", "Corp. Tax", "Misc." };
//			cm = new ColorMap(data.Length);
//			ds.CMap = cm.Jet();
//			ds.DataList.Clear();
//			for (int i = 0; i < data.Length; i++)
//			{
//				ds.AddData(data[i]);
//				ds.LabelList[i] = labels[i];
//			}
//			ds.ExplodeList[0] = 15;
			
			// Create a partial pie:
			float[] data = new float[3] { 0.3f, 0.1f, 0.25f};
			string[] labels = new string[3] { "0.3 -- 30%", "0.1 -- 10%",
				"0.25 -- 25%"};
			cm = new ColorMap (data.Length);
			ds.CMap = cm.Cool ();
			ds.DataList.Clear ();
			for (int i = 0; i < data.Length; i++) {
				ds.AddData (data [i]);
				ds.LabelList [i] = labels [i];
			}
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

		}
		
		private void PlotPanelPaint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			AddData();
			ds.AddPie(g, cs);
			lg.AddLegend(g, ds, cs);
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

