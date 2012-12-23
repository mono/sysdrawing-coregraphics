using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using MonoTouch.UIKit;

namespace MTExample1_5a
{
	public class DrawingView : UIView {
		public DrawingView (RectangleF rect) : base (rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			this.AutoresizingMask = UIViewAutoresizing.All;
			BackgroundColor = UIColor.White;
		}

		public override void Draw (RectangleF rect)
		{
			Graphics g = new Graphics();
			
			//g.Clear(Color.White);
			
			// Create a pen object:
			Pen aPen = new Pen(Color.Blue, 1 / g.DpiX);
			// Create a brush object with a transparent red color:
			SolidBrush aBrush = new SolidBrush(Color.Red);
			
			g.PageUnit = GraphicsUnit.Inch;
			g.PageScale = 2;
			g.RenderingOrigin = new PointF(0.5f,0.0f);

			// Draw a rectangle:
			g.DrawRectangle(aPen, .20f, .20f, 1.00f, .50f);
			// Draw a filled rectangle:
			g.FillRectangle(aBrush, .20f, .90f, 1.00f, .50f);
			// Draw ellipse:
			g.DrawEllipse(aPen, new RectangleF(.20f, 1.60f, 1.00f, .50f));
			// Draw filled ellipse:
			g.FillEllipse(aBrush, new RectangleF(1.70f, .20f, 1.00f, .50f));
			// Draw arc:
			g.DrawArc(aPen, new RectangleF(1.70f, .90f, 1.00f, .50f), -90, 180);
			
			// Draw filled pie pieces
			g.FillPie(aBrush, 1.70f, 1.60f, 1.00f, 1.00f, -90, 90);
			g.FillPie(Brushes.Green, 1.70f, 1.60f, 1.00f, 1.00f, -90, -90);
			
			g.Dispose();
		}

	}
}