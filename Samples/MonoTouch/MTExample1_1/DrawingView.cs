using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using MonoTouch.UIKit;

namespace MTExample1_1
{
	public class DrawingView : UIView {
		public DrawingView (RectangleF rect) : base (rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			this.AutoresizingMask = UIViewAutoresizing.All;

		}

		public override void Draw (RectangleF rect)
		{
			var g = new Graphics();

			g.Clear(Color.White);
			
			//RectangleF ClientRectangle = this.Bounds;
			RectangleF ClientRectangle = rect;
			Console.WriteLine(rect);
			// Following codes draw a line from (0, 0) to (1, 1) in unit of inch:
			/*g.PageUnit = GraphicsUnit.Inch;
			Pen blackPen = new Pen(Color.Black, 1/g.DpiX);
			g.DrawLine(blackPen, 0, 0, 1, 1);*/
			
			// Following code shifts the origin to the center of 
			// client area, and then draw a line from (0,0) to (1, 1) inch: 
			g.PageUnit = GraphicsUnit.Inch;
			g.TranslateTransform((ClientRectangle.Width / g.DpiX) / 2,
			                     (ClientRectangle.Height / g.DpiY) / 2);
			Pen greenPen = new Pen(Color.Green, 1 /  g.DpiX);
			g.DrawLine(greenPen, 0, 0, 1, 1);
			
			g.Dispose ();
		}

	}
}