using System.Drawing;

using CoreGraphics;
using UIKit;

namespace MTExample1_7 {
	public class DrawingView : UIView {
		public DrawingView (CGRect rect) : base (rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			AutoresizingMask = UIViewAutoresizing.All;
			var bgc = Color.LightGreen;
			BackgroundColor = UIColor.FromRGBA (bgc.R,bgc.G,bgc.B, bgc.A);
		}

		public override void Draw (CGRect rect)
		{
			Graphics g = Graphics.FromCurrentContext ();
			g.Dispose ();
		}
	}
}
