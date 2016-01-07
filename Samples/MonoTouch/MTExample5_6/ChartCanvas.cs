using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using CoreGraphics;
using UIKit;

namespace MTExample5_6 {
	public class ChartCanvas : UIView {

		public PlotPanel panel1;

		public ChartCanvas (CGRect rect) : base (rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			AutoresizingMask = UIViewAutoresizing.All;
			BackColor = Color.Wheat;

			var panelRect = new CGRect (rect.X,rect.Y,rect.Width,rect.Height- 20/UIScreen.MainScreen.Scale);
			panelRect.Inflate (-20/UIScreen.MainScreen.Scale,-20/UIScreen.MainScreen.Scale);
			panel1 = new PlotPanel (panelRect);
			panel1.BackColor = Color.AliceBlue;

			AddSubview (panel1);

			// Subscribing to a paint eventhandler to drawingPanel: 
			panel1.Paint += PlotPanelPaint;
		}

		void PlotPanelPaint (object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			var borderPen = new Pen(Brushes.Black,1);
			g.DrawRectangle (borderPen, (float)panel1.Bounds.X, (float)panel1.Bounds.Y, (float)panel1.Bounds.Width, (float)panel1.Bounds.Height);
			borderPen.Dispose ();

			g.SmoothingMode = SmoothingMode.AntiAlias;
			float a;

			if (panel1.Height < panel1.Width)
				a = panel1.Height / 3;
			else
				a = panel1.Height / 4;

			var ds = new DrawSphere (this, a, 0, 0, -a / 2);
			ds.DrawIsometricView (g);
			ds = new DrawSphere(this, 2 * a / 3, -a/2, -a/2, a / 2);
			ds.DrawIsometricView (g);
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
					font = new Font ("Helvetica", 12f);
				return font;
			}
			set {
				font = value;
			}
		}
		#endregion
	}
}

