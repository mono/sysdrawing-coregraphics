using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using UIKit;

namespace MTExample1_2
{
	public class DrawingView : UIView {

		// Define the drawing area
		private Rectangle drawingRectangle;
		// Unit defined in world coordinate system:
		private float xMin = 4f;
		private float xMax = 6f;
		private float yMin = 3f;
		private float yMax = 6f;
		// Define offset in unit of pixel:
		private int offset = 30;

		public DrawingView (RectangleF rect) : base (rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			this.AutoresizingMask = UIViewAutoresizing.All;

		}

		public override void Draw (System.Drawing.RectangleF dirtyRect)
		{
			var g = Graphics.FromCurrentContext();
			
			// NSView does not have a background color so we just use Clear to white here
			g.Clear(Color.White);
			
			//RectangleF ClientRectangle = this.Bounds;
			RectangleF ClientRectangle = dirtyRect;
			
			// Calculate the location and size of the drawing area
			// within which we want to draw the graphics:
			Rectangle rect = new Rectangle((int)ClientRectangle.X, (int)ClientRectangle.Y, 
			                               (int)ClientRectangle.Width, (int)ClientRectangle.Height);
			drawingRectangle = new Rectangle(rect.Location, rect.Size);
			drawingRectangle.Inflate(-offset, -offset);
			//Draw ClientRectangle and drawingRectangle using Pen:
			g.DrawRectangle(Pens.Red, rect);
			g.DrawRectangle(Pens.Black, drawingRectangle);
			// Draw a line from point (3,2) to Point (6, 7)
			// using the Pen with a width of 3 pixels:
			Pen aPen = new Pen(Color.Green, 3);
			g.DrawLine(aPen, Point2D(new PointF(3, 2)),
			           Point2D(new PointF(6, 7)));
			
			g.PageUnit = GraphicsUnit.Inch;
			ClientRectangle = new RectangleF(0.5f,0.5f, 1.5f, 1.5f);
			aPen.Width = 1 / g.DpiX;
			g.DrawRectangle(aPen, ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
			
			aPen.Dispose();
			

			g.Dispose();
		}

		private PointF Point2D(PointF ptf)
		{
			PointF aPoint = new PointF();
			aPoint.X = drawingRectangle.X + (ptf.X - xMin) *
				drawingRectangle.Width / (xMax - xMin);
			aPoint.Y = drawingRectangle.Bottom - (ptf.Y - yMin) *
				drawingRectangle.Height / (yMax - yMin);
			return aPoint;
		}


	}
}
