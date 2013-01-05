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
			HatchBrush hBrush = new HatchBrush(HatchStyle.Divot, Color.Blue, Color.LightCoral);
			HatchBrush hBrush2 = new HatchBrush(HatchStyle.Cross, Color.Blue, Color.LightCoral);
			HatchBrush hBrush3 = new HatchBrush(HatchStyle.BackwardDiagonal, Color.Blue, Color.LightCoral);
			HatchBrush hBrush4 = new HatchBrush(HatchStyle.Sphere, Color.Blue, Color.LightCoral);

			// Draw a rectangle:
			g.DrawRectangle(aPen, 20, 20, 100, 50);
			// Draw a filled rectangle:
			g.FillRectangle(hBrush, 20, 90, 100, 50);
			// Draw ellipse:
			g.DrawEllipse(aPen, new Rectangle(20, 160, 100, 50));
			// Draw filled ellipse:
			g.FillEllipse(hBrush2, new Rectangle(170, 20, 100, 50));
			// Draw arc:
			g.DrawArc(aPen, new Rectangle(170, 90, 100, 50), -90, 180);
			
			// Draw filled pie pieces
			g.FillPie(aBrush, new Rectangle(170, 160, 100, 100), -90, 90);
			g.FillPie(hBrush4, new Rectangle(170, 160, 100, 100), -90, -90);

			// Create pens.
			Pen redPen   = new Pen(Color.Red, 3);
			Pen greenPen = new Pen(Color.Green, 3);
			greenPen.DashStyle = DashStyle.DashDotDot;
			SolidBrush transparentBrush = new SolidBrush(Color.FromArgb(150, Color.Wheat));
			
			// define point array to draw a curve:
			Point point1 = new Point(300, 250);
			Point point2 = new Point(350, 125);
			Point point3 = new Point(400, 110);
			Point point4 = new Point(450, 210);
			Point point5 = new Point(500, 300);
			Point[] curvePoints ={ point1, point2, point3, point4, point5};
			
			// Draw lines between original points to screen.
			g.DrawLines(redPen, curvePoints);
			
			// Fill Curve
			g.FillClosedCurve(transparentBrush, curvePoints);
			
			// Draw closed curve to screen.
			g.DrawClosedCurve(greenPen, curvePoints);
			


			g.Dispose();
		}

	}
}