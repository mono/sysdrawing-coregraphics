using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using CoreGraphics;
using UIKit;

namespace MTExample4_7 {
	public class ChartCanvas : UIView {
		DataCollection dc;
		DataSeries ds;
		ChartStyle cs;
		ColorMap cm;

		public PlotPanel PlotPanel;

		public ChartCanvas (CGRect rect) : base (rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			AutoresizingMask = UIViewAutoresizing.All;
			BackColor = Color.Wheat;

			PlotPanel = new PlotPanel (rect);

			AddSubview (PlotPanel);

			// Subscribing to a paint eventhandler to drawingPanel: 
			PlotPanel.Paint += PlotPanelPaint;
			
			dc = new DataCollection ();
			cs = new ChartStyle (this);
			cs.XLimMin = 0f;
			cs.XLimMax = 10f;
			cs.YLimMin = 0f;
			cs.YLimMax = 10f;
			cs.XTick = 2f;
			cs.YTick = 2f;
			cs.XLabel = "This is X axis";
			cs.YLabel = "This is Y axis";
			cs.Title = "Area Plot";
			cs.IsXGrid = true;
			cs.IsYGrid = true;
		}
		
		void AddData ()
		{
			dc.DataSeriesList.Clear ();
			// Add Sine data:
			ds = new DataSeries();
			for (int i = 0; i < 21; i++)
				ds.AddPoint (new CGPoint (.5f * i, 2f + (float)Math.Sin (.5f * i)));
			dc.Add (ds);
			
			// Add Cosine data:
			ds = new DataSeries ();
			for (int i = 0; i < 21; i++) {
				ds.AddPoint (new CGPoint (.5f * i, 2f + (float)Math.Cos (.5f * i)));
			}
			dc.Add (ds);
			
			// Add another Sine data:
			ds = new DataSeries();
			for (int i = 0; i < 21; i++) {
				ds.AddPoint(new CGPoint (.5f * i, 3f + (float)Math.Sin (.5f * i)));
			}
			dc.Add (ds);
			
			cm = new ColorMap (dc.DataSeriesList.Count, 150);
			dc.CMap = cm.Summer ();
		}
		
		void PlotPanelPaint (object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			AddData ();
			cs.PlotPanelStyle (g);
			dc.AddAreas (g, cs, dc.DataSeriesList.Count, ds.PointList.Count);
		}

		#region Form interface
		public Rectangle ClientRectangle {
			get {
				return new Rectangle ((int)Bounds.X, (int)Bounds.Y, (int)Bounds.Width, (int)Bounds.Height);
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
					font = new Font ("Helvetica", 12);
				return font;
			}
			set {
				font = value;
			}
		}
		#endregion

		public override void Draw (CGRect rect)
		{
			Graphics g = Graphics.FromCurrentContext ();
			cs.ChartArea = ClientRectangle;
			cs.SetChartArea (g);
		}
	}
}

