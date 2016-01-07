using System.Drawing;

using CoreGraphics;
using UIKit;

namespace MTExample1_5a {
	public class DrawingView : UIView {
		public DrawingView (CGRect rect) : base (rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			AutoresizingMask = UIViewAutoresizing.All;
			BackgroundColor = UIColor.White;
		}

		public override void Draw (CGRect rect)
		{
			Graphics g = Graphics.FromCurrentContext ();
			// Create a pen object:
			var aPen = new Pen (Color.Blue, 1 / g.DpiX);
			// Create a brush object with a transparent red color:
			var aBrush = new SolidBrush (Color.Red);
			
			g.PageUnit = GraphicsUnit.Inch;
			g.PageScale = 2;
			g.RenderingOrigin = new PointF (.5f, .0f);

			// Draw a rectangle:
			g.DrawRectangle (aPen, .2f, .2f, 1f, .5f);
			// Draw a filled rectangle:
			g.FillRectangle (aBrush, .2f, .9f, 1f, .5f);
			// Draw ellipse:
			g.DrawEllipse (aPen, new RectangleF (.2f, 1.6f, 1f, .5f));
			// Draw filled ellipse:
			g.FillEllipse (aBrush, new RectangleF (1.7f, .2f, 1f, .5f));
			// Draw arc:
			g.DrawArc (aPen, new RectangleF (1.7f, .9f, 1f, .5f), -90, 180);
			
			// Draw filled pie pieces
			g.FillPie (aBrush, 1.7f, 1.6f, 1f, 1f, -90, 90);
			g.FillPie (Brushes.Green, 1.7f, 1.6f, 1f, 1f, -90, -90);
			
			g.Dispose ();
		}

	}
}
