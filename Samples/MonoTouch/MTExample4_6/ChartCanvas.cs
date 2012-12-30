using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using MonoTouch.UIKit;

namespace MTExample4_6
{
	public class ChartCanvas : UIView {

		private ChartStyle cs;
		private DataSeries ds;
		private ColorMap cm;
		private Legend lg;

		public PlotPanel PlotPanel;

		public ChartCanvas (RectangleF rect) : base (rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			this.AutoresizingMask = UIViewAutoresizing.All;
			this.BackColor = Color.Wheat;

			PlotPanel = new PlotPanel(rect);

			this.AddSubview(PlotPanel);

			// Subscribing to a paint eventhandler to drawingPanel: 
			PlotPanel.Paint +=
				new PaintEventHandler(PlotPanelPaint);
			
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
			float[] data = new float[5] { 30, 35, 15, 10, 8 };
			string[] labels = new string[5] { "Soc. Sec. Tax", "Income Tax", 
				"Borrowing", "Corp. Tax", "Misc." };
			cm = new ColorMap(data.Length);
			ds.CMap = cm.Jet();
			ds.DataList.Clear();
			for (int i = 0; i < data.Length; i++)
			{
				ds.AddData(data[i]);
				ds.LabelList[i] = labels[i];
			}
			ds.ExplodeList[0] = 15;
			
//			// Create a partial pie:
//			float[] data = new float[3] { 0.3f, 0.1f, 0.25f};
//			string[] labels = new string[3] { "0.3 -- 30%", "0.1 -- 10%",
//				"0.25 -- 25%"};
//			cm = new ColorMap (data.Length);
//			ds.CMap = cm.Cool ();
//			ds.DataList.Clear ();
//			for (int i = 0; i < data.Length; i++) {
//				ds.AddData (data [i]);
//				ds.LabelList [i] = labels [i];
//			}
		}
		

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

		public Color BackColor 
		{
			get {
				float red;
				float green;
				float blue;
				float alpha;
				BackgroundColor.GetRGBA(out red, out green, out blue, out alpha);
				return Color.FromArgb((int)alpha, (int)red, (int)green, (int)blue);
			}

			set {
				var bgc = value;
				BackgroundColor = UIColor.FromRGBA(bgc.R,bgc.G,bgc.B, bgc.A);

			}
		}

		Font font;
		public Font Font
		{
			get {
				if (font == null)
					font = new Font("Helvetica",12);
				return font;
			}
			set 
			{
				font = value;
			}
		}

		#endregion


//		public override void Draw (RectangleF dirtyRect)
//		{
//			Graphics g = new Graphics();
//			cs.ChartArea = this.ClientRectangle;
//			cs.SetChartArea(g);
//		}

		private void PlotPanelPaint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			AddData();
			ds.AddPie(g, cs);
			lg.AddLegend(g, ds, cs);
		}


	}
}

