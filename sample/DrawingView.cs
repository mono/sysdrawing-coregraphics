using System;
using System.Drawing;
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
			
			var red = new SolidBrush (Color.Red);
			
			ctx.DrawLine (new Pen (red), new PointF (1, 1), new PointF (100, 100));
			
			ctx.Dispose ();
		}
	}
}