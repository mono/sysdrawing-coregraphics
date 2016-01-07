using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using CoreGraphics;
using UIKit;

namespace MTExample4_8 {
	public class ChartCanvas : UIView {
		ChartStyle cs;
		DataCollection dc;
		Legend lg;

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
			cs.RMax = 1f;
			cs.RMin = -5f;
			cs.NTicks = 4;
			cs.AngleStep = 30;
			cs.AngleDirection = ChartStyle.AngleDirectionEnum.CounterClockWise;
			lg = new Legend ();
			lg.IsLegendVisible = true;
		}

		void AddData ()
		{
			dc.DataSeriesList.Clear ();
			// Add log-sine data:
			var ds = new DataSeries ();
			ds.LineStyle.LineColor = Color.Red;
			ds.LineStyle.Thickness = 1f;
			ds.LineStyle.Pattern = DashStyle.Solid;
			ds.SeriesName = "Sin(theta)";
			for (int i = 0; i < 360; i++) {
				float theta = 1f * i;
				var r = (float)Math.Log (1.001f + Math.Sin (2 * theta * Math.PI / 180));
				ds.AddPoint (new CGPoint(theta, r));
			}
			dc.Add (ds);

			ds = new DataSeries ();
			ds.LineStyle.LineColor = Color.Green;
			ds.LineStyle.Thickness = 1f;
			ds.LineStyle.Pattern = DashStyle.Solid;
			ds.SeriesName = "Cos(theta)";
			for (int i = 0; i < 360; i++) {
				float theta = 1f * i;
				var r = (float)Math.Log (1.001f + Math.Cos (2 * theta * Math.PI / 180));
				ds.AddPoint (new CGPoint (theta, r));
			}
			dc.Add (ds);
		}

		void PlotPanelPaint (object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			AddData ();
			cs.SetPolarAxes (g);
			dc.AddPolar (g,cs);
			lg.AddLegend (g, dc, cs);
			g.Dispose ();
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
	}
}

