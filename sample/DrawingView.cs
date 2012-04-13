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
			
			ctx.DrawLine (new Pen (new SolidBrush (Color.Red)), new PointF (1, 1), new PointF (100, 100));
			ctx.Dispose ();
		}
	}
}