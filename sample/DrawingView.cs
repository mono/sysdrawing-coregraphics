using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using MonoTouch.UIKit;

namespace sample
{
	public class DrawingView : UIView {
		public DrawingView (RectangleF rect) : base (rect)
		{
		}
		
		public override void Draw (RectangleF rect)
		{
			var ctx = new Graphics (UIGraphics.GetCurrentContext ());

////			var red = new SolidBrush (Color.Red);
////			
////			ctx.DrawLine (new Pen (red), new PointF (1, 1), new PointF (100, 100));
//			ctx.Clear(Color.White);
//			
//			//RectangleF ClientRectangle = this.Bounds;
//			RectangleF ClientRectangle = rect;
//
//			// Following codes draw a line from (0, 0) to (1, 1) in unit of inch:
//			/*g.PageUnit = GraphicsUnit.Inch;
//			Pen blackPen = new Pen(Color.Black, 1/g.DpiX);
//			g.DrawLine(blackPen, 0, 0, 1, 1);*/
//			
//			// Following code shifts the origin to the center of 
//			// client area, and then draw a line from (0,0) to (1, 1) inch: 
//			ctx.PageUnit = GraphicsUnit.Inch;
//			ctx.TranslateTransform((ClientRectangle.Width / ctx.DpiX) / 2,
//			                     (ClientRectangle.Height / ctx.DpiY) / 2);
//			Pen greenPen = new Pen(Color.Green, 1 /  ctx.DpiX);
//			ctx.DrawLine(greenPen, 0, 0, 1, 1);
//			
//			ctx.Dispose ();	
//			var g = new Graphics (UIGraphics.GetCurrentContext ());
//
//			g.Clear(Color.White);
//			// Create a pen object:
//			Pen aPen = new Pen(Color.Blue, 4);
//			
//			// Set line caps and dash style:
//			aPen.StartCap = LineCap.Flat;
//			aPen.EndCap = LineCap.ArrowAnchor;
//			aPen.DashStyle = DashStyle.DashDot;
//			aPen.DashOffset = 50;
//			
//			//draw straight line:
//			g.DrawLine(aPen, 50, 30, 200, 30);
//			// define point array to draw a curve:
//			Point point1 = new Point(50, 200);
//			Point point2 = new Point(100, 75);
//			Point point3 = new Point(150, 60);
//			Point point4 = new Point(200, 160);
//			Point point5 = new Point(250, 250);
//			Point[] Points ={ point1, point2, point3, point4, point5};
//			g.DrawCurve(aPen, Points);
//			
//			aPen.Dispose();
//			g.Dispose();

			var g = new Graphics (UIGraphics.GetCurrentContext ());

			g.Clear(Color.White);
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