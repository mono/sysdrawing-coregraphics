using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using MonoTouch.UIKit;

namespace MTExample1_5
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
			Pen aPen = new Pen(Color.Blue, 2);
			// Create a brush object with a transparent red color:
			SolidBrush aBrush = new SolidBrush(Color.Red);
			
			// Draw a rectangle:
			g.DrawRectangle(aPen, 20, 20, 100, 50);
			// Draw a filled rectangle:
			g.FillRectangle(aBrush, 20, 90, 100, 50);
			// Draw ellipse:
			g.DrawEllipse(aPen, new Rectangle(20, 160, 100, 50));
			// Draw filled ellipse:
			g.FillEllipse(aBrush, new Rectangle(170, 20, 100, 50));
			// Draw arc:
			g.DrawArc(aPen, new Rectangle(170, 90, 100, 50), -90, 180);
			
			// Draw filled pie pieces
			g.FillPie(aBrush, new Rectangle(170, 160, 100, 100), -90, 90);
			g.FillPie(Brushes.Green, new Rectangle(170, 160, 100, 100), -90, -90);
			
			g.Dispose();
		}

	}
}