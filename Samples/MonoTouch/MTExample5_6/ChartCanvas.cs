using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using MonoTouch.UIKit;

namespace MTExample5_6
{
	public class ChartCanvas : UIView {

		public PlotPanel panel1;

		public ChartCanvas (RectangleF rect) : base (rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			this.AutoresizingMask = UIViewAutoresizing.All;
			this.BackColor = Color.Wheat;

			var panelRect = new RectangleF(rect.X,rect.Y,rect.Width,rect.Height-100);
			panelRect.Inflate(-100,-100);
			panel1 = new PlotPanel(panelRect);
			panel1.BackColor = Color.AliceBlue;

			this.AddSubview(panel1);

			// Subscribing to a paint eventhandler to drawingPanel: 
			panel1.Paint +=
				new PaintEventHandler(PlotPanelPaint);
			
		}

		private void PlotPanelPaint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			var borderPen = new Pen(Brushes.Black,1);
			g.DrawRectangle(borderPen,panel1.Bounds);
			borderPen.Dispose();

			g.SmoothingMode = SmoothingMode.AntiAlias;
			float a;

			if (panel1.Height < panel1.Width)
				a = panel1.Height / 3;
			else
				a = panel1.Height / 4;

			DrawSphere ds = new DrawSphere(this, a, 0, 0, -a / 2);
			ds.DrawIsometricView(g);
			ds = new DrawSphere(this, 2 * a / 3, -a/2, -a/2, a / 2);
			ds.DrawIsometricView(g);
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
//			
//		}

	}
}

