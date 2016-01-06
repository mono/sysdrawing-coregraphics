using System;
using System.Drawing;

using CoreGraphics;
using UIKit;

namespace MTExample3_1 {
	public class DrawingView : UIView {
		// Define the drawing area
		Rectangle PlotArea;
		// Unit defined in world coordinate system:
		float xMin = 0f;
		float xMax = 6;
		float yMin = -1.1f;
		float yMax = 1.1f;
		int nPoints = 122;
		// Unit in pixel:
		int offset = 30;

		public DrawingView (CGRect rect) : base (rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			AutoresizingMask = UIViewAutoresizing.All;
			var bgc = Color.White;
			BackgroundColor = UIColor.FromRGBA (bgc.R,bgc.G,bgc.B, bgc.A);
		}

		public override void Draw (CGRect dirtyRect)
		{
			Graphics g = Graphics.FromCurrentContext();
			
			var ClientRectangle = new Rectangle ((int)dirtyRect.X, (int)dirtyRect.Y, (int)dirtyRect.Width, (int)dirtyRect.Height);
			
			// Calculate the location and size of the drawing area
			// Within which we want to draw the graphics:
			//Rectangle ChartArea = ClientRectangle;
			var ChartArea = new Rectangle (50, 50, ClientRectangle.Width - 70, ClientRectangle.Height - 70);
			g.DrawRectangle (Pens.LightCoral, ChartArea);
			PlotArea = new Rectangle (ChartArea.Location, ChartArea.Size);
			PlotArea.Inflate (-offset, -offset);
			//Draw ClientRectangle and PlotArea using pen:
			g.DrawRectangle (Pens.Black, PlotArea);
			// Generate Sine and Cosine data points to plot:
			CGPoint[] pt1 = new CGPoint[nPoints];
			CGPoint[] pt2 = new CGPoint[nPoints];
			for (int i = 0; i < nPoints; i++) {
				pt1[i] = new CGPoint (i / 5.0f, (float)Math.Sin (i/5.0f));
				pt2[i] = new CGPoint (i / 5.0f, (float)Math.Cos (i/5.0f));
			}

			for (int i = 1; i < nPoints; i++) {
				g.DrawLine (Pens.Blue, Point2D (pt1[i - 1]), Point2D (pt1[i]));
				g.DrawLine (Pens.Red, Point2D (pt2[i - 1]), Point2D (pt2[i]));
			}

			g.Dispose();
		}
		
		PointF Point2D (CGPoint ptf)
		{
			var aPoint = new PointF ();
			if (ptf.X < xMin || ptf.X > xMax || ptf.Y < yMin || ptf.Y > yMax) {
				ptf.X = float.NaN;
				ptf.Y = float.NaN;
			}

			aPoint.X = (float)(PlotArea.X + (ptf.X - xMin) * PlotArea.Width / (xMax - xMin));
			aPoint.Y = (float)(PlotArea.Bottom - (ptf.Y - yMin) * PlotArea.Height / (yMax - yMin));
			return aPoint;
		}
	}
}
