using System;
using System.Drawing;

using CoreGraphics;
using UIKit;

namespace MTExample1_6 {
	public class DrawingView : UIView {
		float Width = 0;

		public DrawingView (CGRect rect) : base (rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			AutoresizingMask = UIViewAutoresizing.All;
			var bgc = Color.LightGreen;
			BackgroundColor = UIColor.FromRGBA(bgc.R,bgc.G,bgc.B, bgc.A);
		}

		public override void Draw (CGRect rect)
		{
			Graphics g = Graphics.FromCurrentContext ();
			Width = (float)rect.Width;

			DrawFlag (g, 20f, 50f, Width - 50f);
			g.Dispose ();
		}
		
		void DrawFlag (Graphics g, float x0, float y0, float width)
		{
			var whiteBrush = new SolidBrush (Color.White);
			var blueBrush = new SolidBrush (Color.FromArgb (0, 0, 128));
			var redBrush = new SolidBrush (Color.Red);
			float height = 10 * width / 19;
			g.FillRectangle (whiteBrush, x0, y0, width, height);
			
			// Draw the seven red stripes.
			for (int i = 0; i < 7; i++)
				g.FillRectangle (redBrush, x0, y0 + 2 * i * height / 13, width, height / 13);
			
			// Draw blue box.
			// Size it so that it covers two fifths of the flag width and
			// the top four red stripes vertically.
			var blueBox = new RectangleF (x0, y0, 2 * width / 5, 7 * height / 13);
			g.FillRectangle (blueBrush, blueBox);
			
			// Draw fifty stars in the blue box.
			float offset = blueBox.Width / 40;
			float dx = (blueBox.Width - 2 * offset) / 11;
			float dy = (blueBox.Height - 2 * offset) / 9;
			
			for (int j = 0; j < 9; j++) {
				float yc = y0 + offset + j * dy + dy / 2;
				for (int i = 0; i < 11; i++) {
					float xc = x0 + offset + i * dx + dx / 2;
					if ((i + j) % 2 == 0)
						DrawStar (g, Width / 55, xc, yc);
				}
			}

			whiteBrush.Dispose ();
			blueBrush.Dispose ();
			redBrush.Dispose ();
		}
		
		void DrawStar (Graphics g, float r, float xc, float yc)
		{
			// r: determines the size of the star.
			// xc, yc: determine the location of the star.
			var sin36 = (float)Math.Sin (36.0 * Math.PI / 180.0);
			var sin72 = (float)Math.Sin (72.0 * Math.PI / 180.0);
			var cos36 = (float)Math.Cos (36.0 * Math.PI / 180.0);
			var cos72 = (float)Math.Cos (72.0 * Math.PI / 180.0);
			var r1 = r * cos72 / cos36;
			// Fill the star:
			PointF[] pts = {
				new PointF (xc, yc - r),
				new PointF (xc + r1 * sin36, yc - r1 * cos36),
				new PointF (xc + r * sin72, yc - r * cos72),
				new PointF (xc + r1 * sin72, yc + r1 * cos72),
				new PointF (xc + r * sin36, yc + r * cos36),
				new PointF (xc, yc + r1),
				new PointF (xc - r * sin36, yc + r * cos36),
				new PointF (xc - r1 * sin72, yc + r1 * cos72),
				new PointF (xc - r * sin72, yc - r * cos72),
				new PointF (xc - r1 * sin36, yc - r1 * cos36)
			};

			g.FillPolygon (Brushes.White, pts);
		}
	}
}
