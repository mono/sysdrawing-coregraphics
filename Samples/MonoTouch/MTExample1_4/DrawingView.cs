using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using MonoTouch.UIKit;

namespace MTExample1_4
{
	public class DrawingView : UIView {



		// Define the drawing area
		private Rectangle drawingRectangle;

		public DrawingView (RectangleF rect) : base (rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			this.AutoresizingMask = UIViewAutoresizing.All;
			BackgroundColor = UIColor.White;
			
		}
		
		public override void Draw (RectangleF dirtyRect)
		{
			Graphics g = new Graphics();
			// Create a pen object:
			Pen aPen = new Pen(Color.Blue, 4);
			
			// Set line caps and dash style:
			aPen.StartCap = LineCap.Flat;
			aPen.EndCap = LineCap.ArrowAnchor;
			aPen.DashStyle = DashStyle.Dot;
			aPen.DashOffset = 50;
			
			//g.ScaleTransform(4,4);
			//draw straight line:
			g.DrawLine(aPen, 50, 30, 200, 30);
			// define point array to draw a curve:
			Point point1 = new Point(50, 200);
			Point point2 = new Point(100, 75);
			Point point3 = new Point(150, 60);
			Point point4 = new Point(200, 160);
			Point point5 = new Point(250, 250);
			Point[] Points ={ point1, point2, point3, point4, point5};
			g.DrawCurve(aPen, Points);
			
			aPen.Dispose();
			//g.Dispose();
			
			//Text rotation:
			RectangleF ClientRectangle = dirtyRect;
			string s = "A simple text string";
			
			Rectangle rect = new Rectangle((int)ClientRectangle.X, (int)ClientRectangle.Y, 
			                               (int)ClientRectangle.Width, (int)ClientRectangle.Height);
			drawingRectangle = new Rectangle(rect.Location, rect.Size);
			Size sz = new Size(rect.Width, rect.Height);
			var font = new Font("Arialss",14.0f,FontStyle.Bold | FontStyle.Italic | FontStyle.Underline);
			SizeF stringSize = g.MeasureString(s, font);
			Point Middle = new Point(sz.Width / 30,
			                        sz.Height / 2 - (int)stringSize.Height / 2);
			g.DrawLine(Pens.Black, new Point(0, rect.Height/2), new Point(rect.Width, rect.Height/2));
			g.DrawLine(Pens.Black, new Point(rect.Width / 2, 0), new Point(rect.Width / 2, rect.Height));
			g.TranslateTransform(Middle.X, Middle.Y);
			g.RotateTransform (-90);
			//StringFormat format = new StringFormat(StringFormatFlags.NoClip);
			StringFormat format = new StringFormat();
			format.Alignment = StringAlignment.Center;
			//format.LineAlignment = StringAlignment.Center;
			g.DrawString (s, font, Brushes.Black, 0, 0, format);
			
			g.Dispose();
		}
		
	}
}