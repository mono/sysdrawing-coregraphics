using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using MonoTouch.UIKit;

namespace MTExample1_7
{
	public class DrawingView : UIView {
		public DrawingView (RectangleF rect) : base (rect)
		{
			ContentMode = UIViewContentMode.Redraw;
			this.AutoresizingMask = UIViewAutoresizing.All;
			var bgc = Color.LightGreen;
			BackgroundColor = UIColor.FromRGBA(bgc.R,bgc.G,bgc.B, bgc.A);
		}

		float Width = 0;

		public override void Draw (RectangleF rect)
		{
			Graphics g = new Graphics();
			
			//g.Clear(Color.LightGreen);
			// TODO: Fill in the UI Just lazy right now	
			g.Dispose();
		}
		

	}
}