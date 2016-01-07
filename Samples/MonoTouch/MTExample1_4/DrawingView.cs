using System.Drawing;
using System.Drawing.Drawing2D;

using CoreGraphics;
using UIKit;

namespace MTExample1_4 {
	public class DrawingView : UIView {
		CGRect drawingRectangle;

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
			var aPen = new Pen (Color.Blue, 4);
			
			// Set line caps and dash style:
			aPen.StartCap = LineCap.Flat;
			aPen.EndCap = LineCap.ArrowAnchor;
			aPen.DashStyle = DashStyle.Dot;
			aPen.DashOffset = 50;

			//draw straight line:
			g.DrawLine (aPen, 50, 30, 200, 30);
			// define point array to draw a curve:
			var point1 = new Point (50, 200);
			var point2 = new Point (100, 75);
			var point3 = new Point (150, 60);
			var point4 = new Point (200, 160);
			var point5 = new Point (250, 250);
			Point[] Points = {
				point1, point2, point3, point4, point5
			};
			g.DrawCurve (aPen, Points);
			
			aPen.Dispose ();
			
			//Text rotation:
			CGRect ClientRectangle = rect;
			string s = "A simple text string";
			var rectangle = new CGRect ((int)ClientRectangle.X, (int)ClientRectangle.Y, (int)ClientRectangle.Width, (int)ClientRectangle.Height);
			drawingRectangle = new CGRect (rectangle.Location, rectangle.Size);
			var sz = new CGSize (rectangle.Width, rectangle.Height);
			var font = new Font ("Arialss", 14f, FontStyle.Bold | FontStyle.Italic | FontStyle.Underline);
			CGSize stringSize = g.MeasureString (s, font);
			var Middle = new CGPoint (sz.Width / 30, sz.Height / 2 - (int)stringSize.Height / 2);

			g.DrawLine (Pens.Black, new Point (0, (int)(rectangle.Height / 2)), new Point ((int)rectangle.Width, (int)(rectangle.Height / 2)));
			g.DrawLine (Pens.Black, new Point ((int)(rectangle.Width / 2), 0), new Point ((int)(rectangle.Width / 2), (int)rectangle.Height));

			g.TranslateTransform ((float)Middle.X, (float)Middle.Y);
			g.RotateTransform (-90);
			var format = new StringFormat ();
			format.Alignment = StringAlignment.Center;
			g.DrawString (s, font, Brushes.Black, 0, 0, format);
			
			g.Dispose ();
		}
		
	}
}
