using System.Drawing;

using CoreGraphics;
using UIKit;

namespace MTExample1_2 {
	public class DrawingView : UIView {
		// Define the drawing area
		Rectangle drawingRectangle;
		// Unit defined in world coordinate system:
		float xMin = 4f;
		float xMax = 6f;
		float yMin = 3f;
		float yMax = 6f;

		// Define offset in unit of pixel:
		int offset = 30;

		public DrawingView (CGRect rect) : base (rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			AutoresizingMask = UIViewAutoresizing.All;
		}

		public override void Draw (CGRect rect)
		{
			var g = Graphics.FromCurrentContext ();
			
			// NSView does not have a background color so we just use Clear to white here
			g.Clear (Color.White);
			CGRect ClientRectangle = rect;
			var rectangle = new Rectangle ((int)ClientRectangle.X, (int)ClientRectangle.Y, (int)ClientRectangle.Width, (int)ClientRectangle.Height);

			drawingRectangle = new Rectangle (rectangle.Location, rectangle.Size);
			drawingRectangle.Inflate (-offset, -offset);
			//Draw ClientRectangle and drawingRectangle using Pen:
			g.DrawRectangle (Pens.Red, rectangle);
			g.DrawRectangle (Pens.Black, drawingRectangle);
			// Draw a line from point (3,2) to Point (6, 7)
			// using the Pen with a width of 3 pixels:
			var aPen = new Pen (Color.Green, 3);
			g.DrawLine (aPen, Point2D (new CGPoint (3, 2)), Point2D (new CGPoint (6, 7)));

			g.PageUnit = GraphicsUnit.Inch;
			ClientRectangle = new CGRect(0.5f,0.5f, 1.5f, 1.5f);
			aPen.Width = 1 / g.DpiX;
			g.DrawRectangle (aPen, (float)ClientRectangle.X, (float)ClientRectangle.Y, (float)ClientRectangle.Width, (float)ClientRectangle.Height);
			
			aPen.Dispose();
			g.Dispose();
		}

		PointF Point2D (CGPoint ptf)
		{
			var aPoint = new PointF ();
			aPoint.X = (float)(drawingRectangle.X + (ptf.X - xMin) * drawingRectangle.Width / (xMax - xMin));
			aPoint.Y = (float)(drawingRectangle.Bottom - (ptf.Y - yMin) * drawingRectangle.Height / (yMax - yMin));
			return aPoint;
		}
	}
}
