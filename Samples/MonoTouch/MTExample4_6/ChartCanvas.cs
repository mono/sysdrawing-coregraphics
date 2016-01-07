using System;
using CoreGraphics;
using System.Drawing.Drawing2D;
using UIKit;
using System.Drawing;

namespace MTExample4_6 {
	public class ChartCanvas : UIView {
		ChartStyle cs;
		DataSeries ds;
		ColorMap cm;
		Legend lg;

		public PlotPanel PlotPanel { get; set; }

		public ChartCanvas (CGRect rect) : base (rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			AutoresizingMask = UIViewAutoresizing.All;
			BackColor = Color.Wheat;

			PlotPanel = new PlotPanel (rect);
			AddSubview (PlotPanel);

			// Subscribing to a paint eventhandler to drawingPanel: 
			PlotPanel.Paint += PlotPanelPaint;
			
			// Subscribing to a paint eventhandler to drawingPanel: 
			PlotPanel.Paint += PlotPanelPaint;
			
			cs = new ChartStyle (this);
			ds = new DataSeries ();
			lg = new Legend (this);
			lg.IsLegendVisible = true;
		}
		
		private void AddData()
		{
			// Create a standard pie chart:
			float[] data = { 30, 35, 15, 10, 8 };
			string[] labels = { "Soc. Sec. Tax", "Income Tax", "Borrowing", "Corp. Tax", "Misc." };
			cm = new ColorMap (data.Length);
			ds.CMap = cm.Jet ();
			ds.DataList.Clear ();
			for (int i = 0; i < data.Length; i++) {
				ds.AddData(data[i]);
				ds.LabelList[i] = labels[i];
			}
			ds.ExplodeList[0] = 15;
		}
		

		#region Form interface
		public CGRect ClientRectangle  {
			get {
				return new CGRect((int)Bounds.X, (int)Bounds.Y, (int)Bounds.Width, (int)Bounds.Height);
			}
		}

		public Color BackColor  {
			get {
				nfloat red;
				nfloat green;
				nfloat blue;
				nfloat alpha;
				BackgroundColor.GetRGBA (out red, out green, out blue, out alpha);
				return Color.FromArgb ((int)alpha, (int)red, (int)green, (int)blue);
			}
			set {
				var bgc = value;
				BackgroundColor = UIColor.FromRGBA (bgc.R,bgc.G,bgc.B, bgc.A);
			}
		}

		Font font;
		public Font Font {
			get {
				if (font == null)
					font = new Font("Helvetica", 12f);
				return font;
			}
			set {
				font = value;
			}
		}
		#endregion

		void PlotPanelPaint (object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			AddData ();
			ds.AddPie (g, cs);
			lg.AddLegend (g, ds, cs);
		}
	}
}

