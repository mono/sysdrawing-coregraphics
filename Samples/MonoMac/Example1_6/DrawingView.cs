
using System;
using System.Collections.Generic;
using System.Linq;
using System.DrawingNative.Drawing2D;

using MonoMac.Foundation;
using MonoMac.AppKit;
using System.DrawingNative;

using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using Rectangle = System.Drawing.Rectangle;
using SizeF = System.Drawing.SizeF;
using PointF = System.Drawing.PointF;
using RectangleF = System.Drawing.RectangleF;

namespace Example1_6
{
	public partial class DrawingView : MonoMac.AppKit.NSView
	{

		#region Constructors
		
		// Called when created from unmanaged code
		public DrawingView (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public DrawingView (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		
		// Shared initialization code
		void Initialize ()
		{
			this.AutoresizingMask = NSViewResizingMask.HeightSizable | NSViewResizingMask.WidthSizable;
		}

		public DrawingView (RectangleF rect) : base (rect)
		{
			Initialize();
		}
		
#endregion
		float Width = 0;

		public override void DrawRect (System.Drawing.RectangleF dirtyRect)
		{
			Graphics g = Graphics.FromCurrentContext();

			g.Clear(Color.LightGreen);

			Width = dirtyRect.Width;

			DrawFlag(g, 20, 20, this.Width - 50);
			g.Dispose();
		}
		
		private void DrawFlag(Graphics g, float x0, float y0, float width)
		{
			SolidBrush whiteBrush = new SolidBrush(Color.White);
			SolidBrush blueBrush = new SolidBrush(Color.FromArgb(0, 0, 128));
			SolidBrush redBrush = new SolidBrush(Color.Red);
			
			float height = 10 * width / 19;
			
			// Draw white rectangle background:
			g.FillRectangle(whiteBrush, x0, y0, width, height);
			
			// Draw the seven red stripes.
			for (int i = 0; i < 7; i++)
			{
				g.FillRectangle(redBrush, x0, y0 + 2 * i * height / 13, width, height / 13);
			}
			
			// Draw blue box.
			// Size it so that it covers two fifths of the flag width and
			// the top four red stripes vertically.
			RectangleF blueBox = new RectangleF(x0, y0, 2 * width / 5, 7 * height / 13);
			g.FillRectangle(blueBrush, blueBox);
			
			// Draw fifty stars in the blue box.
			float offset = blueBox.Width / 40;
			// Divide the blue box into a grid of 11 x 9 squares and place a star
			// in every other square.
			
			float dx = (blueBox.Width - 2 * offset) / 11;
			float dy = (blueBox.Height - 2 * offset) / 9;
			
			for (int j = 0; j < 9; j++)
			{
				float yc = y0 + offset + j * dy + dy / 2;
				for (int i = 0; i < 11; i++)
				{
					float xc = x0 + offset + i * dx + dx / 2;
					if ((i + j) % 2 == 0)
					{
						DrawStar(g, this.Width/55, xc, yc);
					}
				}
			}
			whiteBrush.Dispose();
			blueBrush.Dispose();
			redBrush.Dispose();
		}
		
		private void DrawStar(Graphics g, float r, float xc, float yc)
		{
			// r: determines the size of the star.
			// xc, yc: determine the location of the star.
			float sin36 = (float)Math.Sin(36.0 * Math.PI / 180.0);
			float sin72 = (float)Math.Sin(72.0 * Math.PI / 180.0);
			float cos36 = (float)Math.Cos(36.0 * Math.PI / 180.0);
			float cos72 = (float)Math.Cos(72.0 * Math.PI / 180.0);
			float r1 = r * cos72 / cos36;
			// Fill the star:
			PointF[] pts = new PointF[10];
			pts[0] = new PointF(xc, yc - r);
			pts[1] = new PointF(xc + r1 * sin36, yc - r1 * cos36);
			pts[2] = new PointF(xc + r * sin72, yc - r * cos72);
			pts[3] = new PointF(xc + r1 * sin72, yc + r1 * cos72);
			pts[4] = new PointF(xc + r * sin36, yc + r * cos36);
			pts[5] = new PointF(xc, yc + r1);
			pts[6] = new PointF(xc - r * sin36, yc + r * cos36);
			pts[7] = new PointF(xc - r1 * sin72, yc + r1 * cos72);
			pts[8] = new PointF(xc - r * sin72, yc - r * cos72);
			pts[9] = new PointF(xc - r1 * sin36, yc - r1 * cos36);
			g.FillPolygon(Brushes.White, pts);
		}  		
		//		public override bool IsFlipped {
		//			get {
		//				//return base.IsFlipped;
		//				return false;
		//			}
		//		}

	}
}

