
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing.Drawing2D;

using MonoMac.Foundation;
using MonoMac.AppKit;
using System.Drawing;

namespace Example1_5
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
		
		public override void DrawRect (System.Drawing.RectangleF dirtyRect)
		{
			Graphics g = new Graphics();

			g.Clear(Color.White);

			// Create a pen object:
			Pen aPen = new Pen(Color.Blue, 2);
			// Create a brush object with a transparent red color:
			SolidBrush aBrush = new SolidBrush(Color.Red);
			
			// Draw a rectangle:
			g.DrawRectangle(aPen, 20, 20, 100, 50);
			// Draw a filled rectangle:
			g.FillRectangle(aBrush, 20, 90, 100, 50);
			// Draw ellipse:
			g.DrawEllipse(aPen, new Rectangle(20, 160, 100, 50));
			// Draw filled ellipse:
			g.FillEllipse(aBrush, new Rectangle(170, 20, 100, 50));
			// Draw arc:
			g.DrawArc(aPen, new Rectangle(170, 90, 100, 50), -90, 180);

			// Draw filled pie pieces
			g.FillPie(aBrush, new Rectangle(170, 160, 100, 100), -90, 90);
			g.FillPie(Brushes.Green, new Rectangle(170, 160, 100, 100), -90, -90);
			

			// Create pens.
			Pen redPen   = new Pen(Color.Red, 3);
			Pen greenPen = new Pen(Color.Green, 3);
			greenPen.DashStyle = DashStyle.DashDotDot;
			SolidBrush transparentBrush = new SolidBrush(Color.FromArgb(150, Color.Wheat));

			// define point array to draw a curve:
			Point point1 = new Point(300, 250);
			Point point2 = new Point(350, 125);
			Point point3 = new Point(400, 110);
			Point point4 = new Point(450, 210);
			Point point5 = new Point(500, 300);
			Point[] curvePoints ={ point1, point2, point3, point4, point5};

			// Draw lines between original points to screen.
			g.DrawLines(redPen, curvePoints);

			// Fill Curve
			g.FillClosedCurve(transparentBrush, curvePoints);

			// Draw closed curve to screen.
			g.DrawClosedCurve(greenPen, curvePoints);



			g.Dispose();
		}
		
		//		public override bool IsFlipped {
		//			get {
		//				//return base.IsFlipped;
		//				return false;
		//			}
		//		}

	}
}

