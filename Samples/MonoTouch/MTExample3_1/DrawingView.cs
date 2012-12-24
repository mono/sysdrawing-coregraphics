using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using MonoTouch.UIKit;

namespace MTExample3_1
{
	public class DrawingView : UIView {

		// Define the drawing area
		private Rectangle PlotArea;
		// Unit defined in world coordinate system:
		private float xMin = 0f;
		private float xMax = 6;
		private float yMin = -1.1f;
		private float yMax = 1.1f;
		private int nPoints = 122;
		// Unit in pixel:
		private int offset = 30;

		public DrawingView (RectangleF rect) : base (rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			this.AutoresizingMask = UIViewAutoresizing.All;
			var bgc = Color.White;
			BackgroundColor = UIColor.FromRGBA(bgc.R,bgc.G,bgc.B, bgc.A);
		}

		public override void Draw (RectangleF dirtyRect)
		{
			Graphics g = new Graphics();
			
			var ClientRectangle = new Rectangle((int)dirtyRect.X,
			                                    (int)dirtyRect.Y,
			                                    (int)dirtyRect.Width,
			                                    (int)dirtyRect.Height);
			
			// Calculate the location and size of the drawing area
			// Within which we want to draw the graphics:
			//Rectangle ChartArea = ClientRectangle;
			Rectangle ChartArea = new Rectangle(50, 50,
			                                    ClientRectangle.Width - 70, ClientRectangle.Height - 70);
			g.DrawRectangle(Pens.LightCoral, ChartArea);
			
			
			PlotArea = new Rectangle(ChartArea.Location, ChartArea.Size);
			PlotArea.Inflate(-offset, -offset);
			//Draw ClientRectangle and PlotArea using pen:
			g.DrawRectangle(Pens.Black, PlotArea);
			// Generate Sine and Cosine data points to plot:
			PointF[] pt1 = new PointF[nPoints];
			PointF[] pt2 = new PointF[nPoints];
			for (int i = 0; i < nPoints; i++)
			{
				pt1[i] = new PointF(i / 5.0f, (float)Math.Sin(i/5.0f));
				pt2[i] = new PointF(i / 5.0f, (float)Math.Cos(i/5.0f));
			}
			for (int i = 1; i < nPoints; i++)
			{
				g.DrawLine(Pens.Blue, Point2D(pt1[i - 1]), Point2D(pt1[i]));
				g.DrawLine(Pens.Red, Point2D(pt2[i - 1]), Point2D(pt2[i]));
			}
			g.Dispose();
		}
		
		private PointF Point2D(PointF ptf)
		{
			PointF aPoint = new PointF();
			if (ptf.X < xMin || ptf.X > xMax || 
			    ptf.Y < yMin || ptf.Y > yMax)
			{
				ptf.X = Single.NaN;
				ptf.Y = Single.NaN;
			}         
			aPoint.X = PlotArea.X + (ptf.X - xMin) *
				PlotArea.Width / (xMax - xMin);
			aPoint.Y = PlotArea.Bottom - (ptf.Y - yMin) *
				PlotArea.Height / (yMax - yMin);
			return aPoint;
		}
	}
}